using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace com.FDT.Common
{
	[InitializeOnLoad]
	class AllSceneNameHandler
	{

		public static string[] sceneNames;
		public static string[] sceneNamesBuild;
		public static Dictionary<string,string> pathByName = new Dictionary<string, string>();
		public static Dictionary<string,string> nameByPath = new Dictionary<string, string>();

		static AllSceneNameHandler ()
		{
			GetAllSceneNames();
			EditorApplication.projectWindowChanged += changedPW;
		}
		public static void changedPW()
		{
			GetAllSceneNames();
		}
		public static string RemoveUnity(string completePath)
		{
			if (completePath.Contains(".unity"))
				completePath = completePath.Remove(completePath.IndexOf(".unity"));
			return completePath;
		}
		static void GetAllSceneNames()
		{
			pathByName.Clear();
			nameByPath.Clear();
			var BuildSettingsScenes = new List<string>();
			foreach ( EditorBuildSettingsScene s in UnityEditor.EditorBuildSettings.scenes)
			{
				if (s.enabled)
					BuildSettingsScenes.Add(s.path);
			}

			var sceneGUIDS = AssetDatabase.FindAssets("t:Scene");
			var sceneNamesList = new List<string>();
			var sceneNamesBuildList = new List<string>();
			foreach (string guid in sceneGUIDS)
			{
				string scenenamepath = AssetDatabase.GUIDToAssetPath(guid);
				string scenename = scenenamepath.Substring(0, scenenamepath.LastIndexOf(".unity"));
				string scenenameNoExt = scenename.Substring(scenename.LastIndexOf("/") + 1);
				pathByName[scenenameNoExt] = scenename;
				nameByPath[scenename] = scenenameNoExt;
				sceneNamesList.Add(scenenameNoExt);
				if (BuildSettingsScenes.Contains(scenenamepath))
					sceneNamesBuildList.Add(scenenamepath);
			}

			sceneNames = sceneNamesList.ToArray();
			sceneNamesBuild = sceneNamesBuildList.ToArray();
		}

		private bool ArrayCheck(string[] a, string[] b)
		{
			int i = 0;
			int l = a.Length;
			for (; i < l; i++)
			{
				if (a[i] != b[i])
					return false;
			}
			return true;
		}
	}
}