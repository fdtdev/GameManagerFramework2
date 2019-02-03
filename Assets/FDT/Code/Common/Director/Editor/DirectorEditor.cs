using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

namespace com.FDT.Common
{
    [CustomEditor(typeof(Director), true)]
	public class DirectorEditor : CastedEditor<Director>
    {
        ReorderableList assetsList;
        SerializedProperty runningProperty;
		SerializedProperty EditableProperty;

		protected override void OnEnable ()
		{
			base.OnEnable ();
		
            runningProperty = serializedObject.FindProperty("running");
			EditableProperty = serializedObject.FindProperty("editable");

			CreateList();
		}
		protected void CreateList()
		{
            assetsList = new ReorderableList(serializedObject, 
                serializedObject.FindProperty("items"), 
                true, true, true, true);
            assetsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = assetsList.serializedProperty.GetArrayElementAtIndex(index);
                Color oldcolor = GUI.backgroundColor;
                if (runningProperty.boolValue && serializedObject.FindProperty("index").intValue == index)
                {
                    GUI.backgroundColor = Color.cyan;
                }
                EditorGUI.PropertyField(rect, element, GUIContent.none);
                GUI.backgroundColor = oldcolor;

            };
			
            assetsList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Director Items");
            };
            assetsList.onRemoveCallback = (ReorderableList l) =>
            {
                if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete this item?", "Yes", "No"))
                {
                    ReorderableList.defaultBehaviours.DoRemoveButton(l);
                }
            };
			assetsList.elementHeightCallback = ( int idx) =>
			{
				return EditorGUIUtility.singleLineHeight;
			};
			assetsList.onAddCallback = (ReorderableList l) => {
				var index = l.serializedProperty.arraySize;
				l.serializedProperty.arraySize++;
				l.index = index;
				var element = assetsList.serializedProperty.GetArrayElementAtIndex(index);
				SerializedProperty gP = element.FindPropertyRelative("Guid");
				gP.intValue = GetHashCode();
				serializedObject.ApplyModifiedProperties ();
			};
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Space(20);

            EditorGUILayout.HelpBox("This component provides a list of actions to do in order.\nIt requires a DirectorManager Singleton object to function, and creates one if there's none.", MessageType.Info);

            GUILayout.Space(10);

			SerializedProperty autoStartProperty = serializedObject.FindProperty("autoStart");
			if (EditableProperty.boolValue)
				EditorGUILayout.PropertyField(autoStartProperty);
			else
				EditorGUILayout.LabelField(new GUIContent("Auto Start:    " + (autoStartProperty.boolValue?"Enabled":"Disabled") , "AutoStart option in FixedDirector instances is read only"));

			GUILayout.Space(10);
			if (EditableProperty.boolValue)
            	EditorGUILayout.PropertyField(serializedObject.FindProperty("directorName"));
			else
				EditorGUILayout.LabelField(new GUIContent("Director Name:  " + serializedObject.FindProperty("directorName").stringValue, "Director name in FixedDirector instances is read only"));

            GUILayout.Space(10);

            assetsList.DoLayoutList();

			if (assetsList.index!= -1)
			{
				SerializedProperty element = assetsList.serializedProperty.GetArrayElementAtIndex(assetsList.index);
				int events = Mathf.Max(0, element.FindPropertyRelative ("callback").FindPropertyRelative ("m_PersistentCalls").FindPropertyRelative ("m_Calls").arraySize-1);
				float h =  140f + (events*43);
				Rect r = EditorTools.GetInspectorRect(h);
				DrawSelected(r, element, null);
			}

			EditorGUI.BeginChangeCheck();
			EditableProperty.boolValue = GUILayout.Toggle ( EditableProperty.boolValue,"Editable", "Button");
			if (EditorGUI.EndChangeCheck())
				CreateList();
            serializedObject.ApplyModifiedProperties();
        }

		public void DrawSelected (Rect position, SerializedProperty property, GUIContent label)
		{

			var activeProperty = property.FindPropertyRelative ("active");
			var typeProperty = property.FindPropertyRelative ("type");
			var callbackProperty = property.FindPropertyRelative ("callback");
			var subItemsProperty = property.FindPropertyRelative ("subItems");
			var wafterProperty = property.FindPropertyRelative ("waitAfter");
			var wbeforeProperty = property.FindPropertyRelative ("waitBefore");
			var wafterBoolProperty = property.FindPropertyRelative ("waitingAfter");
			var wbeforeBoolProperty = property.FindPropertyRelative ("waitingBefore");
			var autoCompleteProperty = property.FindPropertyRelative("autoComplete");

			Color oldColor = GUI.backgroundColor;
			if (!activeProperty.boolValue)
				GUI.backgroundColor = Color.gray;

			EditorGUI.BeginProperty (position, label, property);
			GUI.Box (position, string.Empty);
			float mid = position.width / 2;
			float quarter = position.width / 4;
			GUIContent emptyContent = new GUIContent (string.Empty);

			EditorGUI.LabelField (new Rect (position.x, position.y + 2, quarter, 16f), new GUIContent( "Active:", "Inactive items will be ignored."));

			EditorGUI.PropertyField (new Rect (position.x + quarter, position.y + 2, quarter, 16f), activeProperty, emptyContent);
			EditorGUI.LabelField (new Rect (position.x + mid, position.y + 2, quarter, 16f),  new GUIContent( "Type:", "Single mode only wait for a completion message to step to the next Item. Multi mode waits for a number of individual completion messages to be delivered to step to the next Item."));
			EditorGUI.PropertyField (new Rect (position.x + (quarter * 3), position.y + 2, quarter - 20f, 16f), typeProperty, emptyContent);

			int stateStartCount = Mathf.Max(0, callbackProperty.FindPropertyRelative ("m_PersistentCalls").FindPropertyRelative ("m_Calls").arraySize-1);
			Rect r = new Rect (position.x, position.y + 20f, position.width - 3f, 90f + (stateStartCount*43));
			GUI.Box(r, string.Empty);
			EditorGUI.PropertyField(r, callbackProperty);

			if (typeProperty.enumValueIndex == 1)
			{
				EditorGUI.LabelField (new Rect (position.x + 5, position.y + 115f + (stateStartCount*43), 75f, 16f), new GUIContent( "SubItems: ", "Number of individual completion messages to wait until step to the next Item."));
				EditorGUI.PropertyField (new Rect (position.x + 75f, position.y + 115f + (stateStartCount*43), 35f, 16f), subItemsProperty, emptyContent);
			}
			else
			{
				EditorGUI.LabelField (new Rect (position.x + 5, position.y + 115f + (stateStartCount*43), 110f, 16f), new GUIContent( "AutoComplete: ", "Enabled to not wait for a complete operation."));
				EditorGUI.PropertyField (new Rect (position.x + 100f, position.y + 115f + (stateStartCount*43), 35f, 16f), autoCompleteProperty, emptyContent);
			}
			if (wbeforeBoolProperty.boolValue)
			{
				GUI.backgroundColor = Color.green;
			}
			EditorGUI.LabelField (new Rect (position.x + position.width - 230f, position.y + 115f + (stateStartCount*43), 70f, 16f),  new GUIContent("Wait Before: ", "Seconds to wait between the activation of the step and the execution of the UnityEvent designed."));
			EditorGUI.PropertyField (new Rect (position.x + position.width - 160f, position.y + 115f + (stateStartCount*43), 35f, 16f), wbeforeProperty, emptyContent);
			GUI.backgroundColor = oldColor;
			if (wafterBoolProperty.boolValue)
			{
				GUI.backgroundColor = Color.green;
			}
			EditorGUI.LabelField (new Rect (position.x + position.width - 105f, position.y + 115f + (stateStartCount*43), 70f, 16f), new GUIContent("Wait After: ", "Seconds to wait after the execution of the UnityEvent assigned to this Item before stepping to the next."));
			EditorGUI.PropertyField (new Rect (position.x + position.width - 35f, position.y + 115f + (stateStartCount*43), 35f, 16f), wafterProperty, emptyContent);
			GUI.backgroundColor = oldColor;

			if (GUI.Button(new Rect(position.x + (position.width-20), position.y+2, 20, EditorGUIUtility.singleLineHeight), "X"))
			{
				assetsList.index = -1;
				serializedObject.ApplyModifiedProperties();
			}
		}
    }
}