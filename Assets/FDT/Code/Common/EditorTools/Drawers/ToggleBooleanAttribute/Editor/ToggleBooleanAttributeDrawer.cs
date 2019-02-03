using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace com.FDT.Common
{
	[CustomPropertyDrawer(typeof(ToggleBooleanAttribute))]
	public class ToggleBooleanAttributeDrawer : CastedPropertyDrawer<ToggleBooleanAttribute>
	{
		protected override List<SerializedPropertyType> validTypes { get { return new List<SerializedPropertyType> (new SerializedPropertyType[] { SerializedPropertyType.Boolean }); } }

		public override void DoOnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var centeredStyle = GUI.skin.GetStyle ("Label");
			centeredStyle.alignment = TextAnchor.UpperCenter;
			EditorGUI.BeginProperty (position, label, property);
			property.boolValue = EditorGUI.Toggle (position, property.boolValue, "Button");
			EditorGUI.LabelField (position, label, centeredStyle);
			EditorGUI.EndProperty ();
		}
	}
}