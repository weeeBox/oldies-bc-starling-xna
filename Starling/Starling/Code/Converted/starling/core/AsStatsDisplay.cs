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
		private float mTotalTime = 0;
		public AsStatsDisplay()
		{
			mBackground = new AsQuad(49, 18, 0x0);
			mTextField = new AsTextField(60, 18, "", AsBitmapFont.MINI, AsBitmapFont.NATIVE_SIZE, 0xffffff);
			mTextField.setX(2);
			mTextField.setHAlign(AsHAlign.LEFT);
			mTextField.setVAlign(AsVAlign.TOP);
			addChild(mBackground);
			addChild(mTextField);
			addEventListener(AsEvent.ENTER_FRAME, onEnterFrame);
			updateText(0, getMemory());
			setBlendMode(AsBlendMode.NONE);
		}
		private void updateText(float fps, float memory)
		{
			mTextField.setText(((("FPS: " + fps) + "\
			MEM: ") + memory));
			mBackground.setWidth(((((fps >= 100) || (memory >= 100))) ? (55) : (49)));
		}
		private float getMemory()
		{
			return (AsSystem.getTotalMemory() * 0.000000954f);
		}
		private void onEnterFrame(AsEnterFrameEvent _event)
		{
			mTotalTime = (mTotalTime + _event.getPassedTime());
			mFrameCount++;
			if((mTotalTime > 1.0f))
			{
				updateText((mFrameCount / mTotalTime), getMemory());
				mFrameCount = (int)(mTotalTime = 0);
			}
		}
	}
}
