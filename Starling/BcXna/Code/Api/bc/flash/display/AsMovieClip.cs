using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.errors;
 
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
		public virtual void gotoAndPlay(Object frame, String scene)
		{
			throw new AsNotImplementedError();
		}
		public virtual void gotoAndPlay(Object frame)
		{
			gotoAndPlay(frame, null);
		}
		public virtual void gotoAndStop(Object frame, String scene)
		{
			throw new AsNotImplementedError();
		}
		public virtual void gotoAndStop(Object frame)
		{
			gotoAndStop(frame, null);
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
