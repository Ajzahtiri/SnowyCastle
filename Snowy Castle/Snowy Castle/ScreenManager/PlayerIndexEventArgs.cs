using System;
using Microsoft.Xna.Framework;

namespace Snowy_Castle
{
    class PlayerIndexEventArgs : EventArgs
    {
        PlayerIndex playerIndex;
        public PlayerIndexEventArgs(PlayerIndex pi)
        {
            this.playerIndex = pi;
        }

        public PlayerIndex PlayerIndex
        {
            get 
            { 
                return playerIndex;
            }
        }        
    }
}