using System;
 
using bc.flash;
using bc.flash.xml;
 
namespace bc.flash.xml
{
	public class AsXMLElement : AsXML
	{
		private AsXMLList mAttributes;
		private AsXMLList mChildren;
		public AsXMLElement(String name)
		 : base(name)
		{
			mAttributes = new AsXMLList();
			mChildren = new AsXMLList();
		}
		public override AsXML appendChild(AsXML child)
		{
			return mChildren.appendChild(child);
		}
		public virtual AsXML appendAttribute(String name, String _value)
		{
			return mAttributes.appendChild(new AsXMLAttribute(name, _value));
		}
		public override bool hasOwnProperty(String name)
		{
			return mAttributes.containsChild(name);
		}
		public override String attributeValue(String arg)
		{
			AsVector<AsXML> __attrs_ = mAttributes.list();
			if (__attrs_ != null)
			{
				foreach (AsXML attr in __attrs_)
				{
					if((attr.name() == arg))
					{
						return ((AsXMLAttribute)(attr))._value();
					}
				}
			}
			return null;
		}
		public override AsXMLList attributes()
		{
			return mAttributes;
		}
		public override AsXMLList child(String name)
		{
			AsXMLList result = new AsXMLList();
			AsVector<AsXML> __childs_ = mChildren.list();
			if (__childs_ != null)
			{
				foreach (AsXML child in __childs_)
				{
					if((child.name() == name))
					{
						result.appendChild(child);
					}
				}
			}
			return result;
		}
		public override AsXMLList children()
		{
			return mChildren;
		}
		public override bool contains(String name)
		{
			AsVector<AsXML> __childs_ = mChildren.list();
			if (__childs_ != null)
			{
				foreach (AsXML child in __childs_)
				{
					if((child.name() == name))
					{
						return true;
					}
				}
			}
			return false;
		}
		public override AsXMLList elements(String name)
		{
			if((name == "*"))
			{
				return mChildren;
			}
			AsXMLList result = new AsXMLList();
			AsVector<AsXML> __childs_ = mChildren.list();
			if (__childs_ != null)
			{
				foreach (AsXML child in __childs_)
				{
					if(((child.name() == name) && (child.nodeKind() == nodeKind())))
					{
						result.appendChild(child);
					}
				}
			}
			return result;
		}
		public virtual AsXMLList elements()
		{
			return elements("*");
		}
		public override String nodeKind()
		{
			return "element";
		}
	}
}
