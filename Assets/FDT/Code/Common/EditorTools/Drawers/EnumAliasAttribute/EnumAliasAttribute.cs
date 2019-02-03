using System;
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
	[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, Inherited = true)]
	public class EnumAliasAttribute : PropertyAttributeBase
	{
		public EnumAliasAttribute()
		{
		}
	    public EnumAliasAttribute(string label)
	    {
	        this.label = label;
	    }
	}
}