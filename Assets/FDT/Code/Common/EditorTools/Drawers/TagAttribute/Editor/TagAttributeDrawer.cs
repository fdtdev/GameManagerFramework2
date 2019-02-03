using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
#region Header
/**
 *
 * original version available in https://github.com/uranuno/MyPropertyDrawers
 * 
**/
#endregion 
namespace com.FDT.Common
{
	[CustomPropertyDrawer(typeof(TagAttribute))]
	public class TagAttributeDrawer : CastedPropertyDrawer<TagAttribute> {

		protected override List<SerializedPropertyType> validTypes { get { return new List<SerializedPropertyType> (new SerializedPropertyType[] { SerializedPropertyType.String }); } }

		public override void DoOnGUI(Rect position, SerializedProperty property, GUIContent label) 
		{
			EditorGUI.BeginProperty (position, label, property);
			
			property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
			
			EditorGUI.EndProperty ();
		}
	}
}