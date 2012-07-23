using System;
 
using bc.flash;
using starling.display;
using starling.textures;
 
namespace starling.text
{
	public class AsBitmapChar : AsObject
	{
		private AsTexture mTexture;
		private int mCharID;
		private float mXOffset;
		private float mYOffset;
		private float mXAdvance;
		private AsDictionary mKernings;
		public AsBitmapChar(int id, AsTexture texture, float xOffset, float yOffset, float xAdvance)
		{
			mCharID = id;
			mTexture = texture;
			mXOffset = xOffset;
			mYOffset = yOffset;
			mXAdvance = xAdvance;
			mKernings = null;
		}
		public virtual void addKerning(int charID, float amount)
		{
			if((mKernings == null))
			{
				mKernings = new AsDictionary();
			}
			mKernings[charID] = amount;
		}
		public virtual float getKerning(int charID)
		{
			if(((mKernings == null) || (mKernings[charID] == null)))
			{
				return 0.0f;
			}
			else
			{
				return (float)(mKernings[charID]);
			}
		}
		public virtual AsImage createImage()
		{
			return new AsImage(mTexture);
		}
		public virtual int getCharID()
		{
			return mCharID;
		}
		public virtual float getXOffset()
		{
			return mXOffset;
		}
		public virtual float getYOffset()
		{
			return mYOffset;
		}
		public virtual float getXAdvance()
		{
			return mXAdvance;
		}
		public virtual AsTexture getTexture()
		{
			return mTexture;
		}
		public virtual float getWidth()
		{
			return mTexture.getWidth();
		}
		public virtual float getHeight()
		{
			return mTexture.getHeight();
		}
	}
}
