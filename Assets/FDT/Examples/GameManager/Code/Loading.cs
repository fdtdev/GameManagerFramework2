using UnityEngine;
using System.Collections;
using com.FDT.GameManager;
using com.FDT.Common;

namespace com.FDT.GameManager.Example
{
	public class Loading : MonoBase
	{

		public GameObject loading;

		protected override void Awake ()
		{
			loading.SetActive (false);
		}

		public void OnChangeRequest (GameStateAsset a, GameStateAsset b)
		{
			loading.SetActive (true);
		}

		public void OnLoaded (GameStateAsset a)
		{
			loading.SetActive (false);
		}
	}
}