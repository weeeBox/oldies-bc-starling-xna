using System;
 
using bc.flash;
using bc.flash.core;
using bc.flash.display;
using bc.flash.errors;
using bc.flash.events;
using bc.flash.geom;
using bc.flash.utils;
 
namespace bc.flash.display
{
	public class AsDisplayObject : AsEventDispatcher, AsIBitmapDrawable
	{
		private float mPivotX;
		private float mPivotY;
		private float mX;
		private float mY;
		private float mScaleX;
		private float mScaleY;
		private float mRotation;
		private float mAlpha;
		private bool mVisible;
		private bool mTouchable;
		private String mName;
		private float mLastTouchTimestamp;
		private AsDisplayObjectContainer mParent;
		private AsRectangle mScrollRect;
		private AsTransform mTransform;
		private static AsVector<AsDisplayObject> sAncestors = new AsVector<AsDisplayObject>();
		private static AsRectangle sHelperRect = new AsRectangle();
		private static AsPoint sHelperPoint = new AsPoint();
		private static AsMatrix sHelperMatrix = new AsMatrix();
		private static AsMatrix sTargetMatrix = new AsMatrix();
		public AsDisplayObject()
		{
			if(AsGlobal.getQualifiedClassName(this) == "starling.display::DisplayObject")
			{
				throw new AsAbstractClassError();
			}
			mX = mY = mPivotX = mPivotY = mRotation = 0.0f;
			mScaleX = mScaleY = mAlpha = 1.0f;
			mVisible = mTouchable = true;
			mLastTouchTimestamp = -1;
			mTransform = new AsTransform(this);
		}
		public virtual void dispose()
		{
			removeEventListeners();
		}
		public virtual void removeFromParent(bool dispose)
		{
			if(mParent != null)
			{
				mParent.removeChild(this);
			}
			if(dispose)
			{
				this.dispose();
			}
		}
		public virtual void removeFromParent()
		{
			removeFromParent(false);
		}
		public virtual AsRectangle getBounds(AsDisplayObject targetSpace, AsRectangle resultRect)
		{
			throw new AsAbstractMethodError("Method needs to be implemented in subclass");
		}
		public virtual AsRectangle getBounds(AsDisplayObject targetSpace)
		{
			return getBounds(targetSpace, null);
		}
		public virtual AsDisplayObject hitTest(AsPoint localPoint, bool forTouch)
		{
			if(forTouch && (!mVisible || !mTouchable))
			{
				return null;
			}
			if(getBounds(this, sHelperRect).containsPoint(localPoint))
			{
				return this;
			}
			else
			{
				return null;
			}
		}
		public virtual AsDisplayObject hitTest(AsPoint localPoint)
		{
			return hitTest(localPoint, false);
		}
		public virtual bool hitTestPoint(float x, float y, bool shapeFlag)
		{
			if(shapeFlag)
			{
				throw new AsNotImplementedError();
			}
			sHelperPoint.x = x;
			sHelperPoint.y = y;
			return hitTest(sHelperPoint, true) != null;
		}
		public virtual bool hitTestPoint(float x, float y)
		{
			return hitTestPoint(x, y, false);
		}
		public virtual AsMatrix getTransformationMatrix(AsDisplayObject targetSpace, AsMatrix resultMatrix)
		{
			if(resultMatrix != null)
			{
				resultMatrix.identity();
			}
			else
			{
				resultMatrix = new AsMatrix();
			}
			AsDisplayObject currentObject = this;
			if(targetSpace == this)
			{
				return resultMatrix;
			}
			else
			{
				if(targetSpace == mParent || (targetSpace == null && mParent == null))
				{
					if(mPivotX != 0.0f || mPivotY != 0.0f)
					{
						resultMatrix.translate(-mPivotX, -mPivotY);
					}
					if(mScaleX != 1.0f || mScaleY != 1.0f)
					{
						resultMatrix.scale(mScaleX, mScaleY);
					}
					if(mRotation != 0.0f)
					{
						resultMatrix.rotate(AsMathHelper.toRadians(mRotation));
					}
					if(mX != 0.0f || mY != 0.0f)
					{
						resultMatrix.translate(mX, mY);
					}
					return resultMatrix;
				}
				else
				{
					if(targetSpace == null)
					{
						currentObject = this;
						while(currentObject != null)
						{
							currentObject.getTransformationMatrix(currentObject.mParent, sHelperMatrix);
							resultMatrix.concat(sHelperMatrix);
							currentObject = currentObject.getParent();
						}
						return resultMatrix;
					}
					else
					{
						if(targetSpace.mParent == this)
						{
							targetSpace.getTransformationMatrix(this, resultMatrix);
							resultMatrix.invert();
							return resultMatrix;
						}
					}
				}
			}
			sAncestors.setLength(0);
			AsDisplayObject commonParent = null;
			while(currentObject != null)
			{
				sAncestors.push(currentObject);
				currentObject = currentObject.getParent();
			}
			currentObject = targetSpace;
			while(currentObject != null && sAncestors.indexOf(currentObject) == -1)
			{
				currentObject = currentObject.getParent();
			}
			if(currentObject == null)
			{
				throw new AsArgumentError("Object not connected to target");
			}
			else
			{
				commonParent = currentObject;
			}
			currentObject = this;
			while(currentObject != commonParent)
			{
				currentObject.getTransformationMatrix(currentObject.mParent, sHelperMatrix);
				resultMatrix.concat(sHelperMatrix);
				currentObject = currentObject.getParent();
			}
			sTargetMatrix.identity();
			currentObject = targetSpace;
			while(currentObject != commonParent)
			{
				currentObject.getTransformationMatrix(currentObject.mParent, sHelperMatrix);
				sTargetMatrix.concat(sHelperMatrix);
				currentObject = currentObject.getParent();
			}
			sTargetMatrix.invert();
			resultMatrix.concat(sTargetMatrix);
			return resultMatrix;
		}
		public virtual AsMatrix getTransformationMatrix(AsDisplayObject targetSpace)
		{
			return getTransformationMatrix(targetSpace, null);
		}
		public virtual AsPoint localToGlobal(AsPoint localPoint)
		{
			sTargetMatrix.identity();
			AsDisplayObject currentObject = this;
			while(currentObject != null)
			{
				currentObject.getTransformationMatrix(currentObject.mParent, sHelperMatrix);
				sTargetMatrix.concat(sHelperMatrix);
				currentObject = currentObject.getParent();
			}
			return sTargetMatrix.transformPoint(localPoint);
		}
		public virtual AsPoint globalToLocal(AsPoint globalPoint)
		{
			sTargetMatrix.identity();
			AsDisplayObject currentObject = this;
			while(currentObject != null)
			{
				currentObject.getTransformationMatrix(currentObject.mParent, sHelperMatrix);
				sTargetMatrix.concat(sHelperMatrix);
				currentObject = currentObject.getParent();
			}
			sTargetMatrix.invert();
			return sTargetMatrix.transformPoint(globalPoint);
		}
		public virtual void render(AsRenderSupport support, float alpha)
		{
			throw new AsAbstractMethodError("Method needs to be implemented in subclass");
		}
		public override void dispatchEvent(AsEvent _event)
		{
			if(_event is AsTouchEvent)
			{
				AsTouchEvent touchEvent = _event as AsTouchEvent;
				if(touchEvent.getTimestamp() == mLastTouchTimestamp)
				{
					return;
				}
				else
				{
					mLastTouchTimestamp = touchEvent.getTimestamp();
				}
			}
			base.dispatchEvent(_event);
		}
		public virtual void setParent(AsDisplayObjectContainer _value)
		{
			mParent = _value;
		}
		public virtual void dispatchEventOnChildren(AsEvent _event)
		{
			dispatchEvent(_event);
		}
		public virtual float getWidth()
		{
			return getBounds(mParent, sHelperRect).width;
		}
		public virtual void setWidth(float _value)
		{
			mScaleX = 1.0f;
			float actualWidth = getWidth();
			if(actualWidth != 0.0f)
			{
				setScaleX(_value / actualWidth);
			}
			else
			{
				setScaleX(1.0f);
			}
		}
		public virtual float getHeight()
		{
			return getBounds(mParent, sHelperRect).height;
		}
		public virtual void setHeight(float _value)
		{
			mScaleY = 1.0f;
			float actualHeight = getHeight();
			if(actualHeight != 0.0f)
			{
				setScaleY(_value / actualHeight);
			}
			else
			{
				setScaleY(1.0f);
			}
		}
		public virtual AsDisplayObject getRoot()
		{
			AsDisplayObject currentObject = this;
			while(currentObject.getParent() != null)
			{
				currentObject = currentObject.getParent();
			}
			return currentObject;
		}
		public virtual float getX()
		{
			return mX;
		}
		public virtual void setX(float _value)
		{
			mX = _value;
		}
		public virtual float getY()
		{
			return mY;
		}
		public virtual void setY(float _value)
		{
			mY = _value;
		}
		public virtual float getPivotX()
		{
			return mPivotX;
		}
		public virtual void setPivotX(float _value)
		{
			mPivotX = _value;
		}
		public virtual float getPivotY()
		{
			return mPivotY;
		}
		public virtual void setPivotY(float _value)
		{
			mPivotY = _value;
		}
		public virtual float getScaleX()
		{
			return mScaleX;
		}
		public virtual void setScaleX(float _value)
		{
			mScaleX = _value;
		}
		public virtual float getScaleY()
		{
			return mScaleY;
		}
		public virtual void setScaleY(float _value)
		{
			mScaleY = _value;
		}
		public virtual float getRotation()
		{
			return mRotation;
		}
		public virtual void setRotation(float _value)
		{
			while(_value < -180)
			{
				_value = _value + 360;
			}
			while(_value > 180)
			{
				_value = _value - 360;
			}
			mRotation = _value;
		}
		public virtual float getAlpha()
		{
			return mAlpha;
		}
		public virtual void setAlpha(float _value)
		{
			mAlpha = _value < 0.0f ? 0.0f : _value > 1.0f ? 1.0f : _value;
		}
		public virtual bool getVisible()
		{
			return mVisible;
		}
		public virtual void setVisible(bool _value)
		{
			mVisible = _value;
		}
		public virtual bool getTouchable()
		{
			return mTouchable;
		}
		public virtual void setTouchable(bool _value)
		{
			mTouchable = _value;
		}
		public virtual String getName()
		{
			return mName;
		}
		public virtual void setName(String _value)
		{
			mName = _value;
		}
		public virtual AsDisplayObjectContainer getParent()
		{
			return mParent;
		}
		public virtual AsStage getStage()
		{
			return AsStage.getInstance();
		}
		public virtual AsTransform getTransform()
		{
			return mTransform;
		}
		public virtual void setTransform(AsTransform _value)
		{
			mTransform = _value;
		}
		public virtual AsRectangle getScrollRect()
		{
			return mScrollRect;
		}
		public virtual void setScrollRect(AsRectangle _value)
		{
			mScrollRect = _value;
		}
		public virtual uint getOpaqueBackground()
		{
			return (uint)(0xffffffff);
		}
		public virtual void setOpaqueBackground(uint _value)
		{
		}
	}
}
