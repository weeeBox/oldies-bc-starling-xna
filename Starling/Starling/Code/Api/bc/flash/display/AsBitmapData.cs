using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.error;
using bc.flash.geom;
 
namespace bc.flash.display
{
	public class AsBitmapData : AsObject, AsIBitmapDrawable
	{
		private int mWidth;
		private int mHeight;
		private bool mTransparent;
		private uint mFillColor;
		public AsBitmapData(int width, int height, bool transparent, uint fillColor)
		{
			mWidth = width;
			mHeight = height;
			mTransparent = transparent;
			mFillColor = fillColor;
		}
		public AsBitmapData(int width, int height, bool transparent)
		 : this(width, height, transparent, 0xffffffff)
		{
		}
		public AsBitmapData(int width, int height)
		 : this(width, height, true, 0xffffffff)
		{
		}
		public virtual void draw(AsIBitmapDrawable source, AsMatrix matrix, AsColorTransform colorTransform, String blendMode, AsRectangle clipRect, bool smoothing)
		{
			throw new AsAbstractClassError();
		}
		public virtual void draw(AsIBitmapDrawable source, AsMatrix matrix, AsColorTransform colorTransform, String blendMode, AsRectangle clipRect)
		{
			draw(source, matrix, colorTransform, blendMode, clipRect, false);
		}
		public virtual void draw(AsIBitmapDrawable source, AsMatrix matrix, AsColorTransform colorTransform, String blendMode)
		{
			draw(source, matrix, colorTransform, blendMode, null, false);
		}
		public virtual void draw(AsIBitmapDrawable source, AsMatrix matrix, AsColorTransform colorTransform)
		{
			draw(source, matrix, colorTransform, null, null, false);
		}
		public virtual void draw(AsIBitmapDrawable source, AsMatrix matrix)
		{
			draw(source, matrix, null, null, null, false);
		}
		public virtual void draw(AsIBitmapDrawable source)
		{
			draw(source, null, null, null, null, false);
		}
		public virtual void dispose()
		{
			throw new AsAbstractClassError();
		}
		public virtual int getWidth()
		{
			return mWidth;
		}
		public virtual int getHeight()
		{
			return mHeight;
		}
	}
}
