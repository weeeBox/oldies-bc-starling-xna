using System;
 
using bc.flash;
using bc.flash.errors;
using bc.flash.geom;
using starling.display;
using starling.events;
 
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
		private static AsMatrix sHelperMatrix = new AsMatrix();
		public AsTouch(int id, float globalX, float globalY, String phase, AsDisplayObject target)
		{
			mID = id;
			mGlobalX = mPreviousGlobalX = globalX;
			mGlobalY = mPreviousGlobalY = globalY;
			mTapCount = 0;
			mPhase = phase;
			mTarget = target;
		}
		public virtual AsPoint getLocation(AsDisplayObject space)
		{
			AsPoint point = new AsPoint(mGlobalX, mGlobalY);
			mTarget.get_base().getTransformationMatrix(space, sHelperMatrix);
			return sHelperMatrix.transformPoint(point);
		}
		public virtual AsPoint getPreviousLocation(AsDisplayObject space)
		{
			AsPoint point = new AsPoint(mPreviousGlobalX, mPreviousGlobalY);
			mTarget.get_base().getTransformationMatrix(space, sHelperMatrix);
			return sHelperMatrix.transformPoint(point);
		}
		public virtual AsPoint getMovement(AsDisplayObject space)
		{
			return getLocation(space).subtract(getPreviousLocation(space));
		}
		public virtual String toString()
		{
			throw new AsNotImplementedError();
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
		public virtual void setPosition(float globalX, float globalY)
		{
			mPreviousGlobalX = mGlobalX;
			mPreviousGlobalY = mGlobalY;
			mGlobalX = globalX;
			mGlobalY = globalY;
		}
		public virtual void setPhase(String _value)
		{
			mPhase = _value;
		}
		public virtual void setTapCount(int _value)
		{
			mTapCount = _value;
		}
		public virtual void setTarget(AsDisplayObject _value)
		{
			mTarget = _value;
		}
		public virtual void setTimestamp(float _value)
		{
			mTimestamp = _value;
		}
	}
}
