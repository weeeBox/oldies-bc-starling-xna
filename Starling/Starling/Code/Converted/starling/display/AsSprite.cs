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
		public AsSprite()
		 : base()
		{
		}
		public override void dispose()
		{
			unflatten();
			base.dispose();
		}
		public virtual void flatten()
		{
			broadcastEventWith(AsEvent.FLATTEN);
			if((mFlattenedContents == null))
			{
				mFlattenedContents = new AsVector<AsQuadBatch>();
			}
			AsQuadBatch.compile(this, mFlattenedContents);
		}
		public virtual void unflatten()
		{
			if(mFlattenedContents != null)
			{
				int numBatches = (int)(mFlattenedContents.getLength());
				int i = 0;
				for (; (i < numBatches); ++i)
				{
					mFlattenedContents[i].dispose();
				}
				mFlattenedContents = null;
			}
		}
		public virtual bool getIsFlattened()
		{
			return (mFlattenedContents != null);
		}
		public override void render(AsRenderSupport support, float parentAlpha)
		{
			if(mFlattenedContents != null)
			{
				float alpha = (parentAlpha * this.getAlpha());
				int numBatches = (int)(mFlattenedContents.getLength());
				AsMatrix mvpMatrix = support.getMvpMatrix();
				support.finishQuadBatch();
				support.raiseDrawCount((uint)(numBatches));
				int i = 0;
				for (; (i < numBatches); ++i)
				{
					AsQuadBatch quadBatch = mFlattenedContents[i];
					String blendMode = (((quadBatch.getBlendMode() == AsBlendMode.AUTO)) ? (support.getBlendMode()) : (quadBatch.getBlendMode()));
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
