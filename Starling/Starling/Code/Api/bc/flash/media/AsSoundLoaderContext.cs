using System;
 
using bc.flash;
 
namespace bc.flash.media
{
	public class AsSoundLoaderContext : AsObject
	{
		public float bufferTime;
		public bool checkPolicyFile;
		public AsSoundLoaderContext(float bufferTime, bool checkPolicyFile)
		{
			this.bufferTime = bufferTime;
			this.checkPolicyFile = checkPolicyFile;
		}
		public AsSoundLoaderContext(float bufferTime)
		 : this(bufferTime, false)
		{
		}
		public AsSoundLoaderContext()
		 : this(1000, false)
		{
		}
	}
}
