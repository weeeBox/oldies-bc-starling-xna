using System;
 
using bc.flash;
 
namespace starling.errors
{
	public class AsMissingContextError : AsError
	{
		public AsMissingContextError(Object message, Object id)
		 : base(message, id)
		{
		}
		public AsMissingContextError(Object message)
		 : this(message, 0)
		{
		}
		public AsMissingContextError()
		 : this("", 0)
		{
		}
	}
}
