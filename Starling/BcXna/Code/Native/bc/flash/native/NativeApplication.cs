using bc.flash.native.input;
using bc.flash.resources;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using bc.flash.display;
using System;
using bc.flash.core;
using bc.flash.input;

namespace bc.flash.native
{
    public class NativeApplication : GamePadListener, KeyboardListener, TouchListener
    {
        // private Application application;        
        private AsRenderSupport renderSupport;

        private NativeInput input;
        private BcResFactory resFactory;
        private AsStage stage;

        private bool running;

        public NativeApplication(int width, int height, ContentManager content)
        {
            resFactory = new BcResFactory(content);

            renderSupport = new AsRenderSupport();

            input = new NativeInput();
            input.AddGamePadListener(this);
            input.AddKeyboardListener(this);
            input.AddTouchListener(this);            
            running = true;

            stage = new AsStage(width, height);
        }        
        
        public void Stop()
        {
            running = false;
        }                

        public void Tick(float deltaTime)
        {
            input.Tick();
            updateGamePads();

            stage.tick(deltaTime);
        }

        private void updateGamePads()
        {
            for (uint playerIndex = 0; playerIndex < input.getPlayersCount(); ++playerIndex)
            {
                AsGamePad gamePad = AsGamePad.player(playerIndex);
                GamePadTriggers triggers = input.Triggers(playerIndex);
                GamePadThumbSticks sticks = input.ThumbSticks(playerIndex);
                gamePad.update(triggers.Left, triggers.Right);
                gamePad.getLeftStick().update(sticks.Left.X, sticks.Left.Y);
                gamePad.getRightStick().update(sticks.Right.X, sticks.Right.Y);
            }
        }

        public void Draw(GraphicsDevice device)
        {
            BcRenderSupport.Begin(device, stage.getStageWidth(), stage.getStageHeight());
            stage.render(renderSupport, 1.0f);
            BcRenderSupport.End();
        }

        public void PointerMoved(int x, int y)
        {
            stage.touchMove(x, y, 0);
        }

        public void PointerPressed(int x, int y)
        {            
            stage.touchDown(x, y, 0);
        }

        public void PointerDragged(int x, int y)
        {            
            stage.touchDragged(x, y, 0);
        }

        public void PointerReleased(int x, int y)
        {            
            stage.touchUp(x, y, 0);
        }

        public void ButtonPressed(ButtonEventArg e)
        {
            uint playerIndex = e.playerIndex;
            uint code = (uint)e.button;           
            
            stage.buttonPressed(playerIndex, code);
        }

        public void ButtonReleased(ButtonEventArg e)
        {
            uint playerIndex = e.playerIndex;
            uint code = (uint)e.button;

            stage.buttonReleased(playerIndex, code);
        }

        public void GamePadConnected(uint playerIndex)
        {
            // application.GamePadConnected(playerIndex);
            stage.gamePadConnected(playerIndex);
        }

        public void GamePadDisconnected(uint playerIndex)
        {
            stage.gamePadDisconnected(playerIndex);
        }

        public void KeyPressed(Keys key)
        {
            // int playerIndex = InputHelper.GetPlayerIndex(key);
            KeyCode code = InputHelper.GetKeyCode(key);
            // KeyAction action = InputHelper.GetKeyAction(key);
            // application.KeyPressed(new KeyEvent(playerIndex, code, action));
            stage.keyPressed((uint)code);
        }

        public void KeyReleased(Keys key)
        {
            // int playerIndex = InputHelper.GetPlayerIndex(key);
            KeyCode code = InputHelper.GetKeyCode(key);
            // KeyAction action = InputHelper.GetKeyAction(key);
            // application.KeyReleased(new KeyEvent(playerIndex, code, action));
            stage.keyReleased((uint)code);
        }

        public bool isRunning()
        {
            return running;
        }

        public void Dispose()
        {            
            resFactory.Dispose();
            input.Dispose();
        }
    }
}
