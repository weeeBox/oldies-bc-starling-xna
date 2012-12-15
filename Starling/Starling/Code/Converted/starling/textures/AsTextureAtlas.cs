using System;
 
using bc.flash;
using bc.flash.geom;
using bc.flash.xml;
using starling.textures;
 
namespace starling.textures
{
	public class AsTextureAtlas : AsObject
	{
		private AsTexture mAtlasTexture;
		private AsDictionary mTextureRegions;
		private AsDictionary mTextureFrames;
		public AsTextureAtlas(AsTexture texture, AsXML atlasXml)
		{
			mTextureRegions = new AsDictionary();
			mTextureFrames = new AsDictionary();
			mAtlasTexture = texture;
			if(atlasXml != null)
			{
				parseAtlasXml(atlasXml);
			}
		}
		public AsTextureAtlas(AsTexture texture)
		 : this(texture, null)
		{
		}
		public virtual void dispose()
		{
			mAtlasTexture.dispose();
		}
		protected virtual void parseAtlasXml(AsXML atlasXml)
		{
			float scale = mAtlasTexture.getScale();
			AsSubTexture __subTextures_ = atlasXml.SubTexture;
			if (__subTextures_ != null)
			{
				foreach (AsXML subTexture in __subTextures_)
				{
					String name = subTexture.attribute("name");
					float x = AsGlobal.parseFloat(subTexture.attribute("x")) / scale;
					float y = AsGlobal.parseFloat(subTexture.attribute("y")) / scale;
					float width = AsGlobal.parseFloat(subTexture.attribute("width")) / scale;
					float height = AsGlobal.parseFloat(subTexture.attribute("height")) / scale;
					float frameX = AsGlobal.parseFloat(subTexture.attribute("frameX")) / scale;
					float frameY = AsGlobal.parseFloat(subTexture.attribute("frameY")) / scale;
					float frameWidth = AsGlobal.parseFloat(subTexture.attribute("frameWidth")) / scale;
					float frameHeight = AsGlobal.parseFloat(subTexture.attribute("frameHeight")) / scale;
					AsRectangle region = new AsRectangle(x, y, width, height);
					AsRectangle frame = frameWidth > 0 && frameHeight > 0 ? new AsRectangle(frameX, frameY, frameWidth, frameHeight) : null;
					addRegion(name, region, frame);
				}
			}
		}
		public virtual AsTexture getTexture(String name)
		{
			AsRectangle region = (AsRectangle)(mTextureRegions[name]);
			if(region == null)
			{
				return null;
			}
			else
			{
				return AsTexture.fromTexture(mAtlasTexture, region, (AsRectangle)(mTextureFrames[name]));
			}
		}
		public virtual AsVector<AsTexture> getTextures(String prefix)
		{
			AsVector<AsTexture> textures = new AsVector<AsTexture>();
			AsVector<String> names = new AsVector<String>();
			AsDictionary __names_ = mTextureRegions;
			if (__names_ != null)
			{
				foreach (String name in __names_)
				{
					if(AsString.indexOf(name, prefix) == 0)
					{
						names.push(name);
					}
				}
			}
			names.sort(AsArray.CASEINSENSITIVE);
			AsVector<String> __names_ = names;
			if (__names_ != null)
			{
				foreach (String name in __names_)
				{
					textures.push(getTexture(name));
				}
			}
			return textures;
		}
		public virtual AsVector<AsTexture> getTextures()
		{
			return getTextures("");
		}
		public virtual AsRectangle getRegion(String name)
		{
			return (AsRectangle)(mTextureRegions[name]);
		}
		public virtual AsRectangle getFrame(String name)
		{
			return (AsRectangle)(mTextureFrames[name]);
		}
		public virtual void addRegion(String name, AsRectangle region, AsRectangle frame)
		{
			mTextureRegions[name] = region;
			mTextureFrames[name] = frame;
		}
		public virtual void addRegion(String name, AsRectangle region)
		{
			addRegion(name, region, null);
		}
		public virtual void removeRegion(String name)
		{
			mTextureRegions.remove(name);
			mTextureFrames.remove(name);
		}
	}
}
