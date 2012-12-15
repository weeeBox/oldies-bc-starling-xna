using System;
 
using bc.flash;
using bc.flash.geom;
using bc.flash.system;
using starling.core;
using starling.display;
using starling.errors;
using starling.events;
using starling.filters;
using starling.utils;
 
namespace starling.display
{
	public class AsDisplayObjectContainer : AsDisplayObject
	{
		private AsVector<AsDisplayObject> mChildren;
		private static AsMatrix sHelperMatrix = new AsMatrix();
		private static AsPoint sHelperPoint = new AsPoint();
		private static AsVector<AsDisplayObject> sBroadcastListeners = new AsVector<AsDisplayObject>();
		public AsDisplayObjectContainer()
		{
			if(AsCapabilities.getIsDebugger() && AsGlobal.getQualifiedClassName(this) == "starling.display::DisplayObjectContainer")
			{
				throw new AsAbstractClassError();
			}
			mChildren = new AsVector<AsDisplayObject>();
		}
		public override void dispose()
		{
			int i = (int)(mChildren.getLength() - 1);
			for (; i >= 0; --i)
			{
				mChildren[i].dispose();
			}
			base.dispose();
		}
		public virtual AsDisplayObject addChild(AsDisplayObject child)
		{
			addChildAt(child, getNumChildren());
			return child;
		}
		public virtual AsDisplayObject addChildAt(AsDisplayObject child, int index)
		{
			int numChildren = (int)(mChildren.getLength());
			if(index >= 0 && index <= numChildren)
			{
				child.removeFromParent();
				if(index == numChildren)
				{
					mChildren.push(child);
				}
				else
				{
					mChildren.splice(index, (uint)(0), child);
				}
				child.setParent(this);
				child.dispatchEventWith(AsEvent.ADDED, true);
				if(getStage() != null)
				{
					AsDisplayObjectContainer container = child as AsDisplayObjectContainer;
					if(container != null)
					{
						container.broadcastEventWith(AsEvent.ADDED_TO_STAGE);
					}
					else
					{
						child.dispatchEventWith(AsEvent.ADDED_TO_STAGE);
					}
				}
				return child;
			}
			else
			{
				throw new AsRangeError("Invalid child index");
			}
		}
		public virtual AsDisplayObject removeChild(AsDisplayObject child, bool dispose)
		{
			int childIndex = getChildIndex(child);
			if(childIndex != -1)
			{
				removeChildAt(childIndex, dispose);
			}
			return child;
		}
		public virtual AsDisplayObject removeChild(AsDisplayObject child)
		{
			return removeChild(child, false);
		}
		public virtual AsDisplayObject removeChildAt(int index, bool dispose)
		{
			if(index >= 0 && index < getNumChildren())
			{
				AsDisplayObject child = mChildren[index];
				child.dispatchEventWith(AsEvent.REMOVED, true);
				if(getStage() != null)
				{
					AsDisplayObjectContainer container = child as AsDisplayObjectContainer;
					if(container != null)
					{
						container.broadcastEventWith(AsEvent.REMOVED_FROM_STAGE);
					}
					else
					{
						child.dispatchEventWith(AsEvent.REMOVED_FROM_STAGE);
					}
				}
				child.setParent(null);
				index = mChildren.indexOf(child);
				if(index >= 0)
				{
					mChildren.splice(index, (uint)(1));
				}
				if(dispose)
				{
					child.dispose();
				}
				return child;
			}
			else
			{
				throw new AsRangeError("Invalid child index");
			}
		}
		public virtual AsDisplayObject removeChildAt(int index)
		{
			return removeChildAt(index, false);
		}
		public virtual void removeChildren(int beginIndex, int endIndex, bool dispose)
		{
			if(endIndex < 0 || endIndex >= getNumChildren())
			{
				endIndex = getNumChildren() - 1;
			}
			int i = beginIndex;
			for (; i <= endIndex; ++i)
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
			if(index >= 0 && index < getNumChildren())
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
			for (; i < numChildren; ++i)
			{
				if(mChildren[i].getName() == name)
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
			if(oldIndex == -1)
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
			if(index1 == -1 || index2 == -1)
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
		public virtual void sortChildren(AsDisplayObjectContainerCompareFunction compareFunction)
		{
			mChildren = mChildren.sort(compareFunction);
		}
		public virtual bool contains(AsDisplayObject child)
		{
			while(child != null)
			{
				if(child == this)
				{
					return true;
				}
				else
				{
					child = child.getParent();
				}
			}
			return false;
		}
		public override AsRectangle getBounds(AsDisplayObject targetSpace, AsRectangle resultRect)
		{
			if(resultRect == null)
			{
				resultRect = new AsRectangle();
			}
			int numChildren = (int)(mChildren.getLength());
			if(numChildren == 0)
			{
				getTransformationMatrix(targetSpace, sHelperMatrix);
				AsMatrixUtil.transformCoords(sHelperMatrix, 0.0f, 0.0f, sHelperPoint);
				resultRect.setTo(sHelperPoint.x, sHelperPoint.y, 0, 0);
				return resultRect;
			}
			else
			{
				if(numChildren == 1)
				{
					return mChildren[0].getBounds(targetSpace, resultRect);
				}
				else
				{
					float minX = AsNumber.MAX_VALUE;
					float maxX = -AsNumber.MAX_VALUE;
					float minY = AsNumber.MAX_VALUE;
					float maxY = -AsNumber.MAX_VALUE;
					int i = 0;
					for (; i < numChildren; ++i)
					{
						mChildren[i].getBounds(targetSpace, resultRect);
						minX = minX < resultRect.x ? minX : resultRect.x;
						maxX = maxX > resultRect.getRight() ? maxX : resultRect.getRight();
						minY = minY < resultRect.y ? minY : resultRect.y;
						maxY = maxY > resultRect.getBottom() ? maxY : resultRect.getBottom();
					}
					resultRect.setTo(minX, minY, maxX - minX, maxY - minY);
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
			if(forTouch && (!getVisible() || !getTouchable()))
			{
				return null;
			}
			float localX = localPoint.x;
			float localY = localPoint.y;
			int numChildren = (int)(mChildren.getLength());
			int i = numChildren - 1;
			for (; i >= 0; --i)
			{
				AsDisplayObject child = mChildren[i];
				getTransformationMatrix(child, sHelperMatrix);
				AsMatrixUtil.transformCoords(sHelperMatrix, localX, localY, sHelperPoint);
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
		public override void render(AsRenderSupport support, float parentAlpha)
		{
			float alpha = parentAlpha * this.getAlpha();
			int numChildren = (int)(mChildren.getLength());
			String blendMode = support.getBlendMode();
			int i = 0;
			for (; i < numChildren; ++i)
			{
				AsDisplayObject child = mChildren[i];
				if(child.getHasVisibleArea())
				{
					AsFragmentFilter filter = child.getFilter();
					support.pushMatrix();
					support.transformMatrix(child);
					support.setBlendMode(child.getBlendMode());
					if(filter != null)
					{
						filter.render(child, support, alpha);
					}
					else
					{
						child.render(support, alpha);
					}
					support.setBlendMode(blendMode);
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
			int fromIndex = (int)(sBroadcastListeners.getLength());
			getChildEventListeners(this, _event.getType(), sBroadcastListeners);
			int toIndex = (int)(sBroadcastListeners.getLength());
			int i = fromIndex;
			for (; i < toIndex; ++i)
			{
				sBroadcastListeners[i].dispatchEvent(_event);
			}
			sBroadcastListeners.setLength(fromIndex);
		}
		public virtual void broadcastEventWith(String type, Object data)
		{
			AsEvent _event = AsEvent.fromPool(type, false, data);
			broadcastEvent(_event);
			AsEvent.toPool(_event);
		}
		public virtual void broadcastEventWith(String type)
		{
			broadcastEventWith(type, null);
		}
		private void getChildEventListeners(AsDisplayObject _object, String eventType, AsVector<AsDisplayObject> listeners)
		{
			AsDisplayObjectContainer container = _object as AsDisplayObjectContainer;
			if(_object.hasEventListener(eventType))
			{
				listeners.push(_object);
			}
			if(container != null)
			{
				AsVector<AsDisplayObject> children = container.mChildren;
				int numChildren = (int)(children.getLength());
				int i = 0;
				for (; i < numChildren; ++i)
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
