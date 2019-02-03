using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace com.FDT.Common
{
	[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
	public class EnumFlagsAttributeDrawer : CastedPropertyDrawer<EnumFlagsAttribute>
	{
		protected override List<SerializedPropertyType> validTypes { get { return new List<SerializedPropertyType> (new SerializedPropertyType[] { SerializedPropertyType.Enum }); } }

		public override void DoOnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
		{
			_property.intValue = EditorGUI.MaskField( _position, _label, _property.intValue, _property.enumNames );
		}
	}
}
	