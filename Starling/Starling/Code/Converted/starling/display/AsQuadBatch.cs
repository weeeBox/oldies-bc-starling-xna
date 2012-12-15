using System;
 
using bc.flash;
using bc.flash.display3D;
using bc.flash.geom;
using starling.core;
using starling.display;
using starling.errors;
using starling.events;
using starling.filters;
using starling.textures;
using starling.utils;
 
namespace starling.display
{
	public class AsQuadBatch : AsDisplayObject
	{
		private static String QUAD_PROGRAM_NAME = "QB_q";
		private int mNumQuads;
		private bool mSyncRequired;
		private bool mTinted;
		private AsTexture mTexture;
		private String mSmoothing;
		private AsVertexData mVertexData;
		private AsVertexBuffer3D mVertexBuffer;
		private AsVector<uint> mIndexData;
		private AsIndexBuffer3D mIndexBuffer;
		private static AsMatrix sHelperMatrix = new AsMatrix();
		private static AsVector<float> sRenderAlpha = new AsVector<float>();
		private static AsMatrix3D sRenderMatrix = new AsMatrix3D();
		private static AsDictionary sProgramNameCache = new AsDictionary();
		public AsQuadBatch()
		{
			mVertexData = new AsVertexData(0, true);
			mIndexData = new AsVector<uint>();
			mNumQuads = 0;
			mTinted = false;
			mSyncRequired = false;
			AsStarling.getCurrent().getStage3D().addEventListener(AsEvent.CONTEXT3D_CREATE, onContextCreated, false, 0, true);
		}
		public override void dispose()
		{
			AsStarling.getCurrent().getStage3D().removeEventListener(AsEvent.CONTEXT3D_CREATE, onContextCreated);
			if(mVertexBuffer != null)
			{
				mVertexBuffer.dispose();
			}
			if(mIndexBuffer != null)
			{
				mIndexBuffer.dispose();
			}
			base.dispose();
		}
		private void onContextCreated(Object _event)
		{
			createBuffers();
			registerPrograms();
		}
		public virtual AsQuadBatch clone()
		{
			AsQuadBatch clone = new AsQuadBatch();
			clone.mVertexData = mVertexData.clone(0, mNumQuads * 4);
			clone.mIndexData = mIndexData.slice(0, mNumQuads * 6);
			clone.mNumQuads = mNumQuads;
			clone.mTinted = mTinted;
			clone.mTexture = mTexture;
			clone.mSmoothing = mSmoothing;
			clone.mSyncRequired = true;
			clone.setBlendMode(getBlendMode());
			clone.setAlpha(getAlpha());
			return clone;
		}
		private void expand(int newCapacity)
		{
			int oldCapacity = getCapacity();
			if(newCapacity < 0)
			{
				newCapacity = oldCapacity * 2;
			}
			if(newCapacity == 0)
			{
				newCapacity = 16;
			}
			if(newCapacity <= oldCapacity)
			{
				return;
			}
			mVertexData.setNumVertices(newCapacity * 4);
			int i = oldCapacity;
			for (; i < newCapacity; ++i)
			{
				mIndexData[(i * 6)] = (uint)(i * 4);
				mIndexData[(i * 6 + 1)] = (uint)(i * 4 + 1);
				mIndexData[(i * 6 + 2)] = (uint)(i * 4 + 2);
				mIndexData[(i * 6 + 3)] = (uint)(i * 4 + 1);
				mIndexData[(i * 6 + 4)] = (uint)(i * 4 + 3);
				mIndexData[(i * 6 + 5)] = (uint)(i * 4 + 2);
			}
			createBuffers();
			registerPrograms();
		}
		private void expand()
		{
			expand(-1);
		}
		private void createBuffers()
		{
			int numVertices = mVertexData.getNumVertices();
			int numIndices = (int)(mIndexData.getLength());
			AsContext3D context = AsStarling.getContext();
			if(mVertexBuffer != null)
			{
				mVertexBuffer.dispose();
			}
			if(mIndexBuffer != null)
			{
				mIndexBuffer.dispose();
			}
			if(numVertices == 0)
			{
				return;
			}
			if(context == null)
			{
				throw new AsMissingContextError();
			}
			mVertexBuffer = context.createVertexBuffer(numVertices, AsVertexData.ELEMENTS_PER_VERTEX);
			mVertexBuffer.uploadFromVector(mVertexData.getRawData(), 0, numVertices);
			mIndexBuffer = context.createIndexBuffer(numIndices);
			mIndexBuffer.uploadFromVector(mIndexData, 0, numIndices);
			mSyncRequired = false;
		}
		private void syncBuffers()
		{
			if(mVertexBuffer == null)
			{
				createBuffers();
			}
			else
			{
				mVertexBuffer.uploadFromVector(mVertexData.getRawData(), 0, mVertexData.getNumVertices());
				mSyncRequired = false;
			}
		}
		public virtual void renderCustom(AsMatrix mvpMatrix, float parentAlpha, String blendMode)
		{
			if(mNumQuads == 0)
			{
				return;
			}
			if(mSyncRequired)
			{
				syncBuffers();
			}
			bool pma = mVertexData.getPremultipliedAlpha();
			AsContext3D context = AsStarling.getContext();
			bool tinted = mTinted || (parentAlpha != 1.0f);
			String programName = mTexture != null ? getImageProgramName(tinted, mTexture.getMipMapping(), mTexture.getRepeat(), mTexture.getFormat(), mSmoothing) : QUAD_PROGRAM_NAME;
			sRenderAlpha[0] = sRenderAlpha[1] = sRenderAlpha[2] = pma ? parentAlpha : 1.0f;
			sRenderAlpha[3] = parentAlpha;
			AsMatrixUtil.convertTo3D(mvpMatrix, sRenderMatrix);
			AsRenderSupport.setBlendFactors(pma, blendMode != null ? blendMode : this.getBlendMode());
			context.setProgram(AsStarling.getCurrent().getProgram(programName));
			context.setProgramConstantsFromVector(AsContext3DProgramType.VERTEX, 0, sRenderAlpha, 1);
			context.setProgramConstantsFromMatrix(AsContext3DProgramType.VERTEX, 1, sRenderMatrix, true);
			context.setVertexBufferAt(0, mVertexBuffer, AsVertexData.POSITION_OFFSET, AsContext3DVertexBufferFormat.FLOAT_2);
			if(mTexture == null || tinted)
			{
				context.setVertexBufferAt(1, mVertexBuffer, AsVertexData.COLOR_OFFSET, AsContext3DVertexBufferFormat.FLOAT_4);
			}
			if(mTexture != null)
			{
				context.setTextureAt(0, mTexture.get_base());
				context.setVertexBufferAt(2, mVertexBuffer, AsVertexData.TEXCOORD_OFFSET, AsContext3DVertexBufferFormat.FLOAT_2);
			}
			context.drawTriangles(mIndexBuffer, 0, mNumQuads * 2);
			if(mTexture != null)
			{
				context.setTextureAt(0, null);
				context.setVertexBufferAt(2, null);
			}
			context.setVertexBufferAt(1, null);
			context.setVertexBufferAt(0, null);
		}
		public virtual void renderCustom(AsMatrix mvpMatrix, float parentAlpha)
		{
			renderCustom(mvpMatrix, parentAlpha, null);
		}
		public virtual void renderCustom(AsMatrix mvpMatrix)
		{
			renderCustom(mvpMatrix, 1.0f, null);
		}
		public virtual void reset()
		{
			mNumQuads = 0;
			mTexture = null;
			mSmoothing = null;
			mSyncRequired = true;
		}
		public virtual void addImage(AsImage image, float parentAlpha, AsMatrix modelViewMatrix, String blendMode)
		{
			addQuad(image, parentAlpha, image.getTexture(), image.getSmoothing(), modelViewMatrix, blendMode);
		}
		public virtual void addImage(AsImage image, float parentAlpha, AsMatrix modelViewMatrix)
		{
			addImage(image, parentAlpha, modelViewMatrix, null);
		}
		public virtual void addImage(AsImage image, float parentAlpha)
		{
			addImage(image, parentAlpha, null, null);
		}
		public virtual void addImage(AsImage image)
		{
			addImage(image, 1.0f, null, null);
		}
		public virtual void addQuad(AsQuad quad, float parentAlpha, AsTexture texture, String smoothing, AsMatrix modelViewMatrix, String blendMode)
		{
			if(modelViewMatrix == null)
			{
				modelViewMatrix = quad.getTransformationMatrix();
			}
			bool tinted = texture != null ? quad.getTinted() || parentAlpha != 1.0f : false;
			float alpha = parentAlpha * quad.getAlpha();
			int vertexID = mNumQuads * 4;
			if(mNumQuads + 1 > mVertexData.getNumVertices() / 4)
			{
				expand();
			}
			if(mNumQuads == 0)
			{
				this.setBlendMode(blendMode != null ? blendMode : quad.getBlendMode());
				mTexture = texture;
				mTinted = tinted;
				mSmoothing = smoothing;
				mVertexData.setPremultipliedAlpha(texture != null ? texture.getPremultipliedAlpha() : true, false);
			}
			quad.copyVertexDataTo(mVertexData, vertexID);
			mVertexData.transformVertex(vertexID, modelViewMatrix, 4);
			if(alpha != 1.0f)
			{
				mVertexData.scaleAlpha(vertexID, alpha, 4);
			}
			mSyncRequired = true;
			mNumQuads++;
		}
		public virtual void addQuad(AsQuad quad, float parentAlpha, AsTexture texture, String smoothing, AsMatrix modelViewMatrix)
		{
			addQuad(quad, parentAlpha, texture, smoothing, modelViewMatrix, null);
		}
		public virtual void addQuad(AsQuad quad, float parentAlpha, AsTexture texture, String smoothing)
		{
			addQuad(quad, parentAlpha, texture, smoothing, null, null);
		}
		public virtual void addQuad(AsQuad quad, float parentAlpha, AsTexture texture)
		{
			addQuad(quad, parentAlpha, texture, null, null, null);
		}
		public virtual void addQuad(AsQuad quad, float parentAlpha)
		{
			addQuad(quad, parentAlpha, null, null, null, null);
		}
		public virtual void addQuad(AsQuad quad)
		{
			addQuad(quad, 1.0f, null, null, null, null);
		}
		public virtual void addQuadBatch(AsQuadBatch quadBatch, float parentAlpha, AsMatrix modelViewMatrix, String blendMode)
		{
			if(modelViewMatrix == null)
			{
				modelViewMatrix = quadBatch.getTransformationMatrix();
			}
			bool tinted = quadBatch.mTinted || parentAlpha != 1.0f;
			float alpha = parentAlpha * quadBatch.getAlpha();
			int vertexID = mNumQuads * 4;
			int numQuads = quadBatch.getNumQuads();
			if(mNumQuads + numQuads > getCapacity())
			{
				expand(mNumQuads + numQuads);
			}
			if(mNumQuads == 0)
			{
				this.setBlendMode(blendMode != null ? blendMode : quadBatch.getBlendMode());
				mTexture = quadBatch.mTexture;
				mTinted = tinted;
				mSmoothing = quadBatch.mSmoothing;
				mVertexData.setPremultipliedAlpha(quadBatch.mVertexData.getPremultipliedAlpha(), false);
			}
			quadBatch.mVertexData.copyTo(mVertexData, vertexID, 0, numQuads * 4);
			mVertexData.transformVertex(vertexID, modelViewMatrix, numQuads * 4);
			if(alpha != 1.0f)
			{
				mVertexData.scaleAlpha(vertexID, alpha, numQuads * 4);
			}
			mSyncRequired = true;
			mNumQuads = mNumQuads + numQuads;
		}
		public virtual void addQuadBatch(AsQuadBatch quadBatch, float parentAlpha, AsMatrix modelViewMatrix)
		{
			addQuadBatch(quadBatch, parentAlpha, modelViewMatrix, null);
		}
		public virtual void addQuadBatch(AsQuadBatch quadBatch, float parentAlpha)
		{
			addQuadBatch(quadBatch, parentAlpha, null, null);
		}
		public virtual void addQuadBatch(AsQuadBatch quadBatch)
		{
			addQuadBatch(quadBatch, 1.0f, null, null);
		}
		public virtual bool isStateChange(bool tinted, float parentAlpha, AsTexture texture, String smoothing, String blendMode, int numQuads)
		{
			if(mNumQuads == 0)
			{
				return false;
			}
			else
			{
				if(mNumQuads + numQuads > 8192)
				{
					return true;
				}
				else
				{
					if(mTexture == null && texture == null)
					{
						return false;
					}
					else
					{
						if(mTexture != null && texture != null)
						{
							return mTexture.get_base() != texture.get_base() || mTexture.getRepeat() != texture.getRepeat() || mSmoothing != smoothing || mTinted != (tinted || parentAlpha != 1.0f) || this.getBlendMode() != blendMode;
						}
						else
						{
							return true;
						}
					}
				}
			}
		}
		public virtual bool isStateChange(bool tinted, float parentAlpha, AsTexture texture, String smoothing, String blendMode)
		{
			return isStateChange(tinted, parentAlpha, texture, smoothing, blendMode, 1);
		}
		public override AsRectangle getBounds(AsDisplayObject targetSpace, AsRectangle resultRect)
		{
			if(resultRect == null)
			{
				resultRect = new AsRectangle();
			}
			AsMatrix transformationMatrix = targetSpace == this ? null : getTransformationMatrix(targetSpace, sHelperMatrix);
			return mVertexData.getBounds(transformationMatrix, 0, mNumQuads * 4, resultRect);
		}
		public virtual AsRectangle getBounds(AsDisplayObject targetSpace)
		{
			return getBounds(targetSpace, null);
		}
		public override void render(AsRenderSupport support, float parentAlpha)
		{
			support.finishQuadBatch();
			support.raiseDrawCount();
			renderCustom(support.getMvpMatrix(), getAlpha() * parentAlpha, support.getBlendMode());
		}
		public static void compile(AsDisplayObject _object, AsVector<AsQuadBatch> quadBatches)
		{
			compileObject(_object, quadBatches, -1, new AsMatrix());
		}
		private static int compileObject(AsDisplayObject _object, AsVector<AsQuadBatch> quadBatches, int quadBatchID, AsMatrix transformationMatrix, float alpha, String blendMode, bool ignoreCurrentFilter)
		{
			int i = 0;
			AsQuadBatch quadBatch = null;
			bool isRootObject = false;
			float objectAlpha = _object.getAlpha();
			AsDisplayObjectContainer container = _object as AsDisplayObjectContainer;
			AsQuad quad = _object as AsQuad;
			AsQuadBatch batch = _object as AsQuadBatch;
			AsFragmentFilter filter = _object.getFilter();
			if(quadBatchID == -1)
			{
				isRootObject = true;
				quadBatchID = 0;
				objectAlpha = 1.0f;
				blendMode = _object.getBlendMode();
				if(quadBatches.getLength() == 0)
				{
					quadBatches.push(new AsQuadBatch());
				}
				else
				{
					quadBatches[0].reset();
				}
			}
			if(filter != null && !ignoreCurrentFilter)
			{
				if(filter.getMode() == AsFragmentFilterMode.ABOVE)
				{
					quadBatchID = compileObject(_object, quadBatches, quadBatchID, transformationMatrix, alpha, blendMode, true);
				}
				quadBatchID = compileObject(filter.compile(_object), quadBatches, quadBatchID, transformationMatrix, alpha, blendMode);
				if(filter.getMode() == AsFragmentFilterMode.BELOW)
				{
					quadBatchID = compileObject(_object, quadBatches, quadBatchID, transformationMatrix, alpha, blendMode, true);
				}
			}
			else
			{
				if(container != null)
				{
					int numChildren = container.getNumChildren();
					AsMatrix childMatrix = new AsMatrix();
					for (i = 0; i < numChildren; ++i)
					{
						AsDisplayObject child = container.getChildAt(i);
						bool childVisible = child.getAlpha() != 0.0f && child.getVisible() && child.getScaleX() != 0.0f && child.getScaleY() != 0.0f;
						if(childVisible)
						{
							String childBlendMode = child.getBlendMode() == AsBlendMode.AUTO ? blendMode : child.getBlendMode();
							childMatrix.copyFrom(transformationMatrix);
							AsRenderSupport.transformMatrixForObject(childMatrix, child);
							quadBatchID = compileObject(child, quadBatches, quadBatchID, childMatrix, alpha * objectAlpha, childBlendMode);
						}
					}
				}
				else
				{
					if(quad != null || batch != null)
					{
						AsTexture texture = null;
						String smoothing = null;
						bool tinted = false;
						int numQuads = 0;
						if(quad != null)
						{
							AsImage image = quad as AsImage;
							texture = image != null ? image.getTexture() : null;
							smoothing = image != null ? image.getSmoothing() : null;
							tinted = quad.getTinted();
							numQuads = 1;
						}
						else
						{
							texture = batch.mTexture;
							smoothing = batch.mSmoothing;
							tinted = batch.mTinted;
							numQuads = batch.mNumQuads;
						}
						quadBatch = quadBatches[quadBatchID];
						if(quadBatch.isStateChange(tinted, alpha * objectAlpha, texture, smoothing, blendMode, numQuads))
						{
							quadBatchID++;
							if(quadBatches.getLength() <= quadBatchID)
							{
								quadBatches.push(new AsQuadBatch());
							}
							quadBatch = quadBatches[quadBatchID];
							quadBatch.reset();
						}
						if(quad != null)
						{
							quadBatch.addQuad(quad, alpha, texture, smoothing, transformationMatrix, blendMode);
						}
						else
						{
							quadBatch.addQuadBatch(batch, alpha, transformationMatrix, blendMode);
						}
					}
					else
					{
						throw new AsError("Unsupported display object: " + AsGlobal.getQualifiedClassName(_object));
					}
				}
			}
			if(isRootObject)
			{
				for (i = (int)(quadBatches.getLength() - 1); i > quadBatchID; --i)
				{
					quadBatches.pop().dispose();
				}
			}
			return quadBatchID;
		}
		private static int compileObject(AsDisplayObject _object, AsVector<AsQuadBatch> quadBatches, int quadBatchID, AsMatrix transformationMatrix, float alpha, String blendMode)
		{
			return compileObject(_object, quadBatches, quadBatchID, transformationMatrix, alpha, blendMode, false);
		}
		private static int compileObject(AsDisplayObject _object, AsVector<AsQuadBatch> quadBatches, int quadBatchID, AsMatrix transformationMatrix, float alpha)
		{
			return compileObject(_object, quadBatches, quadBatchID, transformationMatrix, alpha, null, false);
		}
		private static int compileObject(AsDisplayObject _object, AsVector<AsQuadBatch> quadBatches, int quadBatchID, AsMatrix transformationMatrix)
		{
			return compileObject(_object, quadBatches, quadBatchID, transformationMatrix, 1.0f, null, false);
		}
		public virtual int getNumQuads()
		{
			return mNumQuads;
		}
		public virtual bool getTinted()
		{
			return mTinted;
		}
		public virtual AsTexture getTexture()
		{
			return mTexture;
		}
		public virtual String getSmoothing()
		{
			return mSmoothing;
		}
		private int getCapacity()
		{
			return mVertexData.getNumVertices() / 4;
		}
		private static void registerPrograms()
		{
		}
		private static String getImageProgramName(bool tinted, bool mipMap, bool repeat, String format, String smoothing)
		{
			return null;
		}
		private static String getImageProgramName(bool tinted, bool mipMap, bool repeat, String format)
		{
			return getImageProgramName(tinted, mipMap, repeat, format, "bilinear");
		}
		private static String getImageProgramName(bool tinted, bool mipMap, bool repeat)
		{
			return getImageProgramName(tinted, mipMap, repeat, "bgra", "bilinear");
		}
		private static String getImageProgramName(bool tinted, bool mipMap)
		{
			return getImageProgramName(tinted, mipMap, false, "bgra", "bilinear");
		}
		private static String getImageProgramName(bool tinted)
		{
			return getImageProgramName(tinted, true, false, "bgra", "bilinear");
		}
	}
}
