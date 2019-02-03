using UnityEngine;
using System;

namespace com.FDT.Common
{
	[AttributeUsage(AttributeTargets.Field, Inherited = true)]
	public class EnumFlagsAttribute : PropertyAttributeBase
	{
		public EnumFlagsAttribute() { }
	}
}