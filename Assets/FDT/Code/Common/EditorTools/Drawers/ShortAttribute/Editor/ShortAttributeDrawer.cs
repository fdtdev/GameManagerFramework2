using UnityEditor;
using UnityEngine;
#region Header
/**
 *
 * original version available in https://github.com/anchan828/property-drawer-collection
 * 
**/
#endregion 
namespace com.FDT.Common
{
	[CustomPropertyDrawer(typeof(ShortAttribute))]
	public class ShortAttributeDrawer : CastedPropertyDrawer<ShortAttribute>
	{
		protected const string sVector4f = "Vector4f";
		protected const string sVector4 = "Vector4";
		protected const string sQuaternion = "Quaternion";
		protected const string sQuaternionf = "Quaternionf";
		protected const string sAABB = "AABB";
		protected const string sBounds = "Bounds";
		public override void DoOnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginChangeCheck(); 
			switch (property.type)
			{
				case sVector4:
				case sVector4f:
				{
					float vx = GetProperty(property, "x").floatValue;
					float vy = GetProperty(property, "y").floatValue;
					float vz = GetProperty(property, "z").floatValue;
					float vw = GetProperty(property, "w").floatValue;
					Vector4 vector4 = EditorGUI.Vector4Field(position, label.text, new Vector4(vx, vy, vz, vw));
					if (EditorGUI.EndChangeCheck())
					{
						GetProperty(property, "x").floatValue = vector4.x;
						GetProperty(property, "y").floatValue = vector4.y;
						GetProperty(property, "z").floatValue = vector4.z;
						GetProperty(property, "w").floatValue = vector4.w;
					}
					break;
				}
				case sQuaternion:
				case sQuaternionf:
				{
					Quaternion quaternion = property.quaternionValue;
					Vector4 vector4 = EditorGUI.Vector4Field(position, label.text, new Vector4(quaternion.x, quaternion.y, quaternion.z, quaternion.w));
					if (EditorGUI.EndChangeCheck())
					{
						property.quaternionValue = new Quaternion(vector4.x, vector4.y, vector4.z, vector4.w);
					}
					break;
				}
				case sBounds:
				case sAABB:
				{
					Bounds bounds = EditorGUI.BoundsField(position, label, property.boundsValue);
					if (EditorGUI.EndChangeCheck())
					{
						property.boundsValue = bounds;
					}
					break;
				}
				default:
				{
					EditorGUI.LabelField(position, label.text, "Type not implemented");
					EditorGUI.EndChangeCheck();
					break;
				}
			}
			
		}
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) + GetHeight(property);
		}
		
		private const float LineHeightSimple = 16f;
		
		private static float GetHeight(SerializedProperty property)
		{
			float height = 0;
			switch (property.type)
			{
			case sVector4:
			case sVector4f:
			case sQuaternion:
			case sQuaternionf:
				height = LineHeightSimple;
				break;
			case sBounds:
			case sAABB:
				height = LineHeightSimple * 2;
				break;
			default:
				break;
			}
			return height;
		}
		
		private static SerializedProperty GetProperty(SerializedProperty property, string name)
		{
			return property.FindPropertyRelative(name);
		}
	}
}