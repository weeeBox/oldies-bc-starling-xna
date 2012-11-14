using System;
 
using bc.flash;
using bc.flash.display3D;
using bc.flash.display3D.textures;
using bc.flash.geom;
using starling.core;
using starling.display;
using starling.errors;
using starling.textures;
using starling.utils;
using AsTexture = starling.textures.AsTexture;
 
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
			// FIXME: Block of code is cut here
		}
		public virtual void draw(AsDisplayObject _object)
		{
			draw(_object, 0);
		}
		public virtual void drawBundled(AsDrawingBlockCallback drawingBlock, int antiAliasing)
		{
			float scale = mActiveTexture.getScale();
			AsContext3D context = AsStarling.getContext();
			if((context == null))
			{
				throw new AsMissingContextError();
			}
			context.setScissorRectangle(new AsRectangle(0, 0, (mActiveTexture.getWidth() * scale), (mActiveTexture.getHeight() * scale)));
			if(getIsPersistent())
			{
				AsTexture tmpTexture = mActiveTexture;
				mActiveTexture = mBufferTexture;
				mBufferTexture = tmpTexture;
				mHelperImage.setTexture(mBufferTexture);
			}
			context.setRenderToTexture(mActiveTexture.get_base(), false, antiAliasing);
			AsRenderSupport.clear();
			mSupport.setOrthographicProjection((mNativeWidth / scale), (mNativeHeight / scale));
			mSupport.applyBlendMode(true);
			if(getIsPersistent())
			{
				mHelperImage.render(mSupport, 1.0f);
			}
			try
			{
				mDrawing = true;
				if((drawingBlock != null))
				{
					drawingBlock();
				}
			}
			catch ()
			{
			}
			finally{
				mDrawing = false;
				mSupport.finishQuadBatch();
				mSupport.nextFrame();
				context.setScissorRectangle(null);
				context.setRenderToBackBuffer();
			}
		}
		public virtual void drawBundled(AsDrawingBlockCallback drawingBlock)
		{
			drawBundled(drawingBlock, 0);
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
