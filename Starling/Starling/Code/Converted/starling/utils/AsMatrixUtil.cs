using System;
 
using bc.flash;
using bc.flash.geom;
using starling.errors;
 
namespace starling.utils
{
	public class AsMatrixUtil : AsObject
	{
		private static AsVector<float> sRawData = new AsVector<float>();
		public AsMatrixUtil()
		{
			throw new AsAbstractClassError();
		}
		public static AsMatrix3D convertTo3D(AsMatrix matrix, AsMatrix3D resultMatrix)
		{
			if((resultMatrix == null))
			{
				resultMatrix = new AsMatrix3D();
			}
			sRawData[0] = matrix.a;
			sRawData[1] = matrix.b;
			sRawData[4] = matrix.c;
			sRawData[5] = matrix.d;
			sRawData[12] = matrix.tx;
			sRawData[13] = matrix.ty;
			resultMatrix.copyRawDataFrom(sRawData);
			return resultMatrix;
		}
		public static AsMatrix3D convertTo3D(AsMatrix matrix)
		{
			return convertTo3D(matrix, null);
		}
		public static AsPoint transformCoords(AsMatrix matrix, float x, float y, AsPoint resultPoint)
		{
			if((resultPoint == null))
			{
				resultPoint = new AsPoint();
			}
			resultPoint.x = (((matrix.a * x) + (matrix.c * y)) + matrix.tx);
			resultPoint.y = (((matrix.d * y) + (matrix.b * x)) + matrix.ty);
			return resultPoint;
		}
		public static AsPoint transformCoords(AsMatrix matrix, float x, float y)
		{
			return transformCoords(matrix, x, y, null);
		}
		public static void prependTranslation(AsMatrix matrix, float tx, float ty)
		{
			matrix.tx = (matrix.tx + ((matrix.a * tx) + (matrix.c * ty)));
			matrix.ty = (matrix.ty + ((matrix.b * tx) + (matrix.d * ty)));
		}
		public static void prependScale(AsMatrix matrix, float sx, float sy)
		{
			matrix.setTo((matrix.a * sx), (matrix.b * sx), (matrix.c * sy), (matrix.d * sy), matrix.tx, matrix.ty);
		}
		public static void prependRotation(AsMatrix matrix, float angle)
		{
			float sin = AsMath.sin(angle);
			float cos = AsMath.cos(angle);
			matrix.setTo(((matrix.a * cos) + (matrix.c * sin)), ((matrix.b * cos) + (matrix.d * sin)), ((matrix.c * cos) - (matrix.a * sin)), ((matrix.d * cos) - (matrix.b * sin)), matrix.tx, matrix.ty);
		}
	}
}
