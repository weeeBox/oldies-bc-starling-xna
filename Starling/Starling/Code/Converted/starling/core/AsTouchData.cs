using System;
 
using bc.flash;
using starling.display;
using starling.events;
 
namespace starling.core
{
	public class AsTouchData : AsObject
	{
		private AsTouch mTouch;
		private AsDisplayObject mTarget;
		public AsTouchData(AsTouch touch, AsDisplayObject target)
		{
			mTouch = touch;
			mTarget = target;
		}
		public virtual AsTouch getTouch()
		{
			return mTouch;
		}
		public virtual AsDisplayObject getTarget()
		{
			return mTarget;
		}
	}
}
