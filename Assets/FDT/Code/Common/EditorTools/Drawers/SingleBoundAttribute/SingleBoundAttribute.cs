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
	public abstract class SingleBoundAttribute : PropertyAttributeBase
	{
	    public int IntBound
	    { 
	        get
	        {
	            if (FixedType != typeof(int))
	            {
	                throw new UnityException(FixedType.ToString() 
	                                         + " is set, asked for int in " + GetType());
	            }
	            return m_intBound;
	        }
	        protected set
	        {
	            FixedType = typeof(int);
	            m_intBound = value;
	        }
	    }

	    public float FloatBound
	    { 
	        get
	        {
	            if (FixedType != typeof(float))
	            {
	                throw new UnityException(FixedType.ToString() 
	                                         + " is set, asked for float in " + GetType());
	            }
	            return m_floatBound;
	        }
	        protected set
	        {
	            FixedType = typeof(float);
	            m_floatBound = value;
	        }
	    }

	    private Type FixedType { get; set; }
	    private float m_floatBound;
	    private int m_intBound;

	}
}