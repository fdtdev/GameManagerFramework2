using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#region Header
/**
 *  Common Extensions
**/
#endregion
namespace com.FDT.Common
{
	public static class Extensions
	{
		/// <summary>
		/// Extension for Transform that sets the layer recursive.
		/// </summary>
		/// <param name="trans">Trans.</param>
		/// <param name="layer">Layer.</param>
		public static void SetLayerRecursive (this Transform trans, int layer)
		{
			trans.gameObject.layer = layer;
			foreach (Transform child in trans)
				child.SetLayerRecursive (layer);
		}
		/// <summary>
		/// Gets the type in assemblies. This is slow, use it in Editor only and with caution.
		/// </summary>
		/// <returns>The type in assemblies.</returns>
		/// <param name="typeName">Type name.</param>
		public static Type GetTypeInAssemblies (string typeName)
		{
			var type = Type.GetType (typeName);
			if (type != null)
				return type;
			foreach (var a in AppDomain.CurrentDomain.GetAssemblies()) {
				var exportedTypes = a.GetExportedTypes ();
				foreach (var exportedType in exportedTypes) {
					string ble2Name = exportedType.ToString ();
					int lio = ble2Name.LastIndexOf (".");
					string noNameSpace = string.Empty;
					if (lio != -1)
						noNameSpace = ble2Name.Substring (ble2Name.LastIndexOf (".") + 1);
					else
						noNameSpace = ble2Name.ToString ();
					if (noNameSpace == typeName)
						return exportedType;
				}

				type = a.GetType (typeName);
				if (type != null)
					return type;
			}
			return null;
		}
		public static T GetRandom<T>(this IList<T> c)
		{
			if (rnd == null)
				rnd = new System.Random((int)(Time.time*100f));
			int idx = rnd.Next(c.Count);
			return c[idx];
		}
		public static void SetRandom<T>(this IList c, int seed)
		{
			rnd = new System.Random(seed);
		}
		public static System.Random rnd;

	}
}