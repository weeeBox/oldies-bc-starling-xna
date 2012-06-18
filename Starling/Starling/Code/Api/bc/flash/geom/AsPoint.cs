using System;
 
using bc.flash;
using bc.flash.geom;
 
namespace bc.flash.geom
{
	public class AsPoint : AsObject
	{
		public float x;
		public float y;
		public AsPoint(float x, float y)
		{
			this.x = x;
			this.y = y;
		}
		public AsPoint(float x)
		 : this(x, 0)
		{
		}
		public AsPoint()
		 : this(0, 0)
		{
		}
		public virtual AsPoint _add(AsPoint v)
		{
			return new AsPoint((x + v.x), (y + v.y));
		}
		public virtual AsPoint clone()
		{
			return new AsPoint(x, y);
		}
		public static float distance(AsPoint pt1, AsPoint pt2)
		{
			float dx = (pt1.x - pt2.x);
			float dy = (pt1.y - pt2.y);
			return AsMath.sqrt(((dx * dx) + (dy * dy)));
		}
		public virtual bool equals(AsPoint toCompare)
		{
			return (((toCompare != null) && (toCompare.x == x)) && (toCompare.y == y));
		}
		public static AsPoint interpolate(AsPoint pt1, AsPoint pt2, float ratio)
		{
			float invRatio = (1.0f - ratio);
			return new AsPoint(((invRatio * pt1.x) + (ratio * pt2.x)), ((invRatio * pt1.y) + (ratio * pt2.y)));
		}
		public virtual float getLength()
		{
			return AsMath.sqrt(((x * x) + (y * y)));
		}
		public virtual void normalize(float thickness)
		{
			float inverseLength = (thickness / getLength());
			x = (x * inverseLength);
			y = (y * inverseLength);
		}
		public virtual void offset(float dx, float dy)
		{
			x = (x + dx);
			y = (y + dy);
		}
		public static AsPoint polar(float len, float angle)
		{
			return new AsPoint((AsMath.cos(angle) * len), (AsMath.sin(angle) * len));
		}
		public virtual AsPoint subtract(AsPoint v)
		{
			return new AsPoint((x - v.x), (y - v.y));
		}
	}
}
