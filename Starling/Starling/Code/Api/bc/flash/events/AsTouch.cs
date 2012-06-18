using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.error;
using bc.flash.events;
using bc.flash.geom;
 
namespace bc.flash.events
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
			throw new AsNotImplementedError();
		}
		public virtual AsPoint getPreviousLocation(AsDisplayObject space)
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
