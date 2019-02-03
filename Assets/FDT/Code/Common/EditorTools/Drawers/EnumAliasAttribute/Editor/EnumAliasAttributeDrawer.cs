using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#region Header
/**
 *
 * original version available in https://github.com/anchan828/property-drawer-collection
 * 
**/
#endregion 
namespace com.FDT.Common
{
	[CustomPropertyDrawer(typeof(EnumAliasAttribute))]
	public class EnumAliasAttributeDrawer : CastedPropertyDrawer<EnumAliasAttribute>
	{
	    protected Dictionary<string, string> customEnumAliases = new Dictionary<string, string>();
		protected override List<SerializedPropertyType> validTypes { get { return new List<SerializedPropertyType> (new SerializedPropertyType[] { SerializedPropertyType.Enum }); } }

	    public override void DoOnGUI(Rect position, SerializedProperty property, GUIContent label)
	    {
	        ConfigEnumAliases(property, property.enumNames);

            EditorGUI.BeginChangeCheck();
            string[] options = property.enumNames
                    .Where(enumName  => customEnumAliases.ContainsKey(enumName))
                    .Select<string, string>(enumName => customEnumAliases[enumName])
                    .ToArray();
			List<GUIContent> optionsGUI = new List<GUIContent>();
			for (int i = 0; i < options.Length; i++)
			{
				optionsGUI.Add(new GUIContent(options[i], label.tooltip));
			}

			int selectedIndex = EditorGUI.Popup(position, label, property.enumValueIndex, optionsGUI.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                property.enumValueIndex = selectedIndex;
            }
	    }

	    public void ConfigEnumAliases(SerializedProperty property, string[] enumNames)
	    {
	        Type type = property.serializedObject.targetObject.GetType();
	        foreach (FieldInfo fData in type.GetFields())
	        {
	            object[] customAttributes = fData.GetCustomAttributes(typeof(EnumAliasAttribute), false);
	            foreach (EnumAliasAttribute customAttribute in customAttributes)
	            {
	                Type enumType = fData.FieldType;

	                foreach (string enumName in enumNames)
	                {
	                    FieldInfo field = enumType.GetField(enumName);
	                    if (field == null) 
							continue;

	                    EnumAliasAttribute[] attributes = (EnumAliasAttribute[])field.GetCustomAttributes(customAttribute.GetType(), false);

	                    if (!customEnumAliases.ContainsKey(enumName))
	                    {
	                        foreach (EnumAliasAttribute labelAttribute in attributes)
	                        {
	                            customEnumAliases.Add(enumName, labelAttribute.label);
	                        }
	                    }
	                }
	            }
	        }
	    }
	}
}