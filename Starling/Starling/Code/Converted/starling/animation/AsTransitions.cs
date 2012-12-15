using System;
 
using bc.flash;
using starling.errors;
 
namespace starling.animation
{
	public class AsTransitions : AsObject
	{
		public static String LINEAR = "linear";
		public static String EASE_IN = "easeIn";
		public static String EASE_OUT = "easeOut";
		public static String EASE_IN_OUT = "easeInOut";
		public static String EASE_OUT_IN = "easeOutIn";
		public static String EASE_IN_BACK = "easeInBack";
		public static String EASE_OUT_BACK = "easeOutBack";
		public static String EASE_IN_OUT_BACK = "easeInOutBack";
		public static String EASE_OUT_IN_BACK = "easeOutInBack";
		public static String EASE_IN_ELASTIC = "easeInElastic";
		public static String EASE_OUT_ELASTIC = "easeOutElastic";
		public static String EASE_IN_OUT_ELASTIC = "easeInOutElastic";
		public static String EASE_OUT_IN_ELASTIC = "easeOutInElastic";
		public static String EASE_IN_BOUNCE = "easeInBounce";
		public static String EASE_OUT_BOUNCE = "easeOutBounce";
		public static String EASE_IN_OUT_BOUNCE = "easeInOutBounce";
		public static String EASE_OUT_IN_BOUNCE = "easeOutInBounce";
		private static AsDictionary sTransitions;
		public AsTransitions()
		{
			throw new AsAbstractClassError();
		}
		public static AsTransitionCallback getTransition(String name)
		{
			if(sTransitions == null)
			{
				registerDefaults();
			}
			return (AsTransitionCallback)(sTransitions[name]);
		}
		public static void register(String name, AsTransitionCallback func)
		{
			if(sTransitions == null)
			{
				registerDefaults();
			}
			sTransitions[name] = func;
		}
		private static void registerDefaults()
		{
			sTransitions = new AsDictionary();
			register(LINEAR, linear);
			register(EASE_IN, easeIn);
			register(EASE_OUT, easeOut);
			register(EASE_IN_OUT, easeInOut);
			register(EASE_OUT_IN, easeOutIn);
			register(EASE_IN_BACK, easeInBack);
			register(EASE_OUT_BACK, easeOutBack);
			register(EASE_IN_OUT_BACK, easeInOutBack);
			register(EASE_OUT_IN_BACK, easeOutInBack);
			register(EASE_IN_ELASTIC, easeInElastic);
			register(EASE_OUT_ELASTIC, easeOutElastic);
			register(EASE_IN_OUT_ELASTIC, easeInOutElastic);
			register(EASE_OUT_IN_ELASTIC, easeOutInElastic);
			register(EASE_IN_BOUNCE, easeInBounce);
			register(EASE_OUT_BOUNCE, easeOutBounce);
			register(EASE_IN_OUT_BOUNCE, easeInOutBounce);
			register(EASE_OUT_IN_BOUNCE, easeOutInBounce);
		}
		private static float linear(float ratio)
		{
			return ratio;
		}
		private static float easeIn(float ratio)
		{
			return ratio * ratio * ratio;
		}
		private static float easeOut(float ratio)
		{
			float invRatio = ratio - 1.0f;
			return invRatio * invRatio * invRatio + 1;
		}
		private static float easeInOut(float ratio)
		{
			return easeCombined(easeIn, easeOut, ratio);
		}
		private static float easeOutIn(float ratio)
		{
			return easeCombined(easeOut, easeIn, ratio);
		}
		private static float easeInBack(float ratio)
		{
			float s = 1.70158f;
			return AsMath.pow(ratio, 2) * ((s + 1.0f) * ratio - s);
		}
		private static float easeOutBack(float ratio)
		{
			float invRatio = ratio - 1.0f;
			float s = 1.70158f;
			return AsMath.pow(invRatio, 2) * ((s + 1.0f) * invRatio + s) + 1.0f;
		}
		private static float easeInOutBack(float ratio)
		{
			return easeCombined(easeInBack, easeOutBack, ratio);
		}
		private static float easeOutInBack(float ratio)
		{
			return easeCombined(easeOutBack, easeInBack, ratio);
		}
		private static float easeInElastic(float ratio)
		{
			if(ratio == 0 || ratio == 1)
			{
				return ratio;
			}
			else
			{
				float p = 0.3f;
				float s = p / 4.0f;
				float invRatio = ratio - 1;
				return -1.0f * AsMath.pow(2.0f, 10.0f * invRatio) * AsMath.sin((invRatio - s) * (2.0f * AsMath.PI) / p);
			}
		}
		private static float easeOutElastic(float ratio)
		{
			if(ratio == 0 || ratio == 1)
			{
				return ratio;
			}
			else
			{
				float p = 0.3f;
				float s = p / 4.0f;
				return AsMath.pow(2.0f, -10.0f * ratio) * AsMath.sin((ratio - s) * (2.0f * AsMath.PI) / p) + 1;
			}
		}
		private static float easeInOutElastic(float ratio)
		{
			return easeCombined(easeInElastic, easeOutElastic, ratio);
		}
		private static float easeOutInElastic(float ratio)
		{
			return easeCombined(easeOutElastic, easeInElastic, ratio);
		}
		private static float easeInBounce(float ratio)
		{
			return 1.0f - easeOutBounce(1.0f - ratio);
		}
		private static float easeOutBounce(float ratio)
		{
			float s = 7.5625f;
			float p = 2.75f;
			float l = 0;
			if(ratio < (1.0f / p))
			{
				l = s * AsMath.pow(ratio, 2);
			}
			else
			{
				if(ratio < (2.0f / p))
				{
					ratio = ratio - 1.5f / p;
					l = s * AsMath.pow(ratio, 2) + 0.75f;
				}
				else
				{
					if(ratio < 2.5f / p)
					{
						ratio = ratio - 2.25f / p;
						l = s * AsMath.pow(ratio, 2) + 0.9375f;
					}
					else
					{
						ratio = ratio - 2.625f / p;
						l = s * AsMath.pow(ratio, 2) + 0.984375f;
					}
				}
			}
			return l;
		}
		private static float easeInOutBounce(float ratio)
		{
			return easeCombined(easeInBounce, easeOutBounce, ratio);
		}
		private static float easeOutInBounce(float ratio)
		{
			return easeCombined(easeOutBounce, easeInBounce, ratio);
		}
		private static float easeCombined(AsTransitionCallback startFunc, AsTransitionCallback endFunc, float ratio)
		{
			if(ratio < 0.5f)
			{
				return 0.5f * startFunc(ratio * 2.0f);
			}
			else
			{
				return 0.5f * endFunc((ratio - 0.5f) * 2.0f) + 0.5f;
			}
		}
	}
}
