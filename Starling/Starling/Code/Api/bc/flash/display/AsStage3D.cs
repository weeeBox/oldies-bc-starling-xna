using System;
 
using bc.flash;
using bc.flash.display3D;
using bc.flash.errors;
using bc.flash.events;
 
namespace bc.flash.display
{
	public class AsStage3D : AsEventDispatcher
	{
		public virtual AsContext3D getContext3D()
		{
			throw new AsNotImplementedError();
		}
		public virtual void requestContext3D(String context3DRenderMode)
		{
			throw new AsNotImplementedError();
		}
		public virtual void requestContext3D()
		{
			requestContext3D("auto");
		}
		public virtual bool getVisible()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setVisible(bool _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual float getX()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setX(float _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual float getY()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setY(float _value)
		{
			throw new AsNotImplementedError();
		}
	}
}
