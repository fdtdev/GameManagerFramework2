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
	[CustomPropertyDrawer(typeof(LayerAttribute))]
	public class LayerDrawer : CastedPropertyDrawer<LayerAttribute> {

		protected override List<SerializedPropertyType> validTypes { get { return new List<SerializedPropertyType> (new SerializedPropertyType[] { SerializedPropertyType.Integer }); } }

		public override void DoOnGUI(Rect position, SerializedProperty prop, GUIContent label) {
			
			EditorGUI.BeginProperty (position, label, prop);
			
			prop.intValue = EditorGUI.LayerField(position, label, prop.intValue);
			
			EditorGUI.EndProperty ();
		}
	}
}