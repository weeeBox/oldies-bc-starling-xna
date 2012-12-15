using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace bc.flash.media
{
    public class AsMusicChannel : AsSoundChannel
    {
        private Song mSong;

        public AsMusicChannel(Song song, AsSoundTransform transform) : base(transform)
        {
            this.mSong = song;
            applyTransform(transform);
        }

        public override float getLeftPeak()
        {
            return 0.0f;
        }

        public override float getRightPeak()
        {
            return 0.0f;
        }

        public override float getPosition()
        {
            return (float)MediaPlayer.PlayPosition.TotalSeconds;
        }

        protected override void applyTransform(AsSoundTransform transform)
        {
            MediaPlayer.Volume = transform.getVolume();
        }

        public override void play(int loopsCount)
        {
            MediaPlayer.IsRepeating = loopsCount != 0;
            MediaPlayer.Play(mSong);
        }

        public override void stop()
        {
            MediaPlayer.Stop();
        }
    }
}
