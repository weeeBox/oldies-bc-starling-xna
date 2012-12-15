using System;
 
using bc.flash;
using starling.animation;
using starling.events;
 
namespace starling.animation
{
	public class AsTween : AsEventDispatcher, AsIAnimatable
	{
		private Object mTarget;
		private AsTransitionCallback mTransitionFunc;
		private String mTransitionName;
		private AsVector<String> mProperties;
		private AsVector<float> mStartValues;
		private AsVector<float> mEndValues;
		private AsTransitionCallback mOnStart;
		private AsTransitionCallback mOnUpdate;
		private AsTransitionCallback mOnRepeat;
		private AsTransitionCallback mOnComplete;
		private AsArray mOnStartArgs;
		private AsArray mOnUpdateArgs;
		private AsArray mOnRepeatArgs;
		private AsArray mOnCompleteArgs;
		private float mTotalTime;
		private float mCurrentTime;
		private float mDelay;
		private bool mRoundToInt;
		private AsTween mNextTween;
		private int mRepeatCount;
		private float mRepeatDelay;
		private bool mReverse;
		private int mCurrentCycle;
		private static AsVector<AsTween> sTweenPool = new AsVector<AsTween>();
		public AsTween(Object target, float time, Object transition)
		{
			reset(target, time, transition);
		}
		public AsTween(Object target, float time)
		 : this(target, time, "linear")
		{
		}
		public virtual AsTween reset(Object target, float time, Object transition)
		{
			mTarget = target;
			mCurrentTime = 0;
			mTotalTime = AsMath.max(0.0001f, time);
			mDelay = mRepeatDelay = 0.0f;
			mOnStart = mOnUpdate = mOnComplete = null;
			mOnStartArgs = mOnUpdateArgs = mOnCompleteArgs = null;
			mRoundToInt = mReverse = false;
			mRepeatCount = 1;
			mCurrentCycle = -1;
			if(transition is String)
			{
				this.setTransition(transition as String);
			}
			else
			{
				if(transition is AsTransitionCallback)
				{
					this.setTransitionFunc(transition as AsTransitionCallback);
				}
				else
				{
					throw new AsArgumentError("Transition must be either a string or a function");
				}
			}
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
		public virtual AsTween reset(Object target, float time)
		{
			return reset(target, time, "linear");
		}
		public virtual void animate(String property, float targetValue)
		{
			if(mTarget == null)
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
			if(time == 0 || (mRepeatCount == 1 && mCurrentTime == mTotalTime))
			{
				return;
			}
			int i = 0;
			float previousTime = mCurrentTime;
			float restTime = mTotalTime - mCurrentTime;
			float carryOverTime = time > restTime ? time - restTime : 0.0f;
			mCurrentTime = AsMath.min(mTotalTime, mCurrentTime + time);
			if(mCurrentTime <= 0)
			{
				return;
			}
			if(mCurrentCycle < 0 && previousTime <= 0 && mCurrentTime > 0)
			{
				mCurrentCycle++;
				if(mOnStart != null)
				{
					mOnStart((float)(mOnStartArgs[0]));
				}
			}
			float ratio = mCurrentTime / mTotalTime;
			bool reversed = mReverse && (mCurrentCycle % 2 == 1);
			int numProperties = (int)(mStartValues.getLength());
			for (i = 0; i < numProperties; ++i)
			{
				if(AsGlobal.isNaN(mStartValues[i]))
				{
					mStartValues[i] = ((AsObject)(mTarget)).getOwnProperty(mProperties[i]) as float;
				}
				float startValue = mStartValues[i];
				float endValue = mEndValues[i];
				float delta = endValue - startValue;
				float transitionValue = reversed ? mTransitionFunc(1.0f - ratio) : mTransitionFunc(ratio);
				float currentValue = startValue + transitionValue * delta;
				if(mRoundToInt)
				{
					currentValue = AsMath.round(currentValue);
				}
				((AsObject)(mTarget)).setOwnProperty(mProperties[i], currentValue);
			}
			if(mOnUpdate != null)
			{
				mOnUpdate((float)(mOnUpdateArgs[0]));
			}
			if(previousTime < mTotalTime && mCurrentTime >= mTotalTime)
			{
				if(mRepeatCount == 0 || mRepeatCount > 1)
				{
					mCurrentTime = -mRepeatDelay;
					mCurrentCycle++;
					if(mRepeatCount > 1)
					{
						mRepeatCount--;
					}
					if(mOnRepeat != null)
					{
						mOnRepeat((float)(mOnRepeatArgs[0]));
					}
				}
				else
				{
					AsTransitionCallback onComplete = mOnComplete;
					AsArray onCompleteArgs = mOnCompleteArgs;
					dispatchEventWith(AsEvent.REMOVE_FROM_JUGGLER);
					if(onComplete != null)
					{
						onComplete((float)(onCompleteArgs[0]));
					}
				}
			}
			if(carryOverTime != 0)
			{
				advanceTime(carryOverTime);
			}
		}
		public virtual bool getIsComplete()
		{
			return mCurrentTime >= mTotalTime && mRepeatCount == 1;
		}
		public virtual Object getTarget()
		{
			return mTarget;
		}
		public virtual String getTransition()
		{
			return mTransitionName;
		}
		public virtual void setTransition(String _value)
		{
			mTransitionName = _value;
			mTransitionFunc = AsTransitions.getTransition(_value);
			if(mTransitionFunc == null)
			{
				throw new AsArgumentError("Invalid transiton: " + _value);
			}
		}
		public virtual AsTransitionCallback getTransitionFunc()
		{
			return mTransitionFunc;
		}
		public virtual void setTransitionFunc(AsTransitionCallback _value)
		{
			mTransitionName = "custom";
			mTransitionFunc = _value;
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
			mCurrentTime = mCurrentTime + mDelay - _value;
			mDelay = _value;
		}
		public virtual int getRepeatCount()
		{
			return mRepeatCount;
		}
		public virtual void setRepeatCount(int _value)
		{
			mRepeatCount = _value;
		}
		public virtual float getRepeatDelay()
		{
			return mRepeatDelay;
		}
		public virtual void setRepeatDelay(float _value)
		{
			mRepeatDelay = _value;
		}
		public virtual bool getReverse()
		{
			return mReverse;
		}
		public virtual void setReverse(bool _value)
		{
			mReverse = _value;
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
		public virtual AsTransitionCallback getOnRepeat()
		{
			return mOnRepeat;
		}
		public virtual void setOnRepeat(AsTransitionCallback _value)
		{
			mOnRepeat = _value;
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
		public virtual AsArray getOnRepeatArgs()
		{
			return mOnRepeatArgs;
		}
		public virtual void setOnRepeatArgs(AsArray _value)
		{
			mOnRepeatArgs = _value;
		}
		public virtual AsArray getOnCompleteArgs()
		{
			return mOnCompleteArgs;
		}
		public virtual void setOnCompleteArgs(AsArray _value)
		{
			mOnCompleteArgs = _value;
		}
		public virtual AsTween getNextTween()
		{
			return mNextTween;
		}
		public virtual void setNextTween(AsTween _value)
		{
			mNextTween = _value;
		}
		public static AsTween fromPool(Object target, float time, Object transition)
		{
			if(sTweenPool.getLength() != 0)
			{
				return sTweenPool.pop().reset(target, time, transition);
			}
			else
			{
				return new AsTween(target, time, transition);
			}
		}
		public static AsTween fromPool(Object target, float time)
		{
			return fromPool(target, time, "linear");
		}
		public static void toPool(AsTween tween)
		{
			tween.mOnStart = tween.mOnUpdate = tween.mOnRepeat = tween.mOnComplete = null;
			tween.mOnStartArgs = tween.mOnUpdateArgs = tween.mOnRepeatArgs = tween.mOnCompleteArgs = null;
			tween.mTarget = null;
			tween.mTransitionFunc = null;
			tween.removeEventListeners();
			sTweenPool.push(tween);
		}
	}
}
