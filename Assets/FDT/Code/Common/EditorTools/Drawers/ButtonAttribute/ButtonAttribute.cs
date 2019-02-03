using UnityEngine;
using System.Collections;
using System;

namespace com.FDT.Common
{
	[AttributeUsage(AttributeTargets.Field, Inherited = true)]
	public class ButtonAttribute:PropertyAttributeBase
	{
		public string buttonName;
		public string method;
		public ButtonAttribute(string buttonName, string method)
		{
			this.buttonName = buttonName;
			this.method = method;
		}
	}
}