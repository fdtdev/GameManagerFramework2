using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System;

#region Header
/**
 *
 * original version available in https://github.com/anchan828/property-drawer-collection
 * 
**/
#endregion
namespace com.FDT.Common
{
	[CustomPropertyDrawer(typeof(AllSceneNameAttribute))]
	public class AllSceneNameAttributeDrawer : CastedPropertyDrawer<AllSceneNameAttribute>
	{
		protected override List<SerializedPropertyType> validTypes { get { return new List<SerializedPropertyType> (new SerializedPropertyType[] { SerializedPropertyType.String }); } }

		public static string[] sceneNames;
		public static Dictionary<string,string> pathByName = new Dictionary<string, string>();
		public static Dictionary<string,string> nameByPath = new Dictionary<string, string>();

		public override void DoOnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			sceneNames = AllSceneNameHandler.sceneNames;
			pathByName = AllSceneNameHandler.pathByName;
			nameByPath = AllSceneNameHandler.nameByPath;

			if (sceneNames == null || sceneNames.Length == 0)
			{
				EditorGUI.LabelField(position, ObjectNames.NicifyVariableName(property.name), "Scene is Empty");
				return;
			}

			int[] sceneNumbers = new int[sceneNames.Length];

			SetSceneNumbers(sceneNumbers, sceneNames);

			if (!string.IsNullOrEmpty(property.stringValue))
				cAttribute.selectedValue = GetIndex(sceneNames, property.stringValue);

			GUIContent[] sceneNamesC = new GUIContent[sceneNames.Length];

			for (int i = 0; i < sceneNames.Length; i++)
			{
				sceneNamesC[i] = new GUIContent(sceneNames[i], cAttribute.tooltip);
			}
			EditorGUI.BeginChangeCheck();
			cAttribute.selectedValue = EditorGUI.IntPopup(position, label, cAttribute.selectedValue, sceneNamesC, sceneNumbers);
			if (EditorGUI.EndChangeCheck())
			{
				if (cAttribute.noPath)
				{
					property.stringValue = sceneNames[cAttribute.selectedValue];
				}
				else
				{
					property.stringValue = pathByName[sceneNames[cAttribute.selectedValue]];
				}
				property.serializedObject.ApplyModifiedProperties();
			}
		}
		void SetSceneNumbers(int[] sceneNumbers, string[] sceneNames)
		{
			for (int i = 0; i < sceneNames.Length; i++)
			{
				sceneNumbers[i] = i;
			}
		}
		int GetIndex(string[] sceneNames, string sceneName)
		{
			if (!cAttribute.noPath)
			{
				if (nameByPath.ContainsKey(sceneName))
					sceneName = nameByPath[sceneName];
				else
				{
					return -1;
				}
			}
			int result = 0;
			for (int i = 0; i < sceneNames.Length; i++)
			{
				if (sceneName == sceneNames[i])
				{
					result = i;
					break;
				}
			}
			return result;
		}

	}
}