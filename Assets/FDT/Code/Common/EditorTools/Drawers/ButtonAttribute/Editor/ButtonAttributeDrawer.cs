using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Reflection;

namespace com.FDT.Common
{
	[CustomPropertyDrawer (typeof(ButtonAttribute))]
	public class ButtonAttributeDrawer : CastedPropertyDrawer<ButtonAttribute>
	{
		protected override List<SerializedPropertyType> validTypes { get { return null; } }

		[ExecuteInEditMode]
		public override void DoOnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			Rect buttonRect = new Rect (position.x+(position.width*0.67f), position.y, position.width*0.33f, EditorGUIUtility.singleLineHeight);
			if (GUI.Button (buttonRect, cAttribute.buttonName)) {
				CallMethod (property.serializedObject.targetObject, cAttribute.method);
			}
			Rect newPos = new Rect (position.x, position.y, position.width*0.67f, position.height);
			EditorGUI.PropertyField (newPos, property, label);
			EditorGUI.EndProperty();

		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight (property, label);// + 20;
		}

		[ExecuteInEditMode]
		protected void CallMethod (object o, string method)
		{
			Type type = o.GetType ();
			MethodInfo methodInfo = type.GetMethod (method);
			methodInfo.Invoke (o, null);
		}
	}
}