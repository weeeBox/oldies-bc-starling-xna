using System;
 
using bc.flash;
using starling.animation;
using starling.events;
 
namespace starling.animation
{
	public class AsTween : AsEventDispatcher, AsIAnimatable
	{
		private AsObject mTarget;
		private String mTransition;
		private AsVector<String> mProperties;
		private AsVector<float> mStartValues;
		private AsVector<float> mEndValues;
		private AsTransitionCallback mOnStart;
		private AsTransitionCallback mOnUpdate;
		private AsTransitionCallback mOnComplete;
		private AsArray mOnStartArgs;
		private AsArray mOnUpdateArgs;
		private AsArray mOnCompleteArgs;
		private float mTotalTime;
		private float mCurrentTime;
		private float mDelay;
		private bool mRoundToInt;
		public AsTween(AsObject target, float time, String transition)
		{
			reset((AsObject)(target), time, transition);
		}
		public AsTween(AsObject target, float time)
		 : this(target, time, "linear")
		{
		}
		public virtual AsTween reset(AsObject target, float time, String transition)
		{
			mTarget = (AsObject)(target);
			mCurrentTime = 0;
			mTotalTime = AsMath.max(0.0001f, time);
			mDelay = 0;
			mTransition = transition;
			mRoundToInt = false;
			mOnStart = mOnUpdate = mOnComplete = null;
			mOnStartArgs = mOnUpdateArgs = mOnCompleteArgs = null;
			if(mProperties != null)
			{
				mProperties.setLength(0);
			}
			else
			{
				mProperties = new AsVector<String>();
			}
			if(mStartValues != null)
			{
				mStartValues.setLength(0);
			}
			else
			{
				mStartValues = new AsVector<float>();
			}
			if(mEndValues != null)
			{
				mEndValues.setLength(0);
			}
			else
			{
				mEndValues = new AsVector<float>();
			}
			return this;
		}
		public virtual AsTween reset(AsObject target, float time)
		{
			return reset((AsObject)(target), time, "linear");
		}
		public virtual void animate(String property, float targetValue)
		{
			if((mTarget == null))
			{
				return;
			}
			mProperties.push(property);
			mStartValues.push(AsNumber.NaN);
			mEndValues.push(targetValue);
		}
		public virtual void scaleTo(float factor)
		{
			animate("scaleX", factor);
			animate("scaleY", factor);
		}
		public virtual void moveTo(float x, float y)
		{
			animate("x", x);
			animate("y", y);
		}
		public virtual void fadeTo(float alpha)
		{
			animate("alpha", alpha);
		}
		public virtual void advanceTime(float time)
		{
			if((time == 0))
			{
				return;
			}
			float previousTime = mCurrentTime;
			mCurrentTime = (mCurrentTime + time);
			if(((mCurrentTime < 0) || (previousTime >= mTotalTime)))
			{
				return;
			}
			NOT.IMPLEMENTED();
			float ratio = (AsMath.min(mTotalTime, mCurrentTime) / mTotalTime);
			int numAnimatedProperties = (int)(mStartValues.getLength());
			int i = 0;
			for (; (i < numAnimatedProperties); ++i)
			{
				if(AsGlobal.isNaN(mStartValues[i]))
				{
					mStartValues[i] = ((mTarget.getOwnProperty(mProperties[i]) is float) ? ((float)(mTarget.getOwnProperty(mProperties[i]))) : null);
				}
				float startValue = mStartValues[i];
				float endValue = mEndValues[i];
				float delta = (endValue - startValue);
				AsTransitionCallback transitionFunc = AsTransitions.getTransition(mTransition);
				float currentValue = (startValue + (transitionFunc(ratio) * delta));
				if(mRoundToInt)
				{
					currentValue = AsMath.round(currentValue);
				}
				mTarget.setOwnProperty(mProperties[i], currentValue);
			}
			NOT.IMPLEMENTED();
			if(((previousTime < mTotalTime) && (mCurrentTime >= mTotalTime)))
			{
				dispatchEventWith(AsEvent.REMOVE_FROM_JUGGLER);
				NOT.IMPLEMENTED();
			}
		}
		public virtual bool getIsComplete()
		{
			return (mCurrentTime >= mTotalTime);
		}
		public virtual AsObject getTarget()
		{
			return (AsObject)(mTarget);
		}
		public virtual String getTransition()
		{
			return mTransition;
		}
		public virtual float getTotalTime()
		{
			return mTotalTime;
		}
		public virtual float getCurrentTime()
		{
			return mCurrentTime;
		}
		public virtual float getDelay()
		{
			return mDelay;
		}
		public virtual void setDelay(float _value)
		{
			mCurrentTime = ((mCurrentTime + mDelay) - _value);
			mDelay = _value;
		}
		public virtual bool getRoundToInt()
		{
			return mRoundToInt;
		}
		public virtual void setRoundToInt(bool _value)
		{
			mRoundToInt = _value;
		}
		public virtual AsTransitionCallback getOnStart()
		{
			return mOnStart;
		}
		public virtual void setOnStart(AsTransitionCallback _value)
		{
			mOnStart = _value;
		}
		public virtual AsTransitionCallback getOnUpdate()
		{
			return mOnUpdate;
		}
		public virtual void setOnUpdate(AsTransitionCallback _value)
		{
			mOnUpdate = _value;
		}
		public virtual AsTransitionCallback getOnComplete()
		{
			return mOnComplete;
		}
		public virtual void setOnComplete(AsTransitionCallback _value)
		{
			mOnComplete = _value;
		}
		public virtual AsArray getOnStartArgs()
		{
			return mOnStartArgs;
		}
		public virtual void setOnStartArgs(AsArray _value)
		{
			mOnStartArgs = _value;
		}
		public virtual AsArray getOnUpdateArgs()
		{
			return mOnUpdateArgs;
		}
		public virtual void setOnUpdateArgs(AsArray _value)
		{
			mOnUpdateArgs = _value;
		}
		public virtual AsArray getOnCompleteArgs()
		{
			return mOnCompleteArgs;
		}
		public virtual void setOnCompleteArgs(AsArray _value)
		{
			mOnCompleteArgs = _value;
		}
	}
}
