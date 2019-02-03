using System;
using UnityEngine;

namespace com.FDT.Common
{
	[AttributeUsage( AttributeTargets.Field, Inherited = true)]
	public class RedNullAttribute : PropertyAttributeBase 
	{
		public RedNullAttribute()
		{

		}
	}
}