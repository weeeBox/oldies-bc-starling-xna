using System;
 
using bc.flash;
using bc.flash.input;
 
namespace bc.flash.input
{
	public class AsGamePad : AsObject
	{
		private uint mPlayerIndex;
		private AsThumbStick mRightStick;
		private AsThumbStick mLeftStick;
		private float mLeftTrigger;
		private float mRightTrigger;
		private static AsVector<AsGamePad> mGamePads = new AsVector<AsGamePad>(new AsGamePad(0), new AsGamePad(1), new AsGamePad(2), new AsGamePad(3));
		public static AsGamePad player(uint index)
		{
			return mGamePads[index];
		}
		public AsGamePad(uint playerIndex)
		{
			mPlayerIndex = playerIndex;
			mRightStick = new AsThumbStick();
			mLeftStick = new AsThumbStick();
		}
		public virtual void update(float leftTrigger, float rightTrigger)
		{
			mLeftTrigger = leftTrigger;
			mRightTrigger = rightTrigger;
		}
		public virtual uint getPlayerIndex()
		{
			return mPlayerIndex;
		}
		public virtual AsThumbStick getLeftStick()
		{
			return mLeftStick;
		}
		public virtual AsThumbStick getRightStick()
		{
			return mRightStick;
		}
		public virtual float getLeftTrigger()
		{
			return mLeftTrigger;
		}
		public virtual float getRightTrigger()
		{
			return mRightTrigger;
		}
	}
}
