using System;
 
using bc.flash.display;
 
namespace bc.flash.display
{
	public class AsInteractiveObject : AsDisplayObject
	{
		private bool m_mouseEnabled;
		private bool m_doubleClickEnabled;
		private bool mFocusRect;
		public virtual bool getDoubleClickEnabled()
		{
			return m_doubleClickEnabled;
		}
		public virtual void setDoubleClickEnabled(bool enabled)
		{
			m_doubleClickEnabled = enabled;
		}
		public virtual bool getFocusRect()
		{
			return mFocusRect;
		}
		public virtual void setFocusRect(bool focusRect)
		{
			mFocusRect = focusRect;
		}
		public virtual bool getMouseEnabled()
		{
			return m_mouseEnabled;
		}
		public virtual void setMouseEnabled(bool enabled)
		{
			m_mouseEnabled = enabled;
		}
	}
}
