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
	[AttributeUsage( AttributeTargets.All, Inherited = true, AllowMultiple = true )]
	public class MHeaderAttribute : PropertyAttributeBase
	{
		public string text;
		
		public MHeaderAttribute (string text)
		{
			this.text = text;
		}
	}
}