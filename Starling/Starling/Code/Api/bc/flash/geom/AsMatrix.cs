using System;

using bc.flash;
using bc.flash.error;
using bc.flash.geom;
using Microsoft.Xna.Framework;

namespace bc.flash.geom
{
    public sealed class AsMatrix : AsObject
    {
        public Matrix internalMatrix;

        public AsMatrix(ref Matrix matrix)
        {
            internalMatrix = matrix;
        }

        public AsMatrix(float a, float b, float c, float d, float tx, float ty)
        {
            setTo(a, b, c, d, tx, ty);
        }

        public AsMatrix(float a, float b, float c, float d, float tx)
        {
            setTo(a, b, c, d, tx, 0);
        }

        public AsMatrix(float a, float b, float c, float d)
        {
            setTo(a, b, c, d, 0, 0);
        }

        public AsMatrix(float a, float b, float c)
        {
            setTo(a, b, c, 1, 0, 0);
        }

        public AsMatrix(float a, float b)
        {
            setTo(a, b, 0, 1, 0, 0);
        }

        public AsMatrix(float a)
        {
            setTo(a, 0, 0, 1, 0, 0);
        }

        public AsMatrix()
        {
            internalMatrix = Matrix.Identity;
        }

        public AsMatrix clone()
        {
            return new AsMatrix(ref internalMatrix);
        }

        public void concat(AsMatrix m)
        {
            concat(ref m.internalMatrix);
        }

        public void concat(ref Matrix m)
        {
            Matrix oldMatrix = internalMatrix;
            Matrix.Multiply(ref oldMatrix, ref m, out internalMatrix);
        }

        public void copyColumnFrom(uint column, AsVector3D vector3D)
        {
            throw new AsNotImplementedError();
        }

        public void copyColumnTo(uint column, AsVector3D vector3D)
        {
            throw new AsNotImplementedError();
        }

        public void copyFrom(AsMatrix sourceMatrix)
        {
            internalMatrix = sourceMatrix.internalMatrix;
        }

        public void copyRowFrom(uint row, AsVector3D vector3D)
        {
            throw new AsNotImplementedError();
        }

        public void copyRowTo(uint row, AsVector3D vector3D)
        {
            throw new AsNotImplementedError();
        }

        public void createBox(float scaleX, float scaleY, float rotation, float tx, float ty)
        {
            identity();
            rotate(rotation);
            scale(scaleX, scaleY);
            translate(tx, ty);
        }

        public void createBox(float scaleX, float scaleY, float rotation, float tx)
        {
            createBox(scaleX, scaleY, rotation, tx, 0);
        }

        public void createBox(float scaleX, float scaleY, float rotation)
        {
            createBox(scaleX, scaleY, rotation, 0, 0);
        }

        public void createBox(float scaleX, float scaleY)
        {
            createBox(scaleX, scaleY, 0, 0, 0);
        }

        public void createGradientBox(float width, float height, float rotation, float tx, float ty)
        {
            throw new AsNotImplementedError();
        }

        public void createGradientBox(float width, float height, float rotation, float tx)
        {
            createGradientBox(width, height, rotation, tx, 0);
        }

        public void createGradientBox(float width, float height, float rotation)
        {
            createGradientBox(width, height, rotation, 0, 0);
        }

        public void createGradientBox(float width, float height)
        {
            createGradientBox(width, height, 0, 0, 0);
        }

        public AsPoint deltaTransformPoint(AsPoint point)
        {
            float nx = point.x * a + point.y * c;
            float ny = point.x * b + point.y * d;
            return new AsPoint(nx, ny);
        }

        public void identity()
        {
            internalMatrix = Matrix.Identity;
        }

        public void invert()
        {
            Matrix oldMatrix = internalMatrix;
            Matrix.Invert(ref oldMatrix, out internalMatrix);
        }

        public void rotate(float angle)
        {
            Matrix transformMatrix;
            Matrix.CreateRotationZ(angle, out transformMatrix);
            concat(ref transformMatrix);
        }

        public void scale(float sx, float sy)
        {
            Matrix transformMatrix;
            Matrix.CreateScale(sx, sy, 1.0f, out transformMatrix);
            concat(ref transformMatrix);
        }

        public void setTo(float a, float b, float c, float d, float tx, float ty)
        {
            internalMatrix.M11 = a;
            internalMatrix.M12 = b;
            internalMatrix.M13 = 0;
            internalMatrix.M14 = 0;

            internalMatrix.M21 = c;
            internalMatrix.M22 = d;
            internalMatrix.M23 = 0;
            internalMatrix.M24 = 0;

            internalMatrix.M31 = 0;
            internalMatrix.M32 = 0;
            internalMatrix.M33 = 1;
            internalMatrix.M34 = 0;

            internalMatrix.M41 = tx;
            internalMatrix.M42 = ty;
            internalMatrix.M43 = 0;
            internalMatrix.M44 = 1;
        }

        public AsPoint transformPoint(AsPoint point)
        {
            float nx = point.x * a + point.y * c + tx;
            float ny = point.x * b + point.y * d + ty;
            return new AsPoint(nx, ny);
        }

        public void transformPointCords(float x, float y, AsPoint point)
        {
            point.x = x * a + y * c + tx;
            point.y = x * b + y * d + ty;
        }

        public void translate(float dx, float dy)
        {
            Matrix transformMatrix;
            Matrix.CreateTranslation(dx, dy, 0.0f, out transformMatrix);
            concat(ref transformMatrix);
        }

        public float a
        {
            get { return internalMatrix.M11; }
            set { internalMatrix.M11 = value; }
        }
        public float b
        {
            get { return internalMatrix.M12; }
            set { internalMatrix.M12 = value; }
        }
        public float c
        {
            get { return internalMatrix.M21; }
            set { internalMatrix.M21 = value; }
        }
        public float d
        {
            get { return internalMatrix.M22; }
            set { internalMatrix.M22 = value; }
        }
        public float tx
        {
            get { return internalMatrix.M41; }
            set { internalMatrix.M41 = value; }
        }
        public float ty
        {
            get { return internalMatrix.M42; }
            set { internalMatrix.M42 = value; }
        }
    }
}
