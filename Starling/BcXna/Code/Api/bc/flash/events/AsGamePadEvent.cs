using System;
 
using bc.flash;
using bc.flash.events;
 
namespace bc.flash.events
{
	public class AsGamePadEvent : AsEvent
	{
		public static String BUTTON_UP = "buttonUp";
		public static String BUTTON_DOWN = "buttonDown";
		public static String CONNECTED = "gamepadConnected";
		public static String DISCONNECTED = "gamepadDisconnected";
		public const uint Undefined = 0;
		public const uint DPadUp = 1;
		public const uint DPadDown = 2;
		public const uint DPadLeft = 4;
		public const uint DPadRight = 8;
		public const uint Start = 16;
		public const uint Back = 32;
		public const uint LeftStick = 64;
		public const uint RightStick = 128;
		public const uint LeftShoulder = 256;
		public const uint RightShoulder = 512;
		public const uint BigButton = 2048;
		public const uint A = 4096;
		public const uint B = 8192;
		public const uint X = 16384;
		public const uint Y = 32768;
		public const uint LeftThumbstickLeft = 2097152;
		public const uint RightTrigger = 4194304;
		public const uint LeftTrigger = 8388608;
		public const uint RightThumbstickUp = 16777216;
		public const uint RightThumbstickDown = 33554432;
		public const uint RightThumbstickRight = 67108864;
		public const uint RightThumbstickLeft = 134217728;
		public const uint LeftThumbstickUp = 268435456;
		public const uint LeftThumbstickDown = 536870912;
		public const uint LeftThumbstickRight = 1073741824;
		private uint mCode;
		private uint mPlayerIndex;
		public AsGamePadEvent(String type, uint playerIndex, uint code)
		 : base(type, false)
		{
			mPlayerIndex = playerIndex;
			mCode = code;
		}
		public AsGamePadEvent(String type, uint playerIndex)
		 : this(type, playerIndex, Undefined)
		{
		}
		public virtual void update(uint playerIndex, uint code)
		{
			mPlayerIndex = playerIndex;
			mCode = code;
		}
		public virtual void update(uint playerIndex)
		{
			update(playerIndex, Undefined);
		}
		public virtual uint getPlayerIndex()
		{
			return mPlayerIndex;
		}
		public virtual uint getCode()
		{
			return mCode;
		}
	}
}
