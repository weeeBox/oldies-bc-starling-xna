using System;
 
using bc.flash;
using starling.errors;
 
namespace starling.textures
{
	public class AsTextureSmoothing : AsObject
	{
		public static String NONE = "none";
		public static String BILINEAR = "bilinear";
		public static String TRILINEAR = "trilinear";
		public AsTextureSmoothing()
		{
			throw new AsAbstractClassError();
		}
		public static bool isValid(String smoothing)
		{
			return smoothing == NONE || smoothing == BILINEAR || smoothing == TRILINEAR;
		}
	}
}
