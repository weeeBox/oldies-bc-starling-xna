using System;
 
using bc.flash.display;
using bc.flash.events;
using bc.flash.ui;
 
namespace bc.flash.display
{
	public class AsSprite : AsDisplayObjectContainer
	{
		private AsGraphics mGraphics;
		private bool mUseHandCursor;
		private bool mMouseChildren;
		public AsSprite()
		 : base()
		{
			mGraphics = new AsGraphics();
		}
		public override void dispose()
		{
			base.dispose();
		}
		public virtual bool getUseHandCursor()
		{
			return mUseHandCursor;
		}
		public virtual void setUseHandCursor(bool _value)
		{
			if((_value == mUseHandCursor))
			{
				return;
			}
			mUseHandCursor = _value;
			if(mUseHandCursor)
			{
				addEventListener(AsTouchEvent.TOUCH, onTouch);
			}
			else
			{
				removeEventListener(AsTouchEvent.TOUCH, onTouch);
			}
		}
		public virtual AsGraphics getGraphics()
		{
			return mGraphics;
		}
		private void onTouch(AsEvent evt)
		{
			AsTouchEvent _event = ((AsTouchEvent)(evt));
			AsMouse.setCursor(((_event.interactsWith(this)) ? (AsMouseCursor.BUTTON) : (AsMouseCursor.AUTO)));
		}
		public virtual bool getMouseChildren()
		{
			return mMouseChildren;
		}
		public virtual void setMouseChildren(bool enable)
		{
			mMouseChildren = enable;
		}
		public virtual bool getTabChildren()
		{
			return false;
		}
		public virtual void setTabChildren(bool enable)
		{
		}
	}
}
