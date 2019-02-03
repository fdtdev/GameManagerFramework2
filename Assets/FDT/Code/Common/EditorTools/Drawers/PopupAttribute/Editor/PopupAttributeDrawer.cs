using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
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
	
	[CustomPropertyDrawer(typeof(PopupAttribute))]
	public class PopupAttributeDrawer : CastedPropertyDrawer<PopupAttribute>
	{
		protected override List<SerializedPropertyType> validTypes { get { return new List<SerializedPropertyType> (new SerializedPropertyType[] { SerializedPropertyType.Integer,  SerializedPropertyType.String }); } }

	    private Action<int> setValue;
	    private Func<int, int> validateValue;
	    private string[] _list = null;

	    private string[] list
	    {
	        get
	        {
	            if (_list == null)
	            {
	                _list = new string[cAttribute.list.Length];
					for (int i = 0; i < cAttribute.list.Length; i++)
	                {
						_list[i] = cAttribute.list[i].ToString();
	                }
	            }
	            return _list;
	        }
	    }

	    public override void DoOnGUI(Rect position, SerializedProperty property, GUIContent label)
	    {
	        if (validateValue == null && setValue == null)
	            SetUp(property);

	        int selectedIndex = 0;

	        for (int i = 0; i < list.Length; i++)
	        {
	            selectedIndex = validateValue(i);
	            if (selectedIndex != 0)
	                break;
	        }

			GUIContent[] listContent = new GUIContent[list.Length];

			for (int i = 0; i < list.Length; i++)
			{
				listContent[i] = new GUIContent(list[i], cAttribute.tooltip);
			}

	        EditorGUI.BeginChangeCheck();
	        selectedIndex = EditorGUI.Popup(position, label, selectedIndex, listContent);
	        if (EditorGUI.EndChangeCheck())
	        {
	            setValue(selectedIndex);
	        }
	    }

	    void SetUp(SerializedProperty property)
	    {
	        if (variableType == typeof(string))
	        {

	            validateValue = (index) =>
	            {
	                return property.stringValue == list[index] ? index : 0;
	            };

	            setValue = (index) =>
	            {
	                property.stringValue = list[index];
	            };
	        }
	        else if (variableType == typeof(int))
	        {

	            validateValue = (index) =>
	            {
	                return property.intValue == Convert.ToInt32(list[index]) ? index : 0;
	            };

	            setValue = (index) =>
	            {
	                property.intValue = Convert.ToInt32(list[index]);
	            };
	        }
	        else if (variableType == typeof(float))
	        {
	            validateValue = (index) =>
	            {
	                return property.floatValue == Convert.ToSingle(list[index]) ? index : 0;
	            };
	            setValue = (index) =>
	            {
	                property.floatValue = Convert.ToSingle(list[index]);
	            };
	        }

	    }

	  	private Type variableType
	    {
	        get
	        {
	            return cAttribute.list[0].GetType();
	        }
	    }
	}
}