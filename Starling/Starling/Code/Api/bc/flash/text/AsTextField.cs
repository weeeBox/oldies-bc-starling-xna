using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.errors;
using bc.flash.geom;
using bc.flash.text;
 
namespace bc.flash.text
{
	public class AsTextField : AsInteractiveObject
	{
		public virtual bool getAlwaysShowSelection()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setAlwaysShowSelection(bool _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual String getAntiAliasType()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setAntiAliasType(String antiAliasType)
		{
			throw new AsNotImplementedError();
		}
		public virtual void appendText(String newText)
		{
			throw new AsNotImplementedError();
		}
		public virtual String getAutoSize()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setAutoSize(String _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getBackground()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setBackground(bool _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual uint getBackgroundColor()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setBackgroundColor(uint _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getBorder()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setBorder(bool _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual uint getBorderColor()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setBorderColor(uint _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual int getBottomScrollV()
		{
			throw new AsNotImplementedError();
		}
		public virtual int getCaretIndex()
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getCondenseWhite()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setCondenseWhite(bool _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual String copyRichText()
		{
			throw new AsNotImplementedError();
		}
		public virtual AsTextFormat getDefaultTextFormat()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setDefaultTextFormat(AsTextFormat format)
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getDisplayAsPassword()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setDisplayAsPassword(bool _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getEmbedFonts()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setEmbedFonts(bool _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsRectangle getCharBoundaries(int charIndex)
		{
			throw new AsNotImplementedError();
		}
		public virtual int getCharIndexAtPoint(float x, float y)
		{
			throw new AsNotImplementedError();
		}
		public virtual int getFirstCharInParagraph(int charIndex)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsDisplayObject getImageReference(String id)
		{
			throw new AsNotImplementedError();
		}
		public virtual int getLineIndexAtPoint(float x, float y)
		{
			throw new AsNotImplementedError();
		}
		public virtual int getLineIndexOfChar(int charIndex)
		{
			throw new AsNotImplementedError();
		}
		public virtual int getLineLength(int lineIndex)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsTextLineMetrics getLineMetrics(int lineIndex)
		{
			throw new AsNotImplementedError();
		}
		public virtual int getLineOffset(int lineIndex)
		{
			throw new AsNotImplementedError();
		}
		public virtual String getLineText(int lineIndex)
		{
			throw new AsNotImplementedError();
		}
		public virtual int getParagraphLength(int charIndex)
		{
			throw new AsNotImplementedError();
		}
		public virtual String getRawText()
		{
			throw new AsNotImplementedError();
		}
		public virtual AsTextFormat getTextFormat(int beginIndex, int endIndex)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsTextFormat getTextFormat(int beginIndex)
		{
			return getTextFormat(beginIndex, -1);
		}
		public virtual AsTextFormat getTextFormat()
		{
			return getTextFormat(-1, -1);
		}
		public virtual AsArray getTextRuns(int beginIndex, int endIndex)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsArray getTextRuns(int beginIndex)
		{
			return getTextRuns(beginIndex, 2147483647);
		}
		public virtual AsArray getTextRuns()
		{
			return getTextRuns(0, 2147483647);
		}
		public virtual String getXMLText(int beginIndex, int endIndex)
		{
			throw new AsNotImplementedError();
		}
		public virtual String getXMLText(int beginIndex)
		{
			return getXMLText(beginIndex, 2147483647);
		}
		public virtual String getXMLText()
		{
			return getXMLText(0, 2147483647);
		}
		public virtual String getGridFitType()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setGridFitType(String gridFitType)
		{
			throw new AsNotImplementedError();
		}
		public virtual String getHtmlText()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setHtmlText(String _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual void insertXMLText(int beginIndex, int endIndex, String richText, bool pasting)
		{
			throw new AsNotImplementedError();
		}
		public virtual void insertXMLText(int beginIndex, int endIndex, String richText)
		{
			insertXMLText(beginIndex, endIndex, richText, false);
		}
		public static bool isFontCompatible(String fontName, String fontStyle)
		{
			throw new AsNotImplementedError();
		}
		public virtual int getLength()
		{
			throw new AsNotImplementedError();
		}
		public virtual int getMaxChars()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setMaxChars(int _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual int getMaxScrollH()
		{
			throw new AsNotImplementedError();
		}
		public virtual int getMaxScrollV()
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getMouseWheelEnabled()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setMouseWheelEnabled(bool _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getMultiline()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setMultiline(bool _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual int getNumLines()
		{
			throw new AsNotImplementedError();
		}
		public virtual bool pasteRichText(String richText)
		{
			throw new AsNotImplementedError();
		}
		public virtual void replaceSelectedText(String _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual void replaceText(int beginIndex, int endIndex, String newText)
		{
			throw new AsNotImplementedError();
		}
		public virtual String getRestrict()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setRestrict(String _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual int getScrollH()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setScrollH(int _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual int getScrollV()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setScrollV(int _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getSelectable()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setSelectable(bool _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual String getSelectedText()
		{
			throw new AsNotImplementedError();
		}
		public virtual int getSelectionBeginIndex()
		{
			throw new AsNotImplementedError();
		}
		public virtual int getSelectionEndIndex()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setSelection(int beginIndex, int endIndex)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setTextFormat(AsTextFormat format, int beginIndex, int endIndex)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setTextFormat(AsTextFormat format, int beginIndex)
		{
			setTextFormat(format, beginIndex, -1);
		}
		public virtual void setTextFormat(AsTextFormat format)
		{
			setTextFormat(format, -1, -1);
		}
		public virtual float getSharpness()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setSharpness(float _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual String getText()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setText(String _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual uint getTextColor()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setTextColor(uint _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual float getTextHeight()
		{
			throw new AsNotImplementedError();
		}
		public virtual String getTextInteractionMode()
		{
			throw new AsNotImplementedError();
		}
		public virtual float getTextWidth()
		{
			throw new AsNotImplementedError();
		}
		public virtual float getThickness()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setThickness(float _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual String getType()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setType(String _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getUseRichTextClipboard()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setUseRichTextClipboard(bool _value)
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getWordWrap()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setWordWrap(bool _value)
		{
			throw new AsNotImplementedError();
		}
	}
}
