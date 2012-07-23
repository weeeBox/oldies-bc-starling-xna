using System;
 
using bc.flash;
using bc.flash.display;
using com.foo;
 
namespace pg
{
	public class AsPlayGround : AsMovieClip
	{
		private AsStage stage1;
		private bc.flash.display.AsStage stage2;
		private float val = AsNumber.MAX_VALUE;
		public virtual void m()
		{
			String str = AsNumber.toFixed(val, 1);
		}
		public virtual bc.flash.display.AsStage foo(AsStage stage1, bc.flash.display.AsStage stage2)
		{
			return new com.foo.AsStage();
		}
		public virtual AsStage bar(AsStage stage1, bc.flash.display.AsStage stage2)
		{
			return new AsStage();
		}
	}
}
