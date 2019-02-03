using UnityEditor;
using UnityEngine;

namespace com.FDT.Common
{
	[CustomPropertyDrawer(typeof(RedNullAttribute), true)]
	public class RedNullAttributeDrawer : CastedPropertyDrawer<RedNullAttribute>
	{
		public override void DoOnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			Color oldcolor = GUI.backgroundColor;

			if (property.propertyType.ToString() == "String")
			{
				if (string.IsNullOrEmpty(property.stringValue))
					GUI.backgroundColor = Color.red;
				
				property.stringValue = EditorGUI.TextField(position, label, property.stringValue);
			}
			else if (property.propertyType.ToString() == "ObjectReference")
			{
				if (property.objectReferenceValue == null)
					GUI.backgroundColor = Color.red;
				
				EditorGUI.ObjectField(position, property, label);
			}

			GUI.backgroundColor = oldcolor;
			EditorGUI.EndProperty();
		}
	}
}