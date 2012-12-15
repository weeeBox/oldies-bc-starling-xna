using System;

using bc.flash;
using bc.flash.display;
using bc.flash.error;
using bc.flash.geom;
using bc.flash.resources;
using bc.flash.utils;

namespace bc.flash.display
{
    public class AsBitmapData : AsObject, AsIBitmapDrawable
    {
        private int mWidth;
        private int mHeight;
        private bool mTransparent;
        private uint mFillColor;

        private BcTexture2D mTexture;

        public AsBitmapData(BcTexture2D texture)
        {
            mTexture = texture;
            init(texture.Width, texture.Height, true, 0xffffffff);
        }

        public AsBitmapData(int width, int height, bool transparent, uint fillColor)
        {
            init(width, height, transparent, fillColor);
        }

        public AsBitmapData(int width, int height, bool transparent)
            : this(width, height, true, 0xffffffff)
        {
        }

        public AsBitmapData(int width, int height)
            : this(width, height, true)
        {
        }

        private void init(int width, int height, bool transparent, uint fillColor)
        {
            mWidth = width;
            mHeight = height;
            mTransparent = transparent;
            mFillColor = fillColor;
        }

        public virtual void draw(AsIBitmapDrawable source, AsMatrix matrix, AsColorTransform colorTransform, String blendMode, AsRectangle clipRect, bool smoothing)
        {

        }

        public virtual void draw(AsIBitmapDrawable source, AsMatrix matrix, AsColorTransform colorTransform, String blendMode, AsRectangle clipRect)
        {
            draw(source, null, null, null, null, false);
        }

        public virtual void draw(AsIBitmapDrawable source, AsMatrix matrix, AsColorTransform colorTransform, String blendMode)
        {
            draw(source, null, null, null, null);
        }

        public virtual void draw(AsIBitmapDrawable source, AsMatrix matrix, AsColorTransform colorTransform)
        {
            draw(source, null, null, null);
        }

        public virtual void draw(AsIBitmapDrawable source, AsMatrix matrix)
        {
            draw(source, null, null);
        }

        public virtual void draw(AsIBitmapDrawable source)
        {
            draw(source, null);
        }

        public virtual void dispose()
        {
            mTexture.Dispose();
        }

        public BcTexture2D Texture
        {
            get { return mTexture; }
        }

        public virtual int getWidth()
        {
            return mWidth;
        }

        public virtual int getHeight()
        {
            return mHeight;
        }

        public AsRectangle getRect()
        {
            throw new NotImplementedException();
        }

        public void copyPixels(AsBitmapData data, AsRectangle asRectangle, AsPoint sOrigin)
        {
            throw new NotImplementedException();
        }

        public void fillRect(AsRectangle bounds, uint p)
        {
            throw new NotImplementedException();
        }

        public void setPixels(AsRectangle rect, AsByteArray bytes)
        {
            throw new NotImplementedException();
        }
    }
}
