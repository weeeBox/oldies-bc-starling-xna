using System;
 
using bc.flash;
using bc.flash.error;
using bc.flash.events;
 
namespace bc.flash.events
{
	public class AsErrorEvent : AsEvent
	{
		public static String ERROR = "error";
		private int mId;
		public AsErrorEvent(String type, bool bubbles, bool cancelable, String text, int id)
		 : base(type, bubbles)
		{
			mId = id;
		}
		public AsErrorEvent(String type, bool bubbles, bool cancelable, String text)
		 : this(type, bubbles, cancelable, text, 0)
		{
		}
		public AsErrorEvent(String type, bool bubbles, bool cancelable)
		 : this(type, bubbles, cancelable, "", 0)
		{
		}
		public AsErrorEvent(String type, bool bubbles)
		 : this(type, bubbles, false, "", 0)
		{
		}
		public AsErrorEvent(String type)
		 : this(type, false, false, "", 0)
		{
		}
		public virtual AsEvent clone()
		{
			throw new AsNotImplementedError();
		}
		public virtual int getErrorID()
		{
			return mId;
		}
		public override String toString()
		{
			throw new AsNotImplementedError();
		}
	}
}
