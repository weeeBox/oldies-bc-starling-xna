using System;
 
using bc.flash;
using bc.flash.system;
using starling.display;
using starling.events;
using starling.text;
using starling.utils;
 
namespace starling.core
{
	public class AsStatsDisplay : AsSprite
	{
		private AsQuad mBackground;
		private AsTextField mTextField;
		private int mFrameCount = 0;
		private int mDrawCount = 0;
		private float mTotalTime = 0;
		public AsStatsDisplay()
		{
			mBackground = new AsQuad(50, 25, 0x0);
			mTextField = new AsTextField(48, 25, "", AsBitmapFont.MINI, AsBitmapFont.NATIVE_SIZE, 0xffffff);
			mTextField.setX(2);
			mTextField.setHAlign(AsHAlign.LEFT);
			mTextField.setVAlign(AsVAlign.TOP);
			addChild(mBackground);
			addChild(mTextField);
			addEventListener(AsEvent.ENTER_FRAME, onEnterFrame);
			updateText(0, getMemory(), 0);
			setBlendMode(AsBlendMode.NONE);
		}
		private void updateText(float fps, float memory, int drawCount)
		{
			mTextField.setText("FPS: " + float.toFixed(fps, fps < 100 ? 1 : 0) + "\nMEM: " + float.toFixed(memory, memory < 100 ? 1 : 0) + "\nDRW: " + drawCount);
		}
		private float getMemory()
		{
			return AsSystem.getTotalMemory() * 0.000000954f;
		}
		private void onEnterFrame(AsEnterFrameEvent _event)
		{
			mTotalTime = mTotalTime + _event.getPassedTime();
			mFrameCount++;
			if(mTotalTime > 1.0f)
			{
				updateText(mFrameCount / mTotalTime, getMemory(), mDrawCount - 2);
				mFrameCount = (int)(mTotalTime = 0);
			}
		}
		public virtual int getDrawCount()
		{
			return mDrawCount;
		}
		public virtual void setDrawCount(int _value)
		{
			mDrawCount = _value;
		}
	}
}
