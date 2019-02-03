using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using com.FDT.Common;

namespace com.FDT.Common
{

	[CustomPropertyDrawer (typeof(Director.DirectorItem))]
	public class DirectorItemDrawer : ArraySafePropertyDrawer
	{

		protected SerializedProperty activeProperty;
		protected SerializedProperty typeProperty;

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			base.OnGUI(position, property, label);
						
			activeProperty = property.FindPropertyRelative ("active");
			typeProperty = property.FindPropertyRelative ("type");

			Color oldColor = GUI.backgroundColor;
			if (!activeProperty.boolValue)
				GUI.backgroundColor = Color.gray;

			EditorGUI.BeginProperty (position, label, property);
			GUI.Box (position, string.Empty);
			float mid = position.width / 2;
			float quarter = position.width / 4;
			GUIContent emptyContent = new GUIContent (string.Empty);

			EditorGUI.LabelField (new Rect (position.x, position.y + 2, quarter, 16f), new GUIContent( "Active:", "Inactive items will be ignored."));

			EditorGUI.PropertyField (new Rect (position.x + quarter, position.y + 2, quarter, 16f), activeProperty, emptyContent);
			EditorGUI.LabelField (new Rect (position.x + mid, position.y + 2, quarter, 16f),  new GUIContent( "Type:", "Single mode only wait for a completion message to step to the next Item. Multi mode waits for a number of individual completion messages to be delivered to step to the next Item."));
			EditorGUI.LabelField (new Rect (position.x + (quarter * 3), position.y + 2, quarter - 20f, 16f), new GUIContent( typeProperty.enumNames[typeProperty.enumValueIndex]), emptyContent);

			GUI.backgroundColor = oldColor;
			EditorGUI.EndProperty ();
		}
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label);
		}
	}
}