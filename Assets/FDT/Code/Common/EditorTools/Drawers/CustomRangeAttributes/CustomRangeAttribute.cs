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
	public class CustomRangeAttribute : PropertyAttributeBase
	{
	    public float min;
	    public float max;
		public bool reflected = false;
		public bool IsInt = false;
		public string minPropertyName;
		public string maxPropertyName;

	    public CustomRangeAttribute(int min, int max)
		{
			reflected = false;
	        this.min = min;
	        this.max = max;
			this.IsInt = true;
	    }
		public CustomRangeAttribute(string minPropertyName, string maxPropertyName)
		{
			reflected = true;
			this.minPropertyName = minPropertyName; 
			this.maxPropertyName = maxPropertyName;
			this.IsInt = false;
		}
		public CustomRangeAttribute(float min, float max)
		{
			reflected = false;
			this.min = min;
			this.max = max;
			this.IsInt = false;
		}
	}
}