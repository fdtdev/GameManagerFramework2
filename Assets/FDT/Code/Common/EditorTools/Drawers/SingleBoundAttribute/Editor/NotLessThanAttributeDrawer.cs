using UnityEditor;
using UnityEngine;
using System;
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
	[CustomPropertyDrawer(typeof(NotLessThanAttribute))]
	public class NotLessThanAttributeDrawer : SingleBoundAttributeDrawer<NotLessThanAttribute>
	{
		protected override int IntGet (int a, int b)
		{
			return Mathf.Max(a,b);
		}
		protected override float FloatGet (float a, float b)
		{
			return Mathf.Max(a,b);
		}
	}
}