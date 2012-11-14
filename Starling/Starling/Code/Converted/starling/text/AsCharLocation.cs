using System;
 
using bc.flash;
using starling.text;
 
namespace starling.text
{
	public class AsCharLocation : AsObject
	{
		public AsBitmapChar _char;
		public float scale;
		public float x;
		public float y;
		public AsCharLocation(AsBitmapChar _char)
		{
			this._char = _char;
		}
	}
}
