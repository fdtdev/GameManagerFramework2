using UnityEngine;
using System.Collections;
using com.FDT.Common;

#region Header
/**	DontDestroyOnLoadGameObject
 *  This component adds a DontDestroyOnLoad flag. It is used to avoid destroying a GameObject on a scene change. The  
 *  GameObject with this component will become a member of the virtual 'DontDestroyOnLoad' scene.
**/
#endregion
namespace com.FDT.Common
{
	public class DontDestroyOnLoadGameObject : MonoBase 
	{
		#region public and serialized variables
		[SerializeField, HelpBox("This component adds a DontDestroyOnLoad flag. It is used to avoid destroying a GameObject on a scene change. The GameObject with this component will become a member of the virtual 'DontDestroyOnLoad' scene.", HelpBoxType.Info, 0)]
		private bool info;
		#endregion
		#region Mono
		protected override void OnEnable ()
		{
			DontDestroyOnLoad (gameObject);
			base.OnEnable ();
		}
		#endregion
	}
}