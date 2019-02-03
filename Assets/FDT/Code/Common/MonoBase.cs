using UnityEngine;
using System.Collections;

#region Header
/**	MonoBase
 *  Monobehavior base class with quit detection and slot methods to register and unregister events.
**/
#endregion
namespace com.FDT.Common
{
	public class MonoBase : MonoBehaviour
	{

		private static bool _isQuitting = false;

		public static bool isQuitting { get { return _isQuitting; } }

		private bool DidOnDisableOrDestroyNotQuit = false;

		protected virtual void Reset()
		{
		}

		protected virtual void OnEnable()
		{

		}

		protected virtual void Start()
		{

		}

		protected virtual void CacheData()
		{
		}

		protected virtual void Awake()
		{
			DidOnDisableOrDestroyNotQuit = false;
			CacheData();
		}

		protected virtual void OnDisable()
		{
			if (!isQuitting)
			{
				if (!DidOnDisableOrDestroyNotQuit)
					HandleOnDisableOrDestroyNotQuit();
				OnDisableNotQuit();
			}
		}

		protected virtual void OnDisableNotQuit()
		{
		}

		protected virtual void OnDestroy()
		{
			if (!isQuitting)
			{
				if (!DidOnDisableOrDestroyNotQuit)
					HandleOnDisableOrDestroyNotQuit();
				OnDestroyNotQuit();
			}
		}

		protected virtual void OnApplicationQuit()
		{
			_isQuitting = true;
		}

		private void HandleOnDisableOrDestroyNotQuit()
		{
			DidOnDisableOrDestroyNotQuit = true;
			OnDisableOrDestroyNotQuit();
		}

		protected virtual void OnDisableOrDestroyNotQuit()
		{
		}

		protected virtual void OnDestroyNotQuit()
		{
		}
	}
}