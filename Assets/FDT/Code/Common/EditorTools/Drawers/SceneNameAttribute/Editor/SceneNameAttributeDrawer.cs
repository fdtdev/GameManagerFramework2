using UnityEngine;
using UnityEditor;
using System.Linq;
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
	[CustomPropertyDrawer(typeof(SceneNameAttribute))]
	public class SceneNameAttributeDrawer : CastedPropertyDrawer<SceneNameAttribute>
	{
		protected override List<SerializedPropertyType> validTypes { get { return new List<SerializedPropertyType> (new SerializedPropertyType[] { SerializedPropertyType.String }); } }

	    public override void DoOnGUI(Rect position, SerializedProperty property, GUIContent label)
	    {
			string[] sceneNames = AllSceneNameHandler.sceneNamesBuild;

	        if (sceneNames.Length == 0)
	        {
	            EditorGUI.LabelField(position, ObjectNames.NicifyVariableName(property.name), "Scene is Empty");
	            return;
	        }

	        int[] sceneNumbers = new int[sceneNames.Length];

	        SetSceneNumbers(sceneNumbers, sceneNames);

	        if (!string.IsNullOrEmpty(property.stringValue))
	            cAttribute.selectedValue = GetIndex(sceneNames, property.stringValue);
			
			GUIContent[] sceneNamesC = new GUIContent[sceneNames.Length];

			var nameByPath = AllSceneNameHandler.nameByPath;

			for (int i = 0; i < sceneNames.Length; i++)
			{
				if (cAttribute.noPath)
				{					
					sceneNamesC[i] = new GUIContent(nameByPath[AllSceneNameHandler.RemoveUnity(sceneNames[i])], cAttribute.tooltip);
				}
				else 
				{					
					sceneNamesC[i] = new GUIContent(sceneNames[i], cAttribute.tooltip);
				}
			}

			cAttribute.selectedValue = EditorGUI.IntPopup(position, label, cAttribute.selectedValue, sceneNamesC, sceneNumbers);

			property.stringValue = sceneNames[cAttribute.selectedValue];
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