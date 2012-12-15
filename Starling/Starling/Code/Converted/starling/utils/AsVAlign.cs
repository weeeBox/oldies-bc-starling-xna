using System;
 
using bc.flash;
using starling.errors;
 
namespace starling.utils
{
	public sealed class AsVAlign : AsObject
	{
		public static String TOP = "top";
		public static String CENTER = "center";
		public static String BOTTOM = "bottom";
		public AsVAlign()
		{
			throw new AsAbstractClassError();
		}
		public static bool isValid(String vAlign)
		{
			return vAlign == TOP || vAlign == CENTER || vAlign == BOTTOM;
		}
	}
}
