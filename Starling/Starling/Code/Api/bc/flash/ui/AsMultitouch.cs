using System;
 
using bc.flash;
using bc.flash.ui;
 
namespace bc.flash.ui
{
	public sealed class AsMultitouch : AsObject
	{
		private static String mInputMode = AsMultitouchInputMode.NONE;
		public static String getInputMode()
		{
			return mInputMode;
		}
		public static void setInputMode(String _value)
		{
			mInputMode = _value;
		}
	}
}
