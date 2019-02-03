using UnityEditor;
using UnityEngine;

namespace com.FDT.Common
{
	[CustomPropertyDrawer(typeof(LabelAliasAttribute))]
	public class LabelAliasAttributeDrawer : CastedPropertyDrawer<LabelAliasAttribute>
	{
		protected override System.Collections.Generic.List<SerializedPropertyType> validTypes {
			get {
				return null;
			}
		}
		public override void DoOnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.PropertyField(position, property, label);
		}
	}
}