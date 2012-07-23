using System;
 
using bc.flash;
 
namespace starling.errors
{
	public class AsMissingContextError : AsError
	{
		public AsMissingContextError(As* message, As* id)
		 : base(message, id)
		{
		}
		public AsMissingContextError(As* message)
		 : this(message, 0)
		{
		}
		public AsMissingContextError()
		 : this("", 0)
		{
		}
	}
}
