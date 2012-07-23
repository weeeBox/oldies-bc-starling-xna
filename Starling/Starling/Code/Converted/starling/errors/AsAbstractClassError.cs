using System;
 
using bc.flash;
 
namespace starling.errors
{
	public class AsAbstractClassError : AsError
	{
		public AsAbstractClassError(As* message, As* id)
		 : base(message, id)
		{
		}
		public AsAbstractClassError(As* message)
		 : this(message, 0)
		{
		}
		public AsAbstractClassError()
		 : this("", 0)
		{
		}
	}
}
