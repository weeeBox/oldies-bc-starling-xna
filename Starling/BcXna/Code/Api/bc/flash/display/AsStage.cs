using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.errors;
using bc.flash.events;
using bc.flash.geom;
 
namespace bc.flash.display
{
	public class AsStage : AsDisplayObjectContainer
	{
		private int mWidth;
		private int mHeight;
		private uint mColor;
		private String mStageQuality;
		private float mFrameRate;
		private String mAlign;
		private String mScaleMode;
		private AsRectangle mFullScreenSourceRect;
		private bool mStageFocusRect;
		private bool mTabChildren;
		private static AsStage mCurrentStage;
		private static AsEnterFrameEvent sEnterFrameEvent = new AsEnterFrameEvent(AsEvent.ENTER_FRAME, 0);
		private static AsKeyboardEvent sKeyDownEvent = new AsKeyboardEvent(AsKeyboardEvent.KEY_DOWN, 0);
		private static AsKeyboardEvent sKeyUpEvent = new AsKeyboardEvent(AsKeyboardEvent.KEY_UP, 0);
		private static AsGamePadEvent sButtonDownEvent = new AsGamePadEvent(AsGamePadEvent.BUTTON_DOWN, 0);
		private static AsGamePadEvent sButtonUpEvent = new AsGamePadEvent(AsGamePadEvent.BUTTON_UP, 0);
		private static AsGamePadEvent sGamePadConnectedEvent = new AsGamePadEvent(AsGamePadEvent.CONNECTED, 0);
		private static AsGamePadEvent sGamePadDisconnectedEvent = new AsGamePadEvent(AsGamePadEvent.DISCONNECTED, 0);
		public AsStage(int width, int height, uint color)
		{
			mWidth = width;
			mHeight = height;
			mColor = color;
			mCurrentStage = this;
		}
		public AsStage(int width, int height)
		 : this(width, height, 0)
		{
		}
		public static AsStage getInstance()
		{
			return mCurrentStage;
		}
		public virtual AsVector<AsStage3D> getStage3Ds()
		{
			throw new AsNotImplementedError();
		}
		public virtual void tick(float dt)
		{
			sEnterFrameEvent.setPassedTime(dt);
			dispatchEventOnChildren(sEnterFrameEvent);
		}
		public override AsDisplayObject hitTest(AsPoint localPoint, bool forTouch)
		{
			if(forTouch && (!getVisible() || !getTouchable()))
			{
				return null;
			}
			AsDisplayObject target = base.hitTest(localPoint, forTouch);
			if(target == null)
			{
				target = this;
			}
			return target;
		}
		public virtual AsDisplayObject hitTest(AsPoint localPoint)
		{
			return hitTest(localPoint, false);
		}
		public override void setWidth(float _value)
		{
			throw new AsIllegalOperationError("Cannot set width of stage");
		}
		public override void setHeight(float _value)
		{
			throw new AsIllegalOperationError("Cannot set height of stage");
		}
		public override void setX(float _value)
		{
			throw new AsIllegalOperationError("Cannot set x-coordinate of stage");
		}
		public override void setY(float _value)
		{
			throw new AsIllegalOperationError("Cannot set y-coordinate of stage");
		}
		public override void setScaleX(float _value)
		{
			throw new AsIllegalOperationError("Cannot scale stage");
		}
		public override void setScaleY(float _value)
		{
			throw new AsIllegalOperationError("Cannot scale stage");
		}
		public override void setRotation(float _value)
		{
			throw new AsIllegalOperationError("Cannot rotate stage");
		}
		public virtual uint getColor()
		{
			return mColor;
		}
		public virtual void setColor(uint _value)
		{
			mColor = _value;
		}
		public virtual int getStageWidth()
		{
			return mWidth;
		}
		public virtual void setStageWidth(int _value)
		{
			mWidth = _value;
		}
		public virtual int getStageHeight()
		{
			return mHeight;
		}
		public virtual void setStageHeight(int _value)
		{
			mHeight = _value;
		}
		public virtual String getQuality()
		{
			return mStageQuality;
		}
		public virtual void setQuality(String _value)
		{
			mStageQuality = _value;
		}
		public virtual float getFrameRate()
		{
			return mFrameRate;
		}
		public virtual void setFrameRate(float _value)
		{
			mFrameRate = _value;
		}
		public virtual String getAlign()
		{
			return mAlign;
		}
		public virtual void setAlign(String _value)
		{
			mAlign = _value;
		}
		public virtual String getScaleMode()
		{
			return mScaleMode;
		}
		public virtual void setScaleMode(String _value)
		{
			mScaleMode = _value;
		}
		public virtual AsRectangle getFullScreenSourceRect()
		{
			return mFullScreenSourceRect;
		}
		public virtual void setFullScreenSourceRect(AsRectangle _value)
		{
			mFullScreenSourceRect = _value;
		}
		public virtual bool getStageFocusRect()
		{
			return mStageFocusRect;
		}
		public virtual void setStageFocusRect(bool on)
		{
			mStageFocusRect = on;
		}
		public virtual bool getTabChildren()
		{
			return mTabChildren;
		}
		public virtual void setTabChildren(bool _value)
		{
			mTabChildren = _value;
		}
		public virtual void keyPressed(uint code)
		{
			sKeyDownEvent.update(code);
			dispatchEvent(sKeyDownEvent);
		}
		public virtual void keyReleased(uint code)
		{
			sKeyUpEvent.update(code);
			dispatchEvent(sKeyUpEvent);
		}
		public virtual void buttonPressed(uint playerIndex, uint code)
		{
			sButtonDownEvent.update(playerIndex, code);
			dispatchEvent(sButtonDownEvent);
		}
		public virtual void buttonReleased(uint playerIndex, uint code)
		{
			sButtonUpEvent.update(playerIndex, code);
			dispatchEvent(sButtonUpEvent);
		}
		public virtual void gamePadConnected(uint playerIndex)
		{
			sGamePadConnectedEvent.update(playerIndex);
			dispatchEvent(sGamePadConnectedEvent);
		}
		public virtual void gamePadDisconnected(uint playerIndex)
		{
			sGamePadDisconnectedEvent.update(playerIndex);
			dispatchEvent(sGamePadDisconnectedEvent);
		}
		public virtual void touchDown(float x, float y, int touchId)
		{
			dispatchEvent(new AsMouseEvent(AsMouseEvent.MOUSE_DOWN, x, y));
		}
		public virtual void touchMove(float x, float y, int touchId)
		{
			dispatchEvent(new AsMouseEvent(AsMouseEvent.MOUSE_MOVE, x, y));
		}
		public virtual void touchDragged(float x, float y, int touchId)
		{
			dispatchEvent(new AsMouseEvent(AsMouseEvent.MOUSE_MOVE, x, y));
		}
		public virtual void touchUp(float x, float y, int touchId)
		{
			dispatchEvent(new AsMouseEvent(AsMouseEvent.MOUSE_UP, x, y));
		}
	}
}
