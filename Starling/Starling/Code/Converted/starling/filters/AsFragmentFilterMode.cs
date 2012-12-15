using System;
 
using bc.flash;
using starling.errors;
 
namespace starling.filters
{
	public class AsFragmentFilterMode : AsObject
	{
		public static String BELOW = "below";
		public static String REPLACE = "replace";
		public static String ABOVE = "above";
		public AsFragmentFilterMode()
		{
			throw new AsAbstractClassError();
		}
	}
}
