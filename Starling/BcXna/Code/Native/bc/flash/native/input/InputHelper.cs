using Microsoft.Xna.Framework.Input;

namespace bc.flash.native.input
{
    public class InputHelper
    {
        public static KeyCode GetKeyCode(Keys key)
        {
            return (KeyCode)key;
        }

        public static KeyCode GetKeyCode(Buttons button)
        {
            switch (button)
            {
                case Buttons.DPadUp:
                    return KeyCode.DPadUp;
                case Buttons.DPadDown:
                    return KeyCode.DPadDown;
                case Buttons.DPadLeft:
                    return KeyCode.DPadLeft;
                case Buttons.DPadRight:
                    return KeyCode.DPadRight;
                case Buttons.Start:
                    return KeyCode.Start;
                case Buttons.Back:
                    return KeyCode.Back;
                case Buttons.LeftStick:
                    return KeyCode.LeftStick;
                case Buttons.RightStick:
                    return KeyCode.RightStick;
                case Buttons.LeftShoulder:
                    return KeyCode.LeftShoulder;
                case Buttons.RightShoulder:
                    return KeyCode.RightShoulder;
                case Buttons.BigButton:
                    return KeyCode.BigButton;
                case Buttons.A:
                    return KeyCode.A;
                case Buttons.B:
                    return KeyCode.B;
                case Buttons.X:
                    return KeyCode.X;
                case Buttons.Y:
                    return KeyCode.Y;
                case Buttons.LeftThumbstickLeft:
                    return KeyCode.LeftThumbstickLeft;
                case Buttons.RightTrigger:
                    return KeyCode.RightTrigger;
                case Buttons.LeftTrigger:
                    return KeyCode.LeftTrigger;
                case Buttons.RightThumbstickUp:
                    return KeyCode.RightThumbstickUp;
                case Buttons.RightThumbstickDown:
                    return KeyCode.RightThumbstickDown;
                case Buttons.RightThumbstickRight:
                    return KeyCode.RightThumbstickRight;
                case Buttons.RightThumbstickLeft:
                    return KeyCode.RightThumbstickLeft;
                case Buttons.LeftThumbstickUp:
                    return KeyCode.LeftThumbstickUp;
                case Buttons.LeftThumbstickDown:
                    return KeyCode.LeftThumbstickDown;
                case Buttons.LeftThumbstickRight:
                    return KeyCode.LeftThumbstickRight;
            }

            return KeyCode.None;
        }

        public static KeyAction GetKeyAction(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                    return KeyAction.UP;
                case Keys.Down:
                    return KeyAction.DOWN;
                case Keys.Left:
                    return KeyAction.LEFT;
                case Keys.Right:
                    return KeyAction.RIGHT;
                case Keys.Enter:
                    return KeyAction.OK;
                case Keys.Escape:
                    return KeyAction.BACK;
            }

            return KeyAction.NONE;
        }

        public static KeyAction GetKeyAction(Buttons button)
        {
            switch (button)
            {
                case Buttons.DPadUp:
                case Buttons.LeftThumbstickUp:
                    return KeyAction.UP;
                case Buttons.DPadDown:
                case Buttons.LeftThumbstickDown:
                    return KeyAction.DOWN;
                case Buttons.DPadLeft:
                case Buttons.LeftThumbstickLeft:
                    return KeyAction.LEFT;
                case Buttons.DPadRight:
                case Buttons.LeftThumbstickRight:
                    return KeyAction.RIGHT;
                case Buttons.Start:
                case Buttons.A:
                    return KeyAction.OK;
                case Buttons.Back:
                case Buttons.B:
                    return KeyAction.BACK;
            }

            return KeyAction.NONE;
        }

        public static int GetPlayerIndex(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.Enter:
                case Keys.Escape:
                    return 0;
            }

            return 0;
        }
    }
}
