using System;

using bc.flash;
using System.Collections.Generic;

namespace bc.flash
{
    public class AsObject
    {
        private Dictionary<String, Object> mProperties;

        public virtual bool hasOwnProperty(String name)
        {
            return mProperties != null && mProperties.ContainsKey(name);
        }

        public virtual Object getOwnProperty(String name)
        {
            return hasOwnProperty(name) ? mProperties[name] : null;
        }

        public virtual void setOwnProperty(String name, Object _value)
        {
            if (mProperties == null)
            {
                mProperties = new Dictionary<String, Object>();
            }
            if (mProperties.ContainsKey(name))
            {
                mProperties.Remove(name);
            }
            mProperties[name] = _value;
        }

        public virtual void deleteOwnProperty(String name)
        {
            if (hasOwnProperty(name))
            {
                mProperties.Remove(name);
            }
        }

        public virtual String toString()
        {
            return "Object";
        }
    }
}
