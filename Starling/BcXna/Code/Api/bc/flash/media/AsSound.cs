using System;

using bc.flash.error;
using bc.flash.events;
using bc.flash.net;
using bc.flash.resources;
using System.Diagnostics;

namespace bc.flash.media
{
    public class AsSound : AsEventDispatcher
    {
        private AsURLRequest mRequest;
        private AsSoundLoaderContext mContext;

        private BcSound mSound;

        public AsSound(AsURLRequest request, AsSoundLoaderContext context)
        {
            mRequest = request;
            mContext = context;
        }
        public AsSound(AsURLRequest request)
            : this(null, null)
        {
        }

        public AsSound()
            : this(null)
        {
        }

        public virtual uint getBytesLoaded()
        {
            throw new AsNotImplementedError();
        }

        public virtual int getBytesTotal()
        {
            throw new AsNotImplementedError();
        }

        public virtual void close()
        {
            mSound.Close();
        }

        public virtual float getLength()
        {
            return mSound.Length;
        }

        public virtual void load(AsURLRequest request, AsSoundLoaderContext context)
        {
            mRequest = request;
            mContext = context;

            String url = request.getUrl();

            if (url.StartsWith("http"))
            {
                throw new NotImplementedException("Http loading not implemented");
            }

            mSound = BcResFactory.GetInstance().LoadSound(url);
            AsDebug.assert(mSound != null, url);

            Debug.WriteLine("Sound loaded: " + url);
            dispatchEvent(new AsEvent(AsEvent.COMPLETE));
        }

        public virtual void load(AsURLRequest request)
        {
            load(request, null);
        }

        public virtual AsSoundChannel play(float startTime, int loops, AsSoundTransform sndTransform)
        {
            return mSound.Play(startTime, loops, sndTransform);
        }

        public virtual AsSoundChannel play(float startTime, int loops)
        {
            return play(0, 0, null);
        }

        public virtual AsSoundChannel play(float startTime)
        {
            return play(0, 0);
        }

        public virtual AsSoundChannel play()
        {
            return play(0);
        }

        public virtual String getUrl()
        {
            return mRequest.getUrl();
        }
    }
}
