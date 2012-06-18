using System;
 
using bc.flash;
using bc.flash.error;
using bc.flash.geom;
 
namespace bc.flash.geom
{
	public class AsRectangle : AsObject
	{
		public float x;
		public float y;
		public float width;
		public float height;
		public AsRectangle(float x, float y, float width, float height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}
		public AsRectangle(float x, float y, float width)
		 : this(x, y, width, 0)
		{
		}
		public AsRectangle(float x, float y)
		 : this(x, y, 0, 0)
		{
		}
		public AsRectangle(float x)
		 : this(x, 0, 0, 0)
		{
		}
		public AsRectangle()
		 : this(0, 0, 0, 0)
		{
		}
		public virtual float getBottom()
		{
			return (y + height);
		}
		public virtual void setBottom(float _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsPoint getBottomRight()
		{
			return new AsPoint((x + width), (y + height));
		}
		public virtual void setBottomRight(AsPoint _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsRectangle clone()
		{
			return new AsRectangle(x, y, width, height);
		}
		public virtual bool contains(float x, float y)
		{
			throw new AsNotImplementedError();
		}
		public virtual bool containsPoint(AsPoint point)
		{
			throw new AsNotImplementedError();
		}
		public virtual bool containsRect(AsRectangle rect)
		{
			throw new AsNotImplementedError();
		}
		public virtual bool equals(AsRectangle toCompare)
		{
			return ((((x == toCompare.x) && (y == toCompare.y)) && (width == toCompare.width)) && (height == toCompare.height));
		}
		public virtual void inflate(float dx, float dy)
		{
			throw new AsNotImplementedError();
		}
		public virtual void inflatePoint(AsPoint point)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsRectangle intersection(AsRectangle toIntersect)
		{
			throw new AsNotImplementedError();
		}
		public virtual bool intersects(AsRectangle toIntersect)
		{
			throw new AsNotImplementedError();
		}
		public virtual bool isEmpty()
		{
			throw new AsNotImplementedError();
		}
		public virtual float getLeft()
		{
			return x;
		}
		public virtual void setLeft(float _value)
		{
			x = _value;
		}
		public virtual void offset(float dx, float dy)
		{
			throw new AsNotImplementedError();
		}
		public virtual void offsetPoint(AsPoint point)
		{
			throw new AsNotImplementedError();
		}
		public virtual float getRight()
		{
			return (x + width);
		}
		public virtual void setRight(float _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setEmpty()
		{
			throw new AsNotImplementedError();
		}
		public virtual AsPoint getSize()
		{
			return new AsPoint(width, height);
		}
		public virtual void setSize(AsPoint _value)
		{
			width = _value.x;
			height = _value.y;
		}
		public virtual float getTop()
		{
			return y;
		}
		public virtual void setTop(float _value)
		{
			y = _value;
		}
		public virtual AsPoint getTopLeft()
		{
			return new AsPoint(x, y);
		}
		public virtual void setTopLeft(AsPoint _value)
		{
			x = _value.x;
			y = _value.y;
		}
		public virtual AsRectangle union(AsRectangle toUnion)
		{
			throw new AsNotImplementedError();
		}
		public virtual String toString()
		{
			return (((((((("(x=" + x) + ", y=") + y) + ", w=") + width) + ", h=") + height) + ")");
		}
	}
}
