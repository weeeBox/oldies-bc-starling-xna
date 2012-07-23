using System;
 
using bc.flash;
using bc.flash.errors;
 
namespace bc.flash.text
{
	public class AsTextFormat : AsObject
	{
		public AsTextFormat(String font, AsObject size, AsObject color, AsObject bold, AsObject italic, AsObject underline, String url, String target, String align, AsObject leftMargin, AsObject rightMargin, AsObject indent, AsObject leading)
		{
			throw new AsNotImplementedError();
		}
		public AsTextFormat(String font, AsObject size, AsObject color, AsObject bold, AsObject italic, AsObject underline, String url, String target, String align, AsObject leftMargin, AsObject rightMargin, AsObject indent)
		 : this(font, size, color, bold, italic, underline, url, target, align, leftMargin, rightMargin, indent, null)
		{
		}
		public AsTextFormat(String font, AsObject size, AsObject color, AsObject bold, AsObject italic, AsObject underline, String url, String target, String align, AsObject leftMargin, AsObject rightMargin)
		 : this(font, size, color, bold, italic, underline, url, target, align, leftMargin, rightMargin, null, null)
		{
		}
		public AsTextFormat(String font, AsObject size, AsObject color, AsObject bold, AsObject italic, AsObject underline, String url, String target, String align, AsObject leftMargin)
		 : this(font, size, color, bold, italic, underline, url, target, align, leftMargin, null, null, null)
		{
		}
		public AsTextFormat(String font, AsObject size, AsObject color, AsObject bold, AsObject italic, AsObject underline, String url, String target, String align)
		 : this(font, size, color, bold, italic, underline, url, target, align, null, null, null, null)
		{
		}
		public AsTextFormat(String font, AsObject size, AsObject color, AsObject bold, AsObject italic, AsObject underline, String url, String target)
		 : this(font, size, color, bold, italic, underline, url, target, null, null, null, null, null)
		{
		}
		public AsTextFormat(String font, AsObject size, AsObject color, AsObject bold, AsObject italic, AsObject underline, String url)
		 : this(font, size, color, bold, italic, underline, url, null, null, null, null, null, null)
		{
		}
		public AsTextFormat(String font, AsObject size, AsObject color, AsObject bold, AsObject italic, AsObject underline)
		 : this(font, size, color, bold, italic, underline, null, null, null, null, null, null, null)
		{
		}
		public AsTextFormat(String font, AsObject size, AsObject color, AsObject bold, AsObject italic)
		 : this(font, size, color, bold, italic, null, null, null, null, null, null, null, null)
		{
		}
		public AsTextFormat(String font, AsObject size, AsObject color, AsObject bold)
		 : this(font, size, color, bold, null, null, null, null, null, null, null, null, null)
		{
		}
		public AsTextFormat(String font, AsObject size, AsObject color)
		 : this(font, size, color, null, null, null, null, null, null, null, null, null, null)
		{
		}
		public AsTextFormat(String font, AsObject size)
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
		public virtual AsObject getBlockIndent()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setBlockIndent(AsObject _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsObject getBold()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setBold(AsObject _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsObject getBullet()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setBullet(AsObject _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsObject getColor()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setColor(AsObject _value)
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
		public virtual AsObject getIndent()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setIndent(AsObject _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsObject getItalic()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setItalic(AsObject _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsObject getKerning()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setKerning(AsObject _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsObject getLeading()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setLeading(AsObject _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsObject getLeftMargin()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setLeftMargin(AsObject _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsObject getLetterSpacing()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setLetterSpacing(AsObject _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsObject getRightMargin()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setRightMargin(AsObject _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsObject getSize()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setSize(AsObject _value)
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
		public virtual AsObject getUnderline()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setUnderline(AsObject _value)
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
