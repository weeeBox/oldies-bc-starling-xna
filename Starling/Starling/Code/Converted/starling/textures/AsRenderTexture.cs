using System;
 
using bc.flash;
using bc.flash.display3D;
using bc.flash.display3D.textures;
using bc.flash.geom;
using starling.core;
using starling.display;
using starling.errors;
using starling.textures;
using AsTexture = starling.textures.AsTexture;
 
namespace starling.textures
{
	public class AsRenderTexture : AsSubTexture
	{
		private const bool PMA = true;
		private AsTexture mActiveTexture;
		private AsTexture mBufferTexture;
		private AsImage mHelperImage;
		private bool mDrawing;
		private bool mBufferReady;
		private AsRenderSupport mSupport;
		private static AsRectangle sScissorRect = new AsRectangle();
		public AsRenderTexture(int width, int height, bool persistent, float scale)
		 : base(mActiveTexture = AsTexture.empty(width, height, PMA, true, scale = scale <= 0 ? AsStarling.getContentScaleFactor() : scale), new AsRectangle(0, 0, width, height), true)
		{
			int nativeWidth = AsGlobal.getNextPowerOfTwo((int)(width * scale));
			int nativeHeight = AsGlobal.getNextPowerOfTwo((int)(height * scale));
			mSupport = new AsRenderSupport();
			mSupport.setOrthographicProjection(0, 0, nativeWidth / scale, nativeHeight / scale);
			if(persistent)
			{
				mBufferTexture = AsTexture.empty(width, height, PMA, true, scale);
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
			if(getIsPersistent())
			{
				mBufferTexture.dispose();
				mHelperImage.dispose();
			}
			base.dispose();
		}
		public virtual void draw(AsDisplayObject _object, AsMatrix matrix, float alpha, int antiAliasing)
		{
		}
		public virtual void draw(AsDisplayObject _object, AsMatrix matrix, float alpha)
		{
			draw(_object, matrix, alpha, 0);
		}
		public virtual void draw(AsDisplayObject _object, AsMatrix matrix)
		{
			draw(_object, matrix, 1.0f, 0);
		}
		public virtual void draw(AsDisplayObject _object)
		{
			draw(_object, null, 1.0f, 0);
		}
		public virtual void drawBundled(AsDrawingBlockCallback drawingBlock, int antiAliasing)
		{
			float scale = mActiveTexture.getScale();
			AsContext3D context = AsStarling.getContext();
			if(context == null)
			{
				throw new AsMissingContextError();
			}
			sScissorRect.setTo(0, 0, mActiveTexture.getWidth() * scale, mActiveTexture.getHeight() * scale);
			context.setScissorRectangle(sScissorRect);
			if(getIsPersistent())
			{
				AsTexture tmpTexture = mActiveTexture;
				mActiveTexture = mBufferTexture;
				mBufferTexture = tmpTexture;
				mHelperImage.setTexture(mBufferTexture);
			}
			mSupport.setRenderTarget(mActiveTexture);
			mSupport.clear();
			if(getIsPersistent() && mBufferReady)
			{
				mHelperImage.render(mSupport, 1.0f);
			}
			else
			{
				mBufferReady = true;
			}
			try
			{
				mDrawing = true;
				if(drawingBlock != null)
				{
					drawingBlock();
				}
			}
			finally
			{
				mDrawing = false;
				mSupport.finishQuadBatch();
				mSupport.nextFrame();
				mSupport.setRenderTarget(null);
				context.setScissorRectangle(null);
			}
		}
		public virtual void drawBundled(AsDrawingBlockCallback drawingBlock)
		{
			drawBundled(drawingBlock, 0);
		}
		public virtual void clear()
		{
			AsContext3D context = AsStarling.getContext();
			if(context == null)
			{
				throw new AsMissingContextError();
			}
			mSupport.setRenderTarget(mActiveTexture);
			mSupport.clear();
			mSupport.setRenderTarget(null);
		}
		public virtual bool getIsPersistent()
		{
			return mBufferTexture != null;
		}
		public override AsTextureBase get_base()
		{
			return mActiveTexture.get_base();
		}
	}
}
