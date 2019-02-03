using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#region Header
/**	SingletonHandler
 *  This Singleton Component is made for Handlers like GameManager or DirectorsHandler. It tracks their defined item 
 *  objects and gets a list of them to access on runtime. 
**/
#endregion

namespace com.FDT.Common
{
	public class SingletonHandler<T, T2> : UnitySingleton<T> where T2:IManagerHandled where T:UnitySingleton<T>
	{
		public List<T2> items = new List<T2> ();
		public Dictionary<string, T2> itemByName = new Dictionary<string, T2> ();

		public void Register (T2 item)
		{
			items.Add (item);
			itemByName [item.HandleName ()] = item;
		}

		public void Unregister (T2 item)
		{
			items.Remove (item);
			itemByName.Remove (item.HandleName ());
		}
	}
}
