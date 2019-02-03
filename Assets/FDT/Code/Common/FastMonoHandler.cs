using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.FDT.Common;
using System;

#region Header
/**	MonoBase
 *  FastMonoHandler is required to register the FastMono instances and make the function calls
**/
#endregion
namespace com.FDT.Common
{
	[UnitySingleton (UnitySingletonAttribute.Type.CreateOnNewGameObject, true)]
	public class FastMonoHandler : UnitySingleton<FastMonoHandler>
	{
		public LinkedList<FastMono> activeMonos = new LinkedList<FastMono> ();
		// Use this for initialization

		protected virtual void FixedUpdate ()
		{
			ExecuteAll (DoFixedUpdate);
		}

		protected void ExecuteAll (Action<FastMono> callback)
		{
			var f = activeMonos.First;
			if (f != null) {
				do {
					if (f == null || f.Value == null)
						Debug.LogError ("Invalid object call");
					callback (f.Value);
					f = f.Next;
				} while(f != null);
			}
		}

		protected virtual void Update ()
		{
			ExecuteAll (DoUpdate);
		}

		protected virtual void LateUpdate ()
		{
			ExecuteAll (DoLateUpdate);
		}

		private void DoUpdate (FastMono e)
		{
			e.DoUpdate ();
		}

		private void DoLateUpdate (FastMono e)
		{
			e.DoLateUpdate ();
		}

		private void DoFixedUpdate (FastMono e)
		{
			e.DoFixedUpdate ();
		}

		public void Add (FastMono e)
		{
			activeMonos.AddLast (e);
		}

		public void Remove (FastMono e)
		{
			activeMonos.Remove (e);
		}
	}
}