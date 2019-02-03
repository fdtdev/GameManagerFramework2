using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
#region Header
/**
 *
 * original version available in https://github.com/anchan828/property-drawer-collection
 * 
**/
#endregion 
namespace com.FDT.Common
{
	[CustomPropertyDrawer(typeof(CustomRangeAttribute))]
	public class CustomRangeAttributeDrawer : CastedPropertyDrawer<CustomRangeAttribute>
	{
		protected override List<SerializedPropertyType> validTypes { get { return new List<SerializedPropertyType> (new SerializedPropertyType[] { SerializedPropertyType.Integer, SerializedPropertyType.Float }); } }

	    public override void DoOnGUI(Rect position, SerializedProperty property, GUIContent label)
	    {
			float minLimit = cAttribute.min;
			float maxLimit = cAttribute.max;

			if (cAttribute.reflected)
			{
				minLimit = MinValueForProperty(property, cAttribute.minPropertyName);
				maxLimit = MaxValueForProperty(property, cAttribute.maxPropertyName);
			}
			EditorGUI.BeginProperty (position, label, property);
			if (property.propertyType == SerializedPropertyType.Integer)
			{
				float floatValue = property.intValue;
				float newfloatValue = EditorGUI.Slider(position, label, floatValue, minLimit, maxLimit);
				if (floatValue != newfloatValue)
					property.intValue = Mathf.RoundToInt(newfloatValue);
			}
			else if (property.propertyType == SerializedPropertyType.Float)
			{
				property.floatValue = EditorGUI.Slider(position, label, property.floatValue, minLimit, maxLimit);
			}
			EditorGUI.EndProperty();

	    }
		public float MinValueForProperty( UnityEditor.SerializedProperty prop, string minPropertyName)
		{		
			var minProp = prop.serializedObject.FindProperty(minPropertyName);
			if(minProp == null)
			{
				Debug.LogWarning("Invalid min property name in ReflectedRangeAttribute");
				return 0.0f;
			}
			return ValueForProperty(minProp); 
		} 

		public float MaxValueForProperty(UnityEditor.SerializedProperty prop, string maxPropertyName)
		{
			var maxProp = prop.serializedObject.FindProperty(maxPropertyName);
			if(maxProp == null)
			{
				Debug.LogWarning("Invalid max property name in ReflectedRangeAttribute");
				return 0.0f;
			}
			return ValueForProperty(maxProp); 
		}

		public float ValueForProperty(UnityEditor.SerializedProperty prop)
		{
			switch(prop.propertyType)
			{
			case UnityEditor.SerializedPropertyType.Integer:
				return prop.intValue;
			case UnityEditor.SerializedPropertyType.Float:
				return prop.floatValue;
			default:
				return 0.0f;
			}
		}
	}
}