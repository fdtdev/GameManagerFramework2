using UnityEngine;
using System.Collections;
using com.FDT.GameManager;
using com.FDT.Common;
using UnityEngine.UI;

namespace com.FDT.GameManager.Example
{
	public class Fade : MonoBehaviour {

		public Director directorIn;
		public Director directorOut;
		public Image img;
		public float duration = 2.0f;

		public void OnChangeRequest(GameStateAsset a, GameStateAsset b)
		{
			if (a == null)
			{
				// only for first state
				Color imgc = img.color;
				imgc.a =  1.0f;
				img.color = imgc;
			}
		}
		public void DoIn()
		{
			StartCoroutine(doingIn());
		}
		public void DoOut()
		{
			StartCoroutine(doingOut());
		}
		protected IEnumerator doingIn()
		{
			
			float it = Time.realtimeSinceStartup;
			float ft = Time.realtimeSinceStartup + duration;
			Color imgc = img.color;
			imgc.a =  1.0f;
			img.color = imgc;
			do
			{
				imgc = img.color;
				imgc.a =  1.0f -(Time.realtimeSinceStartup-it)/duration;
				img.color = imgc;
				yield return new WaitForEndOfFrame();
			}
			while (Time.realtimeSinceStartup<ft);
			imgc = img.color;
			imgc.a =  0.0f;
			img.color = imgc;
			directorIn.ItemCompleted();
		}
		protected IEnumerator doingOut()
		{
			float it = Time.realtimeSinceStartup;
			float ft = Time.realtimeSinceStartup + duration;
			do
			{
				Color imgc = img.color;
				imgc.a = ( (Time.realtimeSinceStartup-it)/duration);
				img.color = imgc;
				yield return new WaitForEndOfFrame();
			}
			while (Time.realtimeSinceStartup<ft);
			directorOut.ItemCompleted();
		}
	}
}
