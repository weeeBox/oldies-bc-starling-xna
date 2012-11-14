using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.geom;
using starling.core;
using starling.display;
using starling.textures;
using starling.utils;
 
namespace starling.display
{
	public class AsImage : AsQuad
	{
		private AsTexture mTexture;
		private String mSmoothing;
		private AsVertexData mVertexDataCache;
		private bool mVertexDataCacheInvalid;
		public AsImage(AsTexture texture)
		{
			if(texture != null)
			{
				AsRectangle frame = texture.getFrame();
				float width = ((frame != null) ? (frame.width) : (texture.getWidth()));
				float height = ((frame != null) ? (frame.height) : (texture.getHeight()));
				bool pma = texture.getPremultipliedAlpha();
				__$super$__(width, height, 0xffffff, pma);
				mVertexData.setTexCoords(0, 0.0f, 0.0f);
				mVertexData.setTexCoords(1, 1.0f, 0.0f);
				mVertexData.setTexCoords(2, 0.0f, 1.0f);
				mVertexData.setTexCoords(3, 1.0f, 1.0f);
				mTexture = texture;
				mSmoothing = AsTextureSmoothing.BILINEAR;
				mVertexDataCache = new AsVertexData(4, pma);
				mVertexDataCacheInvalid = true;
			}
			else
			{
				throw new AsArgumentError("Texture cannot be null");
			}
		}
		public static AsImage fromBitmap(AsBitmap bitmap)
		{
			return new AsImage(AsTexture.fromBitmap(bitmap));
		}
		protected override void onVertexDataChanged()
		{
			mVertexDataCacheInvalid = true;
		}
		public virtual void readjustSize()
		{
			AsRectangle frame = getTexture().getFrame();
			float width = ((frame != null) ? (frame.width) : (getTexture().getWidth()));
			float height = ((frame != null) ? (frame.height) : (getTexture().getHeight()));
			mVertexData.setPosition(0, 0.0f, 0.0f);
			mVertexData.setPosition(1, width, 0.0f);
			mVertexData.setPosition(2, 0.0f, height);
			mVertexData.setPosition(3, width, height);
			onVertexDataChanged();
		}
		public virtual void setTexCoords(int vertexID, AsPoint coords)
		{
			mVertexData.setTexCoords(vertexID, coords.x, coords.y);
			onVertexDataChanged();
		}
		public virtual AsPoint getTexCoords(int vertexID, AsPoint resultPoint)
		{
			if((resultPoint == null))
			{
				resultPoint = new AsPoint();
			}
			mVertexData.getTexCoords(vertexID, resultPoint);
			return resultPoint;
		}
		public virtual AsPoint getTexCoords(int vertexID)
		{
			return getTexCoords(vertexID, null);
		}
		public override void copyVertexDataTo(AsVertexData targetData, int targetVertexID)
		{
			if(mVertexDataCacheInvalid)
			{
				mVertexDataCacheInvalid = false;
				mVertexData.copyTo(mVertexDataCache);
				mTexture.adjustVertexData(mVertexDataCache, 0, 4);
			}
			mVertexDataCache.copyTo(targetData, targetVertexID);
		}
		public virtual void copyVertexDataTo(AsVertexData targetData)
		{
			copyVertexDataTo(targetData, 0);
		}
		public virtual AsTexture getTexture()
		{
			return mTexture;
		}
		public virtual void setTexture(AsTexture _value)
		{
			if((_value == null))
			{
				throw new AsArgumentError("Texture cannot be null");
			}
			else
			{
				if((_value != mTexture))
				{
					mTexture = _value;
					mVertexData.setPremultipliedAlpha(mTexture.getPremultipliedAlpha());
					onVertexDataChanged();
				}
			}
		}
		public virtual String getSmoothing()
		{
			return mSmoothing;
		}
		public virtual void setSmoothing(String _value)
		{
			if(AsTextureSmoothing.isValid(_value))
			{
				mSmoothing = _value;
			}
			else
			{
				throw new AsArgumentError(("Invalid smoothing mode: " + _value));
			}
		}
		public override void render(AsRenderSupport support, float parentAlpha)
		{
			support.batchQuad(this, parentAlpha, mTexture, mSmoothing);
		}
	}
}
