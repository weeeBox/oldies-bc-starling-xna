using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.display3D;
using bc.flash.display3D.textures;
using bc.flash.utils;
using starling.core;
using starling.events;
using starling.textures;
 
namespace starling.textures
{
	public class AsConcreteTexture : AsTexture
	{
		private AsTextureBase mBase;
		private int mWidth;
		private int mHeight;
		private bool mMipMapping;
		private bool mPremultipliedAlpha;
		private bool mOptimizedForRenderTexture;
		private AsObject mData;
		private float mScale;
		public AsConcreteTexture(AsTextureBase _base, int width, int height, bool mipMapping, bool premultipliedAlpha, bool optimizedForRenderTexture, float scale)
		{
			mScale = (((scale <= 0)) ? (1.0f) : (scale));
			mBase = _base;
			mWidth = width;
			mHeight = height;
			mMipMapping = mipMapping;
			mPremultipliedAlpha = premultipliedAlpha;
			mOptimizedForRenderTexture = optimizedForRenderTexture;
		}
		public AsConcreteTexture(AsTextureBase _base, int width, int height, bool mipMapping, bool premultipliedAlpha, bool optimizedForRenderTexture)
		 : this(_base, width, height, mipMapping, premultipliedAlpha, optimizedForRenderTexture, 1)
		{
		}
		public AsConcreteTexture(AsTextureBase _base, int width, int height, bool mipMapping, bool premultipliedAlpha)
		 : this(_base, width, height, mipMapping, premultipliedAlpha, false, 1)
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
		public virtual void restoreOnLostContext(AsObject data)
		{
			if(((mData == null) && (data != null)))
			{
				AsStarling.getCurrent().addEventListener(AsEvent.CONTEXT3D_CREATE, onContextCreated);
			}
			if((data == null))
			{
				AsStarling.getCurrent().removeEventListener(AsEvent.CONTEXT3D_CREATE, onContextCreated);
			}
			mData = (AsObject)(data);
		}
		private void onContextCreated(AsEvent _event)
		{
			AsContext3D context = AsStarling.getContext();
			AsBitmapData bitmapData = ((mData is AsBitmapData) ? ((AsBitmapData)(mData)) : null);
			AsByteArray byteData = ((mData is AsByteArray) ? ((AsByteArray)(mData)) : null);
			bc.flash.display3D.textures.AsTexture nativeTexture = null;
			if(bitmapData != null)
			{
				nativeTexture = context.createTexture(mWidth, mHeight, AsContext3DTextureFormat.BGRA, mOptimizedForRenderTexture);
				AsTexture.uploadBitmapData(nativeTexture, bitmapData, mMipMapping);
			}
			else
			{
				if(byteData != null)
				{
					String format = (((byteData.getOwnProperty(6) == 2)) ? (AsContext3DTextureFormat.COMPRESSED) : (AsContext3DTextureFormat.BGRA));
					nativeTexture = context.createTexture(mWidth, mHeight, format, mOptimizedForRenderTexture);
					AsTexture.uploadAtfData(nativeTexture, byteData);
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
		public override float getWidth()
		{
			return (mWidth / mScale);
		}
		public override float getHeight()
		{
			return (mHeight / mScale);
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
