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
		private AsTouchMarker mTouchMarker;
		private AsVector<AsTouch> mCurrentTouches;
		private AsVector<AsArray> mQueue;
		private AsVector<AsTouch> mLastTaps;
		private bool mShiftDown = false;
		private bool mCtrlDown = false;
		private static AsVector<int> sProcessedTouchIDs = new AsVector<int>();
		private static AsVector<Object> sHoveringTouchData = new AsVector<Object>();
		private static AsVector<AsEventDispatcher> sBubbleChain = new AsVector<AsEventDispatcher>();
		public AsTouchProcessor(AsStage stage)
		{
			mStage = stage;
			mElapsedTime = 0.0f;
			mCurrentTouches = new AsVector<AsTouch>();
			mQueue = new AsVector<AsArray>();
			mLastTaps = new AsVector<AsTouch>();
			mStage.addEventListener(AsKeyboardEvent.KEY_DOWN, onKey);
			mStage.addEventListener(AsKeyboardEvent.KEY_UP, onKey);
			monitorInterruptions(true);
		}
		public virtual void dispose()
		{
			monitorInterruptions(false);
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
			mElapsedTime = mElapsedTime + passedTime;
			if(mLastTaps.getLength() > 0)
			{
				for (i = (int)(mLastTaps.getLength() - 1); i >= 0; --i)
				{
					if(mElapsedTime - mLastTaps[i].getTimestamp() > MULTITAP_TIME)
					{
						mLastTaps.splice(i, (uint)(1));
					}
				}
			}
			while(mQueue.getLength() > 0)
			{
				sProcessedTouchIDs.setLength(sHoveringTouchData.setLength(0));
				AsVector<AsTouch> __touchs_ = mCurrentTouches;
				if (__touchs_ != null)
				{
					foreach (AsTouch touch in __touchs_)
					{
						if(touch.getPhase() == AsTouchPhase.BEGAN || touch.getPhase() == AsTouchPhase.MOVED)
						{
							touch.setPhase(AsTouchPhase.STATIONARY);
						}
					}
				}
				while(mQueue.getLength() > 0 && sProcessedTouchIDs.indexOf(mQueue[mQueue.getLength() - 1][0]) == -1)
				{
					AsArray touchArgs = mQueue.pop();
					touch = getCurrentTouch(touchID);
					if(touch != null && touch.getPhase() == AsTouchPhase.HOVER && touch.getTarget() != null)
					{
						sHoveringTouchData.push((As_AS_REST)(AsObject.createLiteralObject("touch", touch, "target", touch.getTarget())));
					}
					this.(int)(touchArgs[0]), (String)(touchArgs[1]), (float)(touchArgs[2]), (float)(touchArgs[3]), (float)(touchArgs[4]), (float)(touchArgs[5]), (float)(touchArgs[6]);
					sProcessedTouchIDs.push(touchID);
				}
				AsTouchEvent touchEvent = new AsTouchEvent(AsTouchEvent.TOUCH, mCurrentTouches, mShiftDown, mCtrlDown);
				AsVector<Object> __touchDatas_ = sHoveringTouchData;
				if (__touchDatas_ != null)
				{
					foreach (Object touchData in __touchDatas_)
					{
						if(((AsObject)(((AsObject)(touchData)).getOwnProperty("touch"))).getOwnProperty("target") != ((AsObject)(touchData)).getOwnProperty("target"))
						{
							touchEvent.dispatch(getBubbleChain((AsDisplayObject)(((AsObject)(touchData)).getOwnProperty("target")), sBubbleChain));
						}
					}
				}
				AsVector<int> __touchIDs_ = sProcessedTouchIDs;
				if (__touchIDs_ != null)
				{
					foreach (int touchID in __touchIDs_)
					{
						getCurrentTouch(touchID).dispatchEvent(touchEvent);
					}
				}
				for (i = (int)(mCurrentTouches.getLength() - 1); i >= 0; --i)
				{
					if(mCurrentTouches[i].getPhase() == AsTouchPhase.ENDED)
					{
						mCurrentTouches.splice(i, (uint)(1));
					}
				}
			}
			sBubbleChain.setLength(0);
		}
		private AsVector<AsEventDispatcher> getBubbleChain(AsDisplayObject target, AsVector<AsEventDispatcher> result)
		{
			AsDisplayObject element = target;
			int length = 1;
			result.setLength(length);
			result[0] = target;
			while((element = element.getParent()) != null)
			{
				result[length++] = element;
			}
			return result;
		}
		public virtual void enqueue(int touchID, String phase, float globalX, float globalY, float pressure, float width, float height)
		{
			mQueue.unshift(touchID, phase, globalX, globalY, pressure, width, height);
			if(mCtrlDown && getSimulateMultitouch() && touchID == 0)
			{
				mTouchMarker.moveMarker(globalX, globalY, mShiftDown);
				mQueue.unshift(new AsArray(1, phase, mTouchMarker.getMockX(), mTouchMarker.getMockY()));
			}
		}
		public virtual void enqueue(int touchID, String phase, float globalX, float globalY, float pressure, float width)
		{
			enqueue(touchID, phase, globalX, globalY, pressure, width, 1.0f);
		}
		public virtual void enqueue(int touchID, String phase, float globalX, float globalY, float pressure)
		{
			enqueue(touchID, phase, globalX, globalY, pressure, 1.0f, 1.0f);
		}
		public virtual void enqueue(int touchID, String phase, float globalX, float globalY)
		{
			enqueue(touchID, phase, globalX, globalY, 1.0f, 1.0f, 1.0f);
		}
		private void processTouch(int touchID, String phase, float globalX, float globalY, float pressure, float width, float height)
		{
			AsPoint position = new AsPoint(globalX, globalY);
			AsTouch touch = getCurrentTouch(touchID);
			if(touch == null)
			{
				touch = new AsTouch(touchID, globalX, globalY, phase, null);
				addCurrentTouch(touch);
			}
			touch.setPosition(globalX, globalY);
			touch.setPhase(phase);
			touch.setTimestamp(mElapsedTime);
			touch.setPressure(pressure);
			touch.setSize(width, height);
			if(phase == AsTouchPhase.HOVER || phase == AsTouchPhase.BEGAN)
			{
				touch.setTarget(mStage.hitTest(position, true));
			}
			if(phase == AsTouchPhase.BEGAN)
			{
				processTap(touch);
			}
		}
		private void processTouch(int touchID, String phase, float globalX, float globalY, float pressure, float width)
		{
			processTouch(touchID, phase, globalX, globalY, pressure, width, 1.0f);
		}
		private void processTouch(int touchID, String phase, float globalX, float globalY, float pressure)
		{
			processTouch(touchID, phase, globalX, globalY, pressure, 1.0f, 1.0f);
		}
		private void processTouch(int touchID, String phase, float globalX, float globalY)
		{
			processTouch(touchID, phase, globalX, globalY, 1.0f, 1.0f, 1.0f);
		}
		private void onKey(AsKeyboardEvent _event)
		{
			if(_event.getKeyCode() == 17 || _event.getKeyCode() == 15)
			{
				bool wasCtrlDown = mCtrlDown;
				mCtrlDown = _event.getType() == AsKeyboardEvent.KEY_DOWN;
				if(getSimulateMultitouch() && wasCtrlDown != mCtrlDown)
				{
					mTouchMarker.setVisible(mCtrlDown);
					mTouchMarker.moveCenter(mStage.getStageWidth() / 2, mStage.getStageHeight() / 2);
					AsTouch mouseTouch = getCurrentTouch(0);
					AsTouch mockedTouch = getCurrentTouch(1);
					if(mouseTouch != null)
					{
						mTouchMarker.moveMarker(mouseTouch.getGlobalX(), mouseTouch.getGlobalY());
					}
					if(wasCtrlDown && mockedTouch != null && mockedTouch.getPhase() != AsTouchPhase.ENDED)
					{
						mQueue.unshift(new AsArray(1, AsTouchPhase.ENDED, mockedTouch.getGlobalX(), mockedTouch.getGlobalY()));
					}
					else
					{
						if(mCtrlDown && mouseTouch != null)
						{
							if(mouseTouch.getPhase() == AsTouchPhase.BEGAN || mouseTouch.getPhase() == AsTouchPhase.MOVED)
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
				if(_event.getKeyCode() == 16)
				{
					mShiftDown = _event.getType() == AsKeyboardEvent.KEY_DOWN;
				}
			}
		}
		private void processTap(AsTouch touch)
		{
			AsTouch nearbyTap = null;
			float minSqDist = MULTITAP_DISTANCE * MULTITAP_DISTANCE;
			AsVector<AsTouch> __taps_ = mLastTaps;
			if (__taps_ != null)
			{
				foreach (AsTouch tap in __taps_)
				{
					float sqDist = AsMath.pow(tap.getGlobalX() - touch.getGlobalX(), 2) + AsMath.pow(tap.getGlobalY() - touch.getGlobalY(), 2);
					if(sqDist <= minSqDist)
					{
						nearbyTap = tap;
						break;
					}
				}
			}
			if(nearbyTap != null)
			{
				touch.setTapCount(nearbyTap.getTapCount() + 1);
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
			int i = (int)(mCurrentTouches.getLength() - 1);
			for (; i >= 0; --i)
			{
				if(mCurrentTouches[i].getId() == touch.getId())
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
					if(touch.getId() == touchID)
					{
						return touch;
					}
				}
			}
			return null;
		}
		public virtual bool getSimulateMultitouch()
		{
			return mTouchMarker != null;
		}
		public virtual void setSimulateMultitouch(bool _value)
		{
			if(getSimulateMultitouch() == _value)
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
		private void monitorInterruptions(bool enable)
		{
		}
		private void onInterruption(Object _event)
		{
			AsTouch touch = null;
			String phase = null;
			AsVector<AsTouch> __touchs_ = mCurrentTouches;
			if (__touchs_ != null)
			{
				foreach (AsTouch touch in __touchs_)
				{
					if(touch.getPhase() == AsTouchPhase.BEGAN || touch.getPhase() == AsTouchPhase.MOVED || touch.getPhase() == AsTouchPhase.STATIONARY)
					{
						touch.setPhase(AsTouchPhase.ENDED);
					}
				}
			}
			AsTouchEvent touchEvent = new AsTouchEvent(AsTouchEvent.TOUCH, mCurrentTouches, mShiftDown, mCtrlDown);
			AsVector<AsTouch> __touchs_ = mCurrentTouches;
			if (__touchs_ != null)
			{
				foreach (AsTouch touch in __touchs_)
				{
					touch.dispatchEvent(touchEvent);
				}
			}
			mCurrentTouches.setLength(0);
		}
	}
}
