using System;

using bc.flash;
using bc.flash.error;
using bc.flash.geom;
using Microsoft.Xna.Framework;

namespace bc.flash.geom
{
    public sealed class AsMatrix3D : AsObject
    {
        public Matrix internalMatrix;

        public AsMatrix3D(AsVector<float> v)
        {
            internalMatrix.M11 = v[0];
            internalMatrix.M12 = v[1];
            internalMatrix.M13 = v[2];
            internalMatrix.M14 = 0;

            internalMatrix.M21 = v[4];
            internalMatrix.M22 = v[5];
            internalMatrix.M23 = v[6];
            internalMatrix.M24 = 0;

            internalMatrix.M31 = v[8];
            internalMatrix.M32 = v[9];
            internalMatrix.M33 = v[10];
            internalMatrix.M34 = 0;

            internalMatrix.M41 = v[12];
            internalMatrix.M42 = v[13];
            internalMatrix.M43 = v[14];
            internalMatrix.M44 = 1;
        }

        public AsMatrix3D()
        {
            internalMatrix = Matrix.Identity;
        }

        private AsMatrix3D(ref Matrix matrix)
        {
            internalMatrix = matrix;
        }

        public void append(AsMatrix3D lhs)
        {
            append(ref lhs.internalMatrix);
        }

        private void append(ref Matrix matrix)
        {
            Matrix oldMatrix = internalMatrix;
            Matrix.Multiply(ref oldMatrix, ref matrix, out internalMatrix);
        }

        public void appendRotation(float degrees, AsVector3D axis, AsVector3D pivotPoint)
        {
            bool hasPivot = ((pivotPoint != null) && (((pivotPoint.x != 0.0f) || (pivotPoint.y != 0.0f)) || (pivotPoint.z != 0.0f)));
            if (hasPivot)
            {
                appendTranslation(-pivotPoint.x, -pivotPoint.y, -pivotPoint.z);
            }

            float radians = ((0.0055555555555556f * degrees) * AsMath.PI);
            float ax = axis.x;
            float ay = axis.y;
            float az = axis.z;
            if ((((ax == 0.0f) && (ay == 0.0f)) && (az == 1.0f)))
            {
                appendRotationZ(radians);
            }
            else
            {
                if ((((ax == 0.0f) && (ay == 1.0f)) && (az == 0.0f)))
                {
                    appendRotationY(radians);
                }
                else
                {
                    if ((((ax == 1.0f) && (ay == 0.0f)) && (az == 0.0f)))
                    {
                        appendRotationX(radians);
                    }
                    else
                    {
                        throw new AsNotImplementedError();
                    }
                }
            }
            if (hasPivot)
            {
                appendTranslation(pivotPoint.x, pivotPoint.y, pivotPoint.z);
            }
        }

        public void appendRotation(float degrees, AsVector3D axis)
        {
            appendRotation(degrees, axis, null);
        }

        private void appendRotationX(float radians)
        {
            Matrix rotationMatrix = Matrix.CreateRotationX(radians);
            append(ref rotationMatrix);
        }

        private void appendRotationY(float radians)
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(radians);
            append(ref rotationMatrix);
        }

        private void appendRotationZ(float radians)
        {
            Matrix rotationMatrix = Matrix.CreateRotationZ(radians);
            append(ref rotationMatrix);
        }

        public void appendScale(float xScale, float yScale, float zScale)
        {
            Matrix scaleMatrix = Matrix.CreateScale(xScale, yScale, zScale);
            append(ref scaleMatrix);
        }

        public void appendTranslation(float x, float y, float z)
        {
            Matrix translationMatrix = Matrix.CreateTranslation(x, y, z);
            append(ref translationMatrix);
        }

        public AsMatrix3D clone()
        {
            return new AsMatrix3D(ref internalMatrix);
        }

        public void copyColumnFrom(uint column, AsVector3D vector3D)
        {
            throw new NotImplementedException();
        }

        public void copyColumnTo(uint column, AsVector3D vector3D)
        {
            throw new NotImplementedException();
        }

        public void copyFrom(AsMatrix3D sourceMatrix3D)
        {
            internalMatrix = sourceMatrix3D.internalMatrix;
        }

        public void copyRawDataFrom(AsVector<float> vector, uint index, bool transpose)
        {
            throw new NotImplementedException();
        }

        public void copyRawDataFrom(AsVector<float> vector, uint index)
        {
            copyRawDataFrom(vector, index, false);
        }

        public void copyRawDataFrom(AsVector<float> vector)
        {
            copyRawDataFrom(vector, (uint)(0), false);
        }

        public void copyRawDataTo(AsVector<float> vector, uint index, bool transpose)
        {
            throw new NotImplementedException();
        }

        public void copyRawDataTo(AsVector<float> vector, uint index)
        {
            copyRawDataTo(vector, index, false);
        }

        public void copyRawDataTo(AsVector<float> vector)
        {
            copyRawDataTo(vector, (uint)(0), false);
        }

        public void copyRowFrom(uint row, AsVector3D vector3D)
        {
            throw new NotImplementedException();
        }

        public void copyRowTo(uint row, AsVector3D vector3D)
        {
            throw new NotImplementedException();
        }

        public void copyToMatrix3D(AsMatrix3D dest)
        {
            dest.internalMatrix = internalMatrix;
        }

        public AsVector<AsVector3D> decompose(String orientationStyle)
        {
            throw new AsNotImplementedError();
        }

        public AsVector<AsVector3D> decompose()
        {
            return decompose("eulerAngles");
        }

        public AsVector3D deltaTransformVector(AsVector3D v)
        {
            float nx = v.x * internalMatrix.M11 + v.y * internalMatrix.M21 + v.z * internalMatrix.M31;
            float ny = v.x * internalMatrix.M12 + v.y * internalMatrix.M22 + v.z * internalMatrix.M32;
            float nz = v.x * internalMatrix.M13 + v.y * internalMatrix.M23 + v.z * internalMatrix.M33;
            return new AsVector3D(nx, ny, nz);
        }

        public void identity()
        {
            internalMatrix = Matrix.Identity;
        }

        public static AsMatrix3D interpolate(AsMatrix3D thisMat, AsMatrix3D toMat, float percent)
        {
            throw new AsNotImplementedError();
        }

        public void interpolateTo(AsMatrix3D toMat, float percent)
        {
            throw new AsNotImplementedError();
        }

        public bool invert()
        {
            throw new AsNotImplementedError();
        }

        public void pointAt(AsVector3D pos, AsVector3D at, AsVector3D up)
        {
            throw new AsNotImplementedError();
        }

        public void pointAt(AsVector3D pos, AsVector3D at)
        {
            pointAt(pos, at, null);
        }

        public void pointAt(AsVector3D pos)
        {
            pointAt(pos, null, null);
        }

        public void prepend(AsMatrix3D rhs)
        {
            prepend(ref rhs.internalMatrix);
        }

        private void prepend(ref Matrix matrix)
        {
            Matrix oldMatrix = internalMatrix;
            Matrix.Multiply(ref matrix, ref oldMatrix, out internalMatrix);
        }

        public void prependRotation(float degrees, AsVector3D axis, AsVector3D pivotPoint)
        {
            bool hasPivot = ((pivotPoint != null) && (((pivotPoint.x != 0.0f) || (pivotPoint.y != 0.0f)) || (pivotPoint.z != 0.0f)));
            if (hasPivot)
            {
                prependTranslation(-pivotPoint.x, -pivotPoint.y, -pivotPoint.z);
            }

            float radians = ((0.0055555555555556f * degrees) * AsMath.PI);
            float ax = axis.x;
            float ay = axis.y;
            float az = axis.z;
            if ((((ax == 0.0f) && (ay == 0.0f)) && (az == 1.0f)))
            {
                prependRotationZ(radians);
            }
            else
            {
                if ((((ax == 0.0f) && (ay == 1.0f)) && (az == 0.0f)))
                {
                    prependRotationY(radians);
                }
                else
                {
                    if ((((ax == 1.0f) && (ay == 0.0f)) && (az == 0.0f)))
                    {
                        prependRotationX(radians);
                    }
                    else
                    {
                        throw new AsNotImplementedError();
                    }
                }
            }
            if (hasPivot)
            {
                prependTranslation(pivotPoint.x, pivotPoint.y, pivotPoint.z);
            }
        }

        public void prependRotation(float degrees, AsVector3D axis)
        {
            prependRotation(degrees, axis, null);
        }

        private void prependRotationX(float radians)
        {
            Matrix rotationMatrix = Matrix.CreateRotationX(radians);
            prepend(ref rotationMatrix);
        }

        private void prependRotationY(float radians)
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(radians);
            prepend(ref rotationMatrix);
        }

        private void prependRotationZ(float radians)
        {
            Matrix rotationMatrix = Matrix.CreateRotationZ(radians);
            prepend(ref rotationMatrix);
        }

        public void prependScale(float xScale, float yScale, float zScale)
        {
            Matrix scaleMatrix = Matrix.CreateScale(xScale, yScale, zScale);
            prepend(ref scaleMatrix);
        }

        public void prependTranslation(float x, float y, float z)
        {
            Matrix translationMatrix = Matrix.CreateTranslation(x, y, z);
            prepend(ref translationMatrix);
        }

        public bool recompose(AsVector<AsVector3D> components, String orientationStyle)
        {
            throw new AsNotImplementedError();
        }

        public bool recompose(AsVector<AsVector3D> components)
        {
            return recompose(components, "eulerAngles");
        }

        public AsVector3D transformVector(AsVector3D v)
        {
            float nx = v.x * internalMatrix.M11 + v.y * internalMatrix.M21 + v.z * internalMatrix.M31 + internalMatrix.M41;
            float ny = v.x * internalMatrix.M12 + v.y * internalMatrix.M22 + v.z * internalMatrix.M32 + internalMatrix.M42;
            float nz = v.x * internalMatrix.M13 + v.y * internalMatrix.M23 + v.z * internalMatrix.M33 + internalMatrix.M43;
            return new AsVector3D(nx, ny, nz);
        }

        public void transformVectors(AsVector<float> vin, AsVector<float> vout)
        {
            int len = (int)(vin.getLength());
            if (((len % 3) != 0))
            {
                throw new AsArgumentError();
            }
            if ((len > vout.getLength()))
            {
                throw new AsArgumentError();
            }
            int i = 0;
            for (; (i < len); i = (i + 3))
            {
                float x = vin[i];
                float y = vin[i + 1];
                float z = vin[i + 2];

                vout[i] = x * internalMatrix.M11 + y * internalMatrix.M21 + z * internalMatrix.M31 + internalMatrix.M41;
                vout[i + 1] = x * internalMatrix.M12 + y * internalMatrix.M22 + z * internalMatrix.M32 + internalMatrix.M42;
                vout[i + 2] = x * internalMatrix.M13 + y * internalMatrix.M23 + z * internalMatrix.M33 + internalMatrix.M43;
            }
        }

        public void transpose()
        {
            Matrix oldMatrix = internalMatrix;
            Matrix.Transpose(ref oldMatrix, out internalMatrix);
        }

        public float getDeterminant()
        {
            return internalMatrix.Determinant();
        }

        public AsVector3D getPosition()
        {
            return new AsVector3D(internalMatrix.M41, internalMatrix.M42, internalMatrix.M43);
        }

        public AsVector<float> getRawData()
        {
            throw new NotImplementedException();
        }
    }
}
