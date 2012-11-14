using System;
 
using bc.flash;
 
namespace starling.errors
{
	public class AsAbstractClassError : AsError
	{
		public AsAbstractClassError(Object message, Object id)
		 : base(message, id)
		{
		}
		public AsAbstractClassError(Object message)
		 : this(message, 0)
		{
		}
		public AsAbstractClassError()
		 : this("", 0)
		{
		}
	}
}
