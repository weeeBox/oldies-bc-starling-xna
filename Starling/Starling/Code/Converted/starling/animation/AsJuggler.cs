using System;
 
using bc.flash;
using starling.animation;
using starling.events;
 
namespace starling.animation
{
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
			if(((_object != null) && (mObjects.indexOf(_object) == -1)))
			{
				mObjects.push(_object);
				AsEventDispatcher dispatcher = ((_object is AsEventDispatcher) ? ((AsEventDispatcher)(_object)) : null);
				if(dispatcher != null)
				{
					dispatcher.addEventListener(AsEvent.REMOVE_FROM_JUGGLER, onRemove);
				}
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
			int index = mObjects.indexOf(_object);
			if((index != -1))
			{
				mObjects[index] = null;
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
					mObjects[i] = null;
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
		public virtual AsDelayedCall delayCall(AsDelayedCallback call, float delay, params Object[] args)
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
			int numObjects = (int)(mObjects.getLength());
			int currentIndex = 0;
			int i = 0;
			mElapsedTime = (mElapsedTime + time);
			if((numObjects == 0))
			{
				return;
			}
			for (i = 0; (i < numObjects); ++i)
			{
				AsIAnimatable _object = mObjects[i];
				if(_object != null)
				{
					_object.advanceTime(time);
					if((currentIndex != i))
					{
						mObjects[currentIndex] = _object;
						mObjects[i] = null;
					}
					++currentIndex;
				}
			}
			if((currentIndex != i))
			{
				numObjects = (int)(mObjects.getLength());
				while((i < numObjects))
				{
					mObjects[currentIndex++] = mObjects[i++];
				}
				mObjects.setLength(currentIndex);
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
