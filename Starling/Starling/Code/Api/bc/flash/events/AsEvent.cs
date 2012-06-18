using System;
 
using bc.flash;
using bc.flash.events;
 
namespace bc.flash.events
{
	public class AsEvent : AsObject
	{
		public static String ACTIVATE = "activate";
		public static String CANCEL = "cancel";
		public static String CHANGE = "change";
		public static String CLEAR = "clear";
		public static String CLOSE = "close";
		public static String CONNECT = "connect";
		public static String COPY = "copy";
		public static String CUT = "cut";
		public static String DEACTIVATE = "deactivate";
		public static String FRAME_CONSTRUCTED = "frameConstructed";
		public static String EXIT_FRAME = "exitFrame";
		public static String ID3 = "id3";
		public static String INIT = "init";
		public static String MOUSE_LEAVE = "mouseLeave";
		public static String OPEN = "open";
		public static String PASTE = "paste";
		public static String RENDER = "render";
		public static String SCROLL = "scroll";
		public static String TEXT_INTERACTION_MODE_CHANGE = "textInteractionModeChange";
		public static String SELECT = "select";
		public static String SELECT_ALL = "selectAll";
		public static String SOUND_COMPLETE = "soundComplete";
		public static String TAB_CHILDREN_CHANGE = "tabChildrenChange";
		public static String TAB_ENABLED_CHANGE = "tabEnabledChange";
		public static String TAB_INDEX_CHANGE = "tabIndexChange";
		public static String UNLOAD = "unload";
		public static String FULLSCREEN = "fullScreen";
		public static String ADDED = "added";
		public static String ADDED_TO_STAGE = "addedToStage";
		public static String ENTER_FRAME = "enterFrame";
		public static String REMOVED = "removed";
		public static String REMOVED_FROM_STAGE = "removedFromStage";
		public static String TRIGGERED = "triggered";
		public static String MOVIE_COMPLETED = "movieCompleted";
		public static String FLATTEN = "flatten";
		public static String RESIZE = "resize";
		public static String COMPLETE = "complete";
		public static String CONTEXT3D_CREATE = "context3DCreate";
		private AsEventDispatcher mTarget;
		private AsEventDispatcher mCurrentTarget;
		private String mType;
		private bool mBubbles;
		private bool mStopsPropagation;
		private bool mStopsImmediatePropagation;
		public AsEvent(String type, bool bubbles)
		{
			mType = type;
			mBubbles = bubbles;
		}
		public AsEvent(String type)
		 : this(type, false)
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
			return ("Event " + mType);
		}
		public virtual void setTarget(AsEventDispatcher target)
		{
			mTarget = target;
		}
		public virtual void setCurrentTarget(AsEventDispatcher currentTarget)
		{
			mCurrentTarget = currentTarget;
		}
		public virtual bool getStopsPropagation()
		{
			return mStopsPropagation;
		}
		public virtual bool getStopsImmediatePropagation()
		{
			return mStopsImmediatePropagation;
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
	}
}
