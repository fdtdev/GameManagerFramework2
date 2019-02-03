using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace com.FDT.Common
{
	public class CastedPropertyDrawer<T> : PropertyDrawer where T:PropertyAttributeBase
	{
		//protected virtual List<SerializedPropertyType> validTypes { get { return null; } }
		protected virtual List<SerializedPropertyType> validTypes { get { return new List<SerializedPropertyType> (new SerializedPropertyType[] { SerializedPropertyType.ObjectReference }); } }

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			label = string.IsNullOrEmpty (cAttribute.label) ? label : new GUIContent (cAttribute.label, cAttribute.tooltip);
			if (validTypes != null && !validTypes.Contains(property.propertyType))
			{
				string validTypesString = string.Empty;
				foreach (SerializedPropertyType spt in validTypes)
				{
					if (string.IsNullOrEmpty(validTypesString))
						validTypesString+=spt.ToString();
					else
						validTypesString+=","+spt.ToString();
				}
				EditorGUI.HelpBox(position, typeof(T).ToString()+" can only be used with: "+validTypesString, MessageType.Error); 
				return;
			}
			DoOnGUI(position, property, label);
		}
		public virtual void DoOnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
		}

		protected T cAttribute
		{
			get
			{
				return (T)attribute;
			}
		}

	}
}