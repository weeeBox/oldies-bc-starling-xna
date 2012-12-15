using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace bc.flash.native.input
{
    public class NativeInput
    {
        private static Buttons[] GAMEPAD_BUTTONS = 
        {
            Buttons.DPadUp,
            Buttons.DPadDown,
            Buttons.DPadLeft,
            Buttons.DPadRight,
            Buttons.Start,
            Buttons.Back,
            Buttons.LeftStick,
            Buttons.RightStick,
            Buttons.LeftShoulder,
            Buttons.RightShoulder,
            Buttons.BigButton,
            Buttons.A,
            Buttons.B,
            Buttons.X,
            Buttons.Y,
            Buttons.LeftThumbstickLeft,
            Buttons.RightTrigger,
            Buttons.LeftTrigger,
            Buttons.RightThumbstickUp,
            Buttons.RightThumbstickDown,
            Buttons.RightThumbstickRight,
            Buttons.RightThumbstickLeft,
            Buttons.LeftThumbstickUp,
            Buttons.LeftThumbstickDown,
            Buttons.LeftThumbstickRight,
        };

#if WINDOWSPHONE
        private const int MAX_PLAYERS = 1;
#else
        private const int MAX_PLAYERS = 4;
#endif

        private GamePadState[] currentGamePadStates;
        private KeyboardState currentKeyboardState;
        private List<GamePadListener> gamePadListeners;
        private List<KeyboardListener> keyboardListeners;
        private List<TouchListener> touchListeners;

        private GamePadDeadZone deadZone;

        public NativeInput()
        {
            currentGamePadStates = new GamePadState[MAX_PLAYERS];
            deadZone = GamePadDeadZone.Circular;

            for (int playerIndex = 0; playerIndex < MAX_PLAYERS; ++playerIndex)
            {
                PlayerIndex player = (PlayerIndex)playerIndex;
                currentGamePadStates[playerIndex] = GamePad.GetState(player, deadZone);
            }

            currentKeyboardState = Keyboard.GetState();
            gamePadListeners = new List<GamePadListener>();
            keyboardListeners = new List<KeyboardListener>();
            touchListeners = new List<TouchListener>();
        }

        public void Tick()
        {
            #if !WINDOWSPHONE
            UpdateGamePad();
            #endif

            #if WINDOWS
            UpdateKeyboard();
            #endif

            #if WINDOWS || WINDOWSPHONE
            UpdatePointer();
            #endif
        }

        public int getPlayersCount()
        {
            return currentGamePadStates.Length;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        /// GamePad        
        /////////////////////////////////////////////////////////////////////////////////////////////

        private void UpdateGamePad()
        {
            for (uint playerIndex = 0; playerIndex < getPlayersCount(); ++playerIndex)
            {
                UpdateGamePad(playerIndex);
            }
        }

        private void UpdateGamePad(uint playerIndex)
        {
            GamePadState oldState = currentGamePadStates[playerIndex];
            currentGamePadStates[playerIndex] = GamePad.GetState((PlayerIndex)playerIndex, deadZone);

            if (gamePadListeners.Count > 0)
            {
                if (IsGamePadConnected(ref oldState, ref currentGamePadStates[playerIndex]))
                {
                    FireGamePadConnected(playerIndex);
                }
                else if (IsGamePadrDisconnected(ref oldState, ref currentGamePadStates[playerIndex]))
                {
                    FireGamePadDisconnected(playerIndex);
                }

                foreach (Buttons button in GAMEPAD_BUTTONS)
                {
                    if (IsButtonDown(button, ref oldState, ref currentGamePadStates[playerIndex]))
                    {
                        FireButtonPressed(new ButtonEventArg(playerIndex, button));
                    }
                    else if (IsButtonUp(button, ref oldState, ref currentGamePadStates[playerIndex]))
                    {
                        FireButtonReleased(new ButtonEventArg(playerIndex, button));
                    }
                }
            }
        }

        private bool IsGamePadConnected(ref GamePadState oldState, ref GamePadState newState)
        {
            return newState.IsConnected && !oldState.IsConnected;
        }

        private bool IsGamePadrDisconnected(ref GamePadState oldState, ref GamePadState newState)
        {
            return !newState.IsConnected && oldState.IsConnected;
        }

        private bool IsButtonDown(Buttons button, ref GamePadState oldState, ref GamePadState newState)
        {
            return newState.IsButtonDown(button) && oldState.IsButtonUp(button);
        }

        private bool IsButtonUp(Buttons button, ref GamePadState oldState, ref GamePadState newState)
        {
            return newState.IsButtonUp(button) && oldState.IsButtonDown(button);
        }

        public void AddGamePadListener(GamePadListener listener)
        {
            gamePadListeners.Add(listener);
        }

        public void RemoveGamePadListener(GamePadListener listener)
        {
            gamePadListeners.Remove(listener);
        }

        private void FireGamePadConnected(uint playerIndex)
        {
            foreach (GamePadListener l in gamePadListeners)
            {
                l.GamePadConnected(playerIndex);
            }
        }

        private void FireGamePadDisconnected(uint playerIndex)
        {
            foreach (GamePadListener l in gamePadListeners)
            {
                l.GamePadDisconnected(playerIndex);
            }
        }

        private void FireButtonPressed(ButtonEventArg arg)
        {
            foreach (GamePadListener l in gamePadListeners)
            {
                l.ButtonPressed(arg);
            }
        }

        private void FireButtonReleased(ButtonEventArg e)
        {
            foreach (GamePadListener l in gamePadListeners)
            {
                l.ButtonReleased(e);
            }
        }

        public GamePadThumbSticks ThumbSticks(uint playerIndex)
        {
            Debug.Assert(playerIndex >= 0 && playerIndex < getPlayersCount());
            return currentGamePadStates[playerIndex].ThumbSticks;
        }

        public GamePadTriggers Triggers()
        {
            return Triggers(0);
        }

        public GamePadTriggers Triggers(uint playerIndex)
        {
            Debug.Assert(playerIndex >= 0 && playerIndex < getPlayersCount());
            return currentGamePadStates[playerIndex].Triggers;
        }

        public bool IsGamePadConnected(uint playerIndex)
        {
#if WINDOWS
            return true;
#else
            Debug.Assert(playerIndex >= 0 && playerIndex < getPlayersCount());
            return currentGamePadStates[playerIndex].IsConnected;
#endif
        }

        private int GetPlayerIndex(ref PlayerIndex player)
        {
            return (int)player;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        /// Keyboard        
        /////////////////////////////////////////////////////////////////////////////////////////////

        private void UpdateKeyboard()
        {
            KeyboardState oldState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (keyboardListeners.Count > 0)
            {
                Keys[] oldKeys = oldState.GetPressedKeys();
                Keys[] newKeys = currentKeyboardState.GetPressedKeys();

                for (int i = 0; i < newKeys.Length; ++i)
                {
                    if (!oldKeys.Contains(newKeys[i]))
                    {
                        FireKeyPressed(newKeys[i]);
                    }
                }
                for (int i = 0; i < oldKeys.Length; ++i)
                {
                    if (!newKeys.Contains(oldKeys[i]))
                    {
                        FireKeyReleased(oldKeys[i]);
                    }
                }
            }
        }

        public void AddKeyboardListener(KeyboardListener listener)
        {
            keyboardListeners.Add(listener);
        }

        public void RemoveKeyboardListener(KeyboardListener listener)
        {
            keyboardListeners.Remove(listener);
        }

        private void FireKeyPressed(Keys key)
        {
            foreach (KeyboardListener l in keyboardListeners)
            {
                l.KeyPressed(key);
            }
        }

        private void FireKeyReleased(Keys key)
        {
            foreach (KeyboardListener l in keyboardListeners)
            {
                l.KeyReleased(key);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        /// Touch
        /////////////////////////////////////////////////////////////////////////////////////////////

        private bool pointerDown;
        private int pointerLastX;
        private int pointerLastY;

        private void UpdatePointer()
        {
            ButtonState buttonState = Mouse.GetState().LeftButton;

            if (pointerDown)
            {
                if (buttonState == ButtonState.Released)
                {
                    pointerDown = false;
                    pointerLastX = Mouse.GetState().X;
                    pointerLastY = Mouse.GetState().Y;
                    FilePointerReleased(pointerLastX, pointerLastY);
                }
                else
                {
                    int pointerX = Mouse.GetState().X;
                    int pointerY = Mouse.GetState().Y;
                    if (pointerX != pointerLastX || pointerY != pointerLastY)
                    {
                        FirePointerDragged(pointerX, pointerY);
                    }
                    pointerLastX = pointerX;
                    pointerLastY = pointerY;
                }
            }
            else
            {
                if (buttonState == ButtonState.Pressed)
                {
                    pointerDown = true;
                    pointerLastX = Mouse.GetState().X;
                    pointerLastY = Mouse.GetState().Y;
                    FirePointerPressed(pointerLastX, pointerLastY);
                }
#if WINDOWS
                else if (buttonState == ButtonState.Released)
                {
                    int pointerX = Mouse.GetState().X;
                    int pointerY = Mouse.GetState().Y;
                    if (pointerX != pointerLastX || pointerY != pointerLastY)
                    {
                        FirePointerMoved(pointerX, pointerY);
                    }
                    pointerLastX = pointerX;
                    pointerLastY = pointerY;
                }
#endif // WINDOWS
            }
        }

        public void AddTouchListener(TouchListener listner)
        {
            touchListeners.Add(listner);
        }

        public void RemoveTouchListener(TouchListener listener)
        {
            touchListeners.Remove(listener);
        }

        private void FirePointerMoved(int x, int y)
        {
            foreach (TouchListener l in touchListeners)
            {
                l.PointerMoved(x, y);
            }
        }

        private void FilePointerReleased(int x, int y)
        {
            foreach (TouchListener l in touchListeners)
            {
                l.PointerReleased(x, y);
            }
        }

        private void FirePointerDragged(int x, int y)
        {
            foreach (TouchListener l in touchListeners)
            {
                l.PointerDragged(x, y);
            }
        }

        private void FirePointerPressed(int x, int y)
        {
            foreach (TouchListener l in touchListeners)
            {
                l.PointerPressed(x, y);
            }
        }

        public void Dispose()
        {            
        }
    }
}
