using System;
 
using bc.flash;
using bc.flash.errors;
using bc.flash.media;
using starling.animation;
using starling.display;
using starling.events;
using starling.textures;
 
namespace starling.display
{
	public class AsMovieClip : AsImage, AsIAnimatable
	{
		private AsVector<AsTexture> mTextures;
		private AsVector<AsSound> mSounds;
		private AsVector<float> mDurations;
		private AsVector<float> mStartTimes;
		private float mDefaultFrameDuration;
		private float mTotalTime;
		private float mCurrentTime;
		private int mCurrentFrame;
		private bool mLoop;
		private bool mPlaying;
		public AsMovieClip(AsVector<AsTexture> textures, float fps)
		{
			if((textures.getLength() > 0))
			{
				__$super$__(textures[0]);
				init(textures, fps);
			}
			else
			{
				throw new AsArgumentError("Empty texture array");
			}
		}
		public AsMovieClip(AsVector<AsTexture> textures)
		 : this(textures, 12)
		{
		}
		private void init(AsVector<AsTexture> textures, float fps)
		{
			if((fps <= 0))
			{
				throw new AsArgumentError(("Invalid fps: " + fps));
			}
			int numFrames = (int)(textures.getLength());
			mDefaultFrameDuration = (1.0f / fps);
			mLoop = true;
			mPlaying = true;
			mCurrentTime = 0.0f;
			mCurrentFrame = 0;
			mTotalTime = (mDefaultFrameDuration * numFrames);
			mTextures = textures.concat();
			mSounds = new AsVector<AsSound>(numFrames);
			mDurations = new AsVector<float>(numFrames);
			mStartTimes = new AsVector<float>(numFrames);
			int i = 0;
			for (; (i < numFrames); ++i)
			{
				mDurations[i] = mDefaultFrameDuration;
				mStartTimes[i] = (i * mDefaultFrameDuration);
			}
		}
		public virtual void addFrame(AsTexture texture, AsSound sound, float duration)
		{
			addFrameAt(getNumFrames(), texture, sound, duration);
		}
		public virtual void addFrame(AsTexture texture, AsSound sound)
		{
			addFrame(texture, sound, -1);
		}
		public virtual void addFrame(AsTexture texture)
		{
			addFrame(texture, null, -1);
		}
		public virtual void addFrameAt(int frameID, AsTexture texture, AsSound sound, float duration)
		{
			if(((frameID < 0) || (frameID > getNumFrames())))
			{
				throw new AsArgumentError("Invalid frame id");
			}
			if((duration < 0))
			{
				duration = mDefaultFrameDuration;
			}
			mTextures.splice(frameID, (uint)(0), texture);
			mSounds.splice(frameID, (uint)(0), sound);
			mDurations.splice(frameID, (uint)(0), duration);
			mTotalTime = (mTotalTime + duration);
			if(((frameID > 0) && (frameID == getNumFrames())))
			{
				mStartTimes[frameID] = (mStartTimes[(frameID - 1)] + mDurations[(frameID - 1)]);
			}
			else
			{
				updateStartTimes();
			}
		}
		public virtual void addFrameAt(int frameID, AsTexture texture, AsSound sound)
		{
			addFrameAt(frameID, texture, sound, -1);
		}
		public virtual void addFrameAt(int frameID, AsTexture texture)
		{
			addFrameAt(frameID, texture, null, -1);
		}
		public virtual void removeFrameAt(int frameID)
		{
			if(((frameID < 0) || (frameID >= getNumFrames())))
			{
				throw new AsArgumentError("Invalid frame id");
			}
			if((getNumFrames() == 1))
			{
				throw new AsIllegalOperationError("Movie clip must not be empty");
			}
			mTotalTime = (mTotalTime - getFrameDuration(frameID));
			mTextures.splice(frameID, (uint)(1));
			mSounds.splice(frameID, (uint)(1));
			mDurations.splice(frameID, (uint)(1));
			updateStartTimes();
		}
		public virtual AsTexture getFrameTexture(int frameID)
		{
			if(((frameID < 0) || (frameID >= getNumFrames())))
			{
				throw new AsArgumentError("Invalid frame id");
			}
			return mTextures[frameID];
		}
		public virtual void setFrameTexture(int frameID, AsTexture texture)
		{
			if(((frameID < 0) || (frameID >= getNumFrames())))
			{
				throw new AsArgumentError("Invalid frame id");
			}
			mTextures[frameID] = texture;
		}
		public virtual AsSound getFrameSound(int frameID)
		{
			if(((frameID < 0) || (frameID >= getNumFrames())))
			{
				throw new AsArgumentError("Invalid frame id");
			}
			return mSounds[frameID];
		}
		public virtual void setFrameSound(int frameID, AsSound sound)
		{
			if(((frameID < 0) || (frameID >= getNumFrames())))
			{
				throw new AsArgumentError("Invalid frame id");
			}
			mSounds[frameID] = sound;
		}
		public virtual float getFrameDuration(int frameID)
		{
			if(((frameID < 0) || (frameID >= getNumFrames())))
			{
				throw new AsArgumentError("Invalid frame id");
			}
			return mDurations[frameID];
		}
		public virtual void setFrameDuration(int frameID, float duration)
		{
			if(((frameID < 0) || (frameID >= getNumFrames())))
			{
				throw new AsArgumentError("Invalid frame id");
			}
			mTotalTime = (mTotalTime - getFrameDuration(frameID));
			mTotalTime = (mTotalTime + duration);
			mDurations[frameID] = duration;
			updateStartTimes();
		}
		public virtual void play()
		{
			mPlaying = true;
		}
		public virtual void pause()
		{
			mPlaying = false;
		}
		public virtual void stop()
		{
			mPlaying = false;
			setCurrentFrame(0);
		}
		private void updateStartTimes()
		{
			int numFrames = this.getNumFrames();
			mStartTimes.setLength(0);
			mStartTimes[0] = 0;
			int i = 1;
			for (; (i < numFrames); ++i)
			{
				mStartTimes[i] = (mStartTimes[(i - 1)] + mDurations[(i - 1)]);
			}
		}
		public virtual void advanceTime(float passedTime)
		{
			int finalFrame = 0;
			int previousFrame = mCurrentFrame;
			if((mLoop && (mCurrentTime == mTotalTime)))
			{
				mCurrentTime = 0.0f;
				mCurrentFrame = 0;
			}
			if(((!(mPlaying) || (passedTime == 0.0f)) || (mCurrentTime == mTotalTime)))
			{
				return;
			}
			mCurrentTime = (mCurrentTime + passedTime);
			finalFrame = (int)((mTextures.getLength() - 1));
			while((mCurrentTime >= (mStartTimes[mCurrentFrame] + mDurations[mCurrentFrame])))
			{
				if((mCurrentFrame == finalFrame))
				{
					if(hasEventListener(AsEvent.COMPLETE))
					{
						float restTime = (mCurrentTime - mTotalTime);
						mCurrentTime = mTotalTime;
						dispatchEventWith(AsEvent.COMPLETE);
						advanceTime(restTime);
						return;
					}
					if(mLoop)
					{
						mCurrentTime = (mCurrentTime - mTotalTime);
						mCurrentFrame = 0;
					}
					else
					{
						mCurrentTime = mTotalTime;
						break;
					}
				}
				else
				{
					mCurrentFrame++;
					AsSound sound = mSounds[mCurrentFrame];
					if(sound != null)
					{
						sound.play();
					}
				}
			}
			if((mCurrentFrame != previousFrame))
			{
				setTexture(mTextures[mCurrentFrame]);
			}
		}
		public virtual bool getIsComplete()
		{
			return (!(mLoop) && (mCurrentTime >= mTotalTime));
		}
		public virtual float getTotalTime()
		{
			return mTotalTime;
		}
		public virtual int getNumFrames()
		{
			return (int)(mTextures.getLength());
		}
		public virtual bool getLoop()
		{
			return mLoop;
		}
		public virtual void setLoop(bool _value)
		{
			mLoop = _value;
		}
		public virtual int getCurrentFrame()
		{
			return mCurrentFrame;
		}
		public virtual void setCurrentFrame(int _value)
		{
			mCurrentFrame = _value;
			mCurrentTime = 0.0f;
			int i = 0;
			for (; (i < _value); ++i)
			{
				mCurrentTime = (mCurrentTime + getFrameDuration(i));
			}
			setTexture(mTextures[mCurrentFrame]);
			if(mSounds[mCurrentFrame] != null)
			{
				mSounds[mCurrentFrame].play();
			}
		}
		public virtual float getFps()
		{
			return (1.0f / mDefaultFrameDuration);
		}
		public virtual void setFps(float _value)
		{
			if((_value <= 0))
			{
				throw new AsArgumentError(("Invalid fps: " + _value));
			}
			float newFrameDuration = (1.0f / _value);
			float acceleration = (newFrameDuration / mDefaultFrameDuration);
			mCurrentTime = (mCurrentTime * acceleration);
			mDefaultFrameDuration = newFrameDuration;
			int i = 0;
			for (; (i < getNumFrames()); ++i)
			{
				setFrameDuration(i, (getFrameDuration(i) * acceleration));
			}
		}
		public virtual bool getIsPlaying()
		{
			if(mPlaying)
			{
				return (mLoop || (mCurrentTime < mTotalTime));
			}
			else
			{
				return false;
			}
		}
	}
}
