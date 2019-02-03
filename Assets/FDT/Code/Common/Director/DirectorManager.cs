using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.FDT.Common;

#region Header
/**	DirectorManager
 *  This component is automatically created when using one or more Director components.
**/
#endregion
namespace com.FDT.Common
{
	[UnitySingleton (UnitySingletonAttribute.Type.CreateOnNewGameObject, true)]
	public class DirectorManager : SingletonHandler<DirectorManager, Director>
	{
		public void ItemCompleted (string directorName, int identifier)
		{
			itemByName [directorName].ItemCompleted ((directorName.GetHashCode () + identifier.GetHashCode ()) * 7);
		}

		public void StartDirector (string directorName)
		{
			itemByName [directorName].Begin ();
		}
	}
}