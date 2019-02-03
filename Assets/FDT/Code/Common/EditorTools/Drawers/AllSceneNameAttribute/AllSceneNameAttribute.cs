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
	public class AllSceneNameAttribute : PropertyAttributeBase
	{
		public int selectedValue = 0;
		public bool noPath = true;

		public AllSceneNameAttribute ()
		{
			this.noPath = true;
		}

		public AllSceneNameAttribute (bool noPath)
		{
			this.noPath = noPath;
		}
	}
}