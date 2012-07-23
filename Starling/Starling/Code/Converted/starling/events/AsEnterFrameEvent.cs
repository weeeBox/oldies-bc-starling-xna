using System;
 
using bc.flash;
using starling.events;
 
namespace starling.events
{
	public class AsEnterFrameEvent : AsEvent
	{
		public static String ENTER_FRAME = "enterFrame";
		public AsEnterFrameEvent(String type, float passedTime, bool bubbles)
		 : base(type, bubbles, passedTime)
		{
		}
		public AsEnterFrameEvent(String type, float passedTime)
		 : this(type, passedTime, false)
		{
		}
		public virtual float getPassedTime()
		{
			return ((getData() is float) ? ((float)(getData())) : null);
		}
	}
}
