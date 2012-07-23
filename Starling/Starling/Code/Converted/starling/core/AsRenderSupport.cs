using System;
 
using bc.flash;
using bc.flash.geom;
using starling.core;
using starling.display;
using starling.events;
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
		private String mBlendMode;
		private AsVector<String> mBlendModeStack;
		private AsVector<AsQuadBatch> mQuadBatches;
		private int mCurrentQuadBatchID;
		private static AsMatrix sHelperMatrix = new AsMatrix();
		public AsRenderSupport()
		{
			mProjectionMatrix = new AsMatrix();
			mModelViewMatrix = new AsMatrix();
			mMvpMatrix = new AsMatrix();
			mMvpMatrix3D = new AsMatrix3D();
			mMatrixStack = new AsVector<AsMatrix>();
			mMatrixStackSize = 0;
			mBlendMode = AsBlendMode.NORMAL;
			mBlendModeStack = new AsVector<String>();
			mCurrentQuadBatchID = 0;
			mQuadBatches = new AsVector<AsQuadBatch>();
			loadIdentity();
			setOrthographicProjection(400, 300);
			AsStarling.getCurrent().addEventListener(AsEvent.CONTEXT3D_CREATE, onContextCreated);
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
			AsStarling.getCurrent().removeEventListener(AsEvent.CONTEXT3D_CREATE, onContextCreated);
		}
		private void onContextCreated(AsEvent _event)
		{
			mQuadBatches = new AsVector<AsQuadBatch>();
		}
		public virtual void setOrthographicProjection(float width, float height)
		{
			mProjectionMatrix.setTo((2.0f / width), 0, 0, (-2.0f / height), -1.0f, 1.0f);
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
		public virtual void transformMatrix(AsDisplayObject _object)
		{
			transformMatrixForObject(mModelViewMatrix, _object);
		}
		public virtual void pushMatrix()
		{
			if((mMatrixStack.getLength() < (mMatrixStackSize + 1)))
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
		public static void transformMatrixForObject(AsMatrix matrix, AsDisplayObject _object)
		{
			sHelperMatrix.copyFrom(_object.getTransformationMatrix());
			sHelperMatrix.concat(matrix);
			matrix.copyFrom(sHelperMatrix);
		}
		public virtual void pushBlendMode()
		{
			mBlendModeStack.push(mBlendMode);
		}
		public virtual void popBlendMode()
		{
			mBlendMode = (String)(mBlendModeStack.pop());
		}
		public virtual void resetBlendMode()
		{
			mBlendModeStack.setLength(0);
			mBlendMode = AsBlendMode.NORMAL;
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
			if((_value != AsBlendMode.AUTO))
			{
				mBlendMode = _value;
			}
		}
		public virtual void batchQuad(AsQuad quad, float parentAlpha, AsTexture texture, String smoothing)
		{
			if(getCurrentQuadBatch().isStateChange(quad.getTinted(), parentAlpha, texture, smoothing, mBlendMode))
			{
				finishQuadBatch();
			}
			getCurrentQuadBatch().addQuad(quad, parentAlpha, texture, smoothing, mModelViewMatrix, mBlendMode);
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
			if((getCurrentQuadBatch().getNumQuads() != 0))
			{
				getCurrentQuadBatch().renderCustom(mProjectionMatrix);
				getCurrentQuadBatch().reset();
				++mCurrentQuadBatchID;
				if((mQuadBatches.getLength() <= mCurrentQuadBatchID))
				{
					mQuadBatches.push(new AsQuadBatch());
				}
			}
		}
		public virtual void nextFrame()
		{
			resetMatrix();
			resetBlendMode();
			mCurrentQuadBatchID = 0;
		}
		private AsQuadBatch getCurrentQuadBatch()
		{
			return mQuadBatches[mCurrentQuadBatchID];
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
			AsStarling.getContext().clear((AsColor.getRed(rgb) / 255.0f), (AsColor.getGreen(rgb) / 255.0f), (AsColor.getBlue(rgb) / 255.0f), alpha);
		}
		public static void clear(uint rgb)
		{
			clear(rgb, 0.0f);
		}
		public static void clear()
		{
			clear((uint)(0), 0.0f);
		}
	}
}
