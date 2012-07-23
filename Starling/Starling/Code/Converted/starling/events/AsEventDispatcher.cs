using System;
 
using bc.flash;
using starling.display;
using starling.events;
 
namespace starling.events
{
	public delegate void AsEventListenerCallback(AsEvent _event);
	public class AsEventDispatcher : AsObject
	{
		private AsDictionary mEventListeners;
		public AsEventDispatcher()
		{
		}
		public virtual void addEventListener(String type, AsEventListenerCallback listener)
		{
			if((mEventListeners == null))
			{
				mEventListeners = new AsDictionary();
			}
			AsVector<AsEventListenerCallback> listeners = (AsVector<AsEventListenerCallback>)(mEventListeners[type]);
			if((listeners == null))
			{
				mEventListeners[type] = new AsVector<AsEventListenerCallback>();
			}
			else
			{
				if((listeners.indexOf(listener) == -1))
				{
					mEventListeners[type] = listeners.concat(new AsVector<AsEventListenerCallback>());
				}
			}
		}
		public virtual void removeEventListener(String type, AsEventListenerCallback listener)
		{
			if(mEventListeners != null)
			{
				AsVector<AsEventListenerCallback> listeners = (AsVector<AsEventListenerCallback>)(mEventListeners[type]);
				if(listeners != null)
				{
				}
			}
		}
		public virtual void removeEventListeners(String type)
		{
			if(((type != null) && (mEventListeners != null)))
			{
				mEventListeners.remove(type);
			}
			else
			{
				mEventListeners = null;
			}
		}
		public virtual void removeEventListeners()
		{
			removeEventListeners(null);
		}
		public virtual void dispatchEvent(AsEvent _event)
		{
			AsVector<AsEventListenerCallback> listeners = (AsVector<AsEventListenerCallback>)(((mEventListeners != null) ? (mEventListeners[_event.getType()]) : (null)));
			if(((listeners == null) && !(_event.getBubbles())))
			{
				return;
			}
			AsEventDispatcher previousTarget = _event.getTarget();
			if(((previousTarget == null) || (_event.getCurrentTarget() != null)))
			{
				_event.setTarget(this);
			}
			bool stopImmediatePropagation = false;
			int numListeners = (((listeners == null)) ? (0) : (listeners.getLength()));
			if((numListeners != 0))
			{
				_event.setCurrentTarget(this);
			}
			if((((!(stopImmediatePropagation) && _event.getBubbles()) && !(_event.getStopsPropagation())) && this is AsDisplayObject))
			{
				AsDisplayObject targetDisplayObject = ((this is AsDisplayObject) ? ((AsDisplayObject)(this)) : null);
				if((targetDisplayObject.getParent() != null))
				{
					_event.setCurrentTarget(null);
					targetDisplayObject.getParent().dispatchEvent(_event);
				}
			}
			if(previousTarget != null)
			{
				_event.setTarget(previousTarget);
			}
		}
		public virtual void dispatchEventWith(String type, bool bubbles, AsObject data)
		{
			if((bubbles || hasEventListener(type)))
			{
				AsEvent _event = AsEvent.fromPool(type, bubbles, (AsObject)(data));
				dispatchEvent(_event);
				AsEvent.toPool(_event);
			}
		}
		public virtual void dispatchEventWith(String type, bool bubbles)
		{
			dispatchEventWith(type, bubbles, null);
		}
		public virtual void dispatchEventWith(String type)
		{
			dispatchEventWith(type, false, null);
		}
		public virtual bool hasEventListener(String type)
		{
			return ((mEventListeners != null) && (type in mEventListeners));
		}
	}
}
