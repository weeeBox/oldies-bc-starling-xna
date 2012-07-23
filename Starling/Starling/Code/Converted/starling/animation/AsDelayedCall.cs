using System;
 
using bc.flash;
using starling.animation;
using starling.events;
 
namespace starling.animation
{
	public delegate void AsDelayedCallback();
	public class AsDelayedCall : AsEventDispatcher, AsIAnimatable
	{
		private float mCurrentTime = 0;
		private float mTotalTime;
		private AsDelayedCallback mCall;
		private AsArray mArgs;
		private int mRepeatCount = 1;
		public AsDelayedCall(AsDelayedCallback call, float delay, AsArray args)
		{
			mCall = call;
			mTotalTime = AsMath.max(delay, 0.0001f);
			mArgs = args;
		}
		public AsDelayedCall(AsDelayedCallback call, float delay)
		 : this(call, delay, null)
		{
		}
		public virtual void advanceTime(float time)
		{
			float previousTime = mCurrentTime;
			mCurrentTime = AsMath.min(mTotalTime, (mCurrentTime + time));
			if(((previousTime < mTotalTime) && (mCurrentTime >= mTotalTime)))
			{
				if((mRepeatCount > 1))
				{
					mRepeatCount = (mRepeatCount - 1);
					mCurrentTime = 0;
					advanceTime(((previousTime + time) - mTotalTime));
				}
				else
				{
					dispatchEventWith(AsEvent.REMOVE_FROM_JUGGLER);
				}
			}
		}
		public virtual bool getIsComplete()
		{
			return ((mRepeatCount == 1) && (mCurrentTime >= mTotalTime));
		}
		public virtual float getTotalTime()
		{
			return mTotalTime;
		}
		public virtual float getCurrentTime()
		{
			return mCurrentTime;
		}
		public virtual int getRepeatCount()
		{
			return mRepeatCount;
		}
		public virtual void setRepeatCount(int _value)
		{
			mRepeatCount = _value;
		}
	}
}
