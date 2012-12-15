using System;
 
using bc.flash;
using starling.events;
 
namespace starling.events
{
	public class AsEvent : AsObject
	{
		public static String ADDED = "added";
		public static String ADDED_TO_STAGE = "addedToStage";
		public static String ENTER_FRAME = "enterFrame";
		public static String REMOVED = "removed";
		public static String REMOVED_FROM_STAGE = "removedFromStage";
		public static String TRIGGERED = "triggered";
		public static String FLATTEN = "flatten";
		public static String RESIZE = "resize";
		public static String COMPLETE = "complete";
		public static String CONTEXT3D_CREATE = "context3DCreate";
		public static String ROOT_CREATED = "rootCreated";
		public static String REMOVE_FROM_JUGGLER = "removeFromJuggler";
		public static String CHANGE = "change";
		public static String CANCEL = "cancel";
		public static String SCROLL = "scroll";
		public static String OPEN = "open";
		public static String CLOSE = "close";
		public static String SELECT = "select";
		private static AsVector<AsEvent> sEventPool = new AsVector<AsEvent>();
		private AsEventDispatcher mTarget;
		private AsEventDispatcher mCurrentTarget;
		private String mType;
		private bool mBubbles;
		private bool mStopsPropagation;
		private bool mStopsImmediatePropagation;
		private Object mData;
		public AsEvent(String type, bool bubbles, Object data)
		{
			mType = type;
			mBubbles = bubbles;
			mData = data;
		}
		public AsEvent(String type, bool bubbles)
		 : this(type, bubbles, null)
		{
		}
		public AsEvent(String type)
		 : this(type, false, null)
		{
		}
		public virtual void stopPropagation()
		{
			mStopsPropagation = true;
		}
		public virtual void stopImmediatePropagation()
		{
			mStopsPropagation = mStopsImmediatePropagation = true;
		}
		public virtual String toString()
		{
			return AsGlobal.formatString("[{0} type=\"{1}\" bubbles={2}]", (As_AS_REST)(AsString.split(AsGlobal.getQualifiedClassName(this), "::").pop()), mType, mBubbles);
		}
		public virtual bool getBubbles()
		{
			return mBubbles;
		}
		public virtual AsEventDispatcher getTarget()
		{
			return mTarget;
		}
		public virtual AsEventDispatcher getCurrentTarget()
		{
			return mCurrentTarget;
		}
		public virtual String getType()
		{
			return mType;
		}
		public virtual Object getData()
		{
			return mData;
		}
		public virtual void setTarget(AsEventDispatcher _value)
		{
			mTarget = _value;
		}
		public virtual void setCurrentTarget(AsEventDispatcher _value)
		{
			mCurrentTarget = _value;
		}
		public virtual void setData(Object _value)
		{
			mData = _value;
		}
		public virtual bool getStopsPropagation()
		{
			return mStopsPropagation;
		}
		public virtual bool getStopsImmediatePropagation()
		{
			return mStopsImmediatePropagation;
		}
		public static AsEvent fromPool(String type, bool bubbles, Object data)
		{
			if(sEventPool.getLength() != 0)
			{
				return sEventPool.pop().reset(type, bubbles, data);
			}
			else
			{
				return new AsEvent(type, bubbles, data);
			}
		}
		public static AsEvent fromPool(String type, bool bubbles)
		{
			return fromPool(type, bubbles, null);
		}
		public static AsEvent fromPool(String type)
		{
			return fromPool(type, false, null);
		}
		public static void toPool(AsEvent _event)
		{
			_event.mData = _event.mTarget = _event.mCurrentTarget = null;
			sEventPool.push(_event);
		}
		public virtual AsEvent reset(String type, bool bubbles, Object data)
		{
			mType = type;
			mBubbles = bubbles;
			mData = data;
			mTarget = mCurrentTarget = null;
			mStopsPropagation = mStopsImmediatePropagation = false;
			return this;
		}
		public virtual AsEvent reset(String type, bool bubbles)
		{
			return reset(type, bubbles, null);
		}
		public virtual AsEvent reset(String type)
		{
			return reset(type, false, null);
		}
	}
}
