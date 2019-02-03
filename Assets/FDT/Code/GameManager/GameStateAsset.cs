using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using com.FDT.Common;

namespace com.FDT.GameManager
{

	[CreateAssetMenu(menuName = "FDT/GameStateAsset")]
	public class GameStateAsset : ScriptableObject
	{
		[System.Serializable]
		public class SceneDescription
		{
			[SerializeField, AllSceneName(noPath=false)]
			public string scene;

			public string sceneShort { get { return scene.Substring(scene.LastIndexOf("/") + 1); } }

			public bool asyncLoad = true;
		}

		public List<SceneDescription> scenes = new List<SceneDescription>();
	}
}