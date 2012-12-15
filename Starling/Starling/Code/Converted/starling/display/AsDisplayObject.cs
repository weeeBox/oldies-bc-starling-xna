using System;
 
using bc.flash;
using bc.flash.geom;
using bc.flash.system;
using bc.flash.ui;
using starling.core;
using starling.display;
using starling.errors;
using starling.events;
using starling.filters;
using starling.utils;
 
namespace starling.display
{
	public class AsDisplayObject : AsEventDispatcher
	{
		private float mX;
		private float mY;
		private float mPivotX;
		private float mPivotY;
		private float mScaleX;
		private float mScaleY;
		private float mSkewX;
		private float mSkewY;
		private float mRotation;
		private float mAlpha;
		private bool mVisible;
		private bool mTouchable;
		private String mBlendMode;
		private String mName;
		private bool mUseHandCursor;
		private AsDisplayObjectContainer mParent;
		private AsMatrix mTransformationMatrix;
		private bool mOrientationChanged;
		private AsFragmentFilter mFilter;
		private static AsVector<AsDisplayObject> sAncestors = new AsVector<AsDisplayObject>();
		private static AsRectangle sHelperRect = new AsRectangle();
		private static AsMatrix sHelperMatrix = new AsMatrix();
		public AsDisplayObject()
		{
			if(AsCapabilities.getIsDebugger() && AsGlobal.getQualifiedClassName(this) == "starling.display::DisplayObject")
			{
				throw new AsAbstractClassError();
			}
			mX = mY = mPivotX = mPivotY = mRotation = mSkewX = mSkewY = 0.0f;
			mScaleX = mScaleY = mAlpha = 1.0f;
			mVisible = mTouchable = true;
			mBlendMode = AsBlendMode.AUTO;
			mTransformationMatrix = new AsMatrix();
			mOrientationChanged = mUseHandCursor = false;
		}
		public virtual void dispose()
		{
			if(mFilter != null)
			{
				mFilter.dispose();
			}
			removeEventListeners();
		}
		public virtual void removeFromParent(bool dispose)
		{
			if(mParent != null)
			{
				mParent.removeChild(this, dispose);
			}
		}
		public virtual void removeFromParent()
		{
			removeFromParent(false);
		}
		public virtual AsMatrix getTransformationMatrix(AsDisplayObject targetSpace, AsMatrix resultMatrix)
		{
			AsDisplayObject commonParent = null;
			AsDisplayObject currentObject = null;
			if(resultMatrix != null)
			{
				resultMatrix.identity();
			}
			else
			{
				resultMatrix = new AsMatrix();
			}
			if(targetSpace == this)
			{
				return resultMatrix;
			}
			else
			{
				if(targetSpace == mParent || (targetSpace == null && mParent == null))
				{
					resultMatrix.copyFrom(getTransformationMatrix());
					return resultMatrix;
				}
				else
				{
					if(targetSpace == null || targetSpace == get_base())
					{
						currentObject = this;
						while(currentObject != targetSpace)
						{
							resultMatrix.concat(currentObject.getTransformationMatrix());
							currentObject = currentObject.mParent;
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
			commonParent = null;
			currentObject = this;
			while(currentObject != null)
			{
				sAncestors.push(currentObject);
				currentObject = currentObject.mParent;
			}
			currentObject = targetSpace;
			while(currentObject != null && sAncestors.indexOf(currentObject) == -1)
			{
				currentObject = currentObject.mParent;
			}
			sAncestors.setLength(0);
			if(currentObject != null)
			{
				commonParent = currentObject;
			}
			else
			{
				throw new AsArgumentError("Object not connected to target");
			}
			currentObject = this;
			while(currentObject != commonParent)
			{
				resultMatrix.concat(currentObject.getTransformationMatrix());
				currentObject = currentObject.mParent;
			}
			if(commonParent == targetSpace)
			{
				return resultMatrix;
			}
			sHelperMatrix.identity();
			currentObject = targetSpace;
			while(currentObject != commonParent)
			{
				sHelperMatrix.concat(currentObject.getTransformationMatrix());
				currentObject = currentObject.mParent;
			}
			sHelperMatrix.invert();
			resultMatrix.concat(sHelperMatrix);
			return resultMatrix;
		}
		public virtual AsMatrix getTransformationMatrix(AsDisplayObject targetSpace)
		{
			return getTransformationMatrix(targetSpace, null);
		}
		public virtual AsRectangle getBounds(AsDisplayObject targetSpace, AsRectangle resultRect)
		{
			throw new AsAbstractMethodError("Method needs to be implemented in subclass");
			return null;
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
		public virtual AsPoint localToGlobal(AsPoint localPoint, AsPoint resultPoint)
		{
			getTransformationMatrix(get_base(), sHelperMatrix);
			return AsMatrixUtil.transformCoords(sHelperMatrix, localPoint.x, localPoint.y, resultPoint);
		}
		public virtual AsPoint localToGlobal(AsPoint localPoint)
		{
			return localToGlobal(localPoint, null);
		}
		public virtual AsPoint globalToLocal(AsPoint globalPoint, AsPoint resultPoint)
		{
			getTransformationMatrix(get_base(), sHelperMatrix);
			sHelperMatrix.invert();
			return AsMatrixUtil.transformCoords(sHelperMatrix, globalPoint.x, globalPoint.y, resultPoint);
		}
		public virtual AsPoint globalToLocal(AsPoint globalPoint)
		{
			return globalToLocal(globalPoint, null);
		}
		public virtual void render(AsRenderSupport support, float parentAlpha)
		{
			throw new AsAbstractMethodError("Method needs to be implemented in subclass");
		}
		public virtual bool getHasVisibleArea()
		{
			return mAlpha != 0.0f && mVisible && mScaleX != 0.0f && mScaleY != 0.0f;
		}
		public virtual void setParent(AsDisplayObjectContainer _value)
		{
			AsDisplayObject ancestor = _value;
			while(ancestor != this && ancestor != null)
			{
				ancestor = ancestor.mParent;
			}
			if(ancestor == this)
			{
				throw new AsArgumentError("An object cannot be added as a child to itself or one " + "of its children (or children's children, etc.)");
			}
			else
			{
				mParent = _value;
			}
		}
		private float normalizeAngle(float angle)
		{
			while(angle < -AsMath.PI)
			{
				angle = angle + AsMath.PI * 2.0f;
			}
			while(angle > AsMath.PI)
			{
				angle = angle - AsMath.PI * 2.0f;
			}
			return angle;
		}
		public virtual AsMatrix getTransformationMatrix()
		{
			if(mOrientationChanged)
			{
				mOrientationChanged = false;
				mTransformationMatrix.identity();
				if(mSkewX != 0.0f || mSkewY != 0.0f)
				{
					AsMatrixUtil.skew(mTransformationMatrix, mSkewX, mSkewY);
				}
				if(mScaleX != 1.0f || mScaleY != 1.0f)
				{
					mTransformationMatrix.scale(mScaleX, mScaleY);
				}
				if(mRotation != 0.0f)
				{
					mTransformationMatrix.rotate(mRotation);
				}
				if(mX != 0.0f || mY != 0.0f)
				{
					mTransformationMatrix.translate(mX, mY);
				}
				if(mPivotX != 0.0f || mPivotY != 0.0f)
				{
					mTransformationMatrix.tx = mX - mTransformationMatrix.a * mPivotX - mTransformationMatrix.c * mPivotY;
					mTransformationMatrix.ty = mY - mTransformationMatrix.b * mPivotX - mTransformationMatrix.d * mPivotY;
				}
			}
			return mTransformationMatrix;
		}
		public virtual void setTransformationMatrix(AsMatrix matrix)
		{
			mOrientationChanged = false;
			mTransformationMatrix.copyFrom(matrix);
			mX = matrix.tx;
			mY = matrix.ty;
			float a = matrix.a;
			float b = matrix.b;
			float c = matrix.c;
			float d = matrix.d;
			mScaleX = AsMath.sqrt(a * a + b * b);
			if(mScaleX != 0)
			{
				mRotation = AsMath.atan2(b, a);
			}
			else
			{
				mRotation = 0;
			}
			float cosTheta = AsMath.cos(mRotation);
			float sinTheta = AsMath.sin(mRotation);
			mScaleY = d * cosTheta - c * sinTheta;
			if(mScaleY != 0)
			{
				mSkewX = AsMath.atan2(d * sinTheta + c * cosTheta, mScaleY);
			}
			else
			{
				mSkewX = 0;
			}
			mSkewY = 0;
			mPivotX = 0;
			mPivotY = 0;
		}
		public virtual bool getUseHandCursor()
		{
			return mUseHandCursor;
		}
		public virtual void setUseHandCursor(bool _value)
		{
			if(_value == mUseHandCursor)
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
		private void onTouch(AsTouchEvent _event)
		{
			AsMouse.setCursor(_event.interactsWith(this) ? AsMouseCursor.BUTTON : AsMouseCursor.AUTO);
		}
		public virtual AsRectangle getBounds()
		{
			return getBounds(mParent);
		}
		public virtual float getWidth()
		{
			return getBounds(mParent, sHelperRect).width;
		}
		public virtual void setWidth(float _value)
		{
			setScaleX(1.0f);
			float actualWidth = getWidth();
			if(actualWidth != 0.0f)
			{
				setScaleX(_value / actualWidth);
			}
		}
		public virtual float getHeight()
		{
			return getBounds(mParent, sHelperRect).height;
		}
		public virtual void setHeight(float _value)
		{
			setScaleY(1.0f);
			float actualHeight = getHeight();
			if(actualHeight != 0.0f)
			{
				setScaleY(_value / actualHeight);
			}
		}
		public virtual float getX()
		{
			return mX;
		}
		public virtual void setX(float _value)
		{
			if(mX != _value)
			{
				mX = _value;
				mOrientationChanged = true;
			}
		}
		public virtual float getY()
		{
			return mY;
		}
		public virtual void setY(float _value)
		{
			if(mY != _value)
			{
				mY = _value;
				mOrientationChanged = true;
			}
		}
		public virtual float getPivotX()
		{
			return mPivotX;
		}
		public virtual void setPivotX(float _value)
		{
			if(mPivotX != _value)
			{
				mPivotX = _value;
				mOrientationChanged = true;
			}
		}
		public virtual float getPivotY()
		{
			return mPivotY;
		}
		public virtual void setPivotY(float _value)
		{
			if(mPivotY != _value)
			{
				mPivotY = _value;
				mOrientationChanged = true;
			}
		}
		public virtual float getScaleX()
		{
			return mScaleX;
		}
		public virtual void setScaleX(float _value)
		{
			if(mScaleX != _value)
			{
				mScaleX = _value;
				mOrientationChanged = true;
			}
		}
		public virtual float getScaleY()
		{
			return mScaleY;
		}
		public virtual void setScaleY(float _value)
		{
			if(mScaleY != _value)
			{
				mScaleY = _value;
				mOrientationChanged = true;
			}
		}
		public virtual float getSkewX()
		{
			return mSkewX;
		}
		public virtual void setSkewX(float _value)
		{
			_value = normalizeAngle(_value);
			if(mSkewX != _value)
			{
				mSkewX = _value;
				mOrientationChanged = true;
			}
		}
		public virtual float getSkewY()
		{
			return mSkewY;
		}
		public virtual void setSkewY(float _value)
		{
			_value = normalizeAngle(_value);
			if(mSkewY != _value)
			{
				mSkewY = _value;
				mOrientationChanged = true;
			}
		}
		public virtual float getRotation()
		{
			return mRotation;
		}
		public virtual void setRotation(float _value)
		{
			_value = normalizeAngle(_value);
			if(mRotation != _value)
			{
				mRotation = _value;
				mOrientationChanged = true;
			}
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
		public virtual String getBlendMode()
		{
			return mBlendMode;
		}
		public virtual void setBlendMode(String _value)
		{
			mBlendMode = _value;
		}
		public virtual String getName()
		{
			return mName;
		}
		public virtual void setName(String _value)
		{
			mName = _value;
		}
		public virtual AsFragmentFilter getFilter()
		{
			return mFilter;
		}
		public virtual void setFilter(AsFragmentFilter _value)
		{
			mFilter = _value;
		}
		public virtual AsDisplayObjectContainer getParent()
		{
			return mParent;
		}
		public virtual AsDisplayObject get_base()
		{
			AsDisplayObject currentObject = this;
			while(currentObject.mParent != null)
			{
				currentObject = currentObject.mParent;
			}
			return currentObject;
		}
		public virtual AsDisplayObject getRoot()
		{
			AsDisplayObject currentObject = this;
			while(currentObject.mParent != null)
			{
				if(currentObject.mParent is AsStage)
				{
					return currentObject;
				}
				else
				{
					currentObject = currentObject.getParent();
				}
			}
			return null;
		}
		public virtual AsStage getStage()
		{
			return this.get_base() as AsStage;
		}
	}
}
