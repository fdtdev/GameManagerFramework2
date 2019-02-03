using UnityEngine;
using System.Collections;
using com.FDT.Common;
using UnityEngine.SceneManagement;
using com.FDT.GameManager;
using UnityEngine.Events;

namespace com.FDT.GameManager
{
	public sealed class GameManagerProxy : MonoBase, IManagerHandled
	{
		public GMProgressEvent OnProgress;
		public GMChangeRequest OnChangeRequest;
		public GMChangeEnd OnUnloaded;
		public GMChangeEnd OnStartedLoad;
		public GMChangeEnd OnLoaded;
		public GMChangeEnd OnActivated;

		#region IManagerHandled implementation

		public string HandleName()
		{
			return gameObject.name;
		}

		#endregion

		public void ChangeGameState(GameStateAsset s)
		{
			GameManager.Instance.ChangeGameState(s);
		}

		public void RestartGameState()
		{
			GameManager.Instance.RestartGameState();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			GameManager.Instance.Register(this);
		}

		protected override void OnDisableOrDestroyNotQuit()
		{
			GameManager.Instance.Unregister(this);
			base.OnDisableOrDestroyNotQuit();
		}

		public void AppQuit()
		{
			Application.Quit();
		}
	}
}