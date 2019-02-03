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
	public class NotMoreThanAttribute : SingleBoundAttribute
	{
	    public NotMoreThanAttribute(int upperBound) { IntBound = upperBound; }
	    public NotMoreThanAttribute(float upperBound) { FloatBound = upperBound; }
	}
}