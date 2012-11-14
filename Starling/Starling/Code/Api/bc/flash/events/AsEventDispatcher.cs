using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.events;
 
namespace bc.flash.events
{
	public class AsEventDispatcher : AsObject
	{
		private AsDictionary mEventListeners;
		public AsEventDispatcher()
		{
		}
		public virtual void addEventListener(String type, AsEventListenerCallback listener, bool useCapture, int priority, bool useWeakReference)
		{
			if((mEventListeners == null))
			{
				mEventListeners = new AsDictionary();
			}
			AsVector<AsEventListenerCallback> listeners = (AsVector<AsEventListenerCallback>)(mEventListeners[type]);
			if((listeners == null))
			{
				listeners = new AsVector<AsEventListenerCallback>();
				mEventListeners[type] = listeners;
			}
			listeners.push(listener);
		}
		public virtual void addEventListener(String type, AsEventListenerCallback listener, bool useCapture, int priority)
		{
			addEventListener(type, listener, useCapture, priority, false);
		}
		public virtual void addEventListener(String type, AsEventListenerCallback listener, bool useCapture)
		{
			addEventListener(type, listener, useCapture, 0, false);
		}
		public virtual void addEventListener(String type, AsEventListenerCallback listener)
		{
			addEventListener(type, listener, false, 0, false);
		}
		public virtual void removeEventListener(String type, AsEventListenerCallback listener, bool useCapture)
		{
			if(mEventListeners != null)
			{
				AsVector<AsEventListenerCallback> listeners = (AsVector<AsEventListenerCallback>)(mEventListeners[type]);
				if(listeners != null)
				{
					AsVector<AsEventListenerCallback> remainListeners = new AsVector<AsEventListenerCallback>();
					AsVector<AsEventListenerCallback> __eventListeners_ = listeners;
					if (__eventListeners_ != null)
					{
						foreach (AsEventListenerCallback eventListener in __eventListeners_)
						{
							if((eventListener != listener))
							{
								remainListeners.push(eventListener);
							}
						}
					}
					if((remainListeners.getLength() > 0))
					{
						mEventListeners[type] = remainListeners;
					}
					else
					{
						mEventListeners.remove(type);
					}
				}
			}
		}
		public virtual void removeEventListener(String type, AsEventListenerCallback listener)
		{
			removeEventListener(type, listener, false);
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
			uint numListeners = (uint)((((listeners == null)) ? (0) : (listeners.getLength())));
			if((numListeners != 0))
			{
				_event.setCurrentTarget(this);
				int i = 0;
				for (; (i < numListeners); ++i)
				{
					AsEventListenerCallback listener = listeners[i];
					listener(_event);
					if(_event.getStopsImmediatePropagation())
					{
						stopImmediatePropagation = true;
						break;
					}
				}
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
		public virtual bool hasEventListener(String type)
		{
			return ((mEventListeners != null) && (mEventListeners[type] != null));
		}
	}
}
