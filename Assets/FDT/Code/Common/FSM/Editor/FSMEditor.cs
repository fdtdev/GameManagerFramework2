using UnityEngine;
using UnityEditor;
using System.Collections;
using com.FDT.Common;
using UnityEditorInternal;

namespace com.FDT.Common
{
	[CustomEditor(typeof(FSM), true)]
	public class FSMEditor : CastedEditor<FSM>
	{
		ReorderableList assetsList;
		SerializedProperty AutoUpdateProperty;
		SerializedProperty AwakeEventProperty;
		SerializedProperty InitEventProperty;
		SerializedProperty EditableProperty;

		protected override void OnEnable ()
		{
			base.OnEnable ();
			AutoUpdateProperty = serializedObject.FindProperty("AutoUpdate");
			AwakeEventProperty = serializedObject.FindProperty("AwakeEvent");
			InitEventProperty = serializedObject.FindProperty("InitEvent");
			EditableProperty = serializedObject.FindProperty("editable");

			CreateList();
		}
		protected void CreateList()
		{
			assetsList = new ReorderableList(serializedObject, 
				serializedObject.FindProperty("states"), EditableProperty.boolValue, true, EditableProperty.boolValue, EditableProperty.boolValue);
			
			assetsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				Color oldcolor = GUI.backgroundColor;
				var property = assetsList.serializedProperty.GetArrayElementAtIndex(index);
				if (Application.isPlaying && cTarget.currentstateHash == property.FindPropertyRelative("hash").intValue)
				{
					GUI.backgroundColor = Color.cyan;
				}
				GUI.Box(rect, string.Empty);
				SerializedProperty stateNameProperty = property.FindPropertyRelative("stateName");
				EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), stateNameProperty.stringValue);
				GUI.backgroundColor = oldcolor;
			};

			assetsList.elementHeightCallback = (int idx) =>
			{
				return EditorGUIUtility.singleLineHeight;
			};


			assetsList.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(rect, "FSM States");
			};
			assetsList.onRemoveCallback = (ReorderableList l) =>
			{
				if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete this item?", "Yes", "No"))
				{
					ReorderableList.defaultBehaviours.DoRemoveButton(l);
				}
			};
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			GUILayout.Space(20);

			EditorGUILayout.HelpBox("This component provides a list of actions to do in order.\nIt requires a DirectorManager Singleton object to function.", MessageType.Info);

			GUILayout.Space(10);

			if (EditableProperty.boolValue)
				EditorGUILayout.PropertyField(AutoUpdateProperty);
			else
				EditorGUILayout.LabelField("Auto Update:" , AutoUpdateProperty.enumDisplayNames[AutoUpdateProperty.enumValueIndex]);
			
			GUILayout.Space(10);
			EditorGUILayout.PropertyField(AwakeEventProperty);
			EditorGUILayout.PropertyField(InitEventProperty);
			GUILayout.Space(10);

			assetsList.DoLayoutList();
			if (assetsList.index!= -1)
			{
				var property = assetsList.serializedProperty.GetArrayElementAtIndex(assetsList.index);
				int stateStartCount = Mathf.Max(0, property.FindPropertyRelative("stateStart").FindPropertyRelative("m_PersistentCalls").FindPropertyRelative("m_Calls").arraySize - 1);
				int stateEndCount = Mathf.Max(0, property.FindPropertyRelative("stateEnd").FindPropertyRelative("m_PersistentCalls").FindPropertyRelative("m_Calls").arraySize - 1);
				int stateUpdateCount = Mathf.Max(0, property.FindPropertyRelative("stateUpdate").FindPropertyRelative("m_PersistentCalls").FindPropertyRelative("m_Calls").arraySize - 1);

				float h = 300 + (stateStartCount * EditorTools.UNITYEVENT_HEIGHT) + (stateEndCount * EditorTools.UNITYEVENT_HEIGHT) + (stateUpdateCount * EditorTools.UNITYEVENT_HEIGHT);
				Rect r = EditorTools.GetInspectorRect(h);
				DrawSelected(r, property, null);
			}
			EditorGUI.BeginChangeCheck();
			EditableProperty.boolValue = GUILayout.Toggle ( EditableProperty.boolValue,"Editable", "Button");
			if (EditorGUI.EndChangeCheck())
				CreateList();
			serializedObject.ApplyModifiedProperties();
		}
		public void DrawSelected (Rect rect, SerializedProperty property, GUIContent label)
		{			
			GUI.Box(rect, string.Empty);

			SerializedProperty stateNameProperty = property.FindPropertyRelative("stateName");
			SerializedProperty stateStartProperty = property.FindPropertyRelative("stateStart");
			SerializedProperty stateUpdateProperty = property.FindPropertyRelative("stateUpdate");
			SerializedProperty stateEndProperty = property.FindPropertyRelative("stateEnd");

			int stateStartCount = Mathf.Max(0, stateStartProperty.FindPropertyRelative("m_PersistentCalls").FindPropertyRelative("m_Calls").arraySize - 1);
			int stateEndCount = Mathf.Max(0, stateEndProperty.FindPropertyRelative("m_PersistentCalls").FindPropertyRelative("m_Calls").arraySize - 1);
			int stateUpdateCount = Mathf.Max(0, stateUpdateProperty.FindPropertyRelative("m_PersistentCalls").FindPropertyRelative("m_Calls").arraySize - 1);

			float stateStartH = stateStartCount * 43;
			float stateUpdateH = stateUpdateCount * 43;

			if (EditableProperty.boolValue)
				EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), stateNameProperty);
			else
				EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), stateNameProperty.stringValue);


			Rect size = new Rect(rect.x, rect.y + 25, rect.width - 5, 86 + stateStartH);
			GUI.Box(size, string.Empty);
			EditorGUI.PropertyField(size, stateStartProperty);
			size = new Rect(rect.x, rect.y + 115 + stateStartH, rect.width - 5, 86 + stateUpdateH);
			GUI.Box(size, string.Empty);
			EditorGUI.PropertyField(size, stateUpdateProperty);
			size = new Rect(rect.x, rect.y + 205 + stateStartH + stateUpdateH, rect.width - 5, 86 + stateEndCount);
			GUI.Box(size, string.Empty);
			EditorGUI.PropertyField(size, stateEndProperty);

			if (GUI.Button(new Rect(rect.x + (rect.width-20), rect.y+2, 20, EditorGUIUtility.singleLineHeight), "X"))
			{
				assetsList.index = -1;
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}