using System;
 
using bc.flash;
using bc.flash.geom;
using starling.core;
using starling.display;
using starling.textures;
using starling.utils;
 
namespace starling.core
{
	public class AsRenderSupport : AsObject
	{
		private AsMatrix mProjectionMatrix;
		private AsMatrix mModelViewMatrix;
		private AsMatrix mMvpMatrix;
		private AsMatrix3D mMvpMatrix3D;
		private AsVector<AsMatrix> mMatrixStack;
		private int mMatrixStackSize;
		private int mDrawCount;
		private AsTexture mRenderTarget;
		private String mBlendMode;
		private AsVector<AsQuadBatch> mQuadBatches;
		private int mCurrentQuadBatchID;
		public AsRenderSupport()
		{
			mProjectionMatrix = new AsMatrix();
			mModelViewMatrix = new AsMatrix();
			mMvpMatrix = new AsMatrix();
			mMvpMatrix3D = new AsMatrix3D();
			mMatrixStack = new AsVector<AsMatrix>();
			mMatrixStackSize = 0;
			mDrawCount = 0;
			mRenderTarget = null;
			mBlendMode = AsBlendMode.NORMAL;
			mCurrentQuadBatchID = 0;
			mQuadBatches = new AsVector<AsQuadBatch>();
			loadIdentity();
			setOrthographicProjection(0, 0, 400, 300);
		}
		public virtual void dispose()
		{
			AsVector<AsQuadBatch> __quadBatchs_ = mQuadBatches;
			if (__quadBatchs_ != null)
			{
				foreach (AsQuadBatch quadBatch in __quadBatchs_)
				{
					quadBatch.dispose();
				}
			}
		}
		public virtual void setOrthographicProjection(float x, float y, float width, float height)
		{
			mProjectionMatrix.setTo(2.0f / width, 0, 0, -2.0f / height, -(2 * x + width) / width, (2 * y + height) / height);
		}
		public virtual void loadIdentity()
		{
			mModelViewMatrix.identity();
		}
		public virtual void translateMatrix(float dx, float dy)
		{
			AsMatrixUtil.prependTranslation(mModelViewMatrix, dx, dy);
		}
		public virtual void rotateMatrix(float angle)
		{
			AsMatrixUtil.prependRotation(mModelViewMatrix, angle);
		}
		public virtual void scaleMatrix(float sx, float sy)
		{
			AsMatrixUtil.prependScale(mModelViewMatrix, sx, sy);
		}
		public virtual void prependMatrix(AsMatrix matrix)
		{
			AsMatrixUtil.prependMatrix(mModelViewMatrix, matrix);
		}
		public virtual void transformMatrix(AsDisplayObject _object)
		{
			AsMatrixUtil.prependMatrix(mModelViewMatrix, _object.getTransformationMatrix());
		}
		public virtual void pushMatrix()
		{
			if(mMatrixStack.getLength() < mMatrixStackSize + 1)
			{
				mMatrixStack.push(new AsMatrix());
			}
			mMatrixStack[mMatrixStackSize++].copyFrom(mModelViewMatrix);
		}
		public virtual void popMatrix()
		{
			mModelViewMatrix.copyFrom(mMatrixStack[--mMatrixStackSize]);
		}
		public virtual void resetMatrix()
		{
			mMatrixStackSize = 0;
			loadIdentity();
		}
		public static void transformMatrixForObject(AsMatrix matrix, AsDisplayObject _object)
		{
			AsMatrixUtil.prependMatrix(matrix, _object.getTransformationMatrix());
		}
		public virtual AsMatrix getMvpMatrix()
		{
			mMvpMatrix.copyFrom(mModelViewMatrix);
			mMvpMatrix.concat(mProjectionMatrix);
			return mMvpMatrix;
		}
		public virtual AsMatrix3D getMvpMatrix3D()
		{
			return AsMatrixUtil.convertTo3D(getMvpMatrix(), mMvpMatrix3D);
		}
		public virtual AsMatrix getModelViewMatrix()
		{
			return mModelViewMatrix;
		}
		public virtual AsMatrix getProjectionMatrix()
		{
			return mProjectionMatrix;
		}
		public virtual void applyBlendMode(bool premultipliedAlpha)
		{
			setBlendFactors(premultipliedAlpha, mBlendMode);
		}
		public virtual String getBlendMode()
		{
			return mBlendMode;
		}
		public virtual void setBlendMode(String _value)
		{
			if(_value != AsBlendMode.AUTO)
			{
				mBlendMode = _value;
			}
		}
		public virtual AsTexture getRenderTarget()
		{
			return mRenderTarget;
		}
		public virtual void setRenderTarget(AsTexture target)
		{
			mRenderTarget = target;
			if(target != null)
			{
				AsStarling.getContext().setRenderToTexture(target.get_base());
			}
			else
			{
				AsStarling.getContext().setRenderToBackBuffer();
			}
		}
		public virtual void batchQuad(AsQuad quad, float parentAlpha, AsTexture texture, String smoothing)
		{
			if(mQuadBatches[mCurrentQuadBatchID].isStateChange(quad.getTinted(), parentAlpha, texture, smoothing, mBlendMode))
			{
				finishQuadBatch();
			}
			mQuadBatches[mCurrentQuadBatchID].addQuad(quad, parentAlpha, texture, smoothing, mModelViewMatrix, mBlendMode);
		}
		public virtual void batchQuad(AsQuad quad, float parentAlpha, AsTexture texture)
		{
			batchQuad(quad, parentAlpha, texture, null);
		}
		public virtual void batchQuad(AsQuad quad, float parentAlpha)
		{
			batchQuad(quad, parentAlpha, null, null);
		}
		public virtual void finishQuadBatch()
		{
			AsQuadBatch currentBatch = mQuadBatches[mCurrentQuadBatchID];
			if(currentBatch.getNumQuads() != 0)
			{
				currentBatch.renderCustom(mProjectionMatrix);
				currentBatch.reset();
				++mCurrentQuadBatchID;
				++mDrawCount;
				if(mQuadBatches.getLength() <= mCurrentQuadBatchID)
				{
					mQuadBatches.push(new AsQuadBatch());
				}
			}
		}
		public virtual void nextFrame()
		{
			resetMatrix();
			mBlendMode = AsBlendMode.NORMAL;
			mCurrentQuadBatchID = 0;
			mDrawCount = 0;
		}
		public static void setDefaultBlendFactors(bool premultipliedAlpha)
		{
			setBlendFactors(premultipliedAlpha);
		}
		public static void setBlendFactors(bool premultipliedAlpha, String blendMode)
		{
			AsArray blendFactors = AsBlendMode.getBlendFactors(blendMode, premultipliedAlpha);
			AsStarling.getContext().setBlendFactors((String)(blendFactors[0]), (String)(blendFactors[1]));
		}
		public static void setBlendFactors(bool premultipliedAlpha)
		{
			setBlendFactors(premultipliedAlpha, "normal");
		}
		public static void clear(uint rgb, float alpha)
		{
			AsStarling.getContext().clear(AsColor.getRed(rgb) / 255.0f, AsColor.getGreen(rgb) / 255.0f, AsColor.getBlue(rgb) / 255.0f, alpha);
		}
		public static void clear(uint rgb)
		{
			clear(rgb, 0.0f);
		}
		public static void clear()
		{
			clear((uint)(0), 0.0f);
		}
		public virtual void clear(uint rgb, float alpha)
		{
			AsRenderSupport.clear(rgb, alpha);
		}
		public virtual void clear(uint rgb)
		{
			clear(rgb, 0.0f);
		}
		public virtual void clear()
		{
			clear((uint)(0), 0.0f);
		}
		public virtual void raiseDrawCount(uint _value)
		{
			mDrawCount = (int)(mDrawCount + _value);
		}
		public virtual void raiseDrawCount()
		{
			raiseDrawCount((uint)(1));
		}
		public virtual int getDrawCount()
		{
			return mDrawCount;
		}
	}
}
