using System;
 
using bc.flash;
using bc.flash.display3D;
using bc.flash.display3D.textures;
using starling.core;
using starling.display;
using starling.errors;
using starling.textures;
using starling.utils;
 
namespace starling.textures
{
	public class AsRenderTexture : AsTexture
	{
		private AsTexture mActiveTexture;
		private AsTexture mBufferTexture;
		private AsImage mHelperImage;
		private bool mDrawing;
		private int mNativeWidth;
		private int mNativeHeight;
		private AsRenderSupport mSupport;
		public AsRenderTexture(int width, int height, bool persistent, float scale)
		{
			if((scale <= 0))
			{
				scale = AsStarling.getContentScaleFactor();
			}
			mSupport = new AsRenderSupport();
			mNativeWidth = AsGlobal.getNextPowerOfTwo((int)((width * scale)));
			mNativeHeight = AsGlobal.getNextPowerOfTwo((int)((height * scale)));
			mActiveTexture = AsTexture.empty(width, height, (uint)(0x0), true, scale);
			if(persistent)
			{
				mBufferTexture = AsTexture.empty(width, height, (uint)(0x0), true, scale);
				mHelperImage = new AsImage(mBufferTexture);
				mHelperImage.setSmoothing(AsTextureSmoothing.NONE);
			}
		}
		public AsRenderTexture(int width, int height, bool persistent)
		 : this(width, height, persistent, -1)
		{
		}
		public AsRenderTexture(int width, int height)
		 : this(width, height, true, -1)
		{
		}
		public override void dispose()
		{
			mActiveTexture.dispose();
			if(getIsPersistent())
			{
				mBufferTexture.dispose();
				mHelperImage.dispose();
			}
			base.dispose();
		}
		public virtual void draw(AsDisplayObject _object, int antiAliasing)
		{
		}
		public virtual void draw(AsDisplayObject _object)
		{
			draw(_object, 0);
		}
		public virtual void drawBundled(AsObject drawingBlock, int antiAliasing)
		{
		}
		public virtual void drawBundled(AsObject drawingBlock)
		{
			drawBundled((AsObject)(drawingBlock), 0);
		}
		public virtual void clear()
		{
			AsContext3D context = AsStarling.getContext();
			if((context == null))
			{
				throw new AsMissingContextError();
			}
			context.setRenderToTexture(mActiveTexture.get_base());
			AsRenderSupport.clear();
			if(getIsPersistent())
			{
				context.setRenderToTexture(mActiveTexture.get_base());
				AsRenderSupport.clear();
			}
			context.setRenderToBackBuffer();
		}
		public override void adjustVertexData(AsVertexData vertexData, int vertexID, int count)
		{
			mActiveTexture.adjustVertexData(vertexData, vertexID, count);
		}
		public virtual bool getIsPersistent()
		{
			return (mBufferTexture != null);
		}
		public override float getWidth()
		{
			return mActiveTexture.getWidth();
		}
		public override float getHeight()
		{
			return mActiveTexture.getHeight();
		}
		public override float getScale()
		{
			return mActiveTexture.getScale();
		}
		public override bool getPremultipliedAlpha()
		{
			return mActiveTexture.getPremultipliedAlpha();
		}
		public override AsTextureBase get_base()
		{
			return mActiveTexture.get_base();
		}
	}
}
