using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snowy_Castle
{
    public class Background
    {
        private Vector2 screenPos, center, bgSize;
        private Texture2D myBg;
        private int screenHeight;
        private int screenWidth;
        public void Load(GraphicsDevice d, Texture2D bgTexture)
        {
            myBg = bgTexture;
            screenHeight = d.Viewport.Height;
            screenWidth = d.Viewport.Width;
            center = new Vector2(0, screenHeight / 2);
            screenPos = new Vector2(screenWidth / 2, screenHeight / 2);
            bgSize = new Vector2(myBg.Width, 0);
        }

        public void Update(float d)
        {
            screenPos.X += d;
            screenPos.X = screenPos.X % myBg.Width;
        }

        public void Draw(SpriteBatch b)
        {
            if (screenPos.X < screenWidth)
            {
                b.Draw(myBg, screenPos, null, Color.White, 0, center, 1, SpriteEffects.None, 0f);
            }
            b.Draw(myBg, screenPos - bgSize, null, Color.White, 0, center, 1, SpriteEffects.None, 0f);
        }
    }
}
