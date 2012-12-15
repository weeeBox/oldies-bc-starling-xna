using System;
 
using bc.flash;
using bc.flash.display3D;
using starling.filters;
using starling.textures;
 
namespace starling.filters
{
	public class AsColorMatrixFilter : AsFragmentFilter
	{
		private AsProgram3D mShaderProgram;
		private AsVector<float> mUserMatrix;
		private AsVector<float> mShaderMatrix;
		private static AsVector<float> MIN_COLOR = new AsVector<float>();
		private static AsArray IDENTITY = new AsArray(1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0);
		private static float LUMA_R = 0.299f;
		private static float LUMA_G = 0.587f;
		private static float LUMA_B = 0.114f;
		private static AsVector<float> sTmpMatrix1 = new AsVector<float>(20, true);
		private static AsVector<float> sTmpMatrix2 = new AsVector<float>();
		public AsColorMatrixFilter(AsVector<float> matrix)
		{
			mUserMatrix = new AsVector<float>();
			mShaderMatrix = new AsVector<float>();
			mUserMatrix(IDENTITY);
			updateShaderMatrix();
		}
		public AsColorMatrixFilter()
		 : this(null)
		{
		}
		public override void dispose()
		{
			if(mShaderProgram != null)
			{
				mShaderProgram.dispose();
			}
			base.dispose();
		}
		protected override void createPrograms()
		{
			String fragmentProgramCode = "tex ft0, v0,  fs0 <2d, clamp, linear, mipnone>  \n" + "max ft0, ft0, fc5              \n" + "div ft0.xyz, ft0.xyz, ft0.www  \n" + "m44 ft0, ft0, fc0              \n" + "add ft0, ft0, fc4              \n" + "mul ft0.xyz, ft0.xyz, ft0.www  \n" + "mov oc, ft0                    \n";
			mShaderProgram = assembleAgal(fragmentProgramCode);
		}
		protected override void activate(int pass, AsContext3D context, AsTexture texture)
		{
			context.setProgramConstantsFromVector(AsContext3DProgramType.FRAGMENT, 0, mShaderMatrix);
			context.setProgramConstantsFromVector(AsContext3DProgramType.FRAGMENT, 5, MIN_COLOR);
			context.setProgram(mShaderProgram);
		}
		public virtual void invert()
		{
			concatValues(-1, 0, 0, 0, 255, 0, -1, 0, 0, 255, 0, 0, -1, 0, 255, 0, 0, 0, 1, 0);
		}
		public virtual void adjustSaturation(float sat)
		{
			sat = sat + 1;
			float invSat = 1 - sat;
			float invLumR = invSat * LUMA_R;
			float invLumG = invSat * LUMA_G;
			float invLumB = invSat * LUMA_B;
			concatValues(invLumR + sat, invLumG, invLumB, 0, 0, invLumR, invLumG + sat, invLumB, 0, 0, invLumR, invLumG, invLumB + sat, 0, 0, 0, 0, 0, 1, 0);
		}
		public virtual void adjustContrast(float _value)
		{
			float s = _value + 1;
			float o = 128 * (1 - s);
			concatValues(s, 0, 0, 0, o, 0, s, 0, 0, o, 0, 0, s, 0, o, 0, 0, 0, 1, 0);
		}
		public virtual void adjustBrightness(float _value)
		{
			_value = _value * 255;
			concatValues(1, 0, 0, 0, _value, 0, 1, 0, 0, _value, 0, 0, 1, 0, _value, 0, 0, 0, 1, 0);
		}
		public virtual void adjustHue(float _value)
		{
			_value = _value * AsMath.PI;
			float cos = AsMath.cos(_value);
			float sin = AsMath.sin(_value);
			concatValues((LUMA_R + (cos * (1 - LUMA_R))) + (sin * -LUMA_R), (LUMA_G + (cos * -LUMA_G)) + (sin * -LUMA_G), (LUMA_B + (cos * -LUMA_B)) + (sin * (1 - LUMA_B)), 0, 0, (LUMA_R + (cos * -LUMA_R)) + (sin * 0.143f), (LUMA_G + (cos * (1 - LUMA_G))) + (sin * 0.14f), (LUMA_B + (cos * -LUMA_B)) + (sin * -0.283f), 0, 0, (LUMA_R + (cos * -LUMA_R)) + (sin * -1 - LUMA_R), (LUMA_G + (cos * -LUMA_G)) + (sin * LUMA_G), (LUMA_B + (cos * (1 - LUMA_B))) + (sin * LUMA_B), 0, 0, 0, 0, 0, 1, 0);
		}
		public virtual void reset()
		{
			setMatrix(null);
		}
		public virtual void concat(AsVector<float> matrix)
		{
			int i = 0;
			int y = 0;
			for (; y < 4; ++y)
			{
				int x = 0;
				for (; x < 5; ++x)
				{
					sTmpMatrix1[(i + x)] = matrix[i] * mUserMatrix[x] + matrix[(i + 1)] * mUserMatrix[(x + 5)] + matrix[(i + 2)] * mUserMatrix[(x + 10)] + matrix[(i + 3)] * mUserMatrix[(x + 15)] + x == 4 ? matrix[(i + 4)] : 0;
				}
				i = i + 5;
			}
			copyMatrix(sTmpMatrix1, mUserMatrix);
			updateShaderMatrix();
		}
		private void concatValues(float m0, float m1, float m2, float m3, float m4, float m5, float m6, float m7, float m8, float m9, float m10, float m11, float m12, float m13, float m14, float m15, float m16, float m17, float m18, float m19)
		{
			sTmpMatrix2.setLength(0);
			sTmpMatrix2.push(m0, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12, m13, m14, m15, m16, m17, m18, m19);
			concat(sTmpMatrix2);
		}
		private void copyMatrix(AsVector<float> _from, AsVector<float> to)
		{
			int i = 0;
			for (; i < 20; ++i)
			{
				to[i] = _from[i];
			}
		}
		private void updateShaderMatrix()
		{
			mShaderMatrix.setLength(0);
			mShaderMatrix.push(mUserMatrix[0], mUserMatrix[1], mUserMatrix[2], mUserMatrix[3], mUserMatrix[5], mUserMatrix[6], mUserMatrix[7], mUserMatrix[8], mUserMatrix[10], mUserMatrix[11], mUserMatrix[12], mUserMatrix[13], mUserMatrix[15], mUserMatrix[16], mUserMatrix[17], mUserMatrix[18], mUserMatrix[4] / 255.0f, mUserMatrix[9] / 255.0f, mUserMatrix[14] / 255.0f, mUserMatrix[19] / 255.0f);
		}
		public virtual AsVector<float> getMatrix()
		{
			return mUserMatrix;
		}
		public virtual void setMatrix(AsVector<float> _value)
		{
			if(_value != null && _value.getLength() != 20)
			{
				throw new AsArgumentError("Invalid matrix length: must be 20");
			}
			if(_value == null)
			{
				mUserMatrix.setLength(0);
				mUserMatrix(IDENTITY);
			}
			else
			{
				copyMatrix(_value, mUserMatrix);
			}
			updateShaderMatrix();
		}
	}
}
