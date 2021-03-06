using System;
 
using bc.flash;
using bc.flash.geom;
using starling.display;
using starling.events;
using starling.utils;
 
namespace starling.events
{
	public class AsTouch : AsObject
	{
		private int mID;
		private float mGlobalX;
		private float mGlobalY;
		private float mPreviousGlobalX;
		private float mPreviousGlobalY;
		private int mTapCount;
		private String mPhase;
		private AsDisplayObject mTarget;
		private float mTimestamp;
		private float mPressure;
		private float mWidth;
		private float mHeight;
		private AsVector<AsEventDispatcher> mBubbleChain;
		private static AsMatrix sHelperMatrix = new AsMatrix();
		public AsTouch(int id, float globalX, float globalY, String phase, AsDisplayObject target)
		{
			mID = id;
			mGlobalX = mPreviousGlobalX = globalX;
			mGlobalY = mPreviousGlobalY = globalY;
			mTapCount = 0;
			mPhase = phase;
			mTarget = target;
			mPressure = mWidth = mHeight = 1.0f;
			mBubbleChain = new AsVector<AsEventDispatcher>();
		}
		public virtual AsPoint getLocation(AsDisplayObject space, AsPoint resultPoint)
		{
			if(resultPoint == null)
			{
				resultPoint = new AsPoint();
			}
			mTarget.get_base().getTransformationMatrix(space, sHelperMatrix);
			return AsMatrixUtil.transformCoords(sHelperMatrix, mGlobalX, mGlobalY, resultPoint);
		}
		public virtual AsPoint getLocation(AsDisplayObject space)
		{
			return getLocation(space, null);
		}
		public virtual AsPoint getPreviousLocation(AsDisplayObject space, AsPoint resultPoint)
		{
			if(resultPoint == null)
			{
				resultPoint = new AsPoint();
			}
			mTarget.get_base().getTransformationMatrix(space, sHelperMatrix);
			return AsMatrixUtil.transformCoords(sHelperMatrix, mPreviousGlobalX, mPreviousGlobalY, resultPoint);
		}
		public virtual AsPoint getPreviousLocation(AsDisplayObject space)
		{
			return getPreviousLocation(space, null);
		}
		public virtual AsPoint getMovement(AsDisplayObject space, AsPoint resultPoint)
		{
			if(resultPoint == null)
			{
				resultPoint = new AsPoint();
			}
			getLocation(space, resultPoint);
			float x = resultPoint.x;
			float y = resultPoint.y;
			getPreviousLocation(space, resultPoint);
			resultPoint.setTo(x - resultPoint.x, y - resultPoint.y);
			return resultPoint;
		}
		public virtual AsPoint getMovement(AsDisplayObject space)
		{
			return getMovement(space, null);
		}
		public virtual String toString()
		{
			return AsGlobal.formatString("Touch {0}: globalX={1}, globalY={2}, phase={3}", mID, mGlobalX, mGlobalY, mPhase);
		}
		public virtual AsTouch clone()
		{
			AsTouch clone = new AsTouch(mID, mGlobalX, mGlobalY, mPhase, mTarget);
			clone.mPreviousGlobalX = mPreviousGlobalX;
			clone.mPreviousGlobalY = mPreviousGlobalY;
			clone.mTapCount = mTapCount;
			clone.mTimestamp = mTimestamp;
			return clone;
		}
		private void updateBubbleChain()
		{
			if(mTarget != null)
			{
				int length = 1;
				AsDisplayObject element = mTarget;
				mBubbleChain.setLength(0);
				mBubbleChain[0] = element;
				while((element = element.getParent()) != null)
				{
					mBubbleChain[length++] = element;
				}
			}
			else
			{
				mBubbleChain.setLength(0);
			}
		}
		public virtual int getId()
		{
			return mID;
		}
		public virtual float getGlobalX()
		{
			return mGlobalX;
		}
		public virtual float getGlobalY()
		{
			return mGlobalY;
		}
		public virtual float getPreviousGlobalX()
		{
			return mPreviousGlobalX;
		}
		public virtual float getPreviousGlobalY()
		{
			return mPreviousGlobalY;
		}
		public virtual int getTapCount()
		{
			return mTapCount;
		}
		public virtual String getPhase()
		{
			return mPhase;
		}
		public virtual AsDisplayObject getTarget()
		{
			return mTarget;
		}
		public virtual float getTimestamp()
		{
			return mTimestamp;
		}
		public virtual float getPressure()
		{
			return mPressure;
		}
		public virtual float getWidth()
		{
			return mWidth;
		}
		public virtual float getHeight()
		{
			return mHeight;
		}
		public virtual void dispatchEvent(AsTouchEvent _event)
		{
			if(mTarget != null)
			{
				_event.dispatch(mBubbleChain);
			}
		}
		public virtual void setTarget(AsDisplayObject _value)
		{
			mTarget = _value;
			updateBubbleChain();
		}
		public virtual void setPosition(float globalX, float globalY)
		{
			mPreviousGlobalX = mGlobalX;
			mPreviousGlobalY = mGlobalY;
			mGlobalX = globalX;
			mGlobalY = globalY;
		}
		public virtual void setSize(float width, float height)
		{
			mWidth = width;
			mHeight = height;
		}
		public virtual void setPhase(String _value)
		{
			mPhase = _value;
		}
		public virtual void setTapCount(int _value)
		{
			mTapCount = _value;
		}
		public virtual void setTimestamp(float _value)
		{
			mTimestamp = _value;
		}
		public virtual void setPressure(float _value)
		{
			mPressure = _value;
		}
	}
}
