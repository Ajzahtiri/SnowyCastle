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
        public Vector2 screenPos, velocity, center;
        public Rectangle sourceRect;
        public L2Sprite enemy;
        public bool live;

        public Bullet(Texture2D tex)
        {
            texture = tex;
            center = new Vector2(texture.Width / 2, texture.Height / 2);
            live = false;
        }

        public Bullet(Texture2D tex, L2Sprite e)
        {
            texture = tex;
            center = new Vector2(texture.Width / 2, texture.Height / 2);
            live = false;
            this.enemy = e;
        }

        public virtual void Update(GameTime gameTime, Rectangle viewportRect)
        {
            screenPos += velocity;
            //right
            if (screenPos.X + sourceRect.Width / 2 > viewportRect.Right)
            {
                screenPos.X = viewportRect.Left + sourceRect.Width / 2;
            }

            //bottom
            if (screenPos.Y > viewportRect.Bottom)
            {
                screenPos = new Vector2(viewportRect.Bottom + 50, screenPos.Y);
            }

            //left
            if (screenPos.X < viewportRect.Left + sourceRect.Width / 2)
            {
                screenPos.X = viewportRect.Right - sourceRect.Width / 2;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, screenPos, null, Color.White, 0.0f, center, 1f, SpriteEffects.None, 1);
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


