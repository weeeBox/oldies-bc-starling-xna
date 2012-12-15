using System;
 
using bc.flash;
using bc.flash.errors;
using bc.flash.utils;
 
namespace bc.flash.display3D
{
	public sealed class AsProgram3D : AsObject
	{
		public void dispose()
		{
			throw new AsNotImplementedError();
		}
		public void upload(AsByteArray vertexProgram, AsByteArray fragmentProgram)
		{
			throw new AsNotImplementedError();
		}
	}
}
