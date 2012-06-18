using System;
 
using bc.flash;
using bc.flash.events;
 
namespace bc.flash.events
{
	public class AsIOErrorEvent : AsEvent
	{
		public static String IO_ERROR = "ioError";
		public static String NETWORK_ERROR = "networkError";
		public static String DISK_ERROR = "diskError";
		public static String VERIFY_ERROR = "verifyError";
		public AsIOErrorEvent(String type, bool bubbles)
		 : base(type, bubbles)
		{
		}
		public AsIOErrorEvent(String type)
		 : this(type, false)
		{
		}
	}
}
