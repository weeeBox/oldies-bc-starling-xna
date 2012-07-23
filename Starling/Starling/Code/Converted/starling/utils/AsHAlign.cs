using System;
 
using bc.flash;
using starling.errors;
 
namespace starling.utils
{
	public sealed class AsHAlign : AsObject
	{
		public static String LEFT = "left";
		public static String CENTER = "center";
		public static String RIGHT = "right";
		public AsHAlign()
		{
			throw new AsAbstractClassError();
		}
		public static bool isValid(String hAlign)
		{
			return (((hAlign == LEFT) || (hAlign == CENTER)) || (hAlign == RIGHT));
		}
	}
}
