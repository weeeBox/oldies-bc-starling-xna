using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace bc.flash.native.input
{
    public interface KeyboardListener
    {
        void KeyPressed(Keys key);
        void KeyReleased(Keys key);
    }
}
