using System;
 
using bc.flash;
using bc.flash.display3D.textures;
using bc.flash.geom;
using starling.textures;
using starling.utils;
 
namespace starling.textures
{
	public class AsSubTexture : AsTexture
	{
		private AsTexture mParent;
		private AsRectangle mClipping;
		private AsRectangle mRootClipping;
		private bool mOwnsParent;
		private static AsPoint sTexCoords = new AsPoint();
		public AsSubTexture(AsTexture parentTexture, AsRectangle region, bool ownsParent)
		{
			mParent = parentTexture;
			mOwnsParent = ownsParent;
			if((region == null))
			{
				setClipping(new AsRectangle(0, 0, 1, 1));
			}
			else
			{
				setClipping(new AsRectangle((region.x / parentTexture.getWidth()), (region.y / parentTexture.getHeight()), (region.width / parentTexture.getWidth()), (region.height / parentTexture.getHeight())));
			}
		}
		public AsSubTexture(AsTexture parentTexture, AsRectangle region)
		 : this(parentTexture, region, false)
		{
		}
		public override void dispose()
		{
			if(mOwnsParent)
			{
				mParent.dispose();
			}
			base.dispose();
		}
		private void setClipping(AsRectangle _value)
		{
			mClipping = _value;
			mRootClipping = _value.clone();
			AsSubTexture parentTexture = ((mParent is AsSubTexture) ? ((AsSubTexture)(mParent)) : null);
			while(parentTexture != null)
			{
				AsRectangle parentClipping = parentTexture.mClipping;
				mRootClipping.x = (parentClipping.x + (mRootClipping.x * parentClipping.width));
				mRootClipping.y = (parentClipping.y + (mRootClipping.y * parentClipping.height));
				mRootClipping.width = (mRootClipping.width * parentClipping.width);
				mRootClipping.height = (mRootClipping.height * parentClipping.height);
				parentTexture = ((parentTexture.mParent is AsSubTexture) ? ((AsSubTexture)(parentTexture.mParent)) : null);
			}
		}
		public override void adjustVertexData(AsVertexData vertexData, int vertexID, int count)
		{
			base.adjustVertexData(vertexData, vertexID, count);
			float clipX = mRootClipping.x;
			float clipY = mRootClipping.y;
			float clipWidth = mRootClipping.width;
			float clipHeight = mRootClipping.height;
			int endIndex = (vertexID + count);
			int i = vertexID;
			for (; (i < endIndex); ++i)
			{
				vertexData.getTexCoords(i, sTexCoords);
				vertexData.setTexCoords(i, (clipX + (sTexCoords.x * clipWidth)), (clipY + (sTexCoords.y * clipHeight)));
			}
		}
		public virtual AsTexture getParent()
		{
			return mParent;
		}
		public virtual bool getOwnsParent()
		{
			return mOwnsParent;
		}
		public virtual AsRectangle getClipping()
		{
			return mClipping.clone();
		}
		public override AsTextureBase get_base()
		{
			return mParent.get_base();
		}
		public override float getWidth()
		{
			return (mParent.getWidth() * mClipping.width);
		}
		public override float getHeight()
		{
			return (mParent.getHeight() * mClipping.height);
		}
		public override bool getMipMapping()
		{
			return mParent.getMipMapping();
		}
		public override bool getPremultipliedAlpha()
		{
			return mParent.getPremultipliedAlpha();
		}
		public override float getScale()
		{
			return mParent.getScale();
		}
	}
}
