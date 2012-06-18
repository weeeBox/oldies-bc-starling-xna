using System;

using bc.flash.display;
using bc.flash.error;
using bc.flash.net;
using bc.flash.system;
using bc.flash.utils;
using bc.flash.resources;
using bc.flash.events;
using bc.core.display;
using System.Diagnostics;

namespace bc.flash.display
{
    public class AsLoader : AsDisplayObjectContainer
    {
        private AsDisplayObject mContent;
        private AsLoaderInfo mContentLoaderInfo;
        private AsURLRequest mRequest;

        public AsLoader()
        {
            mContentLoaderInfo = new AsLoaderInfo();
        }
        public virtual void close()
        {
        }
        public virtual AsDisplayObject getContent()
        {
            return mContent;
        }
        public virtual AsLoaderInfo getContentLoaderInfo()
        {
            return mContentLoaderInfo;
        }
        public virtual void load(AsURLRequest request, AsLoaderContext context)
        {
            mRequest = request;
            String url = request.getUrl();

            if (url.StartsWith("http"))
            {
                throw new NotImplementedException("Http loading not implemented");
            }

            BcTexture2D image = BcResFactory.GetInstance().LoadImage(url);
            AsDebug.assert(image != null, url);

            AsBitmapData bitmapData = new AsBitmapData(image);
            mContent = new AsBitmap(bitmapData);

            Debug.WriteLine("Image loaded: " + url);
            dispatchEvent(new AsEvent(AsEvent.COMPLETE));
        }
        public override void dispatchEvent(AsEvent _event)
        {
            if (mContentLoaderInfo.hasEventListener(_event.getType()))
            {
                mContentLoaderInfo.dispatchEvent(_event);
            }
            else
            {
                base.dispatchEvent(_event);
            }            
        }

        public virtual void load(AsURLRequest request)
        {
            load(request, null);
        }
        public virtual void loadBytes(AsByteArray bytes, AsLoaderContext context)
        {
            throw new AsNotImplementedError();
        }
        public virtual void loadBytes(AsByteArray bytes)
        {
            loadBytes(bytes, null);
        }
        public virtual void unload()
        {
            throw new AsNotImplementedError();
        }
        public override void addChild(AsDisplayObject child)
        {
            throw new AsIllegalOperationError();
        }
        public override void addChildAt(AsDisplayObject child, int index)
        {
            throw new AsIllegalOperationError();
        }
        public override void removeChild(AsDisplayObject child, bool dispose)
        {
            throw new AsIllegalOperationError();
        }
        public override void removeChild(AsDisplayObject child)
        {
            removeChild(child, false);
        }
        public override void removeChildAt(int index, bool dispose)
        {
            throw new AsIllegalOperationError();
        }
        public override void removeChildAt(int index)
        {
            removeChildAt(index, false);
        }
        public override void setChildIndex(AsDisplayObject child, int index)
        {
            throw new AsIllegalOperationError();
        }
    }
}
