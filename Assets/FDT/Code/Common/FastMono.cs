using UnityEngine;
using System.Collections;
using com.FDT.Common;

#region Header
/**	MonoBase
 *  Faster Monobehaviour with Update, FixedUpdate and Update calls replaced by faster c# side methods
**/
#endregion
namespace com.FDT.Common
{
	public abstract class FastMono : MonoBase
	{

		protected override void OnEnable ()
		{
			base.OnEnable ();
			FastMonoHandler.Instance.Add (this);
		}

		protected override void OnDestroyNotQuit ()
		{
			FastMonoHandler.Instance.Remove (this);
			base.OnDisable ();
		}

		protected override void OnDisableNotQuit ()
		{
			FastMonoHandler.Instance.Remove (this);
			base.OnDisableNotQuit ();

		}

		public virtual void DoUpdate ()
		{
		}

		public virtual void DoFixedUpdate ()
		{
		}

		public virtual void DoLateUpdate ()
		{
		}
	}
}