using System;
 
using bc.flash;
using bc.flash.error;
 
namespace bc.flash.display3D.textures
{
	public class AsTextureBase : AsObject
	{
		public virtual void dispose()
		{
			throw new AsNotImplementedError();
		}
	}
}
