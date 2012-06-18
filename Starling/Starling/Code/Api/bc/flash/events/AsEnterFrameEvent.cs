using System;
 
using bc.flash;
using bc.flash.events;
 
namespace bc.flash.events
{
	public class AsEnterFrameEvent : AsEvent
	{
		private float mPassedTime;
		public AsEnterFrameEvent(String type, float passedTime, bool bubbles)
		 : base(type, bubbles)
		{
			mPassedTime = passedTime;
		}
		public AsEnterFrameEvent(String type, float passedTime)
		 : this(type, passedTime, false)
		{
		}
		public virtual float getPassedTime()
		{
			return mPassedTime;
		}
		public virtual void setPassedTime(float _value)
		{
			mPassedTime = _value;
		}
	}
}
