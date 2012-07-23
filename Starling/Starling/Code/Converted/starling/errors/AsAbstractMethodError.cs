using System;
 
using bc.flash;
 
namespace starling.errors
{
	public class AsAbstractMethodError : AsError
	{
		public AsAbstractMethodError(As* message, As* id)
		 : base(message, id)
		{
		}
		public AsAbstractMethodError(As* message)
		 : this(message, 0)
		{
		}
		public AsAbstractMethodError()
		 : this("", 0)
		{
		}
	}
}
