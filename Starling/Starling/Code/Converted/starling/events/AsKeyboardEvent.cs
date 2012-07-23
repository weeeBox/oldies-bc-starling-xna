using System;
 
using bc.flash;
using starling.events;
 
namespace starling.events
{
	public class AsKeyboardEvent : AsEvent
	{
		public static String KEY_UP = "keyUp";
		public static String KEY_DOWN = "keyDown";
		private uint mCharCode;
		private uint mKeyCode;
		private uint mKeyLocation;
		private bool mAltKey;
		private bool mCtrlKey;
		private bool mShiftKey;
		public AsKeyboardEvent(String type, uint charCode, uint keyCode, uint keyLocation, bool ctrlKey, bool altKey, bool shiftKey)
		 : base(type, false, keyCode)
		{
			mCharCode = charCode;
			mKeyCode = keyCode;
			mKeyLocation = keyLocation;
			mCtrlKey = ctrlKey;
			mAltKey = altKey;
			mShiftKey = shiftKey;
		}
		public AsKeyboardEvent(String type, uint charCode, uint keyCode, uint keyLocation, bool ctrlKey, bool altKey)
		 : this(type, charCode, keyCode, keyLocation, ctrlKey, altKey, false)
		{
		}
		public AsKeyboardEvent(String type, uint charCode, uint keyCode, uint keyLocation, bool ctrlKey)
		 : this(type, charCode, keyCode, keyLocation, ctrlKey, false, false)
		{
		}
		public AsKeyboardEvent(String type, uint charCode, uint keyCode, uint keyLocation)
		 : this(type, charCode, keyCode, keyLocation, false, false, false)
		{
		}
		public AsKeyboardEvent(String type, uint charCode, uint keyCode)
		 : this(type, charCode, keyCode, 0, false, false, false)
		{
		}
		public AsKeyboardEvent(String type, uint charCode)
		 : this(type, charCode, 0, 0, false, false, false)
		{
		}
		public AsKeyboardEvent(String type)
		 : this(type, 0, 0, 0, false, false, false)
		{
		}
		public virtual uint getCharCode()
		{
			return mCharCode;
		}
		public virtual uint getKeyCode()
		{
			return mKeyCode;
		}
		public virtual uint getKeyLocation()
		{
			return mKeyLocation;
		}
		public virtual bool getAltKey()
		{
			return mAltKey;
		}
		public virtual bool getCtrlKey()
		{
			return mCtrlKey;
		}
		public virtual bool getShiftKey()
		{
			return mShiftKey;
		}
	}
}
