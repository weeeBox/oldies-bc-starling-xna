using System;
 
using bc.flash;
using bc.flash.display3D;
using starling.filters;
using starling.textures;
 
namespace starling.filters
{
	public class AsGrayscaleFilter : AsFragmentFilter
	{
		private AsVector<float> mQuantifiers;
		private AsProgram3D mShaderProgram;
		public AsGrayscaleFilter(float red, float green, float blue)
		{
			mQuantifiers = new AsVector<float>();
		}
		public AsGrayscaleFilter(float red, float green)
		 : this(red, green, 0.114f)
		{
		}
		public AsGrayscaleFilter(float red)
		 : this(red, 0.587f, 0.114f)
		{
		}
		public AsGrayscaleFilter()
		 : this(0.299f, 0.587f, 0.114f)
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
			String fragmentProgramCode = "tex ft0, v0, fs0 <2d, clamp, linear, mipnone>  \n" + "dp3 ft0.xyz, ft0.xyz, fc0.xyz \n" + "mov oc, ft0                   \n";
			mShaderProgram = assembleAgal(fragmentProgramCode);
		}
		protected override void activate(int pass, AsContext3D context, AsTexture texture)
		{
			context.setProgramConstantsFromVector(AsContext3DProgramType.FRAGMENT, 0, mQuantifiers, 1);
			context.setProgram(mShaderProgram);
		}
	}
}
