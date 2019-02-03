using UnityEngine;
using UnityEditor;
using System.IO; 
using System.Collections;
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
	[CustomPropertyDrawer(typeof(PreviewTextureAttribute))]
	public class PreviewTextureAttributeDrawer : CastedPropertyDrawer<PreviewTextureAttribute>
	{
		protected override List<SerializedPropertyType> validTypes { get { return new List<SerializedPropertyType> (new SerializedPropertyType[] { SerializedPropertyType.ObjectReference,SerializedPropertyType.String }); } }

	    public override void DoOnGUI(Rect position, SerializedProperty property, GUIContent label)
	    {			
			EditorGUI.BeginProperty(position, label, property);
	        position.height = 16;
	        if (property.propertyType == SerializedPropertyType.String)
	        {
	            DrawStringValue(position, property, label);
	        }
	        else if (property.propertyType == SerializedPropertyType.ObjectReference)
	        {
	            DrawTextureValue(position, property, label);
	        }
			EditorGUI.EndProperty();
	    }

	    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	    {
	        return base.GetPropertyHeight(property, label) + cAttribute.lastPosition.height;
	    }

	    void DrawTextureValue(Rect position, SerializedProperty property, GUIContent label)
	    {
	        property.objectReferenceValue = (Texture2D)EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(Texture2D), false);

	        if (property.objectReferenceValue != null)
	            DrawTexture(position, (Texture2D)property.objectReferenceValue);
	    }

	    void DrawStringValue(Rect position, SerializedProperty property, GUIContent label)
	    {
	        EditorGUI.BeginChangeCheck();
	        property.stringValue = EditorGUI.TextField(position, label, property.stringValue);
	        if (EditorGUI.EndChangeCheck())
	        {
				cAttribute.www = null;
				cAttribute.cached = null;
	        }
	        string path = GetCachedTexturePath(property.stringValue);

	        if (!string.IsNullOrEmpty(path))
	        {
	            if (IsExpired(path))
	            {
	                Delete(path);
	            }
				else if (cAttribute.cached == null)
					cAttribute.cached = GetTextureFromCached(path);
	        }
	        else
				cAttribute.cached = null;

			if (cAttribute.cached == null)
	        {
				cAttribute.cached = GetTextureFromWWW(position, property);
	        }
	        else
				DrawTexture(position, cAttribute.cached);
	    }

	    bool IsExpired(string path)
	    {
	        string fileName = Path.GetFileNameWithoutExtension(path);
	        string[] split = fileName.Split('_');
	        return System.DateTime.Now.Ticks >= long.Parse(split[1]);
	    }

	    string GetCachedTexturePath(string stringValue)
	    {
	        int hash = stringValue.GetHashCode();
	        foreach (string path in Directory.GetFiles("Temp"))
	        {
	            if (Path.GetFileNameWithoutExtension(path).StartsWith(hash.ToString()))
	            {
	                return path;
	            }
	        }
	        return string.Empty;
	    }

	    Texture2D GetTextureFromWWW(Rect position, SerializedProperty property)
	    {
			if (cAttribute.www == null)
	        {
				cAttribute.www = new WWW(property.stringValue);
	        }
			else if (!cAttribute.www.isDone)
	        {
				cAttribute.lastPosition = new Rect(position.x, position.y + 16, position.width, 16);
				EditorGUI.ProgressBar(cAttribute.lastPosition, cAttribute.www.progress, "Downloading... " + (cAttribute.www.progress * 100) + "%");
	        }
			else if (cAttribute.www.isDone)
	        {

				if (cAttribute.www.error != null)
	                return null;

	            int hash = property.stringValue.GetHashCode();
				long expire = (System.DateTime.Now.Ticks + cAttribute.expire);
				File.WriteAllBytes(string.Format("Temp/{0}_{1}_{2}_{3}", hash, expire, cAttribute.www.texture.width, cAttribute.www.texture.height), cAttribute.www.bytes);
				return cAttribute.www.texture;
	        }
	        return null;
	    }

	    Texture2D GetTextureFromCached(string path)
	    {
	        string[] split = Path.GetFileNameWithoutExtension(path).Split('_');
	        int width = int.Parse(split[2]);
	        int height = int.Parse(split[3]);
	        Texture2D t = new Texture2D(width, height);

	        return t.LoadImage(File.ReadAllBytes(path)) ? t : null;
	    }

	    private GUIStyle style;

	    void DrawTexture(Rect position, Texture2D texture)
	    {
	        float width = Mathf.Clamp(texture.width, position.width * 0.7f, position.width * 0.7f);
			cAttribute.lastPosition = new Rect(position.width * 0.15f, position.y + 16, width, texture.height * (width / texture.width));

	        if (style == null)
	        {
	            style = new GUIStyle();
	            style.imagePosition = ImagePosition.ImageOnly;
	        }
	        style.normal.background = texture;
			GUI.Label(cAttribute.lastPosition, "", style);
	    }

	    void Delete(string path)
	    {
	        File.Delete(path);
			cAttribute.cached = null;
	    }
	}
}