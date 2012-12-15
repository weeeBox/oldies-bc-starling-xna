using System;
 
using bc.flash;
using bc.flash.display3D;
using starling.filters;
using starling.textures;
 
namespace starling.filters
{
	public class AsIdentityFilter : AsFragmentFilter
	{
		private AsProgram3D mShaderProgram;
		public AsIdentityFilter()
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
			String fragmentProgramCode = "tex oc, v0, fs0 <2d, clamp, linear, mipnone>";
			mShaderProgram = assembleAgal(fragmentProgramCode);
		}
		protected override void activate(int pass, AsContext3D context, AsTexture texture)
		{
			context.setProgram(mShaderProgram);
		}
	}
}
