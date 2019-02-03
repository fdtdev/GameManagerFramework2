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
	public class BHeaderAttribute : PropertyAttributeBase
	{
			public string headerText;
			public string text;

			public BHeaderAttribute (string header)
			{
					headerText = header;
			}
			public BHeaderAttribute (string header, string text)
			{
					headerText = header;
					this.text = text;
			}
	}
}