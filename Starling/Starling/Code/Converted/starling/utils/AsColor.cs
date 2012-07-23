using System;
 
using bc.flash;
using starling.errors;
 
namespace starling.utils
{
	public class AsColor : AsObject
	{
		public const uint WHITE = 0xffffff;
		public const uint SILVER = 0xc0c0c0;
		public const uint GRAY = 0x808080;
		public const uint BLACK = 0x000000;
		public const uint RED = 0xff0000;
		public const uint MAROON = 0x800000;
		public const uint YELLOW = 0xffff00;
		public const uint OLIVE = 0x808000;
		public const uint LIME = 0x00ff00;
		public const uint GREEN = 0x008000;
		public const uint AQUA = 0x00ffff;
		public const uint TEAL = 0x008080;
		public const uint BLUE = 0x0000ff;
		public const uint NAVY = 0x000080;
		public const uint FUCHSIA = 0xff00ff;
		public const uint PURPLE = 0x800080;
		public static int getAlpha(uint color)
		{
			return (int)(((color >> 24) & 0xff));
		}
		public static int getRed(uint color)
		{
			return (int)(((color >> 16) & 0xff));
		}
		public static int getGreen(uint color)
		{
			return (int)(((color >> 8) & 0xff));
		}
		public static int getBlue(uint color)
		{
			return (int)((color & 0xff));
		}
		public static uint rgb(int red, int green, int blue)
		{
			return (uint)((((red << 16) | (green << 8)) | blue));
		}
		public static uint argb(int alpha, int red, int green, int blue)
		{
			return (uint)(((((alpha << 24) | (red << 16)) | (green << 8)) | blue));
		}
		public AsColor()
		{
			throw new AsAbstractClassError();
		}
	}
}
