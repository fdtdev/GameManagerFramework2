using UnityEngine;
using System;
#region Header
/**
 *
 * original version available in https://github.com/uranuno/MyPropertyDrawers
 * 
**/
#endregion 
namespace com.FDT.Common
{
	[AttributeUsage(AttributeTargets.Field, Inherited = true)]
	public class TagAttribute : PropertyAttributeBase {}
}