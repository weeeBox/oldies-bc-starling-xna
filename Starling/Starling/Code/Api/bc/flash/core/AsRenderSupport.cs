using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.error;
using bc.flash.geom;
 
namespace bc.flash.core
{
	public class AsRenderSupport : AsObject
	{
		private AsVector<AsMatrix> mMatrixStack;
		private int mMatrixStackSize;
		private AsMatrix mCurrentMatrix;
		public AsRenderSupport()
		{
			mMatrixStack = new AsVector<AsMatrix>();
			mMatrixStackSize = 0;
			mCurrentMatrix = new AsMatrix();
		}
		public virtual void dispose()
		{
		}
		public virtual void transform(AsMatrix matrix)
		{
			mCurrentMatrix.concat(matrix);
		}
		public virtual void pushMatrix()
		{
			if((mMatrixStack.getLength() < (mMatrixStackSize + 1)))
			{
				mMatrixStack.push(new AsMatrix());
			}
			mMatrixStack[mMatrixStackSize++].copyFrom(mCurrentMatrix);
		}
		public virtual void popMatrix()
		{
			mCurrentMatrix.copyFrom(mMatrixStack[--mMatrixStackSize]);
		}
		public virtual void resetMatrix()
		{
			mMatrixStackSize = 0;
			mCurrentMatrix.identity();
		}
		public virtual void drawBitmap(AsBitmapData bitmap, float alpha)
		{
			throw new AsNotImplementedError();
		}
	}
}
