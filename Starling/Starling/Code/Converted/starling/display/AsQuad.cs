using System;
 
using bc.flash;
using bc.flash.geom;
using starling.core;
using starling.display;
using starling.utils;
 
namespace starling.display
{
	public class AsQuad : AsDisplayObject
	{
		private bool mTinted;
		protected AsVertexData mVertexData;
		private static AsPoint sHelperPoint = new AsPoint();
		private static AsMatrix sHelperMatrix = new AsMatrix();
		public AsQuad(float width, float height, uint color, bool premultipliedAlpha)
		{
			mTinted = (color != 0xffffff);
			mVertexData = new AsVertexData(4, premultipliedAlpha);
			mVertexData.setPosition(0, 0.0f, 0.0f);
			mVertexData.setPosition(1, width, 0.0f);
			mVertexData.setPosition(2, 0.0f, height);
			mVertexData.setPosition(3, width, height);
			mVertexData.setUniformColor(color);
			onVertexDataChanged();
		}
		public AsQuad(float width, float height, uint color)
		 : this(width, height, color, true)
		{
		}
		public AsQuad(float width, float height)
		 : this(width, height, 0xffffff, true)
		{
		}
		protected virtual void onVertexDataChanged()
		{
		}
		public override AsRectangle getBounds(AsDisplayObject targetSpace, AsRectangle resultRect)
		{
			if((resultRect == null))
			{
				resultRect = new AsRectangle();
			}
			if((targetSpace == this))
			{
				mVertexData.getPosition(3, sHelperPoint);
				resultRect.setTo(0.0f, 0.0f, sHelperPoint.x, sHelperPoint.y);
			}
			else
			{
				if(((targetSpace == getParent()) && (getRotation() == 0.0f)))
				{
					float scaleX = this.getScaleX();
					float scaleY = this.getScaleY();
					mVertexData.getPosition(3, sHelperPoint);
					resultRect.setTo((getX() - (getPivotX() * scaleX)), (getY() - (getPivotY() * scaleY)), (sHelperPoint.x * scaleX), (sHelperPoint.y * scaleY));
					if((scaleX < 0))
					{
						resultRect.width = (resultRect.width * -1);
						resultRect.x = (resultRect.x - resultRect.width);
					}
					if((scaleY < 0))
					{
						resultRect.height = (resultRect.height * -1);
						resultRect.y = (resultRect.y - resultRect.height);
					}
				}
				else
				{
					getTransformationMatrix(targetSpace, sHelperMatrix);
					mVertexData.getBounds(sHelperMatrix, 0, 4, resultRect);
				}
			}
			return resultRect;
		}
		public virtual AsRectangle getBounds(AsDisplayObject targetSpace)
		{
			return getBounds(targetSpace, null);
		}
		public virtual uint getVertexColor(int vertexID)
		{
			return mVertexData.getColor(vertexID);
		}
		public virtual void setVertexColor(int vertexID, uint color)
		{
			mVertexData.setColor(vertexID, color);
			onVertexDataChanged();
			if((color != 0xffffff))
			{
				mTinted = true;
			}
			else
			{
				mTinted = mVertexData.getTinted();
			}
		}
		public virtual float getVertexAlpha(int vertexID)
		{
			return mVertexData.getAlpha(vertexID);
		}
		public virtual void setVertexAlpha(int vertexID, float alpha)
		{
			mVertexData.setAlpha(vertexID, alpha);
			onVertexDataChanged();
			if((alpha != 1.0f))
			{
				mTinted = true;
			}
			else
			{
				mTinted = mVertexData.getTinted();
			}
		}
		public virtual uint getColor()
		{
			return mVertexData.getColor(0);
		}
		public virtual void setColor(uint _value)
		{
			int i = 0;
			for (; (i < 4); ++i)
			{
				setVertexColor(i, _value);
			}
			if(((_value != 0xffffff) || (getAlpha() != 1.0f)))
			{
				mTinted = true;
			}
			else
			{
				mTinted = mVertexData.getTinted();
			}
		}
		public override void setAlpha(float _value)
		{
			base.setAlpha(_value);
			if((_value < 1.0f))
			{
				mTinted = true;
			}
			else
			{
				mTinted = mVertexData.getTinted();
			}
		}
		public virtual void copyVertexDataTo(AsVertexData targetData, int targetVertexID)
		{
			mVertexData.copyTo(targetData, targetVertexID);
		}
		public virtual void copyVertexDataTo(AsVertexData targetData)
		{
			copyVertexDataTo(targetData, 0);
		}
		public override void render(AsRenderSupport support, float parentAlpha)
		{
			support.batchQuad(this, parentAlpha);
		}
		public virtual bool getTinted()
		{
			return mTinted;
		}
	}
}
