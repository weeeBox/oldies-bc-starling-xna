using System;
 
using bc.flash;
using bc.flash.geom;
using starling.core;
using starling.display;
using starling.events;
 
namespace starling.core
{
	public class AsTouchProcessor : AsObject
	{
		private static float MULTITAP_TIME = 0.3f;
		private static float MULTITAP_DISTANCE = 25;
		private AsStage mStage;
		private float mElapsedTime;
		private float mOffsetTime;
		private AsTouchMarker mTouchMarker;
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
			if(mTouchMarker != null)
			{
				mTouchMarker.dispose();
			}
		}
		public virtual void advanceTime(float passedTime)
		{
			int i = 0;
			int touchID = 0;
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
				sProcessedTouchIDs.setLength(sHoveringTouchData.setLength(0));
				AsVector<AsTouch> __touchs_ = mCurrentTouches;
				if (__touchs_ != null)
				{
					foreach (AsTouch touch in __touchs_)
					{
						if(((touch.getPhase() == AsTouchPhase.BEGAN) || (touch.getPhase() == AsTouchPhase.MOVED)))
						{
							touch.setPhase(AsTouchPhase.STATIONARY);
						}
						if(((touch.getTarget() != null) && (touch.getTarget().getStage() == null)))
						{
							touch.setTarget(mStage.hitTest(new AsPoint(touch.getGlobalX(), touch.getGlobalY()), true));
						}
					}
				}
				while(((mQueue.getLength() > 0) && (sProcessedTouchIDs.indexOf((AsObject)(mQueue[(mQueue.getLength() - 1)][0])) == -1)))
				{
					AsArray touchArgs = mQueue.pop();
					// FIXME: Block of code is cut here
					touch = getCurrentTouch(touchID);
					if((((touch != null) && (touch.getPhase() == AsTouchPhase.HOVER)) && (touch.getTarget() != null)))
					{
						sHoveringTouchData.push(new AsTouchData(touch, touch.getTarget()));
					}
					// FIXME: Block of code is cut here
					sProcessedTouchIDs.push(touchID);
				}
				AsVector<AsTouchData> __touchDatas_ = sHoveringTouchData;
				if (__touchDatas_ != null)
				{
					foreach (AsTouchData touchData in __touchDatas_)
					{
						if((touchData.getTouch().getTarget() != touchData.getTarget()))
						{
							touchData.getTarget().dispatchEvent(new AsTouchEvent(AsTouchEvent.TOUCH, mCurrentTouches, mShiftDown, mCtrlDown));
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
			// FIXME: Block of code is cut here
			if(((mCtrlDown && getSimulateMultitouch()) && (touchID == 0)))
			{
				mTouchMarker.moveMarker(globalX, globalY, mShiftDown);
				mQueue.unshift(new AsArray(1, phase, mTouchMarker.getMockX(), mTouchMarker.getMockY()));
			}
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
		private void onKey(AsKeyboardEvent _event)
		{
			if(((_event.getKeyCode() == 17) || (_event.getKeyCode() == 15)))
			{
				bool wasCtrlDown = mCtrlDown;
				mCtrlDown = (_event.getType() == AsKeyboardEvent.KEY_DOWN);
				if((getSimulateMultitouch() && (wasCtrlDown != mCtrlDown)))
				{
					mTouchMarker.setVisible(mCtrlDown);
					mTouchMarker.moveCenter((mStage.getStageWidth() / 2), (mStage.getStageHeight() / 2));
					AsTouch mouseTouch = getCurrentTouch(0);
					AsTouch mockedTouch = getCurrentTouch(1);
					if(mouseTouch != null)
					{
						mTouchMarker.moveMarker(mouseTouch.getGlobalX(), mouseTouch.getGlobalY());
					}
					if(((wasCtrlDown && (mockedTouch != null)) && (mockedTouch.getPhase() != AsTouchPhase.ENDED)))
					{
						mQueue.unshift(new AsArray(1, AsTouchPhase.ENDED, mockedTouch.getGlobalX(), mockedTouch.getGlobalY()));
					}
					else
					{
						if((mCtrlDown && (mouseTouch != null)))
						{
							if(((mouseTouch.getPhase() == AsTouchPhase.BEGAN) || (mouseTouch.getPhase() == AsTouchPhase.MOVED)))
							{
								mQueue.unshift(new AsArray(1, AsTouchPhase.BEGAN, mTouchMarker.getMockX(), mTouchMarker.getMockY()));
							}
							else
							{
								mQueue.unshift(new AsArray(1, AsTouchPhase.HOVER, mTouchMarker.getMockX(), mTouchMarker.getMockY()));
							}
						}
					}
				}
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
		public virtual bool getSimulateMultitouch()
		{
			return (mTouchMarker != null);
		}
		public virtual void setSimulateMultitouch(bool _value)
		{
			if((getSimulateMultitouch() == _value))
			{
				return;
			}
			if(_value)
			{
				mTouchMarker = new AsTouchMarker();
				mTouchMarker.setVisible(false);
				mStage.addChild(mTouchMarker);
			}
			else
			{
				mTouchMarker.removeFromParent(true);
				mTouchMarker = null;
			}
		}
	}
}
