using System;
 
using bc.flash;
using bc.flash.error;
 
namespace bc.flash.events
{
	public sealed class AsTouchPhase : AsObject
	{
		public static String HOVER = "hover";
		public static String BEGAN = "began";
		public static String MOVED = "moved";
		public static String STATIONARY = "stationary";
		public static String ENDED = "ended";
		public AsTouchPhase()
		{
			throw new AsAbstractClassError();
		}
	}
}
