using System;
 
using bc.flash;
using bc.flash.geom;
using bc.flash.ui;
using starling.display;
using starling.events;
using starling.text;
using starling.textures;
using starling.utils;
 
namespace starling.display
{
	public class AsButton : AsDisplayObjectContainer
	{
		private static float MAX_DRAG_DIST = 50;
		private AsTexture mUpState;
		private AsTexture mDownState;
		private AsSprite mContents;
		private AsImage mBackground;
		private AsTextField mTextField;
		private AsRectangle mTextBounds;
		private float mScaleWhenDown;
		private float mAlphaWhenDisabled;
		private bool mEnabled;
		private bool mIsDown;
		private bool mUseHandCursor;
		public AsButton(AsTexture upState, String text, AsTexture downState)
		{
			if((upState == null))
			{
				throw new AsArgumentError("Texture cannot be null");
			}
			mUpState = upState;
			mDownState = ((downState != null) ? (downState) : (upState));
			mBackground = new AsImage(upState);
			mScaleWhenDown = ((downState != null) ? (1.0f) : (0.9f));
			mAlphaWhenDisabled = 0.5f;
			mEnabled = true;
			mIsDown = false;
			mUseHandCursor = true;
			mTextBounds = new AsRectangle(0, 0, upState.getWidth(), upState.getHeight());
			mContents = new AsSprite();
			mContents.addChild(mBackground);
			addChild(mContents);
			addEventListener(AsTouchEvent.TOUCH, onTouch);
			if((text.Length != 0))
			{
				this.setText(text);
			}
		}
		public AsButton(AsTexture upState, String text)
		 : this(upState, text, null)
		{
		}
		public AsButton(AsTexture upState)
		 : this(upState, "", null)
		{
		}
		private void resetContents()
		{
			mIsDown = false;
			mBackground.setTexture(mUpState);
			mContents.setX(mContents.setY(0));
			mContents.setScaleX(mContents.setScaleY(1.0f));
		}
		private void createTextField()
		{
			if((mTextField == null))
			{
				mTextField = new AsTextField(mTextBounds.width, mTextBounds.height, "");
				mTextField.setVAlign(AsVAlign.CENTER);
				mTextField.setHAlign(AsHAlign.CENTER);
				mTextField.setTouchable(false);
				mTextField.setAutoScale(true);
				mContents.addChild(mTextField);
			}
			mTextField.setWidth(mTextBounds.width);
			mTextField.setHeight(mTextBounds.height);
			mTextField.setX(mTextBounds.x);
			mTextField.setY(mTextBounds.y);
		}
		private void onTouch(AsTouchEvent _event)
		{
			AsMouse.setCursor(((((mUseHandCursor && mEnabled) && _event.interactsWith(this))) ? (AsMouseCursor.BUTTON) : (AsMouseCursor.AUTO)));
			AsTouch touch = _event.getTouch(this);
			if((!(mEnabled) || (touch == null)))
			{
				return;
			}
			if(((touch.getPhase() == AsTouchPhase.BEGAN) && !(mIsDown)))
			{
				mBackground.setTexture(mDownState);
				mContents.setScaleX(mContents.setScaleY(mScaleWhenDown));
				mContents.setX((((1.0f - mScaleWhenDown) / 2.0f) * mBackground.getWidth()));
				mContents.setY((((1.0f - mScaleWhenDown) / 2.0f) * mBackground.getHeight()));
				mIsDown = true;
			}
			else
			{
				if(((touch.getPhase() == AsTouchPhase.MOVED) && mIsDown))
				{
					AsRectangle buttonRect = getBounds(getStage());
					if(((((touch.getGlobalX() < (buttonRect.x - MAX_DRAG_DIST)) || (touch.getGlobalY() < (buttonRect.y - MAX_DRAG_DIST))) || (touch.getGlobalX() > ((buttonRect.x + buttonRect.width) + MAX_DRAG_DIST))) || (touch.getGlobalY() > ((buttonRect.y + buttonRect.height) + MAX_DRAG_DIST))))
					{
						resetContents();
					}
				}
				else
				{
					if(((touch.getPhase() == AsTouchPhase.ENDED) && mIsDown))
					{
						resetContents();
						dispatchEventWith(AsEvent.TRIGGERED, true);
					}
				}
			}
		}
		public virtual float getScaleWhenDown()
		{
			return mScaleWhenDown;
		}
		public virtual void setScaleWhenDown(float _value)
		{
			mScaleWhenDown = _value;
		}
		public virtual float getAlphaWhenDisabled()
		{
			return mAlphaWhenDisabled;
		}
		public virtual void setAlphaWhenDisabled(float _value)
		{
			mAlphaWhenDisabled = _value;
		}
		public virtual bool getEnabled()
		{
			return mEnabled;
		}
		public virtual void setEnabled(bool _value)
		{
			if((mEnabled != _value))
			{
				mEnabled = _value;
				mContents.setAlpha(((_value) ? (1.0f) : (mAlphaWhenDisabled)));
				resetContents();
			}
		}
		public virtual String getText()
		{
			return ((mTextField != null) ? (mTextField.getText()) : (""));
		}
		public virtual void setText(String _value)
		{
			createTextField();
			mTextField.setText(_value);
		}
		public virtual String getFontName()
		{
			return ((mTextField != null) ? (mTextField.getFontName()) : ("Verdana"));
		}
		public virtual void setFontName(String _value)
		{
			createTextField();
			mTextField.setFontName(_value);
		}
		public virtual float getFontSize()
		{
			return ((mTextField != null) ? (mTextField.getFontSize()) : (12));
		}
		public virtual void setFontSize(float _value)
		{
			createTextField();
			mTextField.setFontSize(_value);
		}
		public virtual uint getFontColor()
		{
			return ((mTextField != null) ? (mTextField.getColor()) : (0x0));
		}
		public virtual void setFontColor(uint _value)
		{
			createTextField();
			mTextField.setColor(_value);
		}
		public virtual bool getFontBold()
		{
			return ((mTextField != null) ? (mTextField.getBold()) : (false));
		}
		public virtual void setFontBold(bool _value)
		{
			createTextField();
			mTextField.setBold(_value);
		}
		public virtual AsTexture getUpState()
		{
			return mUpState;
		}
		public virtual void setUpState(AsTexture _value)
		{
			if((mUpState != _value))
			{
				mUpState = _value;
				if(!(mIsDown))
				{
					mBackground.setTexture(_value);
				}
			}
		}
		public virtual AsTexture getDownState()
		{
			return mDownState;
		}
		public virtual void setDownState(AsTexture _value)
		{
			if((mDownState != _value))
			{
				mDownState = _value;
				if(mIsDown)
				{
					mBackground.setTexture(_value);
				}
			}
		}
		public virtual AsRectangle getTextBounds()
		{
			return mTextBounds.clone();
		}
		public virtual void setTextBounds(AsRectangle _value)
		{
			mTextBounds = _value.clone();
			createTextField();
		}
		public override bool getUseHandCursor()
		{
			return mUseHandCursor;
		}
		public override void setUseHandCursor(bool _value)
		{
			mUseHandCursor = _value;
		}
	}
}
