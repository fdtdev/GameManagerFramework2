using UnityEngine;
using UnityEditor;
#region Header
/**
 *
 * original version available in https://github.com/anchan828/property-drawer-collection
 * 
**/
#endregion 
namespace com.FDT.Common
{
	[CustomPropertyDrawer(typeof(ObserveAttribute))]
	public class ObserveAttributeDrawer : CastedPropertyDrawer<ObserveAttribute>
	{
		protected override System.Collections.Generic.List<SerializedPropertyType> validTypes {
			get {
				return null;
			}
		}
	    public override void DoOnGUI(Rect position, SerializedProperty property, GUIContent label)
	    {
	        EditorGUI.BeginChangeCheck();
	        EditorGUI.PropertyField(position, property, label);
	        if (EditorGUI.EndChangeCheck())
	        {
	            if (IsMonoBehaviour(property))
	            {

	                MonoBehaviour mono = (MonoBehaviour)property.serializedObject.targetObject;

	                foreach (var callbackName in cAttribute.callbackNames)
	                {
	                    mono.Invoke(callbackName, 0);
	                }

	            }
	        }
	    }

	    bool IsMonoBehaviour(SerializedProperty property)
	    {
	        return property.serializedObject.targetObject.GetType().IsSubclassOf(typeof(MonoBehaviour));
	    }
	}
}