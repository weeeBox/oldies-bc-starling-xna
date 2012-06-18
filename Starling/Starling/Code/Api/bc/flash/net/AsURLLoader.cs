using System;

using bc.flash;
using bc.flash.error;
using bc.flash.events;
using bc.flash.net;
using bc.flash.resources;
using System.Diagnostics;

namespace bc.flash.net
{
    public class AsURLLoader : AsEventDispatcher
    {
        public AsObject data;
        public String dataFormat = "text";
        public uint bytesLoaded = 0;
        public uint bytesTotal = 0;

        private AsURLRequest mRequest;

        public AsURLLoader(AsURLRequest request)
        {
            mRequest = request;
        }

        public AsURLLoader()
            : this(null)
        {
        }

        public virtual void close()
        {
            mRequest = null;
        }

        public virtual void load(AsURLRequest request)
        {
            mRequest = request;
            String url = request.getUrl();

            if (url.StartsWith("http"))
            {
                throw new NotImplementedException("Http loading not implemented");
            }

            data = BcResFactory.GetInstance().LoadResource(url);
            AsDebug.assert(data != null, url);

            dispatchEvent(new AsEvent(AsEvent.COMPLETE));
        }
    }
}
