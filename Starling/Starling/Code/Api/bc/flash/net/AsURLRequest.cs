using System;
 
using bc.flash;
 
namespace bc.flash.net
{
	public sealed class AsURLRequest : AsObject
	{
		private String mUrl;
		private String mContentType;
		private AsObject mData;
		private String mDigest;
		private String mMethod;
		public AsURLRequest(String url)
		{
			mUrl = url;
		}
		public AsURLRequest()
		 : this(null)
		{
		}
		public String getContentType()
		{
			return mContentType;
		}
		public void setContentType(String _value)
		{
			mContentType = _value;
		}
		public AsObject getData()
		{
			return (AsObject)(mData);
		}
		public void setData(AsObject _value)
		{
			mData = (AsObject)(_value);
		}
		public String getDigest()
		{
			return mDigest;
		}
		public void setDigest(String _value)
		{
			mDigest = _value;
		}
		public String getMethod()
		{
			return mMethod;
		}
		public void setMethod(String _value)
		{
			mMethod = _value;
		}
		public String getUrl()
		{
			return mUrl;
		}
		public void setUrl(String _value)
		{
			mUrl = _value;
		}
	}
}
