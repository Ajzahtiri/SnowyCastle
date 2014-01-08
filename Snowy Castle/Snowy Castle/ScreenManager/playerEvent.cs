using System;
using Microsoft.Xna.Framework;

namespace Snowy_Castle
{
    class playerEvent : EventArgs
    {
        PlayerIndex pI;
        public playerEvent(PlayerIndex pi)
        {
            this.pI = pi;
        }
        public PlayerIndex pIndex
        {
            get 
            { 
                return pI;
            }
        }        
    }
}