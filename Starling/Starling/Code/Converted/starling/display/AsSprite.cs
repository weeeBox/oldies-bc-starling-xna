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
				AsStarling.getCurrent().addEventListener(AsEvent.CONTEXT3D_CREATE, onContextCreated);
			}
			AsQuadBatch.compile(this, mFlattenedContents);
		}
		public virtual void unflatten()
		{
			if(mFlattenedContents != null)
			{
				AsStarling.getCurrent().removeEventListener(AsEvent.CONTEXT3D_CREATE, onContextCreated);
				int numBatches = (int)(mFlattenedContents.getLength());
				int i = 0;
				for (; (i < numBatches); ++i)
				{
					mFlattenedContents[i].dispose();
				}
				mFlattenedContents = null;
			}
		}
		private void onContextCreated(AsEvent _event)
		{
			if(mFlattenedContents != null)
			{
				mFlattenedContents = new AsVector<AsQuadBatch>();
				flatten();
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
				support.finishQuadBatch();
				float alpha = (parentAlpha * this.getAlpha());
				int numBatches = (int)(mFlattenedContents.getLength());
				AsMatrix mvpMatrix = support.getMvpMatrix();
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
