using System;
 
using bc.flash.display;
using bc.flash.display3D.textures;
using bc.flash.utils;
 
namespace bc.flash.display3D.textures
{
	public sealed class AsCubeTexture : AsTextureBase
	{
		public void uploadCompressedTextureFromByteArray(AsByteArray data, uint byteArrayOffset, bool async)
		{
		}
		public void uploadCompressedTextureFromByteArray(AsByteArray data, uint byteArrayOffset)
		{
			uploadCompressedTextureFromByteArray(data, byteArrayOffset, false);
		}
		public void uploadFromBitmapData(AsBitmapData source, uint side, uint miplevel)
		{
		}
		public void uploadFromBitmapData(AsBitmapData source, uint side)
		{
			uploadFromBitmapData(source, side, (uint)(0));
		}
		public void uploadFromByteArray(AsByteArray data, uint byteArrayOffset, uint side, uint miplevel)
		{
		}
		public void uploadFromByteArray(AsByteArray data, uint byteArrayOffset, uint side)
		{
			uploadFromByteArray(data, byteArrayOffset, side, (uint)(0));
		}
	}
}
