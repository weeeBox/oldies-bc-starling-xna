using System;
 
using bc.flash;
using bc.flash.errors;
using bc.flash.geom;
using starling.display;
using starling.events;
 
namespace starling.display
{
	public class AsStage : AsDisplayObjectContainer
	{
		private int mWidth;
		private int mHeight;
		private uint mColor;
		private AsEnterFrameEvent mEnterFrameEvent = new AsEnterFrameEvent(AsEvent.ENTER_FRAME, 0.0f);
		public AsStage(int width, int height, uint color)
		{
			mWidth = width;
			mHeight = height;
			mColor = color;
		}
		public AsStage(int width, int height)
		 : this(width, height, 0)
		{
		}
		public virtual void advanceTime(float passedTime)
		{
			mEnterFrameEvent.reset(AsEvent.ENTER_FRAME, false, passedTime);
			broadcastEvent(mEnterFrameEvent);
		}
		public override AsDisplayObject hitTest(AsPoint localPoint, bool forTouch)
		{
			if(forTouch && (!getVisible() || !getTouchable()))
			{
				return null;
			}
			if(localPoint.x < 0 || localPoint.x > mWidth || localPoint.y < 0 || localPoint.y > mHeight)
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
	}
}
