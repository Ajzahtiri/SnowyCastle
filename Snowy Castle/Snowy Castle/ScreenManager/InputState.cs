using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Snowy_Castle
{
    public class inputState
    {
        public KeyboardState currentKeyboardState;
        public KeyboardState lastKeyboardState;
        public GamePadState currentGamePadState;        
        public GamePadState lastGamePadState;

        public bool gamePadWasConnected;

        public inputState()
        {
            currentKeyboardState = new KeyboardState();
            lastKeyboardState = new KeyboardState();
            currentGamePadState = new GamePadState();            
            lastGamePadState = new GamePadState();

            gamePadWasConnected = new bool();
        }
        public void Update()
        {
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState((PlayerIndex.One));
            lastGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState((PlayerIndex.One));

            if (currentGamePadState.IsConnected)
            {
                gamePadWasConnected = true;
            }
        }
        public bool isNewKeyPress(Keys key, PlayerIndex? thisPlayer, out PlayerIndex pIndex)
        {
            if (thisPlayer.HasValue)
            {
                pIndex = thisPlayer.Value;
                int i = (int)pIndex;

                return (currentKeyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key));
            }
            else
            {
                return (isNewKeyPress(key, PlayerIndex.One, out pIndex));
            }
        }
        public bool isNewButtonPress(Buttons b, PlayerIndex? thisPlayer, out PlayerIndex pIndex)
        {
            if (thisPlayer.HasValue)
            {
                pIndex = thisPlayer.Value;
                int i = (int)pIndex;
                return (currentGamePadState.IsButtonDown(b) && lastGamePadState.IsButtonUp(b));
            }
            else
            {
                return (isNewButtonPress(b, PlayerIndex.One, out pIndex));
            }
        }
        public bool isSelect(PlayerIndex? thisPlayer, out PlayerIndex pIndex)
        {
            return isNewKeyPress(Keys.Space, thisPlayer, out pIndex) || isNewKeyPress(Keys.Enter, thisPlayer, out pIndex) || isNewButtonPress(Buttons.A, thisPlayer, out pIndex) || isNewButtonPress(Buttons.Start, thisPlayer, out pIndex);
        }
        public bool isCancel(PlayerIndex? thisPlayer, out PlayerIndex pIndex)
        {
            return isNewKeyPress(Keys.Escape, thisPlayer, out pIndex) || isNewButtonPress(Buttons.B, thisPlayer, out pIndex) || isNewButtonPress(Buttons.Back, thisPlayer, out pIndex);
        }
        public bool isMenuUp(PlayerIndex? thisPlayer)
        {
            PlayerIndex pIndex;
            return isNewKeyPress(Keys.Up, thisPlayer, out pIndex) || isNewButtonPress(Buttons.DPadUp, thisPlayer, out pIndex) || isNewButtonPress(Buttons.LeftThumbstickUp, thisPlayer, out pIndex);
        }
        public bool isMenuDown(PlayerIndex? thisPlayer)
        {
            PlayerIndex pIndex;

            return isNewKeyPress(Keys.Down, thisPlayer, out pIndex) || isNewButtonPress(Buttons.DPadDown, thisPlayer, out pIndex) || isNewButtonPress(Buttons.LeftThumbstickDown, thisPlayer, out pIndex);
        }
        public bool isPause(PlayerIndex? thisPlayer)
        {
            PlayerIndex pIndex;

            return isNewKeyPress(Keys.Escape, thisPlayer, out pIndex) || isNewButtonPress(Buttons.Back, thisPlayer, out pIndex) || isNewButtonPress(Buttons.Start, thisPlayer, out pIndex);
        }
    }
}
