using System;
 
using bc.flash;
using bc.flash.core;
using bc.flash.display;
using bc.flash.geom;
using bc.flash.utils;
 
namespace bc.flash.display
{
	public class AsBitmap : AsDisplayObject
	{
		private AsBitmapData mBitmapData;
		private String mPixelSnapping;
		private bool mSmoothing;
		private static AsPoint sHelperPoint = new AsPoint();
		private static AsMatrix sHelperMatrix = new AsMatrix();
		private static AsPoint sPosition = new AsPoint();
		public AsBitmap(AsBitmapData bitmapData, String pixelSnapping, bool smoothing)
		{
			mBitmapData = bitmapData;
			mPixelSnapping = pixelSnapping;
			mSmoothing = smoothing;
		}
		public AsBitmap(AsBitmapData bitmapData, String pixelSnapping)
		 : this(bitmapData, pixelSnapping, false)
		{
		}
		public AsBitmap(AsBitmapData bitmapData)
		 : this(bitmapData, "auto", false)
		{
		}
		public AsBitmap()
		 : this(null, "auto", false)
		{
		}
		public override void render(AsRenderSupport support, float alpha)
		{
		}
		public override AsRectangle getBounds(AsDisplayObject targetSpace, AsRectangle resultRect)
		{
			if((resultRect == null))
			{
				resultRect = new AsRectangle();
			}
			if((targetSpace == this))
			{
				resultRect.x = getX();
				resultRect.y = getY();
				resultRect.width = mBitmapData.getWidth();
				resultRect.height = mBitmapData.getHeight();
				return resultRect;
			}
			float minX = AsMathHelper.MAX_NUMBER;
			float maxX = -AsMathHelper.MAX_NUMBER;
			float minY = AsMathHelper.MAX_NUMBER;
			float maxY = -AsMathHelper.MAX_NUMBER;
			getTransformationMatrix(targetSpace, sHelperMatrix);
			sPosition.x = getX();
			sPosition.y = getY();
			AsGlobal.transformCoords(sHelperMatrix, sPosition.x, sPosition.y, sHelperPoint);
			minX = (((minX < sHelperPoint.x)) ? (minX) : (sHelperPoint.x));
			maxX = (((maxX > sHelperPoint.x)) ? (maxX) : (sHelperPoint.x));
			minY = (((minY < sHelperPoint.y)) ? (minY) : (sHelperPoint.y));
			maxY = (((maxY > sHelperPoint.y)) ? (maxY) : (sHelperPoint.y));
			sPosition.x = (sPosition.x + getBitmapData().getWidth());
			AsGlobal.transformCoords(sHelperMatrix, sPosition.x, sPosition.y, sHelperPoint);
			minX = (((minX < sHelperPoint.x)) ? (minX) : (sHelperPoint.x));
			maxX = (((maxX > sHelperPoint.x)) ? (maxX) : (sHelperPoint.x));
			minY = (((minY < sHelperPoint.y)) ? (minY) : (sHelperPoint.y));
			maxY = (((maxY > sHelperPoint.y)) ? (maxY) : (sHelperPoint.y));
			sPosition.y = (sPosition.y + getBitmapData().getHeight());
			AsGlobal.transformCoords(sHelperMatrix, sPosition.x, sPosition.y, sHelperPoint);
			minX = (((minX < sHelperPoint.x)) ? (minX) : (sHelperPoint.x));
			maxX = (((maxX > sHelperPoint.x)) ? (maxX) : (sHelperPoint.x));
			minY = (((minY < sHelperPoint.y)) ? (minY) : (sHelperPoint.y));
			maxY = (((maxY > sHelperPoint.y)) ? (maxY) : (sHelperPoint.y));
			sPosition.x = getX();
			AsGlobal.transformCoords(sHelperMatrix, sPosition.x, sPosition.y, sHelperPoint);
			minX = (((minX < sHelperPoint.x)) ? (minX) : (sHelperPoint.x));
			maxX = (((maxX > sHelperPoint.x)) ? (maxX) : (sHelperPoint.x));
			minY = (((minY < sHelperPoint.y)) ? (minY) : (sHelperPoint.y));
			maxY = (((maxY > sHelperPoint.y)) ? (maxY) : (sHelperPoint.y));
			resultRect.x = minX;
			resultRect.y = minY;
			resultRect.width = (maxX - minX);
			resultRect.height = (maxY - minY);
			return resultRect;
		}
		public virtual AsRectangle getBounds(AsDisplayObject targetSpace)
		{
			return getBounds(targetSpace, null);
		}
		public virtual AsBitmapData getBitmapData()
		{
			return mBitmapData;
		}
		public virtual void setBitmapData(AsBitmapData _value)
		{
			mBitmapData = _value;
		}
		public virtual String getPixelSnapping()
		{
			return mPixelSnapping;
		}
		public virtual void setPixelSnapping(String _value)
		{
			mPixelSnapping = _value;
		}
		public virtual bool getSmoothing()
		{
			return mSmoothing;
		}
		public virtual void setSmoothing(bool _value)
		{
			mSmoothing = _value;
		}
	}
}
