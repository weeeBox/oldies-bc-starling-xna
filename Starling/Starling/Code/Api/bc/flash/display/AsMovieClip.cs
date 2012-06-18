using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.error;
 
namespace bc.flash.display
{
	public class AsMovieClip : AsSprite
	{
		public virtual int getCurrentFrame()
		{
			throw new AsNotImplementedError();
		}
		public virtual String getCurrentFrameLabel()
		{
			throw new AsNotImplementedError();
		}
		public virtual String getCurrentLabel()
		{
			throw new AsNotImplementedError();
		}
		public virtual AsArray getCurrentLabels()
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getEnabled()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setEnabled(bool _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual int getFramesLoaded()
		{
			throw new AsNotImplementedError();
		}
		public virtual void gotoAndPlay(AsObject frame, String scene)
		{
			throw new AsNotImplementedError();
		}
		public virtual void gotoAndPlay(AsObject frame)
		{
			gotoAndPlay((AsObject)(frame), null);
		}
		public virtual void gotoAndStop(AsObject frame, String scene)
		{
			throw new AsNotImplementedError();
		}
		public virtual void gotoAndStop(AsObject frame)
		{
			gotoAndStop((AsObject)(frame), null);
		}
		public virtual void nextFrame()
		{
			throw new AsNotImplementedError();
		}
		public virtual void play()
		{
			throw new AsNotImplementedError();
		}
		public virtual void prevFrame()
		{
			throw new AsNotImplementedError();
		}
		public virtual void prevScene()
		{
			throw new AsNotImplementedError();
		}
		public virtual void stop()
		{
			throw new AsNotImplementedError();
		}
		public virtual int getTotalFrames()
		{
			throw new AsNotImplementedError();
		}
	}
}
