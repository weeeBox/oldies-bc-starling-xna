using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.errors;
using bc.flash.geom;
 
namespace bc.flash.geom
{
	public class AsTransform : AsObject
	{
		private AsDisplayObject mDisplayObject;
		private AsColorTransform mColorTransform;
		private AsMatrix mMatrix;
		private AsMatrix3D mMatrix3D;
		public AsTransform(AsDisplayObject displayObject)
		{
			mDisplayObject = displayObject;
			mColorTransform = new AsColorTransform();
			mMatrix = new AsMatrix();
		}
		public virtual AsColorTransform getColorTransform()
		{
			return mColorTransform;
		}
		public virtual void setColorTransform(AsColorTransform _value)
		{
			mColorTransform = _value;
		}
		public virtual AsColorTransform getConcatenatedColorTransform()
		{
			throw new AsNotImplementedError();
		}
		public virtual AsMatrix getConcatenatedMatrix()
		{
			throw new AsNotImplementedError();
		}
		public virtual AsMatrix getMatrix()
		{
			mMatrix = mDisplayObject.getTransformationMatrix(mDisplayObject.getParent(), mMatrix);
			return mMatrix;
		}
		public virtual void setMatrix(AsMatrix _value)
		{
			mMatrix = _value;
		}
		public virtual AsMatrix3D getMatrix3D()
		{
			return mMatrix3D;
		}
		public virtual void setMatrix3D(AsMatrix3D m)
		{
			mMatrix3D = m;
		}
	}
}
