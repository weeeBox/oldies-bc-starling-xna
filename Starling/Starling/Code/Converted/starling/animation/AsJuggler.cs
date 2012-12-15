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
			if(_object != null && mObjects.indexOf(_object) == -1)
			{
				mObjects.push(_object);
				AsEventDispatcher dispatcher = _object as AsEventDispatcher;
				if(dispatcher != null)
				{
					dispatcher.addEventListener(AsEvent.REMOVE_FROM_JUGGLER, onRemove);
				}
			}
		}
		public virtual void _remove(AsIAnimatable _object)
		{
			if(_object == null)
			{
				return;
			}
			AsEventDispatcher dispatcher = _object as AsEventDispatcher;
			if(dispatcher != null)
			{
				dispatcher.removeEventListener(AsEvent.REMOVE_FROM_JUGGLER, onRemove);
			}
			int index = mObjects.indexOf(_object);
			if(index != -1)
			{
				mObjects[index] = null;
			}
		}
		public virtual void removeTweens(Object target)
		{
			if(target == null)
			{
				return;
			}
			int i = (int)(mObjects.getLength() - 1);
			for (; i >= 0; --i)
			{
				AsTween tween = mObjects[i] as AsTween;
				if(tween != null && tween.getTarget() == target)
				{
					tween.removeEventListener(AsEvent.REMOVE_FROM_JUGGLER, onRemove);
					mObjects[i] = null;
				}
			}
		}
		public virtual void purge()
		{
			int i = (int)(mObjects.getLength() - 1);
			for (; i >= 0; --i)
			{
				AsEventDispatcher dispatcher = mObjects.pop() as AsEventDispatcher;
				if(dispatcher != null)
				{
					dispatcher.removeEventListener(AsEvent.REMOVE_FROM_JUGGLER, onRemove);
				}
			}
		}
		public virtual AsDelayedCall delayCall(AsDelayedCallback call, float delay, params Object[] args)
		{
			if(call == null)
			{
				return null;
			}
			AsDelayedCall delayedCall = new AsDelayedCall(call, delay, args);
			_add(delayedCall);
			return delayedCall;
		}
		public virtual void tween(Object target, float time, Object properties)
		{
			AsTween tween = AsTween.fromPool(target, time);
			Object __propertys_ = properties;
			if (__propertys_ != null)
			{
				foreach (String property in __propertys_)
				{
					Object _value = ((AsObject)(properties)).getOwnProperty(property);
					if(tween.hasOwnProperty(property))
					{
						tween.setOwnProperty(property, _value);
					}
					else
					{
						if(((AsObject)(target)).hasOwnProperty(property))
						{
							tween.animate(property, _value as float);
						}
						else
						{
							throw new AsArgumentError("Invalid property: " + property);
						}
					}
				}
			}
			tween.addEventListener(AsEvent.REMOVE_FROM_JUGGLER, onPooledTweenComplete);
			_add(tween);
		}
		private void onPooledTweenComplete(AsEvent _event)
		{
			AsTween.toPool(_event.getTarget() as AsTween);
		}
		public virtual void advanceTime(float time)
		{
			int numObjects = (int)(mObjects.getLength());
			int currentIndex = 0;
			int i = 0;
			mElapsedTime = mElapsedTime + time;
			if(numObjects == 0)
			{
				return;
			}
			for (i = 0; i < numObjects; ++i)
			{
				AsIAnimatable _object = mObjects[i];
				if(_object != null)
				{
					if(currentIndex != i)
					{
						mObjects[currentIndex] = _object;
						mObjects[i] = null;
					}
					_object.advanceTime(time);
					++currentIndex;
				}
			}
			if(currentIndex != i)
			{
				numObjects = (int)(mObjects.getLength());
				while(i < numObjects)
				{
					mObjects[currentIndex++] = mObjects[i++];
				}
				mObjects.setLength(currentIndex);
			}
		}
		private void onRemove(AsEvent _event)
		{
			_remove(_event.getTarget() as AsIAnimatable);
			AsTween tween = _event.getTarget() as AsTween;
			if(tween != null && tween.getIsComplete())
			{
				_add(tween.getNextTween());
			}
		}
		public virtual float getElapsedTime()
		{
			return mElapsedTime;
		}
	}
}
