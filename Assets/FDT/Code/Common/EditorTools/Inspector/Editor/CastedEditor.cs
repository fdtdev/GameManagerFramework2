using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Reflection;
using UnityEngine.Events;

namespace com.FDT.Common
{
	public class CastedEditor<T> : Editor where T:UnityEngine.Object
	{
		protected T cTarget;
		protected virtual void OnEnable ()
		{
			cTarget = (T)target;
		}
	}
}