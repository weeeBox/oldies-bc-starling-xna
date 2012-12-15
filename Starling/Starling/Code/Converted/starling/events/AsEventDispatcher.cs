using System;
 
using bc.flash;
using starling.display;
using starling.events;
 
namespace starling.events
{
	public class AsEventDispatcher : AsObject
	{
		private AsDictionary mEventListeners;
		private static AsArray sBubbleChains = new AsArray();
		public AsEventDispatcher()
		{
		}
		public virtual void addEventListener(String type, AsEventListenerCallback listener)
		{
			if(mEventListeners == null)
			{
				mEventListeners = new AsDictionary();
			}
			AsVector<AsEventListenerCallback> listeners = mEventListeners[type] as AsVector<AsEventListenerCallback>;
			if(listeners == null)
			{
				mEventListeners[type] = new AsVector<AsEventListenerCallback>();
			}
			else
			{
				if(listeners.indexOf(listener) == -1)
				{
					listeners.push(listener);
				}
			}
		}
		public virtual void removeEventListener(String type, AsEventListenerCallback listener)
		{
			if(mEventListeners != null)
			{
				AsVector<AsEventListenerCallback> listeners = mEventListeners[type] as AsVector<AsEventListenerCallback>;
				if(listeners != null)
				{
					int numListeners = (int)(listeners.getLength());
					AsVector<AsEventListenerCallback> remainingListeners = new AsVector<AsEventListenerCallback>();
					int i = 0;
					for (; i < numListeners; ++i)
					{
						if(listeners[i] != listener)
						{
							remainingListeners.push(listeners[i]);
						}
					}
					mEventListeners[type] = remainingListeners;
				}
			}
		}
		public virtual void removeEventListeners(String type)
		{
			if(type != null && mEventListeners != null)
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
			bool bubbles = _event.getBubbles();
			if(!bubbles && (mEventListeners == null || !(mEventListeners.containsKey(_event.getType()))))
			{
				return;
			}
			AsEventDispatcher previousTarget = _event.getTarget();
			_event.setTarget(this);
			if(bubbles && this is AsDisplayObject)
			{
				bubbleEvent(_event);
			}
			else
			{
				invokeEvent(_event);
			}
			if(previousTarget != null)
			{
				_event.setTarget(previousTarget);
			}
		}
		public virtual bool invokeEvent(AsEvent _event)
		{
			AsVector<AsEventListenerCallback> listeners = mEventListeners != null ? mEventListeners[_event.getType()] as AsVector<AsEventListenerCallback> : null;
			int numListeners = listeners == null ? 0 : listeners.getLength();
			if(numListeners != 0)
			{
				_event.setCurrentTarget(this);
				int i = 0;
				for (; i < numListeners; ++i)
				{
					AsEventListenerCallback listener = listeners[i] as AsEventListenerCallback;
					if(_event.getStopsImmediatePropagation())
					{
						return true;
					}
				}
				return _event.getStopsPropagation();
			}
			else
			{
				return false;
			}
		}
		public virtual void bubbleEvent(AsEvent _event)
		{
			AsVector<AsEventDispatcher> chain = null;
			AsDisplayObject element = this as AsDisplayObject;
			int length = 1;
			if(sBubbleChains.getLength() > 0)
			{
				chain = (AsVector<AsEventDispatcher>)(sBubbleChains.pop());
				chain[0] = element;
			}
			else
			{
				chain = new AsVector<AsEventDispatcher>();
			}
			while((element = element.getParent()) != null)
			{
				chain[length++] = element;
			}
			int i = 0;
			for (; i < length; ++i)
			{
				bool stopPropagation = chain[i].invokeEvent(_event);
				if(stopPropagation)
				{
					break;
				}
			}
			chain.setLength(0);
			sBubbleChains.push(chain);
		}
		public virtual void dispatchEventWith(String type, bool bubbles, Object data)
		{
			if(bubbles || hasEventListener(type))
			{
				AsEvent _event = AsEvent.fromPool(type, bubbles, data);
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
			AsVector<AsEventListenerCallback> listeners = mEventListeners != null ? mEventListeners[type] as AsVector<AsEventListenerCallback> : null;
			return listeners != null ? listeners.getLength() != 0 : false;
		}
	}
}
