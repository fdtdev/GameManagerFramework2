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
	public class NotLessThanAttribute : SingleBoundAttribute
	{
	    public NotLessThanAttribute(int lowerBound) { IntBound = lowerBound; }
	    public NotLessThanAttribute(float lowerBound) { FloatBound = lowerBound; }
	}
}