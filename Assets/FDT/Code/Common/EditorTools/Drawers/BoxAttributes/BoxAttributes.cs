using System;
using UnityEngine;
#region Header
/**
 *
 * original version available in http://baba-s.hatenablog.com/entry/2014/08/20/112256
 * 
**/
#endregion 
namespace com.FDT.Common
{
	public enum HelpBoxType
	{ 
		None,		
		Info,		
		Warning,		
		Error,
	}

	[AttributeUsage( AttributeTargets.All, Inherited = true, AllowMultiple = true )]
	public sealed class HelpBoxAttribute : PropertyAttributeBase
	{
		public string Message;
		
		public HelpBoxType Type;

		public HelpBoxAttribute( string message)
		{
			Message     = message;
			Type        = HelpBoxType.None;
			this.order  = 0;
		}
		public HelpBoxAttribute( string message, HelpBoxType type)
		{
			Message     = message;
			Type        = type;
			this.order  = 0;
		}
		public HelpBoxAttribute( string message, HelpBoxType type, int order)
		{
			Message     = message;
			Type        = type;
			this.order  = order;
		}
	}
}