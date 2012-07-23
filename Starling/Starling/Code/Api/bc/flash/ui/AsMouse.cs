using System;
 
using bc.flash;
using bc.flash.errors;
 
namespace bc.flash.ui
{
	public sealed class AsMouse : AsObject
	{
		private static String mCursor;
		private static bool mHidden;
		public static String getCursor()
		{
			return mCursor;
		}
		public static void setCursor(String _value)
		{
			mCursor = _value;
		}
		public static void hide()
		{
			mHidden = true;
		}
		public static void show()
		{
			mHidden = false;
		}
		public static bool getSupportsCursor()
		{
			throw new AsNotImplementedError();
		}
	}
}
