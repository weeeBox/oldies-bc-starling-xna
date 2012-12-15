using System;
 
using bc.flash;
using bc.flash.display3D;
using starling.errors;
 
namespace starling.display
{
	public class AsBlendMode : AsObject
	{
		private static AsArray sBlendFactors = new AsArray(AsObject.createLiteralObject("\"none\"", new AsArray(AsContext3DBlendFactor.ONE, AsContext3DBlendFactor.ZERO), "\"normal\"", new AsArray(AsContext3DBlendFactor.SOURCE_ALPHA, AsContext3DBlendFactor.ONE_MINUS_SOURCE_ALPHA), "\"add\"", new AsArray(AsContext3DBlendFactor.SOURCE_ALPHA, AsContext3DBlendFactor.DESTINATION_ALPHA), "\"multiply\"", new AsArray(AsContext3DBlendFactor.DESTINATION_COLOR, AsContext3DBlendFactor.ONE_MINUS_SOURCE_ALPHA), "\"screen\"", new AsArray(AsContext3DBlendFactor.SOURCE_ALPHA, AsContext3DBlendFactor.ONE), "\"erase\"", new AsArray(AsContext3DBlendFactor.ZERO, AsContext3DBlendFactor.ONE_MINUS_SOURCE_ALPHA)), AsObject.createLiteralObject("\"none\"", new AsArray(AsContext3DBlendFactor.ONE, AsContext3DBlendFactor.ZERO), "\"normal\"", new AsArray(AsContext3DBlendFactor.ONE, AsContext3DBlendFactor.ONE_MINUS_SOURCE_ALPHA), "\"add\"", new AsArray(AsContext3DBlendFactor.ONE, AsContext3DBlendFactor.ONE), "\"multiply\"", new AsArray(AsContext3DBlendFactor.DESTINATION_COLOR, AsContext3DBlendFactor.ONE_MINUS_SOURCE_ALPHA), "\"screen\"", new AsArray(AsContext3DBlendFactor.ONE, AsContext3DBlendFactor.ONE_MINUS_SOURCE_COLOR), "\"erase\"", new AsArray(AsContext3DBlendFactor.ZERO, AsContext3DBlendFactor.ONE_MINUS_SOURCE_ALPHA)));
		public static String AUTO = "auto";
		public static String NONE = "none";
		public static String NORMAL = "normal";
		public static String ADD = "add";
		public static String MULTIPLY = "multiply";
		public static String SCREEN = "screen";
		public static String ERASE = "erase";
		public AsBlendMode()
		{
			throw new AsAbstractClassError();
		}
		public static AsArray getBlendFactors(String mode, bool premultipliedAlpha)
		{
			Object modes = sBlendFactors[((int)(premultipliedAlpha))];
			if(modes.hasOwnProperty(mode))
			{
				return (AsArray)(((AsObject)(modes)).getOwnProperty(mode));
			}
			else
			{
				throw new AsArgumentError("Invalid blend mode");
			}
		}
		public static AsArray getBlendFactors(String mode)
		{
			return getBlendFactors(mode, true);
		}
		public static void register(String name, String sourceFactor, String destFactor, bool premultipliedAlpha)
		{
			Object modes = sBlendFactors[((int)(premultipliedAlpha))];
			((AsObject)(modes)).setOwnProperty(name, new AsArray(sourceFactor, destFactor));
			Object otherModes = sBlendFactors[((int)(!premultipliedAlpha))];
			if(!(otherModes.hasOwnProperty(name)))
			{
				((AsObject)(otherModes)).setOwnProperty(name, new AsArray(sourceFactor, destFactor));
			}
		}
		public static void register(String name, String sourceFactor, String destFactor)
		{
			register(name, sourceFactor, destFactor, true);
		}
	}
}
