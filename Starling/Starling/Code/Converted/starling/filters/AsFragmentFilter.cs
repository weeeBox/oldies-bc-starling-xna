using System;
 
using bc.flash;
using bc.flash.display3D;
using bc.flash.errors;
using bc.flash.geom;
using bc.flash.system;
using starling.core;
using starling.display;
using starling.errors;
using starling.events;
using starling.filters;
using starling.textures;
using starling.utils;
using AsAbstractClassError = starling.errors.AsAbstractClassError;
 
namespace starling.filters
{
	public class AsFragmentFilter : AsObject
	{
		protected const bool PMA = true;
		protected static String STD_VERTEX_SHADER = "m44 op, va0, vc0 \n" + "mov v0, va1      \n";
		protected static String STD_FRAGMENT_SHADER = "tex oc, v0, fs0 <2d, clamp, linear, mipnone>";
		private int mNumPasses;
		private AsVector<AsTexture> mPassTextures;
		private String mMode;
		private float mResolution;
		private float mMarginX;
		private float mMarginY;
		private float mOffsetX;
		private float mOffsetY;
		private AsVertexData mVertexData;
		private AsVertexBuffer3D mVertexBuffer;
		private AsVector<uint> mIndexData;
		private AsIndexBuffer3D mIndexBuffer;
		private bool mCacheRequested;
		private AsQuadBatch mCache;
		private AsMatrix mProjMatrix = new AsMatrix();
		private static AsRectangle sBounds = new AsRectangle();
		private static AsMatrix sTransformationMatrix = new AsMatrix();
		public AsFragmentFilter(int numPasses, float resolution)
		{
			if(AsCapabilities.getIsDebugger() && AsGlobal.getQualifiedClassName(this) == "starling.filters::FragmentFilter")
			{
				throw new AsAbstractClassError();
			}
			if(numPasses < 1)
			{
				throw new AsArgumentError("At least one pass is required.");
			}
			mNumPasses = numPasses;
			mMarginX = mMarginY = 0.0f;
			mOffsetX = mOffsetY = 0;
			mResolution = resolution;
			mMode = AsFragmentFilterMode.REPLACE;
			mVertexData = new AsVertexData(4);
			mVertexData.setTexCoords(0, 0, 0);
			mVertexData.setTexCoords(1, 1, 0);
			mVertexData.setTexCoords(2, 0, 1);
			mVertexData.setTexCoords(3, 1, 1);
			mIndexData = new AsVector<uint>();
			mIndexData.setOwnProperty("_fixed", true);
			createPrograms();
			AsStarling.getCurrent().getStage3D().addEventListener(AsEvent.CONTEXT3D_CREATE, onContextCreated, false, 0, true);
		}
		public AsFragmentFilter(int numPasses)
		 : this(numPasses, 1.0f)
		{
		}
		public AsFragmentFilter()
		 : this(1, 1.0f)
		{
		}
		public virtual void dispose()
		{
			if(mVertexBuffer != null)
			{
				mVertexBuffer.dispose();
			}
			if(mIndexBuffer != null)
			{
				mIndexBuffer.dispose();
			}
			disposePassTextures();
			disposeCache();
		}
		private void onContextCreated(Object _event)
		{
			mVertexBuffer = null;
			mIndexBuffer = null;
			mPassTextures = null;
			createPrograms();
		}
		public virtual void render(AsDisplayObject _object, AsRenderSupport support, float parentAlpha)
		{
			if(getMode() == AsFragmentFilterMode.ABOVE)
			{
				_object.render(support, parentAlpha);
			}
			if(mCacheRequested)
			{
				mCacheRequested = false;
				mCache = renderPasses(_object, support, 1.0f, true);
				disposePassTextures();
			}
			if(mCache != null)
			{
				mCache.render(support, _object.getAlpha() * parentAlpha);
			}
			else
			{
				renderPasses(_object, support, parentAlpha, false);
			}
			if(getMode() == AsFragmentFilterMode.BELOW)
			{
				_object.render(support, parentAlpha);
			}
		}
		private AsQuadBatch renderPasses(AsDisplayObject _object, AsRenderSupport support, float parentAlpha, bool intoCache)
		{
			AsTexture cacheTexture = null;
			AsStage stage = _object.getStage();
			AsContext3D context = AsStarling.getContext();
			float scale = AsStarling.getCurrent().getContentScaleFactor();
			if(stage == null)
			{
				throw new AsError("Filtered object must be on the stage.");
			}
			if(context == null)
			{
				throw new AsMissingContextError();
			}
			support.finishQuadBatch();
			support.raiseDrawCount((uint)(mNumPasses));
			support.pushMatrix();
			support.setBlendMode(AsBlendMode.NORMAL);
			AsRenderSupport.setBlendFactors(PMA);
			mProjMatrix.copyFrom(support.getProjectionMatrix());
			AsTexture previousRenderTarget = support.getRenderTarget();
			if(previousRenderTarget != null)
			{
				throw new AsIllegalOperationError("It's currently not possible to stack filters! " + "This limitation will be removed in a future Stage3D version.");
			}
			calculateBounds(_object, stage, sBounds);
			updateBuffers(context, sBounds);
			updatePassTextures((int)(sBounds.width), (int)(sBounds.height), mResolution * scale);
			if(intoCache)
			{
				cacheTexture = AsTexture.empty((int)(sBounds.width), (int)(sBounds.height), PMA, true, mResolution * scale);
			}
			support.setRenderTarget(mPassTextures[0]);
			support.clear();
			support.setOrthographicProjection(sBounds.x, sBounds.y, sBounds.width, sBounds.height);
			_object.render(support, parentAlpha);
			support.finishQuadBatch();
			AsRenderSupport.setBlendFactors(PMA);
			support.loadIdentity();
			context.setVertexBufferAt(0, mVertexBuffer, AsVertexData.POSITION_OFFSET, AsContext3DVertexBufferFormat.FLOAT_2);
			context.setVertexBufferAt(1, mVertexBuffer, AsVertexData.TEXCOORD_OFFSET, AsContext3DVertexBufferFormat.FLOAT_2);
			int i = 0;
			for (; i < mNumPasses; ++i)
			{
				if(i < mNumPasses - 1)
				{
					support.setRenderTarget(getPassTexture(i + 1));
					support.clear();
				}
				else
				{
					if(intoCache)
					{
						support.setRenderTarget(cacheTexture);
						support.clear();
					}
					else
					{
						support.setRenderTarget(previousRenderTarget);
						support.getProjectionMatrix().copyFrom(mProjMatrix);
						support.translateMatrix(mOffsetX, mOffsetY);
						support.setBlendMode(_object.getBlendMode());
						support.applyBlendMode(PMA);
					}
				}
				AsTexture passTexture = getPassTexture(i);
				context.setProgramConstantsFromMatrix(AsContext3DProgramType.VERTEX, 0, support.getMvpMatrix3D(), true);
				context.setTextureAt(0, passTexture.get_base());
				activate(i, context, passTexture);
				context.drawTriangles(mIndexBuffer, 0, 2);
				deactivate(i, context, passTexture);
			}
			context.setVertexBufferAt(0, null);
			context.setVertexBufferAt(1, null);
			context.setTextureAt(0, null);
			support.popMatrix();
			if(intoCache)
			{
				support.setRenderTarget(previousRenderTarget);
				support.getProjectionMatrix().copyFrom(mProjMatrix);
				AsQuadBatch quadBatch = new AsQuadBatch();
				AsImage image = new AsImage(cacheTexture);
				stage.getTransformationMatrix(_object, sTransformationMatrix);
				AsMatrixUtil.prependTranslation(sTransformationMatrix, sBounds.x + mOffsetX, sBounds.y + mOffsetY);
				quadBatch.addImage(image, 1.0f, sTransformationMatrix);
				return quadBatch;
			}
			else
			{
				return null;
			}
		}
		private AsQuadBatch renderPasses(AsDisplayObject _object, AsRenderSupport support, float parentAlpha)
		{
			return renderPasses(_object, support, parentAlpha, false);
		}
		private void updateBuffers(AsContext3D context, AsRectangle bounds)
		{
			mVertexData.setPosition(0, bounds.x, bounds.y);
			mVertexData.setPosition(1, bounds.getRight(), bounds.y);
			mVertexData.setPosition(2, bounds.x, bounds.getBottom());
			mVertexData.setPosition(3, bounds.getRight(), bounds.getBottom());
			if(mVertexBuffer == null)
			{
				mVertexBuffer = context.createVertexBuffer(4, AsVertexData.ELEMENTS_PER_VERTEX);
				mIndexBuffer = context.createIndexBuffer(6);
				mIndexBuffer.uploadFromVector(mIndexData, 0, 6);
			}
			mVertexBuffer.uploadFromVector(mVertexData.getRawData(), 0, 4);
		}
		private void updatePassTextures(int width, int height, float scale)
		{
			int numPassTextures = mNumPasses > 1 ? 2 : 1;
			bool needsUpdate = mPassTextures == null || mPassTextures.getLength() != numPassTextures || mPassTextures[0].getWidth() != width || mPassTextures[0].getHeight() != height;
			if(needsUpdate)
			{
				if(mPassTextures != null)
				{
					AsVector<AsTexture> __textures_ = mPassTextures;
					if (__textures_ != null)
					{
						foreach (AsTexture texture in __textures_)
						{
							texture.dispose();
						}
					}
					mPassTextures.setLength(numPassTextures);
				}
				else
				{
					mPassTextures = new AsVector<AsTexture>(numPassTextures);
				}
				int i = 0;
				for (; i < numPassTextures; ++i)
				{
					mPassTextures[i] = AsTexture.empty(width, height, PMA, true, scale);
				}
			}
		}
		private AsTexture getPassTexture(int pass)
		{
			return mPassTextures[pass % 2];
		}
		private void calculateBounds(AsDisplayObject _object, AsStage stage, AsRectangle resultRect)
		{
			if(_object == stage || _object == AsStarling.getCurrent().getRoot())
			{
				resultRect.setTo(0, 0, stage.getStageWidth(), stage.getStageHeight());
			}
			else
			{
				_object.getBounds(stage, resultRect);
			}
			float deltaMargin = mResolution == 1.0f ? 0.0f : 1.0f / mResolution;
			resultRect.x = resultRect.x - mMarginX + deltaMargin;
			resultRect.y = resultRect.y - mMarginY + deltaMargin;
			resultRect.width = resultRect.width + 2 * (mMarginX + deltaMargin);
			resultRect.height = resultRect.height + 2 * (mMarginY + deltaMargin);
			resultRect.width = AsGlobal.getNextPowerOfTwo((int)(resultRect.width * mResolution)) / mResolution;
			resultRect.height = AsGlobal.getNextPowerOfTwo((int)(resultRect.height * mResolution)) / mResolution;
		}
		private void disposePassTextures()
		{
			AsVector<AsTexture> __textures_ = mPassTextures;
			if (__textures_ != null)
			{
				foreach (AsTexture texture in __textures_)
				{
					texture.dispose();
				}
			}
			mPassTextures = null;
		}
		private void disposeCache()
		{
			if(mCache != null)
			{
				mCache.getTexture().dispose();
				mCache.dispose();
				mCache = null;
			}
		}
		protected virtual void createPrograms()
		{
			throw new AsError("Method has to be implemented in subclass!");
		}
		protected virtual void activate(int pass, AsContext3D context, AsTexture texture)
		{
			throw new AsError("Method has to be implemented in subclass!");
		}
		protected virtual void deactivate(int pass, AsContext3D context, AsTexture texture)
		{
		}
		protected virtual AsProgram3D assembleAgal(String fragmentShader, String vertexShader)
		{
			return null;
		}
		protected virtual AsProgram3D assembleAgal(String fragmentShader)
		{
			return assembleAgal(fragmentShader, null);
		}
		protected virtual AsProgram3D assembleAgal()
		{
			return assembleAgal(null, null);
		}
		public virtual void cache()
		{
			mCacheRequested = true;
			disposeCache();
		}
		public virtual void clearCache()
		{
			mCacheRequested = false;
			disposeCache();
		}
		public virtual AsQuadBatch compile(AsDisplayObject _object)
		{
			if(mCache != null)
			{
				return mCache;
			}
			else
			{
				AsRenderSupport renderSupport = null;
				AsStage stage = _object.getStage();
				if(stage == null)
				{
					throw new AsError("Filtered object must be on the stage.");
				}
				renderSupport = new AsRenderSupport();
				_object.getTransformationMatrix(stage, renderSupport.getModelViewMatrix());
				return renderPasses(_object, renderSupport, 1.0f, true);
			}
		}
		public virtual bool getIsCached()
		{
			return mCache != null || mCacheRequested;
		}
		public virtual float getResolution()
		{
			return mResolution;
		}
		public virtual void setResolution(float _value)
		{
			if(_value <= 0)
			{
				throw new AsArgumentError("Resolution must be > 0");
			}
			else
			{
				mResolution = _value;
			}
		}
		public virtual String getMode()
		{
			return mMode;
		}
		public virtual void setMode(String _value)
		{
			mMode = _value;
		}
		public virtual float getOffsetX()
		{
			return mOffsetX;
		}
		public virtual void setOffsetX(float _value)
		{
			mOffsetX = _value;
		}
		public virtual float getOffsetY()
		{
			return mOffsetY;
		}
		public virtual void setOffsetY(float _value)
		{
			mOffsetY = _value;
		}
		protected virtual float getMarginX()
		{
			return mMarginX;
		}
		protected virtual void setMarginX(float _value)
		{
			mMarginX = _value;
		}
		protected virtual float getMarginY()
		{
			return mMarginY;
		}
		protected virtual void setMarginY(float _value)
		{
			mMarginY = _value;
		}
		protected virtual void setNumPasses(int _value)
		{
			mNumPasses = _value;
		}
		protected virtual int getNumPasses()
		{
			return mNumPasses;
		}
	}
}
