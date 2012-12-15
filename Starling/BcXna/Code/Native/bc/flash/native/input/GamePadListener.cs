using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace bc.flash.native.input
{
    public struct ButtonEventArg
    {
        public Buttons button;
        public uint playerIndex;

        public ButtonEventArg(uint playerIndex, Buttons button)
        {
            this.playerIndex = playerIndex;
            this.button = button;
        }
    }

    public interface GamePadListener
    {
        void ButtonPressed(ButtonEventArg e);
        void ButtonReleased(ButtonEventArg e);
        void GamePadConnected(uint playerIndex);
        void GamePadDisconnected(uint playerIndex);
    }
}
