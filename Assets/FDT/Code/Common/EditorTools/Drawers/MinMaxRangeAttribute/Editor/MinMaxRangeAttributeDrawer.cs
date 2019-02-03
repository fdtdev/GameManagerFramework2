using UnityEngine;
using UnityEditor;
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
	[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
	public class MinMaxRangeAttributeDrawer : CastedPropertyDrawer<MinMaxRangeAttribute>
	{
		protected override List<SerializedPropertyType> validTypes { get { return new List<SerializedPropertyType> (new SerializedPropertyType[] { SerializedPropertyType.Vector2 }); } }

		const int numWidth = 50;
		const int padding = 5;

		public override void DoOnGUI(Rect position, SerializedProperty property, GUIContent label) 
		{
			bool IsInt = false;

			float minLimit = cAttribute.minLimit;
			float maxLimit = cAttribute.maxLimit;
			IsInt = cAttribute.IsInt;
			if (cAttribute.reflected)
			{
				minLimit = MinValueForProperty(property, cAttribute.minPropertyName);
				maxLimit = MaxValueForProperty(property, cAttribute.maxPropertyName);
				IsInt = IsInt && ArePropsInt(property, cAttribute.minPropertyName, cAttribute.maxPropertyName);
			}

			EditorGUI.BeginProperty (position, label, property);

			position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);

			Rect minRect 	= new Rect (position.x, position.y, numWidth, position.height);
			Rect sliderRect = new Rect (minRect.x + minRect.width + padding, position.y, position.width - numWidth*2 - padding*2, position.height);
			Rect maxRect 	= new Rect (sliderRect.x + sliderRect.width + padding, position.y, numWidth, position.height);

			float min = property.vector2Value.x;
			float max = property.vector2Value.y;

			min = Mathf.Clamp(EditorGUI.FloatField (minRect, min), minLimit, max);
			max = Mathf.Clamp(EditorGUI.FloatField (maxRect, max), min, maxLimit);
				 
			EditorGUI.MinMaxSlider (sliderRect, ref min, ref max, minLimit, maxLimit);

			if (IsInt)
			{
				min = Mathf.Round(min);
				max = Mathf.Round(max);
			}
			property.vector2Value = new Vector2(min, max);

			EditorGUI.EndProperty ();		
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
		protected bool ArePropsInt(UnityEditor.SerializedProperty prop, string minPropertyName, string maxPropertyName)
		{
			// returns true if both are Int
			var minProp = prop.serializedObject.FindProperty(minPropertyName);
			var maxProp = prop.serializedObject.FindProperty(maxPropertyName);
			return minProp.propertyType == SerializedPropertyType.Integer && maxProp.propertyType == SerializedPropertyType.Integer;
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
