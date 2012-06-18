using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.events;
using bc.flash.geom;
 
namespace bc.flash.events
{
	public class AsTouchProcessor : AsObject
	{
		private const float MULTITAP_TIME = 0.3f;
		private const float MULTITAP_DISTANCE = 25;
		private AsStage mStage;
		private float mElapsedTime;
		private float mOffsetTime;
		private AsVector<AsTouch> mCurrentTouches;
		private AsVector<AsArray> mQueue;
		private AsVector<AsTouch> mLastTaps;
		private bool mShiftDown = false;
		private bool mCtrlDown = false;
		private static AsVector<int> sProcessedTouchIDs = new AsVector<int>();
		private static AsVector<AsTouchData> sHoveringTouchData = new AsVector<AsTouchData>();
		public AsTouchProcessor(AsStage stage)
		{
			mStage = stage;
			mElapsedTime = mOffsetTime = 0.0f;
			mCurrentTouches = new AsVector<AsTouch>();
			mQueue = new AsVector<AsArray>();
			mLastTaps = new AsVector<AsTouch>();
			mStage.addEventListener(AsKeyboardEvent.KEY_DOWN, onKey);
			mStage.addEventListener(AsKeyboardEvent.KEY_UP, onKey);
		}
		public virtual void dispose()
		{
			mStage.removeEventListener(AsKeyboardEvent.KEY_DOWN, onKey);
			mStage.removeEventListener(AsKeyboardEvent.KEY_UP, onKey);
		}
		public virtual void advanceTime(float passedTime)
		{
			int i = 0;
			AsTouch touch = null;
			mElapsedTime = (mElapsedTime + passedTime);
			mOffsetTime = 0.0f;
			if((mLastTaps.getLength() > 0))
			{
				for (i = (int)((mLastTaps.getLength() - 1)); (i >= 0); --i)
				{
					if(((mElapsedTime - mLastTaps[i].getTimestamp()) > MULTITAP_TIME))
					{
						mLastTaps.splice(i, (uint)(1));
					}
				}
			}
			while((mQueue.getLength() > 0))
			{
				sProcessedTouchIDs.setLength(0);
				sHoveringTouchData.setLength(0);
				AsVector<AsTouch> __currentTouchs_ = mCurrentTouches;
				if (__currentTouchs_ != null)
				{
					foreach (AsTouch currentTouch in __currentTouchs_)
					{
						if(((currentTouch.getPhase() == AsTouchPhase.BEGAN) || (currentTouch.getPhase() == AsTouchPhase.MOVED)))
						{
							currentTouch.setPhase(AsTouchPhase.STATIONARY);
						}
					}
				}
				AsVector<AsTouchData> __touchDatas_ = sHoveringTouchData;
				if (__touchDatas_ != null)
				{
					foreach (AsTouchData touchData in __touchDatas_)
					{
						if((touchData.touch.getTarget() != touchData.target))
						{
							touchData.target.dispatchEvent(new AsTouchEvent(AsTouchEvent.TOUCH, mCurrentTouches, mShiftDown, mCtrlDown));
						}
					}
				}
				AsVector<int> __touchIDs_ = sProcessedTouchIDs;
				if (__touchIDs_ != null)
				{
					foreach (int touchID in __touchIDs_)
					{
						touch = getCurrentTouch(touchID);
						if(touch.getTarget() != null)
						{
							touch.getTarget().dispatchEvent(new AsTouchEvent(AsTouchEvent.TOUCH, mCurrentTouches, mShiftDown, mCtrlDown));
						}
					}
				}
				for (i = (int)((mCurrentTouches.getLength() - 1)); (i >= 0); --i)
				{
					if((mCurrentTouches[i].getPhase() == AsTouchPhase.ENDED))
					{
						mCurrentTouches.splice(i, (uint)(1));
					}
				}
				mOffsetTime = (mOffsetTime + 0.00001f);
			}
		}
		public virtual void enqueue(int touchID, String phase, float globalX, float globalY)
		{
		}
		private void processTouch(int touchID, String phase, float globalX, float globalY)
		{
			AsPoint position = new AsPoint(globalX, globalY);
			AsTouch touch = getCurrentTouch(touchID);
			if((touch == null))
			{
				touch = new AsTouch(touchID, globalX, globalY, phase, null);
				addCurrentTouch(touch);
			}
			touch.setPosition(globalX, globalY);
			touch.setPhase(phase);
			touch.setTimestamp((mElapsedTime + mOffsetTime));
			if(((phase == AsTouchPhase.HOVER) || (phase == AsTouchPhase.BEGAN)))
			{
				touch.setTarget(mStage.hitTest(position, true));
			}
			if((phase == AsTouchPhase.BEGAN))
			{
				processTap(touch);
			}
		}
		private void onKey(AsEvent evt)
		{
			AsKeyboardEvent _event = ((AsKeyboardEvent)(evt));
			if(((_event.getKeyCode() == 17) || (_event.getKeyCode() == 15)))
			{
				bool wasCtrlDown = mCtrlDown;
				mCtrlDown = (_event.getType() == AsKeyboardEvent.KEY_DOWN);
			}
			else
			{
				if((_event.getKeyCode() == 16))
				{
					mShiftDown = (_event.getType() == AsKeyboardEvent.KEY_DOWN);
				}
			}
		}
		private void processTap(AsTouch touch)
		{
			AsTouch nearbyTap = null;
			float minSqDist = (MULTITAP_DISTANCE * MULTITAP_DISTANCE);
			AsVector<AsTouch> __taps_ = mLastTaps;
			if (__taps_ != null)
			{
				foreach (AsTouch tap in __taps_)
				{
					float sqDist = (AsMath.pow((tap.getGlobalX() - touch.getGlobalX()), 2) + AsMath.pow((tap.getGlobalY() - touch.getGlobalY()), 2));
					if((sqDist <= minSqDist))
					{
						nearbyTap = tap;
						break;
					}
				}
			}
			if(nearbyTap != null)
			{
				touch.setTapCount((nearbyTap.getTapCount() + 1));
				mLastTaps.splice(mLastTaps.indexOf(nearbyTap), (uint)(1));
			}
			else
			{
				touch.setTapCount(1);
			}
			mLastTaps.push(touch.clone());
		}
		private void addCurrentTouch(AsTouch touch)
		{
			int i = (int)((mCurrentTouches.getLength() - 1));
			for (; (i >= 0); --i)
			{
				if((mCurrentTouches[i].getId() == touch.getId()))
				{
					mCurrentTouches.splice(i, (uint)(1));
				}
			}
			mCurrentTouches.push(touch);
		}
		private AsTouch getCurrentTouch(int touchID)
		{
			AsVector<AsTouch> __touchs_ = mCurrentTouches;
			if (__touchs_ != null)
			{
				foreach (AsTouch touch in __touchs_)
				{
					if((touch.getId() == touchID))
					{
						return touch;
					}
				}
			}
			return null;
		}
	}
}
