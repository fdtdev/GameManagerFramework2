using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using com.FDT.Common;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

namespace com.FDT.GameManager
{
	#region event classes
	[System.Serializable]
	public class GMProgressEvent:UnityEvent<float>
	{

	}

	[System.Serializable]
	public class GMChangeRequest:UnityEvent<GameStateAsset,GameStateAsset>
	{

	}

	[System.Serializable]
	public class GMChangeEnd:UnityEvent<GameStateAsset>
	{

	}
	#endregion

	[UnitySingleton(UnitySingletonAttribute.Type.ExistsInScene, false), RequireComponent(typeof(FSM))]
	public sealed class GameManager : SingletonHandler<GameManager, GameManagerProxy>, IHierarchyTesteable
	{
		#region private classes

		[System.Serializable]
		private class SceneLoadData
		{
			public string scene;
			public bool asyncLoad;
		}

		#endregion

		#region IHierarchyTesteable implementation

		public bool IsValid
		{
			get
			{
				return fsm != null && directorIn != null && directorOut != null && buildinitAsset != null;
			}
		}

		#endregion

		#region Static

		public static bool useEditorGameStateAsset;
		public static GameStateAsset editorGameStateAsset;
		public static int accessesLogged = 3;
		public static string IDLE_STATE = "Idle";
		public static string IN_STATE = "StateIn";
		public static string OUT_STATE = "StateOut";
		public static string DIRECTOR_IN = "directorIn";
		public static string DIRECTOR_OUT = "directorOut";

		#endregion

		#region classes

		public class GMStateAccess
		{
			public string name;
			public float time;
			public string stackTrace;
		}
			
		#endregion

		#region enums

		public enum GMLoadMode
		{
			RELOAD_REPEATED = 0, LEAVE_REPEATED = 1
		}

		#endregion
		#region public and serialized variables

		public GMProgressEvent OnProgress;
		public GMChangeRequest OnChangeRequest;
		public GMChangeEnd OnUnloaded;
		public GMChangeEnd OnStartedLoad;
		public GMChangeEnd OnLoaded;
		public GMChangeEnd OnActivated;

		public List<GameStateAsset> gameStateAssets = new List<GameStateAsset>();
		[SerializeField, AllSceneName(tooltip="The scene where the GameManager gameObject is. It's required to have an Instance of GameManager ready and configured in the selected scene.")]
		private string gameManagerSceneName = null;
		[SerializeField] private FSM fsm = null;
		[SerializeField] private Director directorIn = null;
		[SerializeField] private Director directorOut = null;
		[SerializeField, RedNull, Tooltip("Initial State to load when the GameManager Starts on Build")] private GameStateAsset buildinitAsset = null;
		[SerializeField] private GMLoadMode loadMode = GMLoadMode.RELOAD_REPEATED;

		public LinkedList<GMStateAccess> accesses = new LinkedList<GMStateAccess>();

		public GameStateAsset CurrentGameState { get { return _currentGameState; } }

		private GameStateAsset _currentGameState = null;

		#endregion

		#region private variables

		private bool ProccessRequests = true;
		private Dictionary<string, GameStateAsset> gameStateAssetsByName = new Dictionary<string, GameStateAsset>();
		private GameStateAsset current = null;
		private GameStateAsset next = null;
		private List<SceneLoadData> toLoad = new List<SceneLoadData>();
		private List<string> toUnload = new List<string>();
		private string activeScene;
		private Scene gameManagerScene;
		private Coroutine DoForceChangeCoroutine;

		#endregion

		#region mono

		protected override void Awake()
		{
			base.Awake();
			accesses.Clear();
			foreach (GameStateAsset gsa in gameStateAssets)
			{
				gameStateAssetsByName[gsa.name] = gsa;
			}
			gameManagerScene = SceneManager.GetSceneByName(gameManagerSceneName);
			SceneManager.SetActiveScene(gameManagerScene);
			fsm.GetState(IDLE_STATE).stateStart.AddListener(IdleStart);
			fsm.GetState(IN_STATE).stateStart.AddListener(StateInStart);
			fsm.GetState(OUT_STATE).stateStart.AddListener(StateOutStart);
		}

		protected override void Start()
		{
			base.Start();
			fsm.ChangeState(IDLE_STATE);
			if (Application.isEditor && useEditorGameStateAsset && editorGameStateAsset != null)
			{
				ChangeGameState(editorGameStateAsset);
			}
			else if (!Application.isEditor && buildinitAsset != null)
			{
				ChangeGameState(buildinitAsset);
			}

		}

		#endregion

		#region api methods
		public void SetLoadMode(GMLoadMode loadMode)
		{
			this.loadMode = loadMode;
		}
		public void ForceChangeGameState(GameStateAsset s)
		{
			bool result = false;
			GameManager.Instance.ChangeGameState(s, out result);
			if (result)
				return;
			if (DoForceChangeCoroutine != null)
			{
				Debug.LogError("It can be only one ForceChangeGameState active at any moment.");
				return;
			}
			DoForceChangeCoroutine = StartCoroutine(DoForceChange(s));
		}

		public IEnumerator DoForceChange(GameStateAsset s)
		{
			bool result = false;
			do
			{
				yield return new WaitForEndOfFrame();
				GameManager.Instance.ChangeGameState(s, out result);
			}
			while(!result);
			DoForceChangeCoroutine = null;
		}

		public void ChangeGameState(GameStateAsset s)
		{
			bool result = false;
			ChangeGameState(s, out result);
		}

		public void ChangeGameState(GameStateAsset s, out bool result)
		{
			if (!ProccessRequests)
			{
				result = false;
				return;
			}
			next = s;
			GetToUnload();
			GetToLoad();
			if ( loadMode == GMLoadMode.LEAVE_REPEATED)
			{
				for(int i = toLoad.Count-1; i>=0; i--)
				{
					SceneLoadData sc = toLoad[i];
					if (toUnload.Contains(sc.scene))
					{
						toUnload.Remove(sc.scene);
						toLoad.RemoveAt(i);
					}
				}
			}
			InvokeOnChangeRequest(current, next);

			if (current != null)
			{				
				fsm.ChangeState(OUT_STATE);
			}
			else
			{
				fsm.ChangeState(IN_STATE);				
			}
			string stackTrace = UnityEngine.StackTraceUtility.ExtractStackTrace();
			AddAccess(s, stackTrace);
			_currentGameState = s;
			result = true;
			return;
		}

		public void ChangeGameState(string n, out bool result)
		{
			if (!ProccessRequests)
			{
				result = false;
				return;
			}
			if (gameStateAssetsByName.ContainsKey(n))
			{
				ChangeGameState(gameStateAssetsByName[n], out result);
				return;
			}
			else
			{
				Debug.LogError("GameState " + n + " not found.");
			}
			result = false;
			return;
		}

		public bool RestartGameState()
		{
			if (!ProccessRequests)
				return false;
			bool result = false;
			ChangeGameState(_currentGameState, out result);
			return result;
		}

		#endregion

		#region private methods

		private void IdleStart()
		{
			ProccessRequests = true;
			if (next != null)
			{
				fsm.ChangeState(IN_STATE);
			}
		}

		private void GetToUnload()
		{
			toUnload.Clear();

			int l = SceneManager.sceneCount;

			for (int i = l - 1; i >= 0; i--)
			{
				var s = SceneManager.GetSceneAt(i);
				if (s != gameManagerScene)
					toUnload.Add(s.name);
			}

		}

		private void GetToLoad()
		{
			toLoad.Clear();
			if (next != null)
			{
				// get scenes from current
				for (int i = 0; i < next.scenes.Count; i++)
				{
					if (next.scenes[i].sceneShort != gameManagerSceneName)
					{
						var sd = new SceneLoadData();
						sd.asyncLoad = next.scenes[i].asyncLoad;
						sd.scene = next.scenes[i].sceneShort;
						toLoad.Add(sd);
					}
				}
			}
		}

		private void DirectorInFinished()
		{
			directorIn.OnFinish.RemoveListener(DirectorInFinished);
			InvokeOnActivated(current);
			fsm.ChangeState(IDLE_STATE);
		}

		private void StateInStart()
		{
			ProccessRequests = false;
			current = next;
			next = null;

			if (current != null)
			{
				StartCoroutine(StateInC());
			}
			else
			{
				fsm.ChangeState(IDLE_STATE);
			}
		}

		private IEnumerator StateInC()
		{
			InvokeOnStartedLoad(current);
			var toLoadCurrent = new List<SceneLoadData>();

			do
			{
				List<AsyncOperation> loads = new List<AsyncOperation>();
				string scenes = string.Empty;
				foreach (var sd in toLoad)
				{
					
					string s = sd.scene;
					AsyncOperation a = SceneManager.LoadSceneAsync(s, LoadSceneMode.Additive);
					a.allowSceneActivation = false;
					loads.Add(a);
					scenes += " " + s;
					toLoadCurrent.Add(sd);
					if (sd.asyncLoad == false)
						break;
				}
				foreach (var sd in toLoadCurrent)
				{
					toLoad.Remove(sd);
				}

				bool loaded = true;
				float cProgress = 0.0f;
				do
				{
					loaded = true;
					foreach (AsyncOperation a in loads)
					{					
						if (a.progress < 0.9f)
							loaded = false;
					}
					yield return new WaitForEndOfFrame();
					float cP = GetProgress(loads);
					if (!loaded && cP != cProgress)
					{
						cProgress = cP;
						InvokeOnProgress(cProgress);
					}
				}
				while (!loaded);

				foreach (AsyncOperation a in loads)
				{
					a.allowSceneActivation = true;
				}
				do
				{
					loaded = true;
					foreach (AsyncOperation a in loads)
					{					
						if (!a.isDone)
							loaded = false;
					}
					yield return new WaitForEndOfFrame();
					float cP = GetProgress(loads);
					if (!loaded && cProgress != cP)
					{
						cProgress = cP;
						InvokeOnProgress(cProgress);
					}
				}
				while (!loaded);
				yield return new WaitForEndOfFrame();


			}
			while (toLoad.Count > 0);
		
			yield return new WaitForEndOfFrame();
			InvokeOnLoaded(current);
			directorIn.OnFinish.AddListener(DirectorInFinished);
			directorIn.Begin();
		}

		private float GetProgress(List<AsyncOperation> l)
		{
			int c = l.Count;
			float s = 0;
			foreach (AsyncOperation a in l)
			{
				s += a.progress;
			}

			return (s / (float)c);
		}

		private void StateOutStart()
		{	
			ProccessRequests = false;
			directorOut.OnFinish.AddListener(DirectorOutFinished);
			directorOut.Begin();
		}

		private void DirectorOutFinished()
		{
			directorOut.OnFinish.RemoveListener(DirectorOutFinished);
			StartCoroutine(StateOutC());
		}

		private IEnumerator StateOutC()
		{			
			string scenes = string.Empty;
			foreach (string s in toUnload)
			{
				SceneManager.UnloadScene(s);
				scenes += " " + s;
			}
			toUnload.Clear();
			Resources.UnloadUnusedAssets();
			InvokeOnUnloaded(current);
			current = null;
			yield return new WaitForEndOfFrame();
			fsm.ChangeState(IDLE_STATE);
		}

		private void InvokeOnProgress(float r)
		{
			OnProgress.Invoke(r);
			foreach (GameManagerProxy p in items)
			{
				p.OnProgress.Invoke(r);
			}
		}

		private void InvokeOnChangeRequest(GameStateAsset o, GameStateAsset n)
		{
			OnChangeRequest.Invoke(o, n);
			foreach (GameManagerProxy p in items)
			{
				p.OnChangeRequest.Invoke(o, n);
			}
		}

		private void InvokeOnUnloaded(GameStateAsset o)
		{
			OnUnloaded.Invoke(o);
			foreach (GameManagerProxy p in items)
			{
				p.OnUnloaded.Invoke(o);
			}
		}

		private void InvokeOnStartedLoad(GameStateAsset o)
		{
			OnStartedLoad.Invoke(o);
			foreach (GameManagerProxy p in items)
			{
				p.OnStartedLoad.Invoke(o);
			}
		}

		private void InvokeOnLoaded(GameStateAsset o)
		{
			OnLoaded.Invoke(o);
			foreach (GameManagerProxy p in items)
			{
				p.OnLoaded.Invoke(o);
			}
		}

		private void InvokeOnActivated(GameStateAsset o)
		{
			OnActivated.Invoke(o);
			foreach (GameManagerProxy p in items)
			{
				p.OnActivated.Invoke(o);
			}
		}

		private void AddAccess(GameStateAsset s, string stackTrace)
		{
			GMStateAccess a = new GMStateAccess();
			a.name = s.name;
			a.time = Time.time;
			a.stackTrace = stackTrace;
			accesses.AddFirst(a);
			if (accesses.Count > accessesLogged)
			{
				accesses.RemoveLast();
			}
		}

		#endregion
	}
}
