using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using bc.flash.xml;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Xml.Linq;

namespace bc.flash.resources
{
    public class BcResFactory
    {
        private ContentManager content;
        private Dictionary<string, object> usedReferences;

        private const string EXT_PNG = ".png";
        private const string EXT_JPG = ".jpg";
        private const string EXT_WAV = ".wav";
        private const string EXT_MP3 = ".mp3";
        private const string EXT_XML = ".xml";

        private static BcResFactory instance;

        public BcResFactory(ContentManager content)
        {
            this.content = content;
            usedReferences = new Dictionary<string, object>();

            instance = this;
        }

        public static BcResFactory GetInstance()
        {
            return instance;
        }

        public AsObject LoadResource(String path)
        {
            String ext = ExtractExt(path);

            if (ext == EXT_PNG || ext == EXT_JPG)
            {
                return LoadImage(path);
            }

            if (ext == EXT_WAV)
            {
                return LoadSound(path);
            }

            if (ext == EXT_MP3)
            {
                return LoadMusic(path);
            }

            if (ext == EXT_XML)
            {
                return LoadXML(path);
            }

            throw new NotImplementedException("Unknown type: " + ext);
        }

        public BcTexture2D LoadImage(String path)
        {            
            String contentPath = CreateContentPath(path);
            Texture2D texture = content.Load<Texture2D>(contentPath);
            return new BcTexture2D(texture);            
        }

        public BcSound LoadSound(String path)
        {
            String ext = ExtractExt(path);

            if (ext == EXT_WAV)
            {
                return LoadSoundEffect(path);
            }

            if (ext == EXT_MP3)
            {
                return LoadMusic(path);
            }

            throw new NotImplementedException();
        }

        private BcMusic LoadMusic(String path)
        {            
            String contentPath = CreateContentPath(path);
            Song song = content.Load<Song>(contentPath);
            return new BcMusic(song);            
        }

        private BcSoundEffect LoadSoundEffect(String path)
        {            
            String contentPath = CreateContentPath(path);
            SoundEffect effect = content.Load<SoundEffect>(contentPath);
            return new BcSoundEffect(effect);         
        }

        public AsObject LoadXML(String path)
        {
            using (ContentManager manager = new ContentManager(content.ServiceProvider, "Content"))
            {
                String contentPath = CreateContentPath(path);
                BcBinaryData data = manager.Load<BcBinaryData>(contentPath);
                using (MemoryStream stream = new MemoryStream(data.Data))
                {
                    XElement doc = XElement.Load(stream);                    
                    return ExtractXML(doc);                    
                }
            }
        }

        private AsXMLElement ExtractXML(XElement node)
        {
            if (node.NodeType == XmlNodeType.Element)
            {
                AsXMLElement element = new AsXMLElement(node.Name.LocalName);

                IEnumerable<XAttribute> attributes = node.Attributes();
                foreach (XAttribute attr in attributes)
                {                    
                    element.appendAttribute(attr.Name.LocalName, attr.Value);
                }

                IEnumerable<XElement> childs = node.Elements();
                foreach (XElement child in childs)
                {
                    AsXMLElement childElement = ExtractXML(child);
                    if (childElement != null)
                    {
                        element.appendChild(childElement);
                    }
                }

                return element;
            }

            return null;
        }

        public void Dispose()
        {
        }

        private String CreateContentPath(String path)
        {
            path = path.Replace("../", "");

            int dotIndex = path.LastIndexOf('.');
            if (dotIndex != -1)
            {
                return path.Substring(0, dotIndex);
            }
            return path;
        }

        private String ExtractExt(String path)
        {
            int dotIndex = path.LastIndexOf('.');
            if (dotIndex != -1)
            {
                return path.Substring(dotIndex, path.Length - dotIndex).ToLower();
            }
            return "";
        }
    }
}
