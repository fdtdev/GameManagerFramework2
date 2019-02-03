using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace com.FDT.Common
{
	[InitializeOnLoad]
	class HierarchyTester
	{
		static HierarchyTester ()
		{
			EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
		}

		static void HierarchyItemCB (int instanceID, Rect selectionRect)
		{
			
			Rect r = new Rect (selectionRect); 

			GameObject go = EditorUtility.InstanceIDToObject (instanceID) as GameObject;
			if (go!= null)
			{
				IHierarchyTesteable comps = go.GetComponent(typeof(IHierarchyTesteable)) as IHierarchyTesteable;

				if (comps != null && !comps.IsValid)
				{
					Color oldColor = GUI.contentColor;
					GUI.contentColor = Color.red;
					Vector2 size = EditorStyles.label.CalcSize(new GUIContent(go.name));
					r.x+=size.x;
					r.y-=2;
					r.width=60;
					EditorGUI.DropShadowLabel (r, " (Error)");
					GUI.contentColor = oldColor;
				}
			}
		}
	}
}