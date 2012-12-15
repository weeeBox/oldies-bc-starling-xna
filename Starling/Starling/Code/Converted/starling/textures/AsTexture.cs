using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.display3D;
using bc.flash.display3D.textures;
using bc.flash.geom;
using bc.flash.system;
using bc.flash.utils;
using starling.core;
using starling.errors;
using starling.textures;
using starling.utils;
 
namespace starling.textures
{
	public class AsTexture : AsObject
	{
		private AsRectangle mFrame;
		private bool mRepeat;
		private static AsPoint sOrigin = new AsPoint();
		public AsTexture()
		{
			if(AsCapabilities.getIsDebugger() && AsGlobal.getQualifiedClassName(this) == "starling.textures::Texture")
			{
				throw new AsAbstractClassError();
			}
			mRepeat = false;
		}
		public virtual void dispose()
		{
		}
		public static AsTexture fromBitmap(AsBitmap data, bool generateMipMaps, bool optimizeForRenderTexture, float scale)
		{
			return fromBitmapData(data.getBitmapData(), generateMipMaps, optimizeForRenderTexture, scale);
		}
		public static AsTexture fromBitmap(AsBitmap data, bool generateMipMaps, bool optimizeForRenderTexture)
		{
			return fromBitmap(data, generateMipMaps, optimizeForRenderTexture, 1);
		}
		public static AsTexture fromBitmap(AsBitmap data, bool generateMipMaps)
		{
			return fromBitmap(data, generateMipMaps, false, 1);
		}
		public static AsTexture fromBitmap(AsBitmap data)
		{
			return fromBitmap(data, true, false, 1);
		}
		public static AsTexture fromBitmapData(AsBitmapData data, bool generateMipMaps, bool optimizeForRenderTexture, float scale)
		{
			int origWidth = data.getWidth();
			int origHeight = data.getHeight();
			int legalWidth = AsGlobal.getNextPowerOfTwo(origWidth);
			int legalHeight = AsGlobal.getNextPowerOfTwo(origHeight);
			AsContext3D context = AsStarling.getContext();
			AsBitmapData potData = null;
			if(context == null)
			{
				throw new AsMissingContextError();
			}
			bc.flash.display3D.textures.AsTexture nativeTexture = context.createTexture(legalWidth, legalHeight, AsContext3DTextureFormat.BGRA, optimizeForRenderTexture);
			if(legalWidth > origWidth || legalHeight > origHeight)
			{
				potData = new AsBitmapData(legalWidth, legalHeight, true, 0);
				potData.copyPixels(data, data.getRect(), sOrigin);
				data = potData;
			}
			uploadBitmapData(nativeTexture, data, generateMipMaps);
			AsConcreteTexture concreteTexture = new AsConcreteTexture(nativeTexture, AsContext3DTextureFormat.BGRA, legalWidth, legalHeight, generateMipMaps, true, optimizeForRenderTexture, scale);
			if(AsStarling.getHandleLostContext())
			{
				concreteTexture.restoreOnLostContext(data);
			}
			else
			{
				if(potData != null)
				{
					potData.dispose();
				}
			}
			if(origWidth == legalWidth && origHeight == legalHeight)
			{
				return concreteTexture;
			}
			else
			{
				return new AsSubTexture(concreteTexture, new AsRectangle(0, 0, origWidth / scale, origHeight / scale), true);
			}
		}
		public static AsTexture fromBitmapData(AsBitmapData data, bool generateMipMaps, bool optimizeForRenderTexture)
		{
			return fromBitmapData(data, generateMipMaps, optimizeForRenderTexture, 1);
		}
		public static AsTexture fromBitmapData(AsBitmapData data, bool generateMipMaps)
		{
			return fromBitmapData(data, generateMipMaps, false, 1);
		}
		public static AsTexture fromBitmapData(AsBitmapData data)
		{
			return fromBitmapData(data, true, false, 1);
		}
		public static AsTexture fromAtfData(AsByteArray data, float scale)
		{
			AsContext3D context = AsStarling.getContext();
			if(context == null)
			{
				throw new AsMissingContextError();
			}
			AsAtfData atfData = new AsAtfData(data);
			bc.flash.display3D.textures.AsTexture nativeTexture = context.createTexture(atfData.getWidth(), atfData.getHeight(), atfData.getFormat(), false);
			uploadAtfData(nativeTexture, data);
			AsConcreteTexture concreteTexture = new AsConcreteTexture(nativeTexture, atfData.getFormat(), atfData.getWidth(), atfData.getHeight(), atfData.getNumTextures() > 1, false, false, scale);
			if(AsStarling.getHandleLostContext())
			{
				concreteTexture.restoreOnLostContext(atfData);
			}
			return concreteTexture;
		}
		public static AsTexture fromAtfData(AsByteArray data)
		{
			return fromAtfData(data, 1);
		}
		public static AsTexture fromColor(int width, int height, uint color, bool optimizeForRenderTexture, float scale)
		{
			if(scale <= 0)
			{
				scale = AsStarling.getContentScaleFactor();
			}
			AsBitmapData bitmapData = new AsBitmapData(width * scale, height * scale, true, color);
			AsTexture texture = fromBitmapData(bitmapData, false, optimizeForRenderTexture, scale);
			if(!AsStarling.getHandleLostContext())
			{
				bitmapData.dispose();
			}
			return texture;
		}
		public static AsTexture fromColor(int width, int height, uint color, bool optimizeForRenderTexture)
		{
			return fromColor(width, height, color, optimizeForRenderTexture, -1);
		}
		public static AsTexture fromColor(int width, int height, uint color)
		{
			return fromColor(width, height, color, false, -1);
		}
		public static AsTexture fromColor(int width, int height)
		{
			return fromColor(width, height, (uint)(0xffffffff), false, -1);
		}
		public static AsTexture empty(int width, int height, bool premultipliedAlpha, bool optimizeForRenderTexture, float scale)
		{
			if(scale <= 0)
			{
				scale = AsStarling.getContentScaleFactor();
			}
			int origWidth = (int)(width * scale);
			int origHeight = (int)(height * scale);
			int legalWidth = AsGlobal.getNextPowerOfTwo(origWidth);
			int legalHeight = AsGlobal.getNextPowerOfTwo(origHeight);
			String format = AsContext3DTextureFormat.BGRA;
			AsContext3D context = AsStarling.getContext();
			if(context == null)
			{
				throw new AsMissingContextError();
			}
			bc.flash.display3D.textures.AsTexture nativeTexture = context.createTexture(legalWidth, legalHeight, AsContext3DTextureFormat.BGRA, optimizeForRenderTexture);
			AsConcreteTexture concreteTexture = new AsConcreteTexture(nativeTexture, format, legalWidth, legalHeight, false, premultipliedAlpha, optimizeForRenderTexture, scale);
			if(origWidth == legalWidth && origHeight == legalHeight)
			{
				return concreteTexture;
			}
			else
			{
				return new AsSubTexture(concreteTexture, new AsRectangle(0, 0, width, height), true);
			}
		}
		public static AsTexture empty(int width, int height, bool premultipliedAlpha, bool optimizeForRenderTexture)
		{
			return empty(width, height, premultipliedAlpha, optimizeForRenderTexture, -1);
		}
		public static AsTexture empty(int width, int height, bool premultipliedAlpha)
		{
			return empty(width, height, premultipliedAlpha, true, -1);
		}
		public static AsTexture empty(int width, int height)
		{
			return empty(width, height, false, true, -1);
		}
		public static AsTexture empty(int width)
		{
			return empty(width, 64, false, true, -1);
		}
		public static AsTexture empty()
		{
			return empty(64, 64, false, true, -1);
		}
		public static AsTexture fromTexture(AsTexture texture, AsRectangle region, AsRectangle frame)
		{
			AsTexture subTexture = new AsSubTexture(texture, region);
			subTexture.mFrame = frame;
			return subTexture;
		}
		public static AsTexture fromTexture(AsTexture texture, AsRectangle region)
		{
			return fromTexture(texture, region, null);
		}
		public static AsTexture fromTexture(AsTexture texture)
		{
			return fromTexture(texture, null, null);
		}
		public virtual void adjustVertexData(AsVertexData vertexData, int vertexID, int count)
		{
			if(mFrame != null)
			{
				if(count != 4)
				{
					throw new AsArgumentError("Textures with a frame can only be used on quads");
				}
				float deltaRight = mFrame.width + mFrame.x - getWidth();
				float deltaBottom = mFrame.height + mFrame.y - getHeight();
				vertexData.translateVertex(vertexID, -mFrame.x, -mFrame.y);
				vertexData.translateVertex(vertexID + 1, -deltaRight, -mFrame.y);
				vertexData.translateVertex(vertexID + 2, -mFrame.x, -deltaBottom);
				vertexData.translateVertex(vertexID + 3, -deltaRight, -deltaBottom);
			}
		}
		public static void uploadBitmapData(bc.flash.display3D.textures.AsTexture nativeTexture, AsBitmapData data, bool generateMipmaps)
		{
			nativeTexture.uploadFromBitmapData(data);
			if(generateMipmaps && data.getWidth() > 1 && data.getHeight() > 1)
			{
				int currentWidth = data.getWidth() >> 1;
				int currentHeight = data.getHeight() >> 1;
				int level = 1;
				AsBitmapData canvas = new AsBitmapData(currentWidth, currentHeight, true, 0);
				AsMatrix transform = new AsMatrix(.5f, 0, 0, .5f);
				AsRectangle bounds = new AsRectangle();
				while(currentWidth >= 1 || currentHeight >= 1)
				{
					bounds.width = currentWidth;
					bounds.height = currentHeight;
					canvas.fillRect(bounds, (uint)(0));
					canvas.draw(data, transform, null, null, null, true);
					nativeTexture.uploadFromBitmapData(canvas, (uint)(level++));
					transform.scale(0.5f, 0.5f);
					currentWidth = currentWidth >> 1;
					currentHeight = currentHeight >> 1;
				}
				canvas.dispose();
			}
		}
		public static void uploadAtfData(bc.flash.display3D.textures.AsTexture nativeTexture, AsByteArray data, int offset)
		{
			nativeTexture.uploadCompressedTextureFromByteArray(data, (uint)(offset));
		}
		public static void uploadAtfData(bc.flash.display3D.textures.AsTexture nativeTexture, AsByteArray data)
		{
			uploadAtfData(nativeTexture, data, 0);
		}
		public virtual AsRectangle getFrame()
		{
			return mFrame != null ? mFrame.clone() : new AsRectangle(0, 0, getWidth(), getHeight());
		}
		public virtual bool getRepeat()
		{
			return mRepeat;
		}
		public virtual void setRepeat(bool _value)
		{
			mRepeat = _value;
		}
		public virtual float getWidth()
		{
			return 0;
		}
		public virtual float getHeight()
		{
			return 0;
		}
		public virtual float getScale()
		{
			return 1.0f;
		}
		public virtual AsTextureBase get_base()
		{
			return null;
		}
		public virtual String getFormat()
		{
			return AsContext3DTextureFormat.BGRA;
		}
		public virtual bool getMipMapping()
		{
			return false;
		}
		public virtual bool getPremultipliedAlpha()
		{
			return false;
		}
	}
}
