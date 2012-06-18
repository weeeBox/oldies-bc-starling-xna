using System;
 
using bc.flash.display;
using bc.flash.display3D.textures;
using bc.flash.error;
using bc.flash.utils;
 
namespace bc.flash.display3D.textures
{
	public sealed class AsTexture : AsTextureBase
	{
		public void uploadCompressedTextureFromByteArray(AsByteArray data, uint byteArrayOffset, bool async)
		{
			throw new AsNotImplementedError();
		}
		public void uploadCompressedTextureFromByteArray(AsByteArray data, uint byteArrayOffset)
		{
			uploadCompressedTextureFromByteArray(data, byteArrayOffset, false);
		}
		public void uploadFromBitmapData(AsBitmapData source, uint miplevel)
		{
			throw new AsNotImplementedError();
		}
		public void uploadFromBitmapData(AsBitmapData source)
		{
			uploadFromBitmapData(source, (uint)(0));
		}
		public void uploadFromByteArray(AsByteArray data, uint byteArrayOffset, uint miplevel)
		{
			throw new AsNotImplementedError();
		}
		public void uploadFromByteArray(AsByteArray data, uint byteArrayOffset)
		{
			uploadFromByteArray(data, byteArrayOffset, (uint)(0));
		}
	}
}
