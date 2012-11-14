using System;
 
using bc.flash;
using bc.flash.display3D;
using bc.flash.utils;
 
namespace starling.textures
{
	public class AsAtfData : AsObject
	{
		private String mFormat;
		private int mWidth;
		private int mHeight;
		private int mNumTextures;
		private AsByteArray mData;
		public AsAtfData(AsByteArray data)
		{
			String signature = null;
			// FIXME: Block of code is cut here
			if((signature != "ATF"))
			{
				throw new AsArgumentError("Invalid ATF data");
			}
			switch(data.getOwnProperty(6))
			{
				case 0:
				case 1:
				mFormat = AsContext3DTextureFormat.BGRA;
				break;
				case 2:
				case 3:
				mFormat = AsContext3DTextureFormat.COMPRESSED;
				break;
				case 4:
				case 5:
				mFormat = "compressedAlpha";
				break;
				default:
				throw new AsError("Invalid ATF format");
			}
			mWidth = (int)(AsMath.pow(2, (float)(data.getOwnProperty(7))));
			mHeight = (int)(AsMath.pow(2, (float)(data.getOwnProperty(8))));
			mNumTextures = (int)(data.getOwnProperty(9));
			mData = data;
		}
		public virtual String getFormat()
		{
			return mFormat;
		}
		public virtual int getWidth()
		{
			return mWidth;
		}
		public virtual int getHeight()
		{
			return mHeight;
		}
		public virtual int getNumTextures()
		{
			return mNumTextures;
		}
		public virtual AsByteArray getData()
		{
			return mData;
		}
	}
}
