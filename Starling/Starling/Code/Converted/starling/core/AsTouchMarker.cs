using System;
 
using bc.flash;
using bc.flash.geom;
using starling.display;
using starling.textures;
 
namespace starling.core
{
	public class AsTouchMarker : AsSprite
	{
		private AsPoint mCenter;
		private AsTexture mTexture;
		public AsTouchMarker()
		{
			mCenter = new AsPoint();
			mTexture = createTexture();
			int i = 0;
			for (; i < 2; ++i)
			{
				AsImage marker = new AsImage(mTexture);
				marker.setPivotX(mTexture.getWidth() / 2);
				marker.setPivotY(mTexture.getHeight() / 2);
				marker.setTouchable(false);
				addChild(marker);
			}
		}
		public override void dispose()
		{
			mTexture.dispose();
			base.dispose();
		}
		public virtual void moveMarker(float x, float y, bool withCenter)
		{
			if(withCenter)
			{
				mCenter.x = mCenter.x + x - getRealMarker().getX();
				mCenter.y = mCenter.y + y - getRealMarker().getY();
			}
			getRealMarker().setX(x);
			getRealMarker().setY(y);
			getMockMarker().setX(2 * mCenter.x - x);
			getMockMarker().setY(2 * mCenter.y - y);
		}
		public virtual void moveMarker(float x, float y)
		{
			moveMarker(x, y, false);
		}
		public virtual void moveCenter(float x, float y)
		{
			mCenter.x = x;
			mCenter.y = y;
			moveMarker(getRealX(), getRealY());
		}
		private AsTexture createTexture()
		{
			return null;
		}
		private AsImage getRealMarker()
		{
			return getChildAt(0) as AsImage;
		}
		private AsImage getMockMarker()
		{
			return getChildAt(1) as AsImage;
		}
		public virtual float getRealX()
		{
			return getRealMarker().getX();
		}
		public virtual float getRealY()
		{
			return getRealMarker().getY();
		}
		public virtual float getMockX()
		{
			return getMockMarker().getX();
		}
		public virtual float getMockY()
		{
			return getMockMarker().getY();
		}
	}
}
