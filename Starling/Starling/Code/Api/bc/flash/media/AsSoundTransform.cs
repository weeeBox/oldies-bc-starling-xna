using System;
 
using bc.flash;
 
namespace bc.flash.media
{
	public sealed class AsSoundTransform : AsObject
	{
		private float mVolume;
		private float mPanning;
		private float mLeftToLeft;
		private float mLeftToRight;
		private float mRightToLeft;
		private float mRightToRight;
		public AsSoundTransform(float vol, float panning)
		{
			mVolume = vol;
			mPanning = panning;
		}
		public AsSoundTransform(float vol)
		 : this(vol, 0)
		{
		}
		public AsSoundTransform()
		 : this(1, 0)
		{
		}
		public float getLeftToLeft()
		{
			return mLeftToLeft;
		}
		public void setLeftToLeft(float leftToLeft)
		{
			mLeftToLeft = leftToLeft;
		}
		public float getLeftToRight()
		{
			return mLeftToRight;
		}
		public void setLeftToRight(float leftToRight)
		{
			mLeftToRight = leftToRight;
		}
		public float getPan()
		{
			return mPanning;
		}
		public void setPan(float panning)
		{
			mPanning = panning;
		}
		public float getRightToLeft()
		{
			return mRightToLeft;
		}
		public void setRightToLeft(float rightToLeft)
		{
			mRightToLeft = rightToLeft;
		}
		public float getRightToRight()
		{
			return mRightToRight;
		}
		public void setRightToRight(float rightToRight)
		{
			mRightToRight = rightToRight;
		}
		public float getVolume()
		{
			return mVolume;
		}
		public void setVolume(float volume)
		{
			mVolume = volume;
		}
	}
}
