using System;
 
using bc.flash;
using bc.flash.geom;
using bc.flash.xml;
using starling.display;
using starling.text;
using starling.textures;
using starling.utils;
 
namespace starling.text
{
	public class AsBitmapFont : AsObject
	{
		private static AsClass MiniXml;
		private static AsClass MiniTexture;
		public const int NATIVE_SIZE = -1;
		public static String MINI = "mini";
		private const int CHAR_SPACE = 32;
		private const int CHAR_TAB = 9;
		private const int CHAR_NEWLINE = 10;
		private AsTexture mTexture;
		private AsDictionary mChars;
		private String mName;
		private float mSize;
		private float mLineHeight;
		private float mBaseline;
		private AsImage mHelperImage;
		private AsVector<AsCharLocation> mCharLocationPool;
		public AsBitmapFont(AsTexture texture, AsXML fontXml)
		{
			if(((texture == null) && (fontXml == null)))
			{
				// FIXME: Block of code is cut here
			}
			mName = "unknown";
			mLineHeight = mSize = mBaseline = 14;
			mTexture = texture;
			mChars = new AsDictionary();
			mHelperImage = new AsImage(texture);
			mCharLocationPool = new AsVector<AsCharLocation>();
			if(fontXml != null)
			{
				parseFontXml(fontXml);
			}
		}
		public AsBitmapFont(AsTexture texture)
		 : this(texture, null)
		{
		}
		public AsBitmapFont()
		 : this(null, null)
		{
		}
		public virtual void dispose()
		{
			if(mTexture != null)
			{
				mTexture.dispose();
			}
		}
		private void parseFontXml(AsXML fontXml)
		{
			float scale = mTexture.getScale();
			AsRectangle frame = mTexture.getFrame();
			mName = fontXml.elements("info").attribute("face");
			mSize = (AsGlobal.parseFloat(fontXml.elements("info").attribute("size")) / scale);
			mLineHeight = (AsGlobal.parseFloat(fontXml.elements("common").attribute("lineHeight")) / scale);
			mBaseline = (AsGlobal.parseFloat(fontXml.elements("common").attribute("base")) / scale);
			if((fontXml.elements("info").attribute("smooth").ToString() == "0"))
			{
				setSmoothing(AsTextureSmoothing.NONE);
			}
			if((mSize <= 0))
			{
				AsGlobal.trace((("[Starling] Warning: invalid font size in '" + mName) + "' font."));
				mSize = (((mSize == 0.0f)) ? (16.0f) : ((mSize * -1.0f)));
			}
			AsXMLList __charElements_ = fontXml.elements("chars").elements("_char");
			if (__charElements_ != null)
			{
				foreach (AsXML charElement in __charElements_)
				{
					int id = (int)(AsGlobal.parseInt(charElement.attribute("id")));
					float xOffset = (AsGlobal.parseFloat(charElement.attribute("xoffset")) / scale);
					float yOffset = (AsGlobal.parseFloat(charElement.attribute("yoffset")) / scale);
					float xAdvance = (AsGlobal.parseFloat(charElement.attribute("xadvance")) / scale);
					AsRectangle region = new AsRectangle();
					region.x = ((AsGlobal.parseFloat(charElement.attribute("x")) / scale) + frame.x);
					region.y = ((AsGlobal.parseFloat(charElement.attribute("y")) / scale) + frame.y);
					region.width = (AsGlobal.parseFloat(charElement.attribute("width")) / scale);
					region.height = (AsGlobal.parseFloat(charElement.attribute("height")) / scale);
					AsTexture texture = AsTexture.fromTexture(mTexture, region);
					AsBitmapChar bitmapChar = new AsBitmapChar(id, texture, xOffset, yOffset, xAdvance);
					addChar(id, bitmapChar);
				}
			}
			AsXMLList __kerningElements_ = fontXml.elements("kernings").elements("kerning");
			if (__kerningElements_ != null)
			{
				foreach (AsXML kerningElement in __kerningElements_)
				{
					int first = (int)(AsGlobal.parseInt(kerningElement.attribute("first")));
					int second = (int)(AsGlobal.parseInt(kerningElement.attribute("second")));
					float amount = (AsGlobal.parseFloat(kerningElement.attribute("amount")) / scale);
					if(mChars.containsKey(second))
					{
						getChar(second).addKerning(first, amount);
					}
				}
			}
		}
		public virtual AsBitmapChar getChar(int charID)
		{
			return (AsBitmapChar)(mChars[charID]);
		}
		public virtual void addChar(int charID, AsBitmapChar bitmapChar)
		{
			mChars[charID] = bitmapChar;
		}
		public virtual AsSprite createSprite(float width, float height, String text, float fontSize, uint color, String hAlign, String vAlign, bool autoScale, bool kerning)
		{
			AsVector<AsCharLocation> charLocations = arrangeChars(width, height, text, fontSize, hAlign, vAlign, autoScale, kerning);
			int numChars = (int)(charLocations.getLength());
			AsSprite sprite = new AsSprite();
			int i = 0;
			for (; (i < numChars); ++i)
			{
				AsCharLocation charLocation = charLocations[i];
				AsImage _char = charLocation._char.createImage();
				_char.setX(charLocation.x);
				_char.setY(charLocation.y);
				_char.setScaleX(_char.setScaleY(charLocation.scale));
				_char.setColor(color);
				sprite.addChild(_char);
			}
			return sprite;
		}
		public virtual AsSprite createSprite(float width, float height, String text, float fontSize, uint color, String hAlign, String vAlign, bool autoScale)
		{
			return createSprite(width, height, text, fontSize, color, hAlign, vAlign, autoScale, true);
		}
		public virtual AsSprite createSprite(float width, float height, String text, float fontSize, uint color, String hAlign, String vAlign)
		{
			return createSprite(width, height, text, fontSize, color, hAlign, vAlign, true, true);
		}
		public virtual AsSprite createSprite(float width, float height, String text, float fontSize, uint color, String hAlign)
		{
			return createSprite(width, height, text, fontSize, color, hAlign, "center", true, true);
		}
		public virtual AsSprite createSprite(float width, float height, String text, float fontSize, uint color)
		{
			return createSprite(width, height, text, fontSize, color, "center", "center", true, true);
		}
		public virtual AsSprite createSprite(float width, float height, String text, float fontSize)
		{
			return createSprite(width, height, text, fontSize, (uint)(0xffffff), "center", "center", true, true);
		}
		public virtual AsSprite createSprite(float width, float height, String text)
		{
			return createSprite(width, height, text, -1, (uint)(0xffffff), "center", "center", true, true);
		}
		public virtual void fillQuadBatch(AsQuadBatch quadBatch, float width, float height, String text, float fontSize, uint color, String hAlign, String vAlign, bool autoScale, bool kerning)
		{
			AsVector<AsCharLocation> charLocations = arrangeChars(width, height, text, fontSize, hAlign, vAlign, autoScale, kerning);
			int numChars = (int)(charLocations.getLength());
			mHelperImage.setColor(color);
			if((numChars > 8192))
			{
				throw new AsArgumentError("Bitmap Font text is limited to 8192 characters.");
			}
			int i = 0;
			for (; (i < numChars); ++i)
			{
				AsCharLocation charLocation = charLocations[i];
				mHelperImage.setTexture(charLocation._char.getTexture());
				mHelperImage.readjustSize();
				mHelperImage.setX(charLocation.x);
				mHelperImage.setY(charLocation.y);
				mHelperImage.setScaleX(mHelperImage.setScaleY(charLocation.scale));
				quadBatch.addImage(mHelperImage);
			}
		}
		public virtual void fillQuadBatch(AsQuadBatch quadBatch, float width, float height, String text, float fontSize, uint color, String hAlign, String vAlign, bool autoScale)
		{
			fillQuadBatch(quadBatch, width, height, text, fontSize, color, hAlign, vAlign, autoScale, true);
		}
		public virtual void fillQuadBatch(AsQuadBatch quadBatch, float width, float height, String text, float fontSize, uint color, String hAlign, String vAlign)
		{
			fillQuadBatch(quadBatch, width, height, text, fontSize, color, hAlign, vAlign, true, true);
		}
		public virtual void fillQuadBatch(AsQuadBatch quadBatch, float width, float height, String text, float fontSize, uint color, String hAlign)
		{
			fillQuadBatch(quadBatch, width, height, text, fontSize, color, hAlign, "center", true, true);
		}
		public virtual void fillQuadBatch(AsQuadBatch quadBatch, float width, float height, String text, float fontSize, uint color)
		{
			fillQuadBatch(quadBatch, width, height, text, fontSize, color, "center", "center", true, true);
		}
		public virtual void fillQuadBatch(AsQuadBatch quadBatch, float width, float height, String text, float fontSize)
		{
			fillQuadBatch(quadBatch, width, height, text, fontSize, (uint)(0xffffff), "center", "center", true, true);
		}
		public virtual void fillQuadBatch(AsQuadBatch quadBatch, float width, float height, String text)
		{
			fillQuadBatch(quadBatch, width, height, text, -1, (uint)(0xffffff), "center", "center", true, true);
		}
		private AsVector<AsCharLocation> arrangeChars(float width, float height, String text, float fontSize, String hAlign, String vAlign, bool autoScale, bool kerning)
		{
			if(((text == null) || (text.Length == 0)))
			{
				return new AsVector<AsCharLocation>();
			}
			if((fontSize < 0))
			{
				fontSize = (fontSize * -mSize);
			}
			AsVector<AsVector<AsCharLocation>> lines = null;
			bool finished = false;
			AsCharLocation charLocation = null;
			int numChars = 0;
			float containerWidth = 0;
			float containerHeight = 0;
			float scale = 0;
			while(!(finished))
			{
				scale = (fontSize / mSize);
				containerWidth = (width / scale);
				containerHeight = (height / scale);
				lines = new AsVector<AsVector<AsCharLocation>>();
				if((mLineHeight <= containerHeight))
				{
					int lastWhiteSpace = -1;
					int lastCharID = -1;
					float currentX = 0;
					float currentY = 0;
					AsVector<AsCharLocation> currentLine = new AsVector<AsCharLocation>();
					numChars = text.Length;
					int i = 0;
					for (; (i < numChars); ++i)
					{
						bool lineFull = false;
						int charID = (int)(String.charCodeAt(text, i));
						AsBitmapChar _char = getChar(charID);
						if((charID == CHAR_NEWLINE))
						{
							lineFull = true;
						}
						else
						{
							if((_char == null))
							{
								AsGlobal.trace(("[Starling] Missing character: " + charID));
							}
							else
							{
								if(((charID == CHAR_SPACE) || (charID == CHAR_TAB)))
								{
									lastWhiteSpace = i;
								}
								if(kerning)
								{
									currentX = (currentX + _char.getKerning(lastCharID));
								}
								charLocation = (((mCharLocationPool.getLength()) != 0) ? (mCharLocationPool.pop()) : (new AsCharLocation(_char)));
								charLocation._char = _char;
								charLocation.x = (currentX + _char.getXOffset());
								charLocation.y = (currentY + _char.getYOffset());
								currentLine.push(charLocation);
								currentX = (currentX + _char.getXAdvance());
								lastCharID = charID;
								if(((charLocation.x + _char.getWidth()) > containerWidth))
								{
									int numCharsToRemove = (((lastWhiteSpace == -1)) ? (1) : ((i - lastWhiteSpace)));
									int removeIndex = (int)((currentLine.getLength() - numCharsToRemove));
									currentLine.splice(removeIndex, (uint)(numCharsToRemove));
									if((currentLine.getLength() == 0))
									{
										break;
									}
									i = (i - numCharsToRemove);
									lineFull = true;
								}
							}
						}
						if((i == (numChars - 1)))
						{
							lines.push(currentLine);
							finished = true;
						}
						else
						{
							if(lineFull)
							{
								lines.push(currentLine);
								if((lastWhiteSpace == i))
								{
									currentLine.pop();
								}
								if(((currentY + (2 * mLineHeight)) <= containerHeight))
								{
									currentLine = new AsVector<AsCharLocation>();
									currentX = 0;
									currentY = (currentY + mLineHeight);
									lastWhiteSpace = -1;
									lastCharID = -1;
								}
								else
								{
									break;
								}
							}
						}
					}
				}
				if((autoScale && !(finished)))
				{
					fontSize = (fontSize - 1);
					lines.setLength(0);
				}
				else
				{
					finished = true;
				}
			}
			AsVector<AsCharLocation> finalLocations = new AsVector<AsCharLocation>();
			int numLines = (int)(lines.getLength());
			float bottom = (currentY + mLineHeight);
			int yOffset = 0;
			if((vAlign == AsVAlign.BOTTOM))
			{
				yOffset = (int)((containerHeight - bottom));
			}
			else
			{
				if((vAlign == AsVAlign.CENTER))
				{
					yOffset = (int)(((containerHeight - bottom) / 2));
				}
			}
			int lineID = 0;
			for (; (lineID < numLines); ++lineID)
			{
				AsVector<AsCharLocation> line = lines[lineID];
				numChars = (int)(line.getLength());
				if((numChars == 0))
				{
					continue;
				}
				AsCharLocation lastLocation = line[(line.getLength() - 1)];
				float right = (lastLocation.x + lastLocation._char.getWidth());
				int xOffset = 0;
				if((hAlign == AsHAlign.RIGHT))
				{
					xOffset = (int)((containerWidth - right));
				}
				else
				{
					if((hAlign == AsHAlign.CENTER))
					{
						xOffset = (int)(((containerWidth - right) / 2));
					}
				}
				int c = 0;
				for (; (c < numChars); ++c)
				{
					charLocation = line[c];
					charLocation.x = (scale * (charLocation.x + xOffset));
					charLocation.y = (scale * (charLocation.y + yOffset));
					charLocation.scale = scale;
					if(((charLocation._char.getWidth() > 0) && (charLocation._char.getHeight() > 0)))
					{
						finalLocations.push(charLocation);
					}
					mCharLocationPool.push(charLocation);
				}
			}
			return finalLocations;
		}
		private AsVector<AsCharLocation> arrangeChars(float width, float height, String text, float fontSize, String hAlign, String vAlign, bool autoScale)
		{
			return arrangeChars(width, height, text, fontSize, hAlign, vAlign, autoScale, true);
		}
		private AsVector<AsCharLocation> arrangeChars(float width, float height, String text, float fontSize, String hAlign, String vAlign)
		{
			return arrangeChars(width, height, text, fontSize, hAlign, vAlign, true, true);
		}
		private AsVector<AsCharLocation> arrangeChars(float width, float height, String text, float fontSize, String hAlign)
		{
			return arrangeChars(width, height, text, fontSize, hAlign, "center", true, true);
		}
		private AsVector<AsCharLocation> arrangeChars(float width, float height, String text, float fontSize)
		{
			return arrangeChars(width, height, text, fontSize, "center", "center", true, true);
		}
		private AsVector<AsCharLocation> arrangeChars(float width, float height, String text)
		{
			return arrangeChars(width, height, text, -1, "center", "center", true, true);
		}
		public virtual String getName()
		{
			return mName;
		}
		public virtual float getSize()
		{
			return mSize;
		}
		public virtual float getLineHeight()
		{
			return mLineHeight;
		}
		public virtual void setLineHeight(float _value)
		{
			mLineHeight = _value;
		}
		public virtual String getSmoothing()
		{
			return mHelperImage.getSmoothing();
		}
		public virtual void setSmoothing(String _value)
		{
			mHelperImage.setSmoothing(_value);
		}
		public virtual float getBaseline()
		{
			return mBaseline;
		}
	}
}
