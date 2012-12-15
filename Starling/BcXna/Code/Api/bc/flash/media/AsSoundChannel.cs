using System;

using bc.flash;
using bc.flash.error;
using bc.flash.media;

namespace bc.flash.media
{
    public abstract class AsSoundChannel : AsObject
    {
        protected AsSoundTransform mSoundTransform;

        protected AsSoundChannel(AsSoundTransform transform)
        {
            mSoundTransform = transform;
        }

        protected abstract void applyTransform(AsSoundTransform transform);
        public abstract float getLeftPeak();
        public abstract float getPosition();
        public abstract float getRightPeak();

        public abstract void play(int loopsCount);
        public abstract void stop();

        public virtual AsSoundTransform getSoundTransform()
        {
            return mSoundTransform;
        }
        public virtual void setSoundTransform(AsSoundTransform sndTransform)
        {
            mSoundTransform = sndTransform;
            applyTransform(sndTransform);
        }
    }
}
