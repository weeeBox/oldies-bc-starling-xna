using System;
 
using bc.flash;
using bc.flash.error;
using bc.flash.geom;
 
namespace bc.flash.geom
{
	public class AsColorTransform : AsObject
	{
		public float redMultiplier;
		public float greenMultiplier;
		public float blueMultiplier;
		public float alphaMultiplier;
		public float redOffset;
		public float greenOffset;
		public float blueOffset;
		public float alphaOffset;
		public AsColorTransform(float redMultiplier, float greenMultiplier, float blueMultiplier, float alphaMultiplier, float redOffset, float greenOffset, float blueOffset, float alphaOffset)
		{
			this.redMultiplier = redMultiplier;
			this.greenMultiplier = greenMultiplier;
			this.blueMultiplier = blueMultiplier;
			this.alphaMultiplier = alphaMultiplier;
			this.redOffset = redOffset;
			this.greenOffset = greenOffset;
			this.blueOffset = blueOffset;
			this.alphaOffset = alphaOffset;
		}
		public AsColorTransform(float redMultiplier, float greenMultiplier, float blueMultiplier, float alphaMultiplier, float redOffset, float greenOffset, float blueOffset)
		 : this(redMultiplier, greenMultiplier, blueMultiplier, alphaMultiplier, redOffset, greenOffset, blueOffset, 0)
		{
		}
		public AsColorTransform(float redMultiplier, float greenMultiplier, float blueMultiplier, float alphaMultiplier, float redOffset, float greenOffset)
		 : this(redMultiplier, greenMultiplier, blueMultiplier, alphaMultiplier, redOffset, greenOffset, 0, 0)
		{
		}
		public AsColorTransform(float redMultiplier, float greenMultiplier, float blueMultiplier, float alphaMultiplier, float redOffset)
		 : this(redMultiplier, greenMultiplier, blueMultiplier, alphaMultiplier, redOffset, 0, 0, 0)
		{
		}
		public AsColorTransform(float redMultiplier, float greenMultiplier, float blueMultiplier, float alphaMultiplier)
		 : this(redMultiplier, greenMultiplier, blueMultiplier, alphaMultiplier, 0, 0, 0, 0)
		{
		}
		public AsColorTransform(float redMultiplier, float greenMultiplier, float blueMultiplier)
		 : this(redMultiplier, greenMultiplier, blueMultiplier, 1.0f, 0, 0, 0, 0)
		{
		}
		public AsColorTransform(float redMultiplier, float greenMultiplier)
		 : this(redMultiplier, greenMultiplier, 1.0f, 1.0f, 0, 0, 0, 0)
		{
		}
		public AsColorTransform(float redMultiplier)
		 : this(redMultiplier, 1.0f, 1.0f, 1.0f, 0, 0, 0, 0)
		{
		}
		public AsColorTransform()
		 : this(1.0f, 1.0f, 1.0f, 1.0f, 0, 0, 0, 0)
		{
		}
		public virtual uint getColor()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setColor(uint newColor)
		{
			throw new AsNotImplementedError();
		}
		public virtual void concat(AsColorTransform second)
		{
			throw new AsNotImplementedError();
		}
	}
}
