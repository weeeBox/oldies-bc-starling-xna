using System;
 
using bc.flash;
using bc.flash.error;
using bc.flash.utils;
 
namespace bc.flash.display3D
{
	public sealed class AsIndexBuffer3D : AsObject
	{
		public void dispose()
		{
			throw new AsNotImplementedError();
		}
		public void uploadFromByteArray(AsByteArray data, int byteArrayOffset, int startOffset, int count)
		{
			throw new AsNotImplementedError();
		}
		public void uploadFromVector(AsVector<uint> data, int startOffset, int count)
		{
			throw new AsNotImplementedError();
		}
	}
}
