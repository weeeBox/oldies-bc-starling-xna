using System;
 
using bc.flash;
using bc.flash.geom;
using starling.core;
using starling.display;
using starling.events;
 
namespace starling.display
{
	public class AsSprite : AsDisplayObjectContainer
	{
		private AsVector<AsQuadBatch> mFlattenedContents;
		private bool mFlattenRequested;
		public AsSprite()
		 : base()
		{
		}
		public override void dispose()
		{
			disposeFlattenedContents();
			base.dispose();
		}
		private void disposeFlattenedContents()
		{
			if(mFlattenedContents != null)
			{
				int i = 0;
				int max = (int)(mFlattenedContents.getLength());
				for (; i < max; ++i)
				{
					mFlattenedContents[i].dispose();
				}
				mFlattenedContents = null;
			}
		}
		public virtual void flatten()
		{
			mFlattenRequested = true;
			broadcastEventWith(AsEvent.FLATTEN);
		}
		public virtual void unflatten()
		{
			mFlattenRequested = false;
			disposeFlattenedContents();
		}
		public virtual bool getIsFlattened()
		{
			return mFlattenedContents != null || mFlattenRequested;
		}
		public override void render(AsRenderSupport support, float parentAlpha)
		{
			if(mFlattenedContents != null || mFlattenRequested)
			{
				if(mFlattenedContents == null)
				{
					mFlattenedContents = new AsVector<AsQuadBatch>();
				}
				if(mFlattenRequested)
				{
					AsQuadBatch.compile(this, mFlattenedContents);
					mFlattenRequested = false;
				}
				float alpha = parentAlpha * this.getAlpha();
				int numBatches = (int)(mFlattenedContents.getLength());
				AsMatrix mvpMatrix = support.getMvpMatrix();
				support.finishQuadBatch();
				support.raiseDrawCount((uint)(numBatches));
				int i = 0;
				for (; i < numBatches; ++i)
				{
					AsQuadBatch quadBatch = mFlattenedContents[i];
					String blendMode = quadBatch.getBlendMode() == AsBlendMode.AUTO ? support.getBlendMode() : quadBatch.getBlendMode();
					quadBatch.renderCustom(mvpMatrix, alpha, blendMode);
				}
			}
			else
			{
				base.render(support, parentAlpha);
			}
		}
	}
}
