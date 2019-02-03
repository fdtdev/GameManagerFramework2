using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;

namespace com.FDT.Common
{
	public class SingletonHandlerEditorBase<T, T2> : FoldedEditor<T> where T:SingletonHandler<T, T2> where T2:IManagerHandled
	{
		protected SerializedProperty itemsProperty;
		protected List<T2> items;

		protected virtual string ItemsListTitle { get { return null; }}

		protected override List<string> dontIncludeMe {
			get {
				List<string> result = base.dontIncludeMe;
				result.AddRange (new string[] {
					"items"
				});
				return result;
			}
		}

		protected override void OnEnable ()
		{
			base.OnEnable ();
			itemsProperty = serializedObject.FindProperty ("items");
		}

		protected virtual string helpBoxContent { get { return string.Empty; } }

		protected override void DrawCustomEditor ()
		{
			base.DrawCustomEditor ();

			GUILayout.Space (20);

			EditorGUILayout.HelpBox (helpBoxContent, MessageType.Info);

			GUILayout.Space (10);

			int l = cTarget.items.Count;
			Color oldColor = GUI.backgroundColor;

			if (Application.isPlaying)
			{
				if (!string.IsNullOrEmpty( ItemsListTitle))
				{
					EditorGUILayout.LabelField(ItemsListTitle);
				}
				for (int i = 0; i < l; i++) {
					T2 item = cTarget.items[i];				
					Rect r = GUILayoutUtility.GetRect (250, 250, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
					DrawItem (r, item, oldColor);
				}
			}
			GUI.backgroundColor = oldColor;
		}

		protected virtual void DrawCustomInspector ()
		{
            
		}

		protected virtual void DrawItem (Rect r, T2 item, Color oldColor)
		{
           
		}
	}
}