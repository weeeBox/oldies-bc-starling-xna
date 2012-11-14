using System;
 
using bc.flash;
using bc.flash.geom;
using starling.utils;
 
namespace starling.utils
{
	public class AsVertexData : AsObject
	{
		public const int ELEMENTS_PER_VERTEX = 8;
		public const int POSITION_OFFSET = 0;
		public const int COLOR_OFFSET = 2;
		public const int TEXCOORD_OFFSET = 6;
		private AsVector<float> mRawData;
		private bool mPremultipliedAlpha;
		private int mNumVertices;
		private static AsPoint sHelperPoint = new AsPoint();
		public AsVertexData(int numVertices, bool premultipliedAlpha)
		{
			mRawData = new AsVector<float>();
			mPremultipliedAlpha = premultipliedAlpha;
			this.setNumVertices(numVertices);
		}
		public AsVertexData(int numVertices)
		 : this(numVertices, false)
		{
		}
		public virtual AsVertexData clone(int vertexID, int numVertices)
		{
			if(((numVertices < 0) || ((vertexID + numVertices) > mNumVertices)))
			{
				numVertices = (mNumVertices - vertexID);
			}
			AsVertexData clone = new AsVertexData(0, mPremultipliedAlpha);
			clone.mNumVertices = numVertices;
			clone.mRawData = mRawData.slice((vertexID * ELEMENTS_PER_VERTEX), (numVertices * ELEMENTS_PER_VERTEX));
			clone.mRawData._fixed = true;
			return clone;
		}
		public virtual AsVertexData clone(int vertexID)
		{
			return clone(vertexID, -1);
		}
		public virtual AsVertexData clone()
		{
			return clone(0, -1);
		}
		public virtual void copyTo(AsVertexData targetData, int targetVertexID, int vertexID, int numVertices)
		{
			if(((numVertices < 0) || ((vertexID + numVertices) > mNumVertices)))
			{
				numVertices = (mNumVertices - vertexID);
			}
			AsVector<float> targetRawData = targetData.mRawData;
			int targetIndex = (targetVertexID * ELEMENTS_PER_VERTEX);
			int sourceIndex = (vertexID * ELEMENTS_PER_VERTEX);
			int dataLength = (numVertices * ELEMENTS_PER_VERTEX);
			int i = sourceIndex;
			for (; (i < dataLength); ++i)
			{
				targetRawData[targetIndex++] = mRawData[i];
			}
		}
		public virtual void copyTo(AsVertexData targetData, int targetVertexID, int vertexID)
		{
			copyTo(targetData, targetVertexID, vertexID, -1);
		}
		public virtual void copyTo(AsVertexData targetData, int targetVertexID)
		{
			copyTo(targetData, targetVertexID, 0, -1);
		}
		public virtual void copyTo(AsVertexData targetData)
		{
			copyTo(targetData, 0, 0, -1);
		}
		public virtual void append(AsVertexData data)
		{
			mRawData._fixed = false;
			AsVector<float> rawData = data.mRawData;
			int rawDataLength = (int)(rawData.getLength());
			int i = 0;
			for (; (i < rawDataLength); ++i)
			{
				mRawData.push(rawData[i]);
			}
			mNumVertices = (mNumVertices + data.getNumVertices());
			mRawData._fixed = true;
		}
		public virtual void setPosition(int vertexID, float x, float y)
		{
			int offset = (getOffset(vertexID) + POSITION_OFFSET);
			mRawData[offset] = x;
			mRawData[((int)((offset + 1)))] = y;
		}
		public virtual void getPosition(int vertexID, AsPoint position)
		{
			int offset = (getOffset(vertexID) + POSITION_OFFSET);
			position.x = mRawData[offset];
			position.y = mRawData[((int)((offset + 1)))];
		}
		public virtual void setColor(int vertexID, uint color)
		{
			int offset = (getOffset(vertexID) + COLOR_OFFSET);
			float multiplier = ((mPremultipliedAlpha) ? (mRawData[((int)((offset + 3)))]) : (1.0f));
			mRawData[offset] = ((((color >> 16) & 0xff) / 255.0f) * multiplier);
			mRawData[((int)((offset + 1)))] = ((((color >> 8) & 0xff) / 255.0f) * multiplier);
			mRawData[((int)((offset + 2)))] = (((color & 0xff) / 255.0f) * multiplier);
		}
		public virtual uint getColor(int vertexID)
		{
			int offset = (getOffset(vertexID) + COLOR_OFFSET);
			float divisor = ((mPremultipliedAlpha) ? (mRawData[(offset + 3)]) : (1.0f));
			if((divisor == 0))
			{
				return (uint)(0);
			}
			else
			{
				float red = (mRawData[offset] / divisor);
				float green = (mRawData[((int)((offset + 1)))] / divisor);
				float blue = (mRawData[((int)((offset + 2)))] / divisor);
				return (uint)((((((int)((red * 255))) << 16) | (((int)((green * 255))) << 8)) | ((int)((blue * 255)))));
			}
		}
		public virtual void setAlpha(int vertexID, float alpha)
		{
			int offset = ((getOffset(vertexID) + COLOR_OFFSET) + 3);
			if(mPremultipliedAlpha)
			{
				if((alpha < 0.001f))
				{
					alpha = 0.001f;
				}
				uint color = getColor(vertexID);
				mRawData[offset] = alpha;
				setColor(vertexID, color);
			}
			else
			{
				mRawData[offset] = alpha;
			}
		}
		public virtual float getAlpha(int vertexID)
		{
			int offset = ((getOffset(vertexID) + COLOR_OFFSET) + 3);
			return mRawData[offset];
		}
		public virtual void setTexCoords(int vertexID, float u, float v)
		{
			int offset = (getOffset(vertexID) + TEXCOORD_OFFSET);
			mRawData[offset] = u;
			mRawData[((int)((offset + 1)))] = v;
		}
		public virtual void getTexCoords(int vertexID, AsPoint texCoords)
		{
			int offset = (getOffset(vertexID) + TEXCOORD_OFFSET);
			texCoords.x = mRawData[offset];
			texCoords.y = mRawData[((int)((offset + 1)))];
		}
		public virtual void translateVertex(int vertexID, float deltaX, float deltaY)
		{
			int offset = (getOffset(vertexID) + POSITION_OFFSET);
			mRawData[offset] = (mRawData[offset] + deltaX);
			mRawData[((int)((offset + 1)))] = (mRawData[((int)((offset + 1)))] + deltaY);
		}
		public virtual void transformVertex(int vertexID, AsMatrix matrix, int numVertices)
		{
			int offset = (getOffset(vertexID) + POSITION_OFFSET);
			int i = 0;
			for (; (i < numVertices); ++i)
			{
				float x = mRawData[offset];
				float y = mRawData[((int)((offset + 1)))];
				mRawData[offset] = (((matrix.a * x) + (matrix.c * y)) + matrix.tx);
				mRawData[((int)((offset + 1)))] = (((matrix.d * y) + (matrix.b * x)) + matrix.ty);
				offset = (offset + ELEMENTS_PER_VERTEX);
			}
		}
		public virtual void transformVertex(int vertexID, AsMatrix matrix)
		{
			transformVertex(vertexID, matrix, 1);
		}
		public virtual void setUniformColor(uint color)
		{
			int i = 0;
			for (; (i < mNumVertices); ++i)
			{
				setColor(i, color);
			}
		}
		public virtual void setUniformAlpha(float alpha)
		{
			int i = 0;
			for (; (i < mNumVertices); ++i)
			{
				setAlpha(i, alpha);
			}
		}
		public virtual void scaleAlpha(int vertexID, float alpha, int numVertices)
		{
			if((alpha == 1.0f))
			{
				return;
			}
			if(((numVertices < 0) || ((vertexID + numVertices) > mNumVertices)))
			{
				numVertices = (mNumVertices - vertexID);
			}
			int i = 0;
			if(mPremultipliedAlpha)
			{
				for (i = 0; (i < numVertices); ++i)
				{
					setAlpha((vertexID + i), (getAlpha((vertexID + i)) * alpha));
				}
			}
			else
			{
				int offset = ((getOffset(vertexID) + COLOR_OFFSET) + 3);
				for (i = 0; (i < numVertices); ++i)
				{
					mRawData[((int)((offset + (i * ELEMENTS_PER_VERTEX))))] = (mRawData[((int)((offset + (i * ELEMENTS_PER_VERTEX))))] * alpha);
				}
			}
		}
		public virtual void scaleAlpha(int vertexID, float alpha)
		{
			scaleAlpha(vertexID, alpha, 1);
		}
		private int getOffset(int vertexID)
		{
			return (vertexID * ELEMENTS_PER_VERTEX);
		}
		public virtual AsRectangle getBounds(AsMatrix transformationMatrix, int vertexID, int numVertices, AsRectangle resultRect)
		{
			if((resultRect == null))
			{
				resultRect = new AsRectangle();
			}
			if(((numVertices < 0) || ((vertexID + numVertices) > mNumVertices)))
			{
				numVertices = (mNumVertices - vertexID);
			}
			float minX = AsNumber.MAX_VALUE;
			float maxX = -AsNumber.MAX_VALUE;
			float minY = AsNumber.MAX_VALUE;
			float maxY = -AsNumber.MAX_VALUE;
			int offset = (getOffset(vertexID) + POSITION_OFFSET);
			float x = 0;
			float y = 0;
			int i = 0;
			if((transformationMatrix == null))
			{
				for (i = vertexID; (i < numVertices); ++i)
				{
					x = mRawData[offset];
					y = mRawData[((int)((offset + 1)))];
					offset = (offset + ELEMENTS_PER_VERTEX);
					minX = (((minX < x)) ? (minX) : (x));
					maxX = (((maxX > x)) ? (maxX) : (x));
					minY = (((minY < y)) ? (minY) : (y));
					maxY = (((maxY > y)) ? (maxY) : (y));
				}
			}
			else
			{
				for (i = vertexID; (i < numVertices); ++i)
				{
					x = mRawData[offset];
					y = mRawData[((int)((offset + 1)))];
					offset = (offset + ELEMENTS_PER_VERTEX);
					AsMatrixUtil.transformCoords(transformationMatrix, x, y, sHelperPoint);
					minX = (((minX < sHelperPoint.x)) ? (minX) : (sHelperPoint.x));
					maxX = (((maxX > sHelperPoint.x)) ? (maxX) : (sHelperPoint.x));
					minY = (((minY < sHelperPoint.y)) ? (minY) : (sHelperPoint.y));
					maxY = (((maxY > sHelperPoint.y)) ? (maxY) : (sHelperPoint.y));
				}
			}
			resultRect.setTo(minX, minY, (maxX - minX), (maxY - minY));
			return resultRect;
		}
		public virtual AsRectangle getBounds(AsMatrix transformationMatrix, int vertexID, int numVertices)
		{
			return getBounds(transformationMatrix, vertexID, numVertices, null);
		}
		public virtual AsRectangle getBounds(AsMatrix transformationMatrix, int vertexID)
		{
			return getBounds(transformationMatrix, vertexID, -1, null);
		}
		public virtual AsRectangle getBounds(AsMatrix transformationMatrix)
		{
			return getBounds(transformationMatrix, 0, -1, null);
		}
		public virtual AsRectangle getBounds()
		{
			return getBounds(null, 0, -1, null);
		}
		public virtual bool getTinted()
		{
			int offset = COLOR_OFFSET;
			int i = 0;
			for (; (i < mNumVertices); ++i)
			{
				int j = 0;
				for (; (j < 4); ++j)
				{
					if((mRawData[((int)((offset + j)))] != 1.0f))
					{
						return true;
					}
				}
				offset = (offset + ELEMENTS_PER_VERTEX);
			}
			return false;
		}
		public virtual void setPremultipliedAlpha(bool _value, bool updateData)
		{
			if((_value == mPremultipliedAlpha))
			{
				return;
			}
			if(updateData)
			{
				int dataLength = (mNumVertices * ELEMENTS_PER_VERTEX);
				int i = COLOR_OFFSET;
				for (; (i < dataLength); i = (i + ELEMENTS_PER_VERTEX))
				{
					float alpha = mRawData[(i + 3)];
					float divisor = ((mPremultipliedAlpha) ? (alpha) : (1.0f));
					float multiplier = ((_value) ? (alpha) : (1.0f));
					if((divisor != 0))
					{
						mRawData[i] = ((mRawData[i] / divisor) * multiplier);
						mRawData[((int)((i + 1)))] = ((mRawData[((int)((i + 1)))] / divisor) * multiplier);
						mRawData[((int)((i + 2)))] = ((mRawData[((int)((i + 2)))] / divisor) * multiplier);
					}
				}
			}
			mPremultipliedAlpha = _value;
		}
		public virtual void setPremultipliedAlpha(bool _value)
		{
			setPremultipliedAlpha(_value, true);
		}
		public virtual bool getPremultipliedAlpha()
		{
			return mPremultipliedAlpha;
		}
		public virtual int getNumVertices()
		{
			return mNumVertices;
		}
		public virtual void setNumVertices(int _value)
		{
			mRawData._fixed = false;
			int i = 0;
			int delta = (_value - mNumVertices);
			for (i = 0; (i < delta); ++i)
			{
				mRawData.push(0, 0, 0, 0, 0, 1, 0, 0);
			}
			for (i = 0; (i < -(delta * ELEMENTS_PER_VERTEX)); ++i)
			{
				mRawData.pop();
			}
			mNumVertices = _value;
			mRawData._fixed = true;
		}
		public virtual AsVector<float> getRawData()
		{
			return mRawData;
		}
	}
}
