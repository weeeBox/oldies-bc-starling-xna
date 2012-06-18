using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bc.flash.native.input
{
    public interface TouchListener
    {
        void PointerMoved(int x, int y);
        void PointerPressed(int x, int y);
        void PointerDragged(int x, int y);
        void PointerReleased(int x, int y);
    }
}
