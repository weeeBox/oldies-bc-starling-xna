using System;
 
using bc.flash;
using bc.flash.error;
using bc.flash.utils;
 
namespace bc.flash.display3D
{
	public class AsVertexBuffer3D : AsObject
	{
		public virtual void dispose()
		{
			throw new AsNotImplementedError();
		}
		public virtual void uploadFromByteArray(AsByteArray data, int byteArrayOffset, int startVertex, int numVertices)
		{
			throw new AsNotImplementedError();
		}
		public virtual void uploadFromVector(AsVector<float> data, int startVertex, int numVertices)
		{
			throw new AsNotImplementedError();
		}
	}
}
