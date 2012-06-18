using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.events;
 
namespace bc.flash.events
{
	public class AsTouchData : AsObject
	{
		public AsTouch touch;
		public AsDisplayObject target;
		public AsTouchData(AsTouch touch, AsDisplayObject target)
		{
			this.touch = touch;
			this.target = target;
		}
	}
}
