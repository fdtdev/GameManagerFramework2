using UnityEditor;
using UnityEngine;
using System.Collections;

namespace com.FDT.GameManager
{
	public class GameManagerFactory
	{
		[MenuItem ("Tools/FDT/GameManager/GameObject/Create GameManager")]
		public static void CreateGameManagerGO ()
		{
			GameObject o = new GameObject();
			o.name = "GameManager";
			o.AddComponent<GameManager>();
		}
	}
}