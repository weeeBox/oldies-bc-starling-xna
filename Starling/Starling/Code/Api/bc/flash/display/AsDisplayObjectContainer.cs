using System;
 
using bc.flash;
using bc.flash.core;
using bc.flash.display;
using bc.flash.error;
using bc.flash.events;
using bc.flash.geom;
using bc.flash.utils;
 
namespace bc.flash.display
{
	public class AsDisplayObjectContainer : AsInteractiveObject
	{
		private AsVector<AsDisplayObject> mChildren;
		private static AsMatrix sHelperMatrix = new AsMatrix();
		private static AsPoint sHelperPoint = new AsPoint();
		public AsDisplayObjectContainer()
		{
			if((AsGlobal.getQualifiedClassName(this) == "DisplayObjectContainer"))
			{
				throw new AsAbstractClassError();
			}
			mChildren = new AsVector<AsDisplayObject>();
		}
		public override void dispose()
		{
			int numChildren = (int)(mChildren.getLength());
			int i = 0;
			for (; (i < numChildren); ++i)
			{
				mChildren[i].dispose();
			}
			base.dispose();
		}
		public virtual void addChild(AsDisplayObject child)
		{
			addChildAt(child, getNumChildren());
		}
		public virtual void addChildAt(AsDisplayObject child, int index)
		{
			if(((index >= 0) && (index <= getNumChildren())))
			{
				child.removeFromParent();
				mChildren.splice(index, (uint)(0), child);
				child.setParent(this);
				child.dispatchEvent(new AsEvent(AsEvent.ADDED, true));
				if(getStage() != null)
				{
					child.dispatchEventOnChildren(new AsEvent(AsEvent.ADDED_TO_STAGE));
				}
			}
			else
			{
				throw new AsRangeError("Invalid child index");
			}
		}
		public virtual void removeChild(AsDisplayObject child, bool dispose)
		{
			int childIndex = getChildIndex(child);
			if((childIndex != -1))
			{
				removeChildAt(childIndex, dispose);
			}
		}
		public virtual void removeChild(AsDisplayObject child)
		{
			removeChild(child, false);
		}
		public virtual void removeChildAt(int index, bool dispose)
		{
			if(((index >= 0) && (index < getNumChildren())))
			{
				AsDisplayObject child = mChildren[index];
				child.dispatchEvent(new AsEvent(AsEvent.REMOVED, true));
				if(getStage() != null)
				{
					child.dispatchEventOnChildren(new AsEvent(AsEvent.REMOVED_FROM_STAGE));
				}
				child.setParent(null);
				mChildren.splice(index, (uint)(1));
				if(dispose)
				{
					child.dispose();
				}
			}
			else
			{
				throw new AsRangeError("Invalid child index");
			}
		}
		public virtual void removeChildAt(int index)
		{
			removeChildAt(index, false);
		}
		public virtual void removeChildren(int beginIndex, int endIndex, bool dispose)
		{
			if(((endIndex < 0) || (endIndex >= getNumChildren())))
			{
				endIndex = (getNumChildren() - 1);
			}
			int i = beginIndex;
			for (; (i <= endIndex); ++i)
			{
				removeChildAt(beginIndex, dispose);
			}
		}
		public virtual void removeChildren(int beginIndex, int endIndex)
		{
			removeChildren(beginIndex, endIndex, false);
		}
		public virtual void removeChildren(int beginIndex)
		{
			removeChildren(beginIndex, -1, false);
		}
		public virtual void removeChildren()
		{
			removeChildren(0, -1, false);
		}
		public virtual AsDisplayObject getChildAt(int index)
		{
			if(((index >= 0) && (index < getNumChildren())))
			{
				return mChildren[index];
			}
			else
			{
				throw new AsRangeError("Invalid child index");
			}
		}
		public virtual AsDisplayObject getChildByName(String name)
		{
			int numChildren = (int)(mChildren.getLength());
			int i = 0;
			for (; (i < numChildren); ++i)
			{
				if((mChildren[i].getName() == name))
				{
					return mChildren[i];
				}
			}
			return null;
		}
		public virtual int getChildIndex(AsDisplayObject child)
		{
			return mChildren.indexOf(child);
		}
		public virtual void setChildIndex(AsDisplayObject child, int index)
		{
			int oldIndex = getChildIndex(child);
			if((oldIndex == -1))
			{
				throw new AsArgumentError("Not a child of this container");
			}
			mChildren.splice(oldIndex, (uint)(1));
			mChildren.splice(index, (uint)(0), child);
		}
		public virtual void swapChildren(AsDisplayObject child1, AsDisplayObject child2)
		{
			int index1 = getChildIndex(child1);
			int index2 = getChildIndex(child2);
			if(((index1 == -1) || (index2 == -1)))
			{
				throw new AsArgumentError("Not a child of this container");
			}
			swapChildrenAt(index1, index2);
		}
		public virtual void swapChildrenAt(int index1, int index2)
		{
			AsDisplayObject child1 = getChildAt(index1);
			AsDisplayObject child2 = getChildAt(index2);
			mChildren[index1] = child2;
			mChildren[index2] = child1;
		}
		public virtual bool contains(AsDisplayObject child)
		{
			if((child == this))
			{
				return true;
			}
			int numChildren = (int)(mChildren.getLength());
			int i = 0;
			for (; (i < numChildren); ++i)
			{
				AsDisplayObject currentChild = mChildren[i];
				AsDisplayObjectContainer currentChildContainer = ((currentChild is AsDisplayObjectContainer) ? ((AsDisplayObjectContainer)(currentChild)) : null);
				if(((currentChildContainer != null) && currentChildContainer.contains(child)))
				{
					return true;
				}
				else
				{
					if((currentChild == child))
					{
						return true;
					}
				}
			}
			return false;
		}
		public override AsRectangle getBounds(AsDisplayObject targetSpace, AsRectangle resultRect)
		{
			if((resultRect == null))
			{
				resultRect = new AsRectangle();
			}
			int numChildren = (int)(mChildren.getLength());
			if((numChildren == 0))
			{
				getTransformationMatrix(targetSpace, sHelperMatrix);
				AsGlobal.transformCoords(sHelperMatrix, 0.0f, 0.0f, sHelperPoint);
				resultRect.x = sHelperPoint.x;
				resultRect.y = sHelperPoint.y;
				resultRect.width = resultRect.height = 0;
				return resultRect;
			}
			else
			{
				if((numChildren == 1))
				{
					return mChildren[0].getBounds(targetSpace, resultRect);
				}
				else
				{
					float minX = AsMathHelper.MAX_NUMBER;
					float maxX = -AsMathHelper.MAX_NUMBER;
					float minY = AsMathHelper.MAX_NUMBER;
					float maxY = -AsMathHelper.MAX_NUMBER;
					int i = 0;
					for (; (i < numChildren); ++i)
					{
						mChildren[i].getBounds(targetSpace, resultRect);
						minX = (((minX < resultRect.x)) ? (minX) : (resultRect.x));
						maxX = (((maxX > resultRect.getRight())) ? (maxX) : (resultRect.getRight()));
						minY = (((minY < resultRect.y)) ? (minY) : (resultRect.y));
						maxY = (((maxY > resultRect.getBottom())) ? (maxY) : (resultRect.getBottom()));
					}
					resultRect.x = minX;
					resultRect.y = minY;
					resultRect.width = (maxX - minX);
					resultRect.height = (maxY - minY);
					return resultRect;
				}
			}
		}
		public virtual AsRectangle getBounds(AsDisplayObject targetSpace)
		{
			return getBounds(targetSpace, null);
		}
		public override AsDisplayObject hitTest(AsPoint localPoint, bool forTouch)
		{
			if((forTouch && (!(getVisible()) || !(getTouchable()))))
			{
				return null;
			}
			float localX = localPoint.x;
			float localY = localPoint.y;
			int numChildren = (int)(mChildren.getLength());
			int i = (numChildren - 1);
			for (; (i >= 0); --i)
			{
				AsDisplayObject child = mChildren[i];
				getTransformationMatrix(child, sHelperMatrix);
				AsGlobal.transformCoords(sHelperMatrix, localX, localY, sHelperPoint);
				AsDisplayObject target = child.hitTest(sHelperPoint, forTouch);
				if(target != null)
				{
					return target;
				}
			}
			return null;
		}
		public virtual AsDisplayObject hitTest(AsPoint localPoint)
		{
			return hitTest(localPoint, false);
		}
		public override void render(AsRenderSupport support, float alpha)
		{
			alpha = (alpha * this.getAlpha());
			int numChildren = (int)(mChildren.getLength());
			int i = 0;
			for (; (i < numChildren); ++i)
			{
				AsDisplayObject child = mChildren[i];
				if(((((child.getAlpha() != 0.0f) && child.getVisible()) && (child.getScaleX() != 0.0f)) && (child.getScaleY() != 0.0f)))
				{
					support.pushMatrix();
					support.transform(child.getTransform().getMatrix());
					child.render(support, alpha);
					support.popMatrix();
				}
			}
		}
		public virtual void broadcastEvent(AsEvent _event)
		{
			if(_event.getBubbles())
			{
				throw new AsArgumentError("Broadcast of bubbling events is prohibited");
			}
			dispatchEventOnChildren(_event);
		}
		public override void dispatchEventOnChildren(AsEvent _event)
		{
			AsVector<AsDisplayObject> listeners = new AsVector<AsDisplayObject>();
			getChildEventListeners(this, _event.getType(), listeners);
			int numListeners = (int)(listeners.getLength());
			int i = 0;
			for (; (i < numListeners); ++i)
			{
				listeners[i].dispatchEvent(_event);
			}
		}
		private void getChildEventListeners(AsDisplayObject _object, String eventType, AsVector<AsDisplayObject> listeners)
		{
			AsDisplayObjectContainer container = ((_object is AsDisplayObjectContainer) ? ((AsDisplayObjectContainer)(_object)) : null);
			if(_object.hasEventListener(eventType))
			{
				listeners.push(_object);
			}
			if(container != null)
			{
				AsVector<AsDisplayObject> children = container.mChildren;
				int numChildren = (int)(children.getLength());
				int i = 0;
				for (; (i < numChildren); ++i)
				{
					getChildEventListeners(children[i], eventType, listeners);
				}
			}
		}
		public virtual int getNumChildren()
		{
			return (int)(mChildren.getLength());
		}
	}
}
