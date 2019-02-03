using System;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using com.FDT.Common;

namespace com.FDT.GameManager
{
	public class GameManagerIntroWindow : EditorWindow
	{
		protected string[] requiredExampleScenes = new string[]{"Singletons","SceneATopLeft",
			"SceneBTopRight",
			"SceneCBottomLeft",
			"SceneDBottomRight",
			"TextScene",
			"SceneButtons",
			"SceneEfirst",
			"SceneFsecond",
			"SceneGthird",
			"SceneHfourth"};
		protected string cPath;
		protected Texture2D texture;
		protected bool neverShowAgain;
		void OnEnable()
		{
			neverShowAgain = true;
			GetPath();
			GetTexture();
		}
		protected void GetPath()
		{
			var script = MonoScript.FromScriptableObject(this);
			cPath = AssetDatabase.GetAssetPath(script);
			var n = script.name;
			cPath = cPath.Remove(cPath.LastIndexOf(n));
		}
		protected void GetTexture()
		{
			texture = AssetDatabase.LoadAssetAtPath<Texture2D>(cPath+"gmf2logo.png");
		}
		[MenuItem ("Tools/FDT/GameManager/Show Intro")]
		public static void OpenIntro ()
		{
			var window = EditorWindow.GetWindow<GameManagerIntroWindow>(true, "Welcome to GameManager", true);
			Rect r = window.position;
			r.width = 480;
			r.height = 440;
			window.position = r;
			window.ShowPopup();
		}
		public void OnGUI()
		{
			GUIStyle style = GUI.skin.box;
			style.normal.textColor = Color.white;

			GUILayout.Label("GameManager");
			Rect cRect = GUILayoutUtility.GetRect(texture.width,texture.width,texture.height,texture.height);
			GUI.DrawTexture(cRect, texture);
			cRect = GUILayoutUtility.GetRect(texture.width,texture.width,50,50);
			Color oldBackColor = GUI.backgroundColor;
			GUI.backgroundColor = Color.black;

			GUI.Box(cRect, new GUIContent("Welcome to Game Manager Framework.\nTo execute the DEMO, you have to follow this steps:"), style);
			GUI.backgroundColor = oldBackColor;
			bool buttonOneNeeded = GetButtonOneRequirements();
			bool buttonTwoNeeded = GetButtonTwoRequirements();
			bool buttonThreeNeeded = false;
			if (!buttonOneNeeded && !buttonTwoNeeded)
				buttonThreeNeeded = GetButtonThreeRequirements();
			
			EditorGUI.BeginDisabledGroup(!buttonOneNeeded);
			if (GUILayout.Button("Add Example Scenes to BuildSettings"))
			{
				AddScenesToBuildSettings();
			}
			EditorGUI.EndDisabledGroup();

			EditorGUI.BeginDisabledGroup(!buttonTwoNeeded);
			if (GUILayout.Button("Load Singletons Scene"))
			{
				LoadSingletonsScene();
			}
			EditorGUI.EndDisabledGroup();

			EditorGUI.BeginDisabledGroup(!buttonThreeNeeded);
			if (GUILayout.Button("Change GameManager editor initial state"))
			{
				SetEditorInitialState();
			}
			EditorGUI.EndDisabledGroup();

			if (GUILayout.Button("Exit"))
			{
				EditorPrefs.SetBool(GameManagerEditor.IntroShownKey, !neverShowAgain);
				Close();

			}
			neverShowAgain = EditorGUILayout.ToggleLeft("Never show this intro again", neverShowAgain);
		}
		public bool GetButtonOneRequirements()
		{
			bool result = false;
			foreach (string scene in requiredExampleScenes)
			{
				if (!EditorTools.IsSceneInBuildSettings(scene))
					result = true;
			}
			return result;
		}
		public bool GetButtonTwoRequirements()
		{
			var currentScene = EditorSceneManager.GetActiveScene();
			return currentScene.name != "Singletons" || EditorSceneManager.sceneCount>1;
		}
		public bool GetButtonThreeRequirements()
		{
			bool result = false;
			if (GameManager.editorGameStateAsset == null)
				result = true;
			if (!GameManager.useEditorGameStateAsset)
				result = true;
			return result;
		}
		public void SetEditorInitialState()
		{
			if (!GameManager.useEditorGameStateAsset)
			{
				GameManager.useEditorGameStateAsset = true;
				EditorPrefs.SetBool(GameManagerEditor.UseEditorGameStateKey + Application.productName, GameManager.useEditorGameStateAsset);
			}
			if (GameManager.editorGameStateAsset == null)
			{
				string statePath = string.Empty;
				GameStateAsset state = null;
				var searchResults = AssetDatabase.FindAssets("state1 t:ScriptableObject");
				foreach (string item in searchResults)
				{
					string o = AssetDatabase.GUIDToAssetPath(item);
					if (EditorTools.TrimmAllPath(o).ToLower() ==  "state1.asset")
					{
						statePath = o;
						state = AssetDatabase.LoadAssetAtPath<GameStateAsset>(o);
						break;
					}
				}

				if (!string.IsNullOrEmpty(statePath))
				{
					var guid = AssetDatabase.AssetPathToGUID(statePath);
					GameManager.editorGameStateAsset = state;
					EditorPrefs.SetString(GameManagerEditor.EditorGameStateKey + Application.productName, guid);
				}
			}
		}
		public void LoadSingletonsScene()
		{			
			string sceneFullPath = EditorTools.GetSceneFullPath("Singletons");	

			EditorSceneManager.OpenScene(sceneFullPath, OpenSceneMode.Single);
		}
		public void AddScenesToBuildSettings()
		{
			List<string> fullPathScenes = new List<string>();
			foreach (string sceneName in requiredExampleScenes)
			{
				string sceneFullPath = EditorTools.GetSceneFullPath(sceneName);
				fullPathScenes.Add(sceneFullPath);
			}
			foreach (string fullPath in fullPathScenes)
			{
				if (!EditorTools.IsSceneInBuildSettings(fullPath))
				{
					EditorTools.AddSceneToBuildSettings(fullPath);
				}
			}
		}
	}
}
