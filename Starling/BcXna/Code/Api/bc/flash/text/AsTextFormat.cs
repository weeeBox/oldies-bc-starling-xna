using System;
 
using bc.flash;
using bc.flash.errors;
 
namespace bc.flash.text
{
	public class AsTextFormat : AsObject
	{
		public AsTextFormat(String font, Object size, Object color, Object bold, Object italic, Object underline, String url, String target, String align, Object leftMargin, Object rightMargin, Object indent, Object leading)
		{
			throw new AsNotImplementedError();
		}
		public AsTextFormat(String font, Object size, Object color, Object bold, Object italic, Object underline, String url, String target, String align, Object leftMargin, Object rightMargin, Object indent)
		 : this(font, size, color, bold, italic, underline, url, target, align, leftMargin, rightMargin, indent, null)
		{
		}
		public AsTextFormat(String font, Object size, Object color, Object bold, Object italic, Object underline, String url, String target, String align, Object leftMargin, Object rightMargin)
		 : this(font, size, color, bold, italic, underline, url, target, align, leftMargin, rightMargin, null, null)
		{
		}
		public AsTextFormat(String font, Object size, Object color, Object bold, Object italic, Object underline, String url, String target, String align, Object leftMargin)
		 : this(font, size, color, bold, italic, underline, url, target, align, leftMargin, null, null, null)
		{
		}
		public AsTextFormat(String font, Object size, Object color, Object bold, Object italic, Object underline, String url, String target, String align)
		 : this(font, size, color, bold, italic, underline, url, target, align, null, null, null, null)
		{
		}
		public AsTextFormat(String font, Object size, Object color, Object bold, Object italic, Object underline, String url, String target)
		 : this(font, size, color, bold, italic, underline, url, target, null, null, null, null, null)
		{
		}
		public AsTextFormat(String font, Object size, Object color, Object bold, Object italic, Object underline, String url)
		 : this(font, size, color, bold, italic, underline, url, null, null, null, null, null, null)
		{
		}
		public AsTextFormat(String font, Object size, Object color, Object bold, Object italic, Object underline)
		 : this(font, size, color, bold, italic, underline, null, null, null, null, null, null, null)
		{
		}
		public AsTextFormat(String font, Object size, Object color, Object bold, Object italic)
		 : this(font, size, color, bold, italic, null, null, null, null, null, null, null, null)
		{
		}
		public AsTextFormat(String font, Object size, Object color, Object bold)
		 : this(font, size, color, bold, null, null, null, null, null, null, null, null, null)
		{
		}
		public AsTextFormat(String font, Object size, Object color)
		 : this(font, size, color, null, null, null, null, null, null, null, null, null, null)
		{
		}
		public AsTextFormat(String font, Object size)
		 : this(font, size, null, null, null, null, null, null, null, null, null, null, null)
		{
		}
		public AsTextFormat(String font)
		 : this(font, null, null, null, null, null, null, null, null, null, null, null, null)
		{
		}
		public AsTextFormat()
		 : this(null, null, null, null, null, null, null, null, null, null, null, null, null)
		{
		}
		public virtual String getAlign()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setAlign(String _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual Object getBlockIndent()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setBlockIndent(Object _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual Object getBold()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setBold(Object _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual Object getBullet()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setBullet(Object _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual Object getColor()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setColor(Object _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual String getDisplay()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setDisplay(String _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual String getFont()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setFont(String _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual Object getIndent()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setIndent(Object _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual Object getItalic()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setItalic(Object _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual Object getKerning()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setKerning(Object _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual Object getLeading()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setLeading(Object _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual Object getLeftMargin()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setLeftMargin(Object _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual Object getLetterSpacing()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setLetterSpacing(Object _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual Object getRightMargin()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setRightMargin(Object _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual Object getSize()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setSize(Object _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsArray getTabStops()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setTabStops(AsArray _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual String getTarget()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setTarget(String _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual Object getUnderline()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setUnderline(Object _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual String getUrl()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setUrl(String _value)
		{
			throw new AsNotImplementedError();
		}
	}
}
