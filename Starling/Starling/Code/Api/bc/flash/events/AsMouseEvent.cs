using System;
 
using bc.flash;
using bc.flash.events;
 
namespace bc.flash.events
{
	public class AsMouseEvent : AsEvent
	{
		public static String CLICK = "click";
		public static String DOUBLE_CLICK = "doubleClick";
		public static String MOUSE_DOWN = "mouseDown";
		public static String MOUSE_MOVE = "mouseMove";
		public static String MOUSE_OUT = "mouseOut";
		public static String MOUSE_OVER = "mouseOver";
		public static String MOUSE_UP = "mouseUp";
		public static String MOUSE_WHEEL = "mouseWheel";
		public static String ROLL_OUT = "rollOut";
		public static String ROLL_OVER = "rollOver";
		private float mX;
		private float mY;
		public AsMouseEvent(String type, float x, float y, bool bubbles)
		 : base(type, bubbles)
		{
			mX = x;
			mY = y;
		}
		public AsMouseEvent(String type, float x, float y)
		 : this(type, x, y, false)
		{
		}
		public virtual float getStageX()
		{
			return mX;
		}
		public virtual float getStageY()
		{
			return mY;
		}
	}
}
