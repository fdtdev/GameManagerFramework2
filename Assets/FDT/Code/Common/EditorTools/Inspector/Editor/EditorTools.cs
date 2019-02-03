using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Reflection;
using UnityEditor.SceneManagement;

namespace com.FDT.Common
{
	public class EditorTools {

		public static readonly float FIXMARGIN = 35.0f;
		public static readonly float UNITYEVENT_HEIGHT = 46.0f;

		public static Rect GetInspectorRect(float h)
		{
			return GUILayoutUtility.GetRect(Screen.width-EditorTools.FIXMARGIN,Screen.width-EditorTools.FIXMARGIN, h, h);
		}
		public static PropertyDrawer GetGenericDrawer(Type comparingType)
		{
			Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();
			foreach( Type t in allTypes )
			{
				if( t.IsSubclassOf(typeof(PropertyDrawer)) && t == comparingType)
				{
					Attribute a = Attribute.GetCustomAttribute(t,typeof( CustomPropertyDrawer));
					if(a != null)
					{
						return (PropertyDrawer)Activator.CreateInstance(t);
					}                
				}
			}
			return null;
		}
		public static bool InspectorClicked(Rect r, Event e)
		{
			return e.isMouse && e.type == EventType.MouseUp && r.Contains( e.mousePosition);
		}
		public static SerializedProperty GetUnityEventCallbacks(SerializedProperty unityEventProperty)
		{
			return unityEventProperty.FindPropertyRelative("m_PersistentCalls").FindPropertyRelative("m_Calls");
		}
		public static int GetUnityEventCallbacksCount(SerializedProperty unityEventProperty)
		{
			return GetUnityEventCallbacks(unityEventProperty).arraySize;
		}
		public static string TrimmAllSceneName(string scene)
		{
			scene = scene.Substring(scene.LastIndexOf('/')+1);
			scene = TrimmSceneExtension(scene);
			return scene;
		}
		public static string TrimmAllPath(string filePath)
		{
			int i =filePath.LastIndexOf('/');
			if (i!= -1)
				filePath = filePath.Substring(i+1);
			return filePath;
		}
		public static string TrimmSceneExtension(string scene)
		{
			if (scene.LastIndexOf('.')!= -1)
				scene = scene.Substring(0, scene.Length-6);
			return scene;
		}
		public static List<string> GetTrimmedScenesFromBuildSettings(bool enabledOnly)
		{
			List<string> result = new List<string>();
			var e = UnityEditor.EditorBuildSettings.scenes;
			foreach (EditorBuildSettingsScene ss in e)
			{
				if (!enabledOnly || ss.enabled)
				{
					string n = TrimmAllSceneName( ss.path);
					result.Add(n);
				}
			}
			return result;
		}
		public static List<string> GetScenesFromBuildSettings(bool enabledOnly)
		{
			List<string> result = new List<string>();
			var e = UnityEditor.EditorBuildSettings.scenes;
			foreach (EditorBuildSettingsScene ss in e)
			{
				if (!enabledOnly || ss.enabled)
				{
					string scene = TrimmSceneExtension(ss.path);
					result.Add(scene);
				}
			}
			return result;
		}
		public static bool IsSceneInBuildSettings(string scene)
		{
			if (IsSceneFullPath(scene))
				return IsSceneInBuildSettingsFull(scene);
			else
				return IsSceneInBuildSettingsShort(scene);
		
		}
		public static bool IsSceneInBuildSettingsShort(string scene)
		{
			var e = UnityEditor.EditorBuildSettings.scenes;
			foreach (EditorBuildSettingsScene ss in e)
			{
				if (ss.enabled)
				{
					string n = ss.path.Substring(ss.path.LastIndexOf('/')+1);
					n = n.Substring(0, n.Length-6);
					if (n == scene)
						return true;
				}
			}
			return false;
		}
		public static bool IsSceneInBuildSettingsFull(string scene)
		{
			var e = UnityEditor.EditorBuildSettings.scenes;
			foreach (EditorBuildSettingsScene ss in e)
			{
				if (ss.enabled)
				{
					string n = ss.path;
					if (n == scene)
						return true;
				}
			}
			return false;
		}
		public static void AddSceneToBuildSettings(string fullPath)
		{
			fullPath = TrimmSceneExtension(fullPath);
			var newScenes = EditorBuildSettings.scenes;
			foreach (EditorBuildSettingsScene s in newScenes)
			{
				string p = TrimmSceneExtension(s.path);
				if (p == fullPath)
				{
					s.enabled = true;
					EditorBuildSettings.scenes = newScenes;
					return;
				}
			}
			EditorBuildSettingsScene newScene = new EditorBuildSettingsScene(fullPath + ".unity", true);
			UnityEditor.ArrayUtility.Add(ref newScenes, newScene);
			EditorBuildSettings.scenes = newScenes;
		}
		public static bool IsSceneFullPath(string path)
		{
			return path.LastIndexOf('/')!=-1;
		}
		public static string GetSceneFullPath(string shortScene)
		{
			var searchResults = AssetDatabase.FindAssets(shortScene + " t:Scene");
			foreach (string item in searchResults)
			{
				string o = AssetDatabase.GUIDToAssetPath(item);
				if (EditorTools.TrimmAllSceneName(o).ToLower() ==  shortScene.ToLower())
				{
					return o;
				}
			}
			return string.Empty;
		}
	}
}