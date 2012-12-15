using System;
 
using bc.flash;
using starling.display;
using starling.events;
 
namespace starling.events
{
	public class AsTouchEvent : AsEvent
	{
		public static String TOUCH = "touch";
		private bool mShiftKey;
		private bool mCtrlKey;
		private float mTimestamp;
		private AsVector<AsEventDispatcher> mVisitedObjects;
		private static AsVector<AsTouch> sTouches = new AsVector<AsTouch>();
		public AsTouchEvent(String type, AsVector<AsTouch> touches, bool shiftKey, bool ctrlKey, bool bubbles)
		 : base(type, bubbles, touches)
		{
			mShiftKey = shiftKey;
			mCtrlKey = ctrlKey;
			mTimestamp = -1.0f;
			mVisitedObjects = new AsVector<AsEventDispatcher>();
			int numTouches = (int)(touches.getLength());
			int i = 0;
			for (; i < numTouches; ++i)
			{
				if(touches[i].getTimestamp() > mTimestamp)
				{
					mTimestamp = touches[i].getTimestamp();
				}
			}
		}
		public AsTouchEvent(String type, AsVector<AsTouch> touches, bool shiftKey, bool ctrlKey)
		 : this(type, touches, shiftKey, ctrlKey, true)
		{
		}
		public AsTouchEvent(String type, AsVector<AsTouch> touches, bool shiftKey)
		 : this(type, touches, shiftKey, false, true)
		{
		}
		public AsTouchEvent(String type, AsVector<AsTouch> touches)
		 : this(type, touches, false, false, true)
		{
		}
		public virtual AsVector<AsTouch> getTouches(AsDisplayObject target, String phase, AsVector<AsTouch> result)
		{
			if(result == null)
			{
				result = new AsVector<AsTouch>();
			}
			AsVector<AsTouch> allTouches = getData() as AsVector<AsTouch>;
			int numTouches = (int)(allTouches.getLength());
			int i = 0;
			for (; i < numTouches; ++i)
			{
				AsTouch touch = allTouches[i];
				bool correctTarget = (touch.getTarget() == target) || ((target is AsDisplayObjectContainer) && target as AsDisplayObjectContainer.contains(touch.getTarget()));
				bool correctPhase = phase == null || phase == touch.getPhase();
				if(correctTarget && correctPhase)
				{
					result.push(touch);
				}
			}
			return result;
		}
		public virtual AsVector<AsTouch> getTouches(AsDisplayObject target, String phase)
		{
			return getTouches(target, phase, null);
		}
		public virtual AsVector<AsTouch> getTouches(AsDisplayObject target)
		{
			return getTouches(target, null, null);
		}
		public virtual AsTouch getTouch(AsDisplayObject target, String phase)
		{
			getTouches(target, phase, sTouches);
			if(sTouches.getLength() != 0)
			{
				AsTouch touch = sTouches[0];
				sTouches.setLength(0);
				return touch;
			}
			else
			{
				return null;
			}
		}
		public virtual AsTouch getTouch(AsDisplayObject target)
		{
			return getTouch(target, null);
		}
		public virtual bool interactsWith(AsDisplayObject target)
		{
			if(getTouch(target) == null)
			{
				return false;
			}
			else
			{
				AsVector<AsTouch> touches = getTouches(target);
				int i = (int)(touches.getLength() - 1);
				for (; i >= 0; --i)
				{
					if(touches[i].getPhase() != AsTouchPhase.ENDED)
					{
						return true;
					}
				}
				return false;
			}
		}
		public virtual void dispatch(AsVector<AsEventDispatcher> chain)
		{
			if(chain != null && chain.getLength() != 0)
			{
				int chainLength = (int)(getBubbles() ? chain.getLength() : 1);
				AsEventDispatcher previousTarget = getTarget();
				setTarget(chain[0] as AsEventDispatcher);
				int i = 0;
				for (; i < chainLength; ++i)
				{
					AsEventDispatcher chainElement = chain[i] as AsEventDispatcher;
					if(mVisitedObjects.indexOf(chainElement) == -1)
					{
						bool stopPropagation = chainElement.invokeEvent(this);
						mVisitedObjects.push(chainElement);
						if(stopPropagation)
						{
							break;
						}
					}
				}
				setTarget(previousTarget);
			}
		}
		public virtual float getTimestamp()
		{
			return mTimestamp;
		}
		public virtual AsVector<AsTouch> getTouches()
		{
			return getData() as AsVector<AsTouch>.concat();
		}
		public virtual bool getShiftKey()
		{
			return mShiftKey;
		}
		public virtual bool getCtrlKey()
		{
			return mCtrlKey;
		}
	}
}
