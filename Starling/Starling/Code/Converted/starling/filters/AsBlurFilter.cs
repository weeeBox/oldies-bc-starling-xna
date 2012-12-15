using System;
 
using bc.flash;
using bc.flash.display3D;
using starling.filters;
using starling.textures;
using starling.utils;
 
namespace starling.filters
{
	public class AsBlurFilter : AsFragmentFilter
	{
		private static float MAX_SIGMA = 2.0f;
		private AsProgram3D mNormalProgram;
		private AsProgram3D mTintedProgram;
		private AsVector<float> mOffsets = new AsVector<float>();
		private AsVector<float> mWeights = new AsVector<float>();
		private AsVector<float> mColor = new AsVector<float>();
		private float mBlurX;
		private float mBlurY;
		private bool mUniformColor;
		private AsVector<float> sTmpWeights = new AsVector<float>(5, true);
		public AsBlurFilter(float blurX, float blurY, float resolution)
		 : base(1, resolution)
		{
			mBlurX = blurX;
			mBlurY = blurY;
			updateMarginsAndPasses();
		}
		public AsBlurFilter(float blurX, float blurY)
		 : this(blurX, blurY, 1)
		{
		}
		public AsBlurFilter(float blurX)
		 : this(blurX, 1, 1)
		{
		}
		public AsBlurFilter()
		 : this(1, 1, 1)
		{
		}
		public static AsBlurFilter createDropShadow(float distance, float angle, uint color, float alpha, float blur, float resolution)
		{
			AsBlurFilter dropShadow = new AsBlurFilter(blur, blur, resolution);
			dropShadow.setOffsetX(AsMath.cos(angle) * distance);
			dropShadow.setOffsetY(AsMath.sin(angle) * distance);
			dropShadow.setMode(AsFragmentFilterMode.BELOW);
			dropShadow.setUniformColor(true, color, alpha);
			return dropShadow;
		}
		public static AsBlurFilter createDropShadow(float distance, float angle, uint color, float alpha, float blur)
		{
			return createDropShadow(distance, angle, color, alpha, blur, 0.5f);
		}
		public static AsBlurFilter createDropShadow(float distance, float angle, uint color, float alpha)
		{
			return createDropShadow(distance, angle, color, alpha, 1.0f, 0.5f);
		}
		public static AsBlurFilter createDropShadow(float distance, float angle, uint color)
		{
			return createDropShadow(distance, angle, color, 0.5f, 1.0f, 0.5f);
		}
		public static AsBlurFilter createDropShadow(float distance, float angle)
		{
			return createDropShadow(distance, angle, (uint)(0x0), 0.5f, 1.0f, 0.5f);
		}
		public static AsBlurFilter createDropShadow(float distance)
		{
			return createDropShadow(distance, 0.785f, (uint)(0x0), 0.5f, 1.0f, 0.5f);
		}
		public static AsBlurFilter createDropShadow()
		{
			return createDropShadow(4.0f, 0.785f, (uint)(0x0), 0.5f, 1.0f, 0.5f);
		}
		public static AsBlurFilter createGlow(uint color, float alpha, float blur, float resolution)
		{
			AsBlurFilter glow = new AsBlurFilter(blur, blur, resolution);
			glow.setMode(AsFragmentFilterMode.BELOW);
			glow.setUniformColor(true, color, alpha);
			return glow;
		}
		public static AsBlurFilter createGlow(uint color, float alpha, float blur)
		{
			return createGlow(color, alpha, blur, 0.5f);
		}
		public static AsBlurFilter createGlow(uint color, float alpha)
		{
			return createGlow(color, alpha, 1.0f, 0.5f);
		}
		public static AsBlurFilter createGlow(uint color)
		{
			return createGlow(color, 1.0f, 1.0f, 0.5f);
		}
		public static AsBlurFilter createGlow()
		{
			return createGlow((uint)(0xffff00), 1.0f, 1.0f, 0.5f);
		}
		public override void dispose()
		{
			if(mNormalProgram != null)
			{
				mNormalProgram.dispose();
			}
			if(mTintedProgram != null)
			{
				mTintedProgram.dispose();
			}
			base.dispose();
		}
		protected override void createPrograms()
		{
			mNormalProgram = createProgram(false);
			mTintedProgram = createProgram(true);
		}
		private AsProgram3D createProgram(bool tinted)
		{
			String vertexProgramCode = "m44 op, va0, vc0       \n" + "mov v0, va1            \n" + "sub v1, va1, vc4.zwxx  \n" + "sub v2, va1, vc4.xyxx  \n" + "add v3, va1, vc4.xyxx  \n" + "add v4, va1, vc4.zwxx  \n";
			String fragmentProgramCode = "tex ft0,  v0, fs0 <2d, clamp, linear, mipnone> \n" + "mul ft5, ft0, fc0.xxxx                         \n" + "tex ft1,  v1, fs0 <2d, clamp, linear, mipnone> \n" + "mul ft1, ft1, fc0.zzzz                         \n" + "add ft5, ft5, ft1                              \n" + "tex ft2,  v2, fs0 <2d, clamp, linear, mipnone> \n" + "mul ft2, ft2, fc0.yyyy                         \n" + "add ft5, ft5, ft2                              \n" + "tex ft3,  v3, fs0 <2d, clamp, linear, mipnone> \n" + "mul ft3, ft3, fc0.yyyy                         \n" + "add ft5, ft5, ft3                              \n" + "tex ft4,  v4, fs0 <2d, clamp, linear, mipnone> \n" + "mul ft4, ft4, fc0.zzzz                         \n";
			if(tinted)
			{
				fragmentProgramCode = fragmentProgramCode + "add ft5, ft5, ft4                              \n" + "mul ft5.xyz, fc1.xyz, ft5.www                  \n" + "mul oc, ft5, fc1.wwww                          \n";
			}
			else
			{
				fragmentProgramCode = fragmentProgramCode + "add  oc, ft5, ft4                              \n";
			}
			return assembleAgal(fragmentProgramCode, vertexProgramCode);
		}
		protected override void activate(int pass, AsContext3D context, AsTexture texture)
		{
			updateParameters(pass, (int)(texture.getWidth() * texture.getScale()), (int)(texture.getHeight() * texture.getScale()));
			context.setProgramConstantsFromVector(AsContext3DProgramType.VERTEX, 4, mOffsets);
			context.setProgramConstantsFromVector(AsContext3DProgramType.FRAGMENT, 0, mWeights);
			if(mUniformColor && pass == getNumPasses() - 1)
			{
				context.setProgramConstantsFromVector(AsContext3DProgramType.FRAGMENT, 1, mColor);
				context.setProgram(mTintedProgram);
			}
			else
			{
				context.setProgram(mNormalProgram);
			}
		}
		private void updateParameters(int pass, int textureWidth, int textureHeight)
		{
			float sigma = 0;
			bool horizontal = pass < mBlurX;
			float pixelSize = 0;
			if(horizontal)
			{
				sigma = AsMath.min(1.0f, mBlurX - pass) * MAX_SIGMA;
				pixelSize = 1.0f / textureWidth;
			}
			else
			{
				sigma = AsMath.min(1.0f, mBlurY - (pass - AsMath.ceil(mBlurX))) * MAX_SIGMA;
				pixelSize = 1.0f / textureHeight;
			}
			float twoSigmaSq = 2 * sigma * sigma;
			float multiplier = 1.0f / AsMath.sqrt(twoSigmaSq * AsMath.PI);
			int i = 0;
			for (; i < 4; ++i)
			{
				sTmpWeights[i] = multiplier * AsMath.exp(-i * i / twoSigmaSq);
			}
			mWeights[0] = sTmpWeights[0];
			mWeights[1] = sTmpWeights[1] + sTmpWeights[2];
			mWeights[2] = sTmpWeights[3] + sTmpWeights[4];
			float weightSum = mWeights[0] + 2 * mWeights[1] + 2 * mWeights[2];
			float invWeightSum = 1.0f / weightSum;
			mWeights[0] = mWeights[0] * invWeightSum;
			mWeights[1] = mWeights[1] * invWeightSum;
			mWeights[2] = mWeights[2] * invWeightSum;
			float offset1 = (pixelSize * sTmpWeights[1] + 2 * pixelSize * sTmpWeights[2]) / mWeights[1];
			float offset2 = (3 * pixelSize * sTmpWeights[3] + 4 * pixelSize * sTmpWeights[4]) / mWeights[2];
			if(horizontal)
			{
				mOffsets[0] = offset1;
				mOffsets[1] = 0;
				mOffsets[2] = offset2;
				mOffsets[3] = 0;
			}
			else
			{
				mOffsets[0] = 0;
				mOffsets[1] = offset1;
				mOffsets[2] = 0;
				mOffsets[3] = offset2;
			}
		}
		private void updateMarginsAndPasses()
		{
			if(mBlurX == 0 && mBlurY == 0)
			{
				mBlurX = 0.001f;
			}
			setNumPasses(AsMath.ceil(mBlurX) + AsMath.ceil(mBlurY));
			setMarginX(4 + AsMath.ceil(mBlurX));
			setMarginY(4 + AsMath.ceil(mBlurY));
		}
		public virtual void setUniformColor(bool enable, uint color, float alpha)
		{
			mColor[0] = AsColor.getRed(color) / 255.0f;
			mColor[1] = AsColor.getGreen(color) / 255.0f;
			mColor[2] = AsColor.getBlue(color) / 255.0f;
			mColor[3] = alpha;
			mUniformColor = enable;
		}
		public virtual void setUniformColor(bool enable, uint color)
		{
			setUniformColor(enable, color, 1.0f);
		}
		public virtual void setUniformColor(bool enable)
		{
			setUniformColor(enable, (uint)(0x0), 1.0f);
		}
		public virtual float getBlurX()
		{
			return mBlurX;
		}
		public virtual void setBlurX(float _value)
		{
			mBlurX = _value;
			updateMarginsAndPasses();
		}
		public virtual float getBlurY()
		{
			return mBlurY;
		}
		public virtual void setBlurY(float _value)
		{
			mBlurY = _value;
			updateMarginsAndPasses();
		}
	}
}
