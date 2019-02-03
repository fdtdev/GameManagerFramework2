using UnityEngine;
using System.Collections;
using UnityEditor;

namespace com.FDT.Common
{
	public class CastedDecoratorDrawer<T> : DecoratorDrawer where T:PropertyAttributeBase
	{
		protected T cAttribute
		{
			get
			{
				return (T)attribute;
			}
		}
	}
}