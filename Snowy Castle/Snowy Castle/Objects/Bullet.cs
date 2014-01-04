using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Snowy_Castle
{
    public class Bullet
    {
        public Texture2D texture;
        public Vector2 screenPos;
        public Rectangle sourceRect;
        public Vector2 velocity;
        public Vector2 origin;
        public bool isVisible;

        public Bullet(Texture2D newTexture)
        {
            texture = newTexture;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            isVisible = false;
        }

        public virtual void Update(GameTime gameTime, Rectangle viewportRect)
        {
            screenPos += velocity;
            //right
            if (screenPos.X + sourceRect.Width / 2 > viewportRect.Right)
            {
                // velocity.X *= -1;
                screenPos.X = viewportRect.Left + sourceRect.Width / 2;
            }

            //bottom
            if (screenPos.Y > viewportRect.Bottom)
            {
                velocity.Y *= 0;
                velocity.X *= 0;
                screenPos.Y = viewportRect.Height + 50;

            }

            //left
            if (screenPos.X < viewportRect.Left + sourceRect.Width / 2)
            {
                //  velocity.X *= -1;
                screenPos.X = viewportRect.Right - sourceRect.Width / 2;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, screenPos, null, Color.White, 0.0f, origin, 1f, SpriteEffects.None, 1);
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)Math.Round(screenPos.X) - sourceRect.Width / 2,
                    (int)Math.Round(screenPos.Y) - sourceRect.Height / 2,
                    sourceRect.Width, sourceRect.Height);
            }
        }

        public virtual bool CollidesWith(L2Sprite sprite)
        {
            return this.BoundingBox.Intersects(sprite.BoundingBox);
        }
    }
}


