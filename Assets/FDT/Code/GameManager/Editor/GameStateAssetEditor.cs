using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using com.FDT.Common;

namespace com.FDT.GameManager
{
	[CustomEditor(typeof(GameStateAsset), true)]
	public class GameStateAssetEditor : CastedEditor<GameStateAsset>
	{
		public SerializedProperty scenesProperty;

		public ReorderableList assetsList;
		private List<string> enabledScenes;
		private bool valid = false;

		protected override void OnEnable()
		{
			base.OnEnable();

			bool isPlaying = Application.isPlaying;
			bool enteringPlay = EditorApplication.isPlayingOrWillChangePlaymode;

			scenesProperty = serializedObject.FindProperty("scenes");
			if (!Application.isPlaying)
			{
				enabledScenes = EditorTools.GetScenesFromBuildSettings(true);
			}
			assetsList = new ReorderableList(serializedObject, 
				scenesProperty, 
				true, true, true, true);
			assetsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				var e = assetsList.serializedProperty.GetArrayElementAtIndex(index);
				var element = e.FindPropertyRelative("scene");
				var Async = e.FindPropertyRelative("asyncLoad");
				Color oldColor = GUI.backgroundColor;
				if ((!isPlaying && !enteringPlay) && !string.IsNullOrEmpty(element.stringValue))
				{
					string scName = EditorTools.TrimmSceneExtension(element.stringValue);
					if (!enabledScenes.Contains(scName))
					{
						GUI.backgroundColor = Color.red;
						valid = false;
					}
				}
				GUI.Label(new Rect(rect.x, rect.y, 60, rect.height), "Scene");
				EditorGUI.PropertyField(new Rect(rect.x + 60, rect.y, rect.width - 50 - 60, rect.height), element, new GUIContent(string.Empty));
				EditorGUI.BeginChangeCheck();
				Async.boolValue = GUI.Toggle(new Rect(rect.x + rect.width - 50, rect.y, 50, rect.height), Async.boolValue, "Async", "Button");
				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					serializedObject.Update();
				}
				GUI.backgroundColor = oldColor;

			};


			assetsList.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(rect, "Scenes");
			};

			assetsList.onRemoveCallback = (ReorderableList l) =>
			{
				if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete this item?", "Yes", "No"))
				{
					ReorderableList.defaultBehaviours.DoRemoveButton(l);
					serializedObject.ApplyModifiedProperties();
				}
			};
			assetsList.onAddCallback = (ReorderableList l) =>
			{
				var index = l.serializedProperty.arraySize;
				l.serializedProperty.arraySize++;
				l.index = index;
				serializedObject.ApplyModifiedProperties();
			};

		}

		public override void OnInspectorGUI()
		{
			valid = true;
			assetsList.DoLayoutList();
			if (!Application.isPlaying && !valid)
			{
				EditorGUILayout.HelpBox("Some of the selected scenes for this GameState are not added to the Build Settings.", MessageType.Error);
				if (GUILayout.Button("Add Scenes to BuildSettings"))
				{
					foreach (var sc in cTarget.scenes)
					{
						string scName = EditorTools.TrimmSceneExtension(sc.scene);
						if (!enabledScenes.Contains(scName))
						{
							EditorTools.AddSceneToBuildSettings(scName);
						}
					}				
				}
				OnEnable();

			}			
		}
	}
}