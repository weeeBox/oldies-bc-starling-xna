using System;
 
using bc.flash;
using bc.flash.display3D;
using starling.filters;
using starling.textures;
 
namespace starling.filters
{
	public class AsInverseFilter : AsFragmentFilter
	{
		private AsProgram3D mShaderProgram;
		private AsVector<float> mOnes = new AsVector<float>();
		private AsVector<float> mMinColor = new AsVector<float>();
		public AsInverseFilter()
		 : base()
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
			String fragmentProgramCode = "tex ft0, v0, fs0 <2d, clamp, linear, mipnone>  \n" + "max ft0, ft0, fc1              \n" + "div ft0.xyz, ft0.xyz, ft0.www  \n" + "sub ft0.xyz, fc0.xyz, ft0.xyz  \n" + "mul ft0.xyz, ft0.xyz, ft0.www  \n" + "mov oc, ft0                    \n";
			mShaderProgram = assembleAgal(fragmentProgramCode);
		}
		protected override void activate(int pass, AsContext3D context, AsTexture texture)
		{
			context.setProgramConstantsFromVector(AsContext3DProgramType.FRAGMENT, 0, mOnes, 1);
			context.setProgramConstantsFromVector(AsContext3DProgramType.FRAGMENT, 1, mMinColor, 1);
			context.setProgram(mShaderProgram);
		}
	}
}
