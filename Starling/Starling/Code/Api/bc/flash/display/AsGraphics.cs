using System;
 
using bc.flash;
using bc.flash.display;
 
namespace bc.flash.display
{
	public sealed class AsGraphics : AsObject
	{
		public void beginFill(uint color, float alpha)
		{
		}
		public void beginFill(uint color)
		{
			beginFill(color, 1.0f);
		}
		public void clear()
		{
		}
		public void curveTo(float controlX, float controlY, float anchorX, float anchorY)
		{
		}
		public void drawCircle(float x, float y, float radius)
		{
		}
		public void drawEllipse(float x, float y, float width, float height)
		{
		}
		public void drawRect(float x, float y, float width, float height)
		{
		}
		public void drawRoundRect(float x, float y, float width, float height, float ellipseWidth, float ellipseHeight)
		{
		}
		public void drawBitmap(AsBitmapData bitmap, float x, float y)
		{
		}
		public void endFill()
		{
		}
		public void lineStyle(float thickness, uint color, float alpha, bool pixelHinting, String scaleMode, String caps, String joints, float miterLimit)
		{
		}
		public void lineStyle(float thickness, uint color, float alpha, bool pixelHinting, String scaleMode, String caps, String joints)
		{
			lineStyle(thickness, color, alpha, pixelHinting, scaleMode, caps, joints, 3);
		}
		public void lineStyle(float thickness, uint color, float alpha, bool pixelHinting, String scaleMode, String caps)
		{
			lineStyle(thickness, color, alpha, pixelHinting, scaleMode, caps, null, 3);
		}
		public void lineStyle(float thickness, uint color, float alpha, bool pixelHinting, String scaleMode)
		{
			lineStyle(thickness, color, alpha, pixelHinting, scaleMode, null, null, 3);
		}
		public void lineStyle(float thickness, uint color, float alpha, bool pixelHinting)
		{
			lineStyle(thickness, color, alpha, pixelHinting, "normal", null, null, 3);
		}
		public void lineStyle(float thickness, uint color, float alpha)
		{
			lineStyle(thickness, color, alpha, false, "normal", null, null, 3);
		}
		public void lineStyle(float thickness, uint color)
		{
			lineStyle(thickness, color, 1.0f, false, "normal", null, null, 3);
		}
		public void lineStyle(float thickness)
		{
			lineStyle(thickness, (uint)(0), 1.0f, false, "normal", null, null, 3);
		}
		public void lineStyle()
		{
			lineStyle(0, (uint)(0), 1.0f, false, "normal", null, null, 3);
		}
		public void lineTo(float x, float y)
		{
		}
		public void moveTo(float x, float y)
		{
		}
	}
}
