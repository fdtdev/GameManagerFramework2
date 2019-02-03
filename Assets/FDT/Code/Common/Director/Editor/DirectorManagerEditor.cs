using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using com.FDT.Common;

namespace com.FDT.Common
{
	[CustomEditor (typeof(DirectorManager))]
	public class DirectorManagerEditor : SingletonHandlerEditorBase<DirectorManager, Director>
	{
		protected override string helpBoxContent {
			get {
				return "In runtime, the DirectorManager shows the current registered directors, with the status and a button to access them in the inspector.";
			}
		}

		protected override void DrawItem (Rect r, Director item, Color oldColor)
		{
			base.DrawItem (r, item, oldColor);

			if (item.running)
				GUI.backgroundColor = Color.cyan;
			else
				GUI.backgroundColor = oldColor;
			GUI.Box (r, string.Empty);
			string directorName = item.directorName;

			Rect r1 = new Rect(r.x, r.y, r.width/3, r.height);
			Rect r2 = new Rect(r.x +(r.width/3), r.y, r.width/3, r.height);
			Rect r3 = new Rect(r.x +((r.width/3)*2), r.y, r.width/3, r.height);


			GUI.Label (r1, directorName);
			EditorGUI.ObjectField(r2, item, typeof(Director), true);


			if (item.running) {
				GUI.Label (r3, "Running");
			} else {
				GUI.Label (r3, "Stopped");
			}
		}
	}
}