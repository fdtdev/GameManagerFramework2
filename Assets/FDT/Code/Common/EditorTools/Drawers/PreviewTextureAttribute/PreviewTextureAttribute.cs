using UnityEngine;
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
	[AttributeUsage(AttributeTargets.Field, Inherited = true)]
	public class PreviewTextureAttribute : PropertyAttributeBase
	{
	    public Rect lastPosition = new Rect (0, 0, 0, 0);
	    public long expire = 6000000000; // 10min
	    public WWW www;
	    public Texture2D cached; 

	    public PreviewTextureAttribute ()
	    {

	    }

	    public PreviewTextureAttribute (int expire)
	    {
	        this.expire = expire * 1000 * 10000; 
	    }
	}
}