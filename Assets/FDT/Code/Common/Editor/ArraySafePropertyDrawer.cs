using UnityEngine;
using UnityEditor;
using System;

namespace com.FDT.Common
{
	public class ArraySafePropertyDrawer : PropertyDrawer
	{
		protected string propertyType = null;
		protected int arrayItemIdx = -1;
		protected SerializedProperty arrayProperty = null;
		protected int Guid = -1;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			propertyType = property.type;
			if (propertyType == "Generic Mono" && property.propertyPath.Contains("Array.data["))
			{
				string pp = property.propertyPath;
				arrayItemIdx = int.Parse(pp.Substring(pp.Length-2,1));
				string n = pp.Substring(0, pp.IndexOf("."));
				arrayProperty = property.serializedObject.FindProperty(n);
				propertyType = arrayProperty.type;
				property = arrayProperty.GetArrayElementAtIndex(arrayItemIdx);
				SerializedProperty GuidProp = property.FindPropertyRelative("Guid");
				if (GuidProp != null)
					Guid = GuidProp.intValue;
			}
		}
	}
}
