using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.error;
using bc.flash.events;
using bc.flash.utils;
 
namespace bc.flash.display
{
	public class AsLoaderInfo : AsEventDispatcher
	{
		public virtual uint getActionScriptVersion()
		{
			return (uint)(3);
		}
		public virtual AsByteArray getBytes()
		{
			throw new AsNotImplementedError();
		}
		public virtual uint getBytesLoaded()
		{
			throw new AsNotImplementedError();
		}
		public virtual uint getBytesTotal()
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getChildAllowsParent()
		{
			throw new AsNotImplementedError();
		}
		public virtual AsDisplayObject getContent()
		{
			throw new AsNotImplementedError();
		}
		public virtual String getContentType()
		{
			throw new AsNotImplementedError();
		}
		public virtual float getFrameRate()
		{
			throw new AsNotImplementedError();
		}
		public virtual int getHeight()
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getIsURLInaccessible()
		{
			throw new AsNotImplementedError();
		}
		public virtual AsLoader getLoader()
		{
			throw new AsNotImplementedError();
		}
		public virtual String getLoaderURL()
		{
			throw new AsNotImplementedError();
		}
		public virtual AsObject getParameters()
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getParentAllowsChild()
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getSameDomain()
		{
			throw new AsNotImplementedError();
		}
		public virtual AsEventDispatcher getSharedEvents()
		{
			throw new AsNotImplementedError();
		}
		public virtual uint getSwfVersion()
		{
			throw new AsNotImplementedError();
		}
		public virtual String getUrl()
		{
			throw new AsNotImplementedError();
		}
		public virtual int getWidth()
		{
			throw new AsNotImplementedError();
		}
	}
}
