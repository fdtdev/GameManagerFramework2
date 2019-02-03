using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using com.FDT.Common;

namespace com.FDT.GameManager
{
	[CustomEditor(typeof(GameManager))]
	public sealed class GameManagerEditor : SingletonHandlerEditorBase<GameManager, GameManagerProxy>
	{
		protected override string ItemsListTitle
		{
			get
			{
				return "Current registered GameManagerProxy Objects:";
			}
		}

		protected override string helpBoxContent
		{
			get
			{
				return "The GameManager Object allows for multiple scene loading as 'game states', loading them asynchronously.";
			}
		}

		protected override List<string> dontIncludeMe
		{
			get
			{
				List<string> result = base.dontIncludeMe;
				result.AddRange(new string[]
					{
						"items", "directorIn", "directorOut", "initOption", "buildinitAsset", "editorinitAsset", "gameManagerSceneName", "fsm", "gameStateAssets"
					});
				return result;
			}
		}

		protected override bool ShowDefaultInspector
		{
			get
			{
				return false;
			}
		}

		protected override bool FoldUnityEvents
		{
			get
			{
				return true;
			}
		}
		private SerializedProperty loadModeProperty;
		private ReorderableList gameStateAssetsList;
		private SerializedProperty gameStateAssetsProperty;
		private SerializedProperty gameManagerSceneNameProperty;
		private SerializedProperty directorInProperty;
		private SerializedProperty directorOutProperty;
		private SerializedProperty fsmProperty;
		private SerializedProperty buildInitAssetProperty;
		private List<string> enabledScenes;
		private bool valid;
		private static bool disabledIntro = true;
		private static UnityEngine.Object disabledIntroObject = null;

		protected override void OnEnable()
		{
			base.OnEnable();
			if (!Application.isPlaying)
			{
				enabledScenes = EditorTools.GetScenesFromBuildSettings(true);
			}
			gameStateAssetsProperty = serializedObject.FindProperty("gameStateAssets");
			gameManagerSceneNameProperty = serializedObject.FindProperty("gameManagerSceneName");
			directorInProperty = serializedObject.FindProperty("directorIn");
			directorOutProperty = serializedObject.FindProperty("directorOut");
			fsmProperty = serializedObject.FindProperty("fsm");
			buildInitAssetProperty = serializedObject.FindProperty("buildinitAsset");
			loadModeProperty = serializedObject.FindProperty("loadMode");

			TestIntegrity();
			GetEditorGState();

			gameStateAssetsList = new ReorderableList(serializedObject, 
				gameStateAssetsProperty, 
				true, true, true, true);
			gameStateAssetsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				bool isPlaying = Application.isPlaying;
				bool enteringPlay = EditorApplication.isPlayingOrWillChangePlaymode;
								
				var element = gameStateAssetsList.serializedProperty.GetArrayElementAtIndex(index);
				Color oldcolor = GUI.backgroundColor;
				bool itemValid = true;
				if (element.objectReferenceValue != null)
				{
					if (!isPlaying && !enteringPlay)
					{
						GameStateAsset gsa = element.objectReferenceValue as GameStateAsset;
						int sCount = gsa.scenes.Count;
						for (int i = 0; i < sCount; i++)
						{
							string sceneName = EditorTools.TrimmSceneExtension(gsa.scenes[i].scene);
							if (!enabledScenes.Contains(sceneName))
							{
								itemValid = false;
								valid = false;
							}
						}
						if (!itemValid)
							GUI.backgroundColor = Color.red;
					}
					Rect itemRect = new Rect(rect.x, rect.y, rect.width + 23, rect.height);
					EditorGUI.ObjectField(itemRect, element.objectReferenceValue, typeof(GameStateAsset), false);
				}
				GUI.backgroundColor = oldcolor;

			};

			gameStateAssetsList.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(rect, "GameState Items");
			};

			gameStateAssetsList.onRemoveCallback = (ReorderableList l) =>
			{
				if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete this item?", "Yes", "No"))
				{
					ReorderableList.defaultBehaviours.DoRemoveButton(l);
				}
			};
			gameStateAssetsList.elementHeight = 16f;

		}

		public static string EditorGameStateKey = "GM_EditorGameState_";
		public static string UseEditorGameStateKey = "GM_UseEditorGameState_";
		public static string StateAccessesKey = "GM_StateAccesses_";
		public static string IntroShownKey = "GM_IntroShown";

		[InitializeOnLoadMethod]
		static void GetEditorGStateStatic()
		{
			string guid = string.Empty;
			if (EditorPrefs.HasKey(GameManagerEditor.EditorGameStateKey + Application.productName))
			{
				guid = EditorPrefs.GetString(GameManagerEditor.EditorGameStateKey + Application.productName);
				if (!string.IsNullOrEmpty(guid))
				{
					GameManager.editorGameStateAsset = AssetDatabase.LoadAssetAtPath<GameStateAsset>(AssetDatabase.GUIDToAssetPath(guid));
				}
			}
			if (EditorPrefs.HasKey(GameManagerEditor.UseEditorGameStateKey + Application.productName))
			{
				GameManager.useEditorGameStateAsset = EditorPrefs.GetBool(GameManagerEditor.UseEditorGameStateKey + Application.productName);
			}
			if (EditorPrefs.HasKey(GameManagerEditor.StateAccessesKey + Application.productName))
			{
				GameManager.accessesLogged = EditorPrefs.GetInt(GameManagerEditor.StateAccessesKey + Application.productName);
			}
			bool isPlaying = Application.isPlaying;
			bool enteringPlay = EditorApplication.isPlayingOrWillChangePlaymode;
			if (disabledIntro && disabledIntroObject == null)
			{
				GetDisabledIntroStatus();
			}
			if ((!isPlaying && !enteringPlay) && DoAutoShowIntro())
			{
				GameManagerIntroWindow.OpenIntro();
			}
		}

		private static void GetDisabledIntroStatus()
		{
			var searchResults = AssetDatabase.FindAssets("GameManagerNoIntro");
			foreach (string item in searchResults)
			{
				string o = AssetDatabase.GUIDToAssetPath(item);
				if (EditorTools.TrimmAllPath(o) == "GameManagerNoIntro")
				{
					disabledIntroObject = AssetDatabase.LoadAssetAtPath<Object>(o);
					disabledIntro = true;
					return;
				}
			}
			disabledIntro = false;
		}

		public static bool DoAutoShowIntro()
		{
			return !disabledIntro && (!EditorPrefs.HasKey(GameManagerEditor.IntroShownKey) || EditorPrefs.GetBool(GameManagerEditor.IntroShownKey));
		}

		private void GetEditorGState()
		{		
			GetEditorGStateStatic();
		}

		private void SetEditorGState()
		{
			var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(GameManager.editorGameStateAsset));
			EditorPrefs.SetString(GameManagerEditor.EditorGameStateKey + Application.productName, guid);
			EditorPrefs.SetBool(GameManagerEditor.UseEditorGameStateKey + Application.productName, GameManager.useEditorGameStateAsset);
			EditorPrefs.SetInt(GameManagerEditor.StateAccessesKey + Application.productName, GameManager.accessesLogged);
		}

		private void TestIntegrity()
		{
			bool modified = false;
			if (directorOutProperty.objectReferenceValue == null || directorInProperty.objectReferenceValue == null)
			{
				modified = true;
				var comps = (target as GameManager).GetComponents<Director>();
				foreach (Director d in comps)
				{
					if (d.directorName == GameManager.DIRECTOR_IN)
						directorInProperty.objectReferenceValue = d;
					else if (d.directorName == GameManager.DIRECTOR_OUT)
						directorOutProperty.objectReferenceValue = d;
				}
				if (directorInProperty.objectReferenceValue == null)
				{
					Director d = (target as GameManager).gameObject.AddComponent<Director>();
					d.directorName = GameManager.DIRECTOR_IN;
					directorInProperty.objectReferenceValue = d;
				}
				if (directorOutProperty.objectReferenceValue == null)
				{
					Director d = (target as GameManager).gameObject.AddComponent<Director>();
					d.directorName = GameManager.DIRECTOR_OUT;
					directorOutProperty.objectReferenceValue = d;
				}
			}
			FSM fsm = null;
			if (fsmProperty.objectReferenceValue == null)
			{
				modified = true;
				fsm = (target as GameManager).gameObject.GetComponent<FSM>();
				if (fsm == null)
				{
					fsm = (target as GameManager).gameObject.AddComponent<FSM>();
				}
				fsmProperty.objectReferenceValue = fsm;
			}
			fsm = (fsmProperty.objectReferenceValue as FSM);

			if (fsm.GetState(GameManager.IDLE_STATE) == null)
			{
				modified = true;
				fsm.AddState(GameManager.IDLE_STATE);
			}
			if (fsm.GetState(GameManager.OUT_STATE) == null)
			{
				modified = true;
				fsm.AddState(GameManager.OUT_STATE);
			}
			if (fsm.GetState(GameManager.IN_STATE) == null)
			{
				modified = true;
				fsm.AddState(GameManager.IN_STATE);
			}
			if (fsm.AutoUpdate != FSM.UpdateOptions.ON_UPDATE)
			{
				modified = true;
				fsm.AutoUpdate = FSM.UpdateOptions.ON_UPDATE;
			}
			if (modified)
			{
				serializedObject.ApplyModifiedProperties();
				serializedObject.Update();
			}
		}


		protected override void DrawCustomEditor()
		{
			valid = true;
			base.DrawCustomEditor();
			if (Application.isPlaying)
			{
				EditorGUILayout.LabelField(new GUIContent("State Changes: ", "List of logged changes to GameState."));
				Rect tblRect = GUILayoutUtility.GetRect(0, Screen.width, 18, 18);
				GUI.Box(tblRect, string.Empty);
				EditorGUI.LabelField(new Rect(tblRect.x, tblRect.y, tblRect.width / 2, tblRect.height), "GameState");
				EditorGUI.LabelField(new Rect(tblRect.x + (tblRect.width / 2), tblRect.y, tblRect.width / 2, tblRect.height), "Time");

				int l = GameManager.Instance.accesses.Count;
				if (l > 0)
				{
					var e = GameManager.Instance.accesses.First;
					while (e != null)
					{
						Rect ar = GUILayoutUtility.GetRect(0, Screen.width, 18, 18);
						GUI.Box(ar, string.Empty);
						EditorGUI.LabelField(new Rect(ar.x, ar.y, ar.width / 2, ar.height), e.Value.name);
						EditorGUI.LabelField(new Rect(ar.x + (ar.width / 2), ar.y, ar.width / 2, ar.height), e.Value.time.ToString());
						if (EditorTools.InspectorClicked(ar, Event.current))
						{
							Debug.Log("stacktrace for ChangeState to " + e.Value.name + ":\n " + e.Value.stackTrace + " \n------\n");
						}
						e = e.Next;
					}
				}
			}
			else
			{
				EditorGUI.BeginChangeCheck();
				GameManager.accessesLogged = EditorGUILayout.DelayedIntField(new GUIContent("Accesses Logged: ", "Maximun number of accesses to ChangeState that are logged."), GameManager.accessesLogged);
				if (EditorGUI.EndChangeCheck())
				{
					SetEditorGState();
				}
			}
			GUILayout.Space(20);
			EditorGUILayout.PropertyField(gameManagerSceneNameProperty, new GUIContent("GM Scene"));
			GUILayout.Space(10);
			EditorGUILayout.PropertyField(loadModeProperty, new GUIContent("Load Mode"));

			GUILayout.Space(10);

			EditorGUILayout.PropertyField(buildInitAssetProperty, new GUIContent("Build Initial State"));
			EditorGUI.BeginChangeCheck();
			Rect r = GUILayoutUtility.GetRect(0, Screen.width, 16, 16);
			Rect r1 = new Rect(r.x, r.y, r.width * 0.385f, r.height);
			Rect r2 = new Rect(r.x + (r.width * 0.385f), r.y, r.width * 0.615f, r.height);
			GameManager.useEditorGameStateAsset = GUI.Toggle(r1, GameManager.useEditorGameStateAsset, new GUIContent("Editor Initial State", "Initial State to load when the GameManager Starts on Editor (saved locally)"), GUI.skin.button);
			GameManager.editorGameStateAsset = EditorGUI.ObjectField(r2, GameManager.editorGameStateAsset, typeof(GameStateAsset), false) as GameStateAsset;
			if (EditorGUI.EndChangeCheck())
			{
				SetEditorGState();
			}

			GUILayout.Space(10);

			gameStateAssetsList.DoLayoutList();
			if (!valid)
			{
				GUIStyle style = GUI.skin.box;
				style.wordWrap = true;
				GUIContent c = new GUIContent("Some of the selected scenes for this GameState are not added to the Build Settings.");
				float h = style.CalcHeight(c, Screen.width);
				EditorGUILayout.HelpBox("Some of the selected scenes for this GameState are not added to the Build Settings.", MessageType.Error);

				GUILayout.Space(40 - h);
			}
			else
				GUILayout.Space(40);
			
			if (GUILayout.Button("Get GameStateAssets"))
			{
				string[] guids = AssetDatabase.FindAssets("t:GameStateAsset");

				gameStateAssetsProperty.ClearArray();
				foreach (string guid in guids)
				{
					string aPath = AssetDatabase.GUIDToAssetPath(guid);
					GameStateAsset o = AssetDatabase.LoadAssetAtPath<GameStateAsset>(aPath);
					gameStateAssetsProperty.InsertArrayElementAtIndex(0);
					gameStateAssetsProperty.GetArrayElementAtIndex(0).objectReferenceValue = o;
				}
				serializedObject.ApplyModifiedProperties();
				(target as GameManager).gameStateAssets.Sort(gameStatesComparer);
				serializedObject.ApplyModifiedProperties();
			}
			GUILayout.Space(20);
		}

		protected override void DrawItem(Rect r, GameManagerProxy item, Color oldColor)
		{
			base.DrawItem(r, item, oldColor);
			GUI.Box(r, string.Empty);
			EditorGUI.ObjectField(r, item, typeof(GameManagerProxy), true);
		}

		private int gameStatesComparer(GameStateAsset a, GameStateAsset b)
		{
			return string.Compare(a.name, b.name);
		}
	}
}