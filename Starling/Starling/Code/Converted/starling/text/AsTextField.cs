using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.geom;
using bc.flash.text;
using starling.core;
using starling.display;
using starling.events;
using starling.text;
using starling.textures;
using starling.utils;
 
namespace starling.text
{
	public class AsTextField : AsDisplayObjectContainer
	{
		private float mFontSize;
		private uint mColor;
		private String mText;
		private String mFontName;
		private String mHAlign;
		private String mVAlign;
		private bool mBold;
		private bool mItalic;
		private bool mUnderline;
		private bool mAutoScale;
		private bool mKerning;
		private bool mRequiresRedraw;
		private bool mIsRenderedText;
		private AsRectangle mTextBounds;
		private AsDisplayObject mHitArea;
		private AsDisplayObjectContainer mBorder;
		private AsImage mImage;
		private AsQuadBatch mQuadBatch;
		private static bc.flash.text.AsTextField sNativeTextField = new bc.flash.text.AsTextField();
		private static AsDictionary sBitmapFonts = new AsDictionary();
		public AsTextField(int width, int height, String text, String fontName, float fontSize, uint color, bool bold)
		{
			mText = ((text != null) ? (text) : (""));
			mFontSize = fontSize;
			mColor = color;
			mHAlign = AsHAlign.CENTER;
			mVAlign = AsVAlign.CENTER;
			mBorder = null;
			mKerning = true;
			mBold = bold;
			this.setFontName(fontName);
			mHitArea = new AsQuad(width, height);
			mHitArea.setAlpha(0.0f);
			addChild(mHitArea);
			addEventListener(AsEvent.FLATTEN, onFlatten);
		}
		public AsTextField(int width, int height, String text, String fontName, float fontSize, uint color)
		 : this(width, height, text, fontName, fontSize, color, false)
		{
		}
		public AsTextField(int width, int height, String text, String fontName, float fontSize)
		 : this(width, height, text, fontName, fontSize, 0x0, false)
		{
		}
		public AsTextField(int width, int height, String text, String fontName)
		 : this(width, height, text, fontName, 12, 0x0, false)
		{
		}
		public AsTextField(int width, int height, String text)
		 : this(width, height, text, "Verdana", 12, 0x0, false)
		{
		}
		public override void dispose()
		{
			removeEventListener(AsEvent.FLATTEN, onFlatten);
			if(mImage != null)
			{
				mImage.getTexture().dispose();
			}
			if(mQuadBatch != null)
			{
				mQuadBatch.dispose();
			}
			base.dispose();
		}
		private void onFlatten(AsEvent _event)
		{
			if(mRequiresRedraw)
			{
				redrawContents();
			}
		}
		public override void render(AsRenderSupport support, float parentAlpha)
		{
			if(mRequiresRedraw)
			{
				redrawContents();
			}
			base.render(support, parentAlpha);
		}
		private void redrawContents()
		{
			if(mIsRenderedText)
			{
				createRenderedContents();
			}
			else
			{
				createComposedContents();
			}
			mRequiresRedraw = false;
		}
		private void createRenderedContents()
		{
			if(mQuadBatch != null)
			{
				mQuadBatch.removeFromParent(true);
				mQuadBatch = null;
			}
			float scale = AsStarling.getContentScaleFactor();
			float width = (mHitArea.getWidth() * scale);
			float height = (mHitArea.getHeight() * scale);
			AsTextFormat textFormat = new AsTextFormat(mFontName, (mFontSize * scale), mColor, mBold, mItalic, mUnderline, null, null, mHAlign);
			textFormat.setKerning(mKerning);
			sNativeTextField.setDefaultTextFormat(textFormat);
			sNativeTextField.setWidth(width);
			sNativeTextField.setHeight(height);
			sNativeTextField.setAntiAliasType(AsAntiAliasType.ADVANCED);
			sNativeTextField.setSelectable(false);
			sNativeTextField.setMultiline(true);
			sNativeTextField.setWordWrap(true);
			sNativeTextField.setText(mText);
			sNativeTextField.setEmbedFonts(true);
			if(((sNativeTextField.getTextWidth() == 0.0f) || (sNativeTextField.getTextHeight() == 0.0f)))
			{
				sNativeTextField.setEmbedFonts(false);
			}
			if(mAutoScale)
			{
				autoScaleNativeTextField(sNativeTextField);
			}
			float textWidth = sNativeTextField.getTextWidth();
			float textHeight = sNativeTextField.getTextHeight();
			float xOffset = 0.0f;
			if((mHAlign == AsHAlign.LEFT))
			{
				xOffset = 2;
			}
			else
			{
				if((mHAlign == AsHAlign.CENTER))
				{
					xOffset = ((width - textWidth) / 2.0f);
				}
				else
				{
					if((mHAlign == AsHAlign.RIGHT))
					{
						xOffset = ((width - textWidth) - 2);
					}
				}
			}
			float yOffset = 0.0f;
			if((mVAlign == AsVAlign.TOP))
			{
				yOffset = 2;
			}
			else
			{
				if((mVAlign == AsVAlign.CENTER))
				{
					yOffset = ((height - textHeight) / 2.0f);
				}
				else
				{
					if((mVAlign == AsVAlign.BOTTOM))
					{
						yOffset = ((height - textHeight) - 2);
					}
				}
			}
			AsBitmapData bitmapData = new AsBitmapData(width, height, true, 0x0);
			bitmapData.draw(sNativeTextField, new AsMatrix(1, 0, 0, 1, 0, (((int)(yOffset)) - 2)));
			if((mTextBounds == null))
			{
				mTextBounds = new AsRectangle();
			}
			mTextBounds.setTo((xOffset / scale), (yOffset / scale), (textWidth / scale), (textHeight / scale));
			AsTexture texture = AsTexture.fromBitmapData(bitmapData, true, false, scale);
			if((mImage == null))
			{
				mImage = new AsImage(texture);
				mImage.setTouchable(false);
				addChild(mImage);
			}
			else
			{
				mImage.getTexture().dispose();
				mImage.setTexture(texture);
				mImage.readjustSize();
			}
		}
		private void autoScaleNativeTextField(bc.flash.text.AsTextField textField)
		{
			float size = ((float)(textField.getDefaultTextFormat().getSize()));
			int maxHeight = (int)((textField.getHeight() - 4));
			int maxWidth = (int)((textField.getWidth() - 4));
			while(((textField.getTextWidth() > maxWidth) || (textField.getTextHeight() > maxHeight)))
			{
				if((size <= 4))
				{
					break;
				}
				AsTextFormat format = textField.getDefaultTextFormat();
				format.setSize(size--);
				textField.setTextFormat(format);
			}
		}
		private void createComposedContents()
		{
			if(mImage != null)
			{
				mImage.removeFromParent(true);
				mImage = null;
			}
			if((mQuadBatch == null))
			{
				mQuadBatch = new AsQuadBatch();
				mQuadBatch.setTouchable(false);
				addChild(mQuadBatch);
			}
			else
			{
				mQuadBatch.reset();
			}
			AsBitmapFont bitmapFont = (AsBitmapFont)(sBitmapFonts[mFontName]);
			if((bitmapFont == null))
			{
				throw new AsError(("Bitmap font not registered: " + mFontName));
			}
			bitmapFont.fillQuadBatch(mQuadBatch, mHitArea.getWidth(), mHitArea.getHeight(), mText, mFontSize, mColor, mHAlign, mVAlign, mAutoScale, mKerning);
			mTextBounds = null;
		}
		private void updateBorder()
		{
			if((mBorder == null))
			{
				return;
			}
			float width = mHitArea.getWidth();
			float height = mHitArea.getHeight();
			AsQuad topLine = ((mBorder.getChildAt(0) is AsQuad) ? ((AsQuad)(mBorder.getChildAt(0))) : null);
			AsQuad rightLine = ((mBorder.getChildAt(1) is AsQuad) ? ((AsQuad)(mBorder.getChildAt(1))) : null);
			AsQuad bottomLine = ((mBorder.getChildAt(2) is AsQuad) ? ((AsQuad)(mBorder.getChildAt(2))) : null);
			AsQuad leftLine = ((mBorder.getChildAt(3) is AsQuad) ? ((AsQuad)(mBorder.getChildAt(3))) : null);
			topLine.setWidth(width);
			topLine.setHeight(1);
			bottomLine.setWidth(width);
			bottomLine.setHeight(1);
			leftLine.setWidth(1);
			leftLine.setHeight(height);
			rightLine.setWidth(1);
			rightLine.setHeight(height);
			rightLine.setX((width - 1));
			bottomLine.setY((height - 1));
			topLine.setColor(rightLine.setColor(bottomLine.setColor(leftLine.setColor(mColor))));
		}
		public virtual AsRectangle getTextBounds()
		{
			if(mRequiresRedraw)
			{
				redrawContents();
			}
			if((mTextBounds == null))
			{
				mTextBounds = mQuadBatch.getBounds(mQuadBatch);
			}
			return mTextBounds.clone();
		}
		public override AsRectangle getBounds(AsDisplayObject targetSpace, AsRectangle resultRect)
		{
			return mHitArea.getBounds(targetSpace, resultRect);
		}
		public virtual AsRectangle getBounds(AsDisplayObject targetSpace)
		{
			return getBounds(targetSpace, null);
		}
		public override void setWidth(float _value)
		{
			mHitArea.setWidth(_value);
			mRequiresRedraw = true;
			updateBorder();
		}
		public override void setHeight(float _value)
		{
			mHitArea.setHeight(_value);
			mRequiresRedraw = true;
			updateBorder();
		}
		public virtual String getText()
		{
			return mText;
		}
		public virtual void setText(String _value)
		{
			if((_value == null))
			{
				_value = "";
			}
			if((mText != _value))
			{
				mText = _value;
				mRequiresRedraw = true;
			}
		}
		public virtual String getFontName()
		{
			return mFontName;
		}
		public virtual void setFontName(String _value)
		{
			if((mFontName != _value))
			{
				if(((_value == AsBitmapFont.MINI) && (sBitmapFonts[_value] == null)))
				{
					registerBitmapFont(new AsBitmapFont());
				}
				mFontName = _value;
				mRequiresRedraw = true;
				mIsRenderedText = (sBitmapFonts[_value] == null);
			}
		}
		public virtual float getFontSize()
		{
			return mFontSize;
		}
		public virtual void setFontSize(float _value)
		{
			if((mFontSize != _value))
			{
				mFontSize = _value;
				mRequiresRedraw = true;
			}
		}
		public virtual uint getColor()
		{
			return mColor;
		}
		public virtual void setColor(uint _value)
		{
			if((mColor != _value))
			{
				mColor = _value;
				updateBorder();
				mRequiresRedraw = true;
			}
		}
		public virtual String getHAlign()
		{
			return mHAlign;
		}
		public virtual void setHAlign(String _value)
		{
			if(!(AsHAlign.isValid(_value)))
			{
				throw new AsArgumentError(("Invalid horizontal align: " + _value));
			}
			if((mHAlign != _value))
			{
				mHAlign = _value;
				mRequiresRedraw = true;
			}
		}
		public virtual String getVAlign()
		{
			return mVAlign;
		}
		public virtual void setVAlign(String _value)
		{
			if(!(AsVAlign.isValid(_value)))
			{
				throw new AsArgumentError(("Invalid vertical align: " + _value));
			}
			if((mVAlign != _value))
			{
				mVAlign = _value;
				mRequiresRedraw = true;
			}
		}
		public virtual bool getBorder()
		{
			return (mBorder != null);
		}
		public virtual void setBorder(bool _value)
		{
			if((_value && (mBorder == null)))
			{
				mBorder = new AsSprite();
				addChild(mBorder);
				int i = 0;
				for (; (i < 4); ++i)
				{
					mBorder.addChild(new AsQuad(1.0f, 1.0f));
				}
				updateBorder();
			}
			else
			{
				if((!(_value) && (mBorder != null)))
				{
					mBorder.removeFromParent(true);
					mBorder = null;
				}
			}
		}
		public virtual bool getBold()
		{
			return mBold;
		}
		public virtual void setBold(bool _value)
		{
			if((mBold != _value))
			{
				mBold = _value;
				mRequiresRedraw = true;
			}
		}
		public virtual bool getItalic()
		{
			return mItalic;
		}
		public virtual void setItalic(bool _value)
		{
			if((mItalic != _value))
			{
				mItalic = _value;
				mRequiresRedraw = true;
			}
		}
		public virtual bool getUnderline()
		{
			return mUnderline;
		}
		public virtual void setUnderline(bool _value)
		{
			if((mUnderline != _value))
			{
				mUnderline = _value;
				mRequiresRedraw = true;
			}
		}
		public virtual bool getKerning()
		{
			return mKerning;
		}
		public virtual void setKerning(bool _value)
		{
			if((mKerning != _value))
			{
				mKerning = _value;
				mRequiresRedraw = true;
			}
		}
		public virtual bool getAutoScale()
		{
			return mAutoScale;
		}
		public virtual void setAutoScale(bool _value)
		{
			if((mAutoScale != _value))
			{
				mAutoScale = _value;
				mRequiresRedraw = true;
			}
		}
		public static void registerBitmapFont(AsBitmapFont bitmapFont, String name)
		{
			sBitmapFonts[((name != null) ? (name) : (bitmapFont.getName()))] = bitmapFont;
		}
		public static void registerBitmapFont(AsBitmapFont bitmapFont)
		{
			registerBitmapFont(bitmapFont, null);
		}
		public static void unregisterBitmapFont(String name, bool dispose)
		{
			if((dispose && (sBitmapFonts[name] != null)))
			{
				((AsBitmapFont)(sBitmapFonts[name])).dispose();
			}
			sBitmapFonts.remove(name);
		}
		public static void unregisterBitmapFont(String name)
		{
			unregisterBitmapFont(name, true);
		}
		public static AsBitmapFont getBitmapFont(String name)
		{
			return (AsBitmapFont)(sBitmapFonts[name]);
		}
	}
}
