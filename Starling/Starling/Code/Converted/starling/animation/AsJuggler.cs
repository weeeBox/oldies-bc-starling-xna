using System;
 
using bc.flash;
using starling.animation;
using starling.events;
 
namespace starling.animation
{
	public delegate void AsDelayedCallback();
	public class AsJuggler : AsObject, AsIAnimatable
	{
		private AsVector<AsIAnimatable> mObjects;
		private float mElapsedTime;
		public AsJuggler()
		{
			mElapsedTime = 0;
			mObjects = new AsVector<AsIAnimatable>();
		}
		public virtual void _add(AsIAnimatable _object)
		{
			if((_object != null))
			{
				mObjects.push(_object);
			}
			AsEventDispatcher dispatcher = ((_object is AsEventDispatcher) ? ((AsEventDispatcher)(_object)) : null);
			if(dispatcher != null)
			{
				dispatcher.addEventListener(AsEvent.REMOVE_FROM_JUGGLER, onRemove);
			}
		}
		public virtual void _remove(AsIAnimatable _object)
		{
			if((_object == null))
			{
				return;
			}
			AsEventDispatcher dispatcher = ((_object is AsEventDispatcher) ? ((AsEventDispatcher)(_object)) : null);
			if(dispatcher != null)
			{
				dispatcher.removeEventListener(AsEvent.REMOVE_FROM_JUGGLER, onRemove);
			}
			int i = (int)((mObjects.getLength() - 1));
			for (; (i >= 0); --i)
			{
				if((mObjects[i] == _object))
				{
					mObjects.splice(i, (uint)(1));
				}
			}
		}
		public virtual void removeTweens(AsObject target)
		{
			if((target == null))
			{
				return;
			}
			int numObjects = (int)(mObjects.getLength());
			int i = (numObjects - 1);
			for (; (i >= 0); --i)
			{
				AsTween tween = ((mObjects[i] is AsTween) ? ((AsTween)(mObjects[i])) : null);
				if(((tween != null) && (tween.getTarget() == target)))
				{
					mObjects.splice(i, (uint)(1));
				}
			}
		}
		public virtual void purge()
		{
			int i = (int)((mObjects.getLength() - 1));
			for (; (i >= 0); --i)
			{
				AsEventDispatcher dispatcher = ((mObjects.pop() is AsEventDispatcher) ? ((AsEventDispatcher)(mObjects.pop())) : null);
				if(dispatcher != null)
				{
					dispatcher.removeEventListener(AsEvent.REMOVE_FROM_JUGGLER, onRemove);
				}
			}
		}
		public virtual AsDelayedCall delayCall(AsDelayedCallback call, float delay, AsArray args)
		{
			if((call == null))
			{
				return null;
			}
			AsDelayedCall delayedCall = new AsDelayedCall(call, delay, args);
			_add(delayedCall);
			return delayedCall;
		}
		public virtual void advanceTime(float time)
		{
			mElapsedTime = (mElapsedTime + time);
			if((mObjects.getLength() == 0))
			{
				return;
			}
			int numObjects = (int)(mObjects.getLength());
			AsVector<AsIAnimatable> objectCopy = mObjects.concat();
			int i = 0;
			for (; (i < numObjects); ++i)
			{
				objectCopy[i].advanceTime(time);
			}
		}
		private void onRemove(AsEvent _event)
		{
			_remove(((_event.getTarget() is AsIAnimatable) ? ((AsIAnimatable)(_event.getTarget())) : null));
		}
		public virtual float getElapsedTime()
		{
			return mElapsedTime;
		}
	}
}
