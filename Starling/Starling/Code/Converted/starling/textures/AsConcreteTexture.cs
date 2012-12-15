using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.display3D;
using bc.flash.display3D.textures;
using starling.core;
using starling.events;
using starling.textures;
 
namespace starling.textures
{
	public class AsConcreteTexture : AsTexture
	{
		private AsTextureBase mBase;
		private String mFormat;
		private int mWidth;
		private int mHeight;
		private bool mMipMapping;
		private bool mPremultipliedAlpha;
		private bool mOptimizedForRenderTexture;
		private Object mData;
		private float mScale;
		public AsConcreteTexture(AsTextureBase _base, String format, int width, int height, bool mipMapping, bool premultipliedAlpha, bool optimizedForRenderTexture, float scale)
		{
			mScale = scale <= 0 ? 1.0f : scale;
			mBase = _base;
			mFormat = format;
			mWidth = width;
			mHeight = height;
			mMipMapping = mipMapping;
			mPremultipliedAlpha = premultipliedAlpha;
			mOptimizedForRenderTexture = optimizedForRenderTexture;
		}
		public AsConcreteTexture(AsTextureBase _base, String format, int width, int height, bool mipMapping, bool premultipliedAlpha, bool optimizedForRenderTexture)
		 : this(_base, format, width, height, mipMapping, premultipliedAlpha, optimizedForRenderTexture, 1)
		{
		}
		public AsConcreteTexture(AsTextureBase _base, String format, int width, int height, bool mipMapping, bool premultipliedAlpha)
		 : this(_base, format, width, height, mipMapping, premultipliedAlpha, false, 1)
		{
		}
		public override void dispose()
		{
			if(mBase != null)
			{
				mBase.dispose();
			}
			restoreOnLostContext(null);
			base.dispose();
		}
		public virtual void restoreOnLostContext(Object data)
		{
			if(mData == null && data != null)
			{
				AsStarling.getCurrent().addEventListener(AsEvent.CONTEXT3D_CREATE, onContextCreated);
			}
			if(data == null)
			{
				AsStarling.getCurrent().removeEventListener(AsEvent.CONTEXT3D_CREATE, onContextCreated);
			}
			mData = data;
		}
		private void onContextCreated(AsEvent _event)
		{
			AsContext3D context = AsStarling.getContext();
			AsBitmapData bitmapData = mData as AsBitmapData;
			AsAtfData atfData = mData as AsAtfData;
			bc.flash.display3D.textures.AsTexture nativeTexture = null;
			if(bitmapData != null)
			{
				nativeTexture = context.createTexture(mWidth, mHeight, AsContext3DTextureFormat.BGRA, mOptimizedForRenderTexture);
				AsTexture.uploadBitmapData(nativeTexture, bitmapData, mMipMapping);
			}
			else
			{
				if(atfData != null)
				{
					nativeTexture = context.createTexture(atfData.getWidth(), atfData.getHeight(), atfData.getFormat(), mOptimizedForRenderTexture);
					AsTexture.uploadAtfData(nativeTexture, atfData.getData());
				}
			}
			mBase = nativeTexture;
		}
		public virtual bool getOptimizedForRenderTexture()
		{
			return mOptimizedForRenderTexture;
		}
		public override AsTextureBase get_base()
		{
			return mBase;
		}
		public override String getFormat()
		{
			return mFormat;
		}
		public override float getWidth()
		{
			return mWidth / mScale;
		}
		public override float getHeight()
		{
			return mHeight / mScale;
		}
		public override float getScale()
		{
			return mScale;
		}
		public override bool getMipMapping()
		{
			return mMipMapping;
		}
		public override bool getPremultipliedAlpha()
		{
			return mPremultipliedAlpha;
		}
	}
}
