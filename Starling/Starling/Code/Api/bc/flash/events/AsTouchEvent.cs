using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.events;
 
namespace bc.flash.events
{
	public class AsTouchEvent : AsEvent
	{
		public static String TOUCH = "touch";
		public static String TOUCH_BEGIN = "touchBegin";
		public static String TOUCH_END = "touchEnd";
		public static String TOUCH_MOVE = "touchMove";
		public static String TOUCH_OVER = "touchOver";
		public static String TOUCH_OUT = "touchOut";
		public static String TOUCH_ROLL_OVER = "touchRollOver";
		public static String TOUCH_ROLL_OUT = "touchRollOut";
		public static String TOUCH_TAP = "touchTap";
		public static String PROXIMITY_BEGIN = "proximityBegin";
		public static String PROXIMITY_END = "proximityEnd";
		public static String PROXIMITY_MOVE = "proximityMove";
		public static String PROXIMITY_OUT = "proximityOut";
		public static String PROXIMITY_OVER = "proximityOver";
		public static String PROXIMITY_ROLL_OUT = "proximityRollOut";
		public static String PROXIMITY_ROLL_OVER = "proximityRollOver";
		private AsVector<AsTouch> mTouches;
		private bool mShiftKey;
		private bool mCtrlKey;
		private float mTimestamp;
		public AsTouchEvent(String type, AsVector<AsTouch> touches, bool shiftKey, bool ctrlKey, bool bubbles)
		 : base(type, bubbles)
		{
			mTouches = ((touches != null) ? (touches) : (new AsVector<AsTouch>()));
			mShiftKey = shiftKey;
			mCtrlKey = ctrlKey;
			mTimestamp = -1.0f;
			int numTouches = (int)(touches.getLength());
			int i = 0;
			for (; (i < numTouches); ++i)
			{
				if((touches[i].getTimestamp() > mTimestamp))
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
		public virtual AsVector<AsTouch> getTouches(AsDisplayObject target, String phase)
		{
			AsVector<AsTouch> touchesFound = new AsVector<AsTouch>();
			int numTouches = (int)(mTouches.getLength());
			int i = 0;
			for (; (i < numTouches); ++i)
			{
				AsTouch touch = mTouches[i];
				bool correctTarget = ((touch.getTarget() == target) || (target is AsDisplayObjectContainer && ((target is AsDisplayObjectContainer) ? ((AsDisplayObjectContainer)(target)) : null).contains(touch.getTarget())));
				bool correctPhase = ((phase == null) || (phase == touch.getPhase()));
				if((correctTarget && correctPhase))
				{
					touchesFound.push(touch);
				}
			}
			return touchesFound;
		}
		public virtual AsVector<AsTouch> getTouches(AsDisplayObject target)
		{
			return getTouches(target, null);
		}
		public virtual AsTouch getTouch(AsDisplayObject target, String phase)
		{
			AsVector<AsTouch> touchesFound = getTouches(target, phase);
			if((touchesFound.getLength() > 0))
			{
				return touchesFound[0];
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
			if((getTouch(target) == null))
			{
				return false;
			}
			else
			{
				AsVector<AsTouch> touches = getTouches(target);
				int i = (int)((touches.getLength() - 1));
				for (; (i >= 0); --i)
				{
					if((touches[i].getPhase() != AsTouchPhase.ENDED))
					{
						return true;
					}
				}
				return false;
			}
		}
		public virtual float getTimestamp()
		{
			return mTimestamp;
		}
		public virtual AsVector<AsTouch> getTouches()
		{
			return mTouches.concat();
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
