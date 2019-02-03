using UnityEngine;
using System;
namespace com.FDT.Common
{
	[AttributeUsage(AttributeTargets.Field, Inherited = true)]
	public class LabelAliasAttribute : PropertyAttributeBase
	{
		public LabelAliasAttribute()
		{
			this.label = null;
		}
		public LabelAliasAttribute(string label)
		{
			this.label = label;
		}
	}
}