using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Reflection;
using UnityEngine.Events;

namespace com.FDT.Common
{
	public class FoldedEditor<T> : CastedEditor<T> where T:UnityEngine.Object
	{
		protected string unfoldedDefaultInspectorKey = string.Empty;
		protected bool unfoldedDefaultInspector = false;
		protected string unfoldedEventsKey = string.Empty;
		protected bool unfoldedEvents = false;

		protected virtual bool FoldUnityEvents { get { return false; } }

		protected virtual bool ShowDefaultInspector { get { return false; } }

		protected virtual List<string> dontIncludeMe { get { return new List<string> (new string[] { "m_Script" }); } }

		protected string[] currentDontInclude;

		protected Color oldColor;

		protected bool isPrefab { get { return _isPrefab; } }

		private bool _isPrefab = false;

		protected List <SerializedProperty> events = new List<SerializedProperty> ();
		protected List <string> eventsNames = new List<string> ();

		protected override void OnEnable ()
		{
			base.OnEnable();
			_isPrefab = (PrefabUtility.GetPrefabType (target) == PrefabType.Prefab);

			if (FoldUnityEvents && events.Count == 0) {
				var e = serializedObject.GetIterator ();
				e.Next (true);
				do {
					if (e.propertyType == SerializedPropertyType.Generic) {
						SerializedProperty ev1 = e.FindPropertyRelative ("m_PersistentCalls");
						SerializedProperty ev2 = ev1 != null ? ev1.FindPropertyRelative ("m_Calls") : null;
						if (ev1 != null && ev2 != null) 
						{	
							events.Add (serializedObject.FindProperty (e.name));
							eventsNames.Add (e.name);
						}
					}
				} while (e.Next (true));
			}

			if (currentDontInclude == null) {
				HashSet<string> s = new HashSet<string> (dontIncludeMe);
				s.Add ("m_Script");
				foreach (string evName in eventsNames)
					s.Add (evName);
				currentDontInclude = new List<string> (s).ToArray ();
			}
			unfoldedDefaultInspectorKey = serializedObject.targetObject.GetInstanceID ().ToString () + "_foldedDI";
			if (!EditorPrefs.HasKey (unfoldedDefaultInspectorKey)) {
				EditorPrefs.SetBool (unfoldedDefaultInspectorKey, false);
			}
			unfoldedDefaultInspector = EditorPrefs.GetBool (unfoldedDefaultInspectorKey);
			unfoldedEventsKey = serializedObject.targetObject.GetInstanceID ().ToString () + "_foldedEvents";
			if (!EditorPrefs.HasKey (unfoldedEventsKey)) {
				EditorPrefs.SetBool (unfoldedEventsKey, false);
			}
			unfoldedEvents = EditorPrefs.GetBool (unfoldedEventsKey);

		}

		protected virtual void DrawCustomEditor ()
		{
			
		}

		public override sealed void OnInspectorGUI ()
		{
			oldColor = GUI.backgroundColor;
			serializedObject.Update ();
			DrawCustomEditor ();
			if (FoldUnityEvents && events.Count > 0) {
				bool b2 = unfoldedEvents;
				b2 = EditorGUILayout.Foldout (b2, "Show Events");
				if (b2 != unfoldedEvents) {
					unfoldedEvents = b2;
					EditorPrefs.SetBool (unfoldedEventsKey, b2);
				}
				if (unfoldedEvents) {
					DrawUnityEvents ();
				}
			}
			if (ShowDefaultInspector) {
				bool b = unfoldedDefaultInspector;
				b = EditorGUILayout.Foldout (b, "Show Default Inspector");
				if (b != unfoldedDefaultInspector) {
					unfoldedDefaultInspector = b;
					EditorPrefs.SetBool (unfoldedDefaultInspectorKey, b);
				}
				if (unfoldedDefaultInspector) {
					DrawPropertiesExcluding (serializedObject, currentDontInclude);
				}
			}

			serializedObject.ApplyModifiedProperties ();
			GUI.backgroundColor = oldColor;
		}

		protected void DrawUnityEvents ()
		{
			foreach (SerializedProperty p in events) 
			{
				if (p!= null)
					EditorGUILayout.PropertyField (p);
			}
		}
	}
}