using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bc.flash.media;

namespace bc.flash.resources
{
    public abstract class BcSound : BcManagedResource
    {
        public abstract float Length { get; }
        public abstract AsSoundChannel Play(float startTime, int loops, AsSoundTransform sndTransform);
        public abstract void Close();        
    }
}
