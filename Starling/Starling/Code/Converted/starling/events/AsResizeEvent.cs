using System;
 
using bc.flash;
using bc.flash.geom;
using starling.events;
 
namespace starling.events
{
	public class AsResizeEvent : AsEvent
	{
		public static String RESIZE = "resize";
		public AsResizeEvent(String type, int width, int height, bool bubbles)
		 : base(type, bubbles, new AsPoint(width, height))
		{
		}
		public AsResizeEvent(String type, int width, int height)
		 : this(type, width, height, false)
		{
		}
		public virtual int getWidth()
		{
			return (int)(getData() as AsPoint.x);
		}
		public virtual int getHeight()
		{
			return (int)(getData() as AsPoint.y);
		}
	}
}
