using System;
using bc.flash.media;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace bc.flash.resources
{
    public class BcSoundEffect : BcSound
    {
        private SoundEffect mEffect;
        private int mInstancesCount;

        public BcSoundEffect(SoundEffect effect)
        {
            mEffect = effect;
        }
        public override float Length
        {
            get { return (float)mEffect.Duration.TotalSeconds; }
        }

        public override AsSoundChannel Play(float startTime, int loops, AsSoundTransform sndTransform)
        {
            Debug.Assert(loops == 0 || loops == 1);

            AsSoundEffectChannel channel = new AsSoundEffectChannel(mEffect, sndTransform);
            channel.play(loops);
            return channel;
        }

        public override void Close()
        {
            if (mEffect != null)
            {
                mEffect.Dispose();
                mEffect = null;
            }
        }

        public override void Dispose()
        {
            Close();
            base.Dispose();                        
        }
    }
}
