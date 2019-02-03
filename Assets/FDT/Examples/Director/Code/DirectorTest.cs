using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace com.FDT.Common
{
	public class DirectorTest : MonoBehaviour
	{
		public Color ColorOn;
		public Color ColorOff;

	    public string directorName = string.Empty;

	    void Start()
	    {
	        DirectorManager.Instance.StartDirector(directorName);
	    }		

		public void SetLightOn(Image img)
		{
			img.color = ColorOn;
		}
		public void SetLightOff(Image img)
		{
			img.color = ColorOff;
		}
		public void ButtonClick(int identifier)
	    {
			DirectorManager.Instance.ItemCompleted(directorName, identifier);
	    }
	}
}