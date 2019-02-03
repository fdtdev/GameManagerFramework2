using UnityEngine;
using System;
#region Header
/**
 *
 * original version available in https://github.com/uranuno/MyPropertyDrawers
 * 
**/
#endregion
namespace com.FDT.Common
{
	[AttributeUsage(AttributeTargets.Field, Inherited = true)]
	public class MinMaxRangeAttribute : PropertyAttributeBase {		

		public float minLimit;
		public float maxLimit;
		public string minPropertyName;
		public string maxPropertyName;
		public bool IsInt = false;
		public bool reflected = false;

		public MinMaxRangeAttribute (string minPropertyName, string maxPropertyName) {
			this.reflected =true;
			this.minPropertyName = minPropertyName;
			this.maxPropertyName = maxPropertyName;
			this.IsInt = true;
		}
		public MinMaxRangeAttribute (int minLimit, int maxLimit) {

			this.reflected = false;
			this.minLimit = minLimit;
			this.maxLimit = maxLimit;
			this.IsInt = true;
		}
		public MinMaxRangeAttribute (float minLimit, float maxLimit) {

			this.reflected = false;
			this.minLimit = minLimit;
			this.maxLimit = maxLimit;
			this.IsInt = false;
		}
	}
}