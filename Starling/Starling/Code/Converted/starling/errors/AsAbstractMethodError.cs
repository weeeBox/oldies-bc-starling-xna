using System;
 
using bc.flash;
 
namespace starling.errors
{
	public class AsAbstractMethodError : AsError
	{
		public AsAbstractMethodError(Object message, Object id)
		 : base(message, id)
		{
		}
		public AsAbstractMethodError(Object message)
		 : this(message, 0)
		{
		}
		public AsAbstractMethodError()
		 : this("", 0)
		{
		}
	}
}
