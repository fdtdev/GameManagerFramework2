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
	public class SceneNameAttribute : PropertyAttributeBase
	{
	    public int selectedValue = 0;
	    public bool enableOnly = true;
		public bool noPath = false;

		public SceneNameAttribute()
		{
			this.enableOnly = true;
		}
		public SceneNameAttribute(bool enableOnly)
		{
			this.enableOnly = enableOnly;
		}
	}
}