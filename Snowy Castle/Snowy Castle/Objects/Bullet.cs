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
    class Bullet
    {
        public bool alive = false;
        public Texture2D texture;
        public Vector2 centre, velocity, screenPos;
        public Rectangle sourceRect;

        public Bullet()
        {

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

        public virtual void Update(GameTime gameTime, Rectangle viewportRect)
        {
            screenPos += velocity;
            //right
            if (screenPos.X + sourceRect.Width / 2 > viewportRect.Right)
            {
                screenPos.X = viewportRect.Left + sourceRect.Width / 2;
            }

            //left
            if (screenPos.X < viewportRect.Left + sourceRect.Width / 2)
            {
                screenPos.X = viewportRect.Right - sourceRect.Width / 2;
            }

            //top
            if (screenPos.Y < viewportRect.Top + sourceRect.Width / 2)
            {
                screenPos.Y = viewportRect.Top + sourceRect.Width / 2;
                alive = false;
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch sb, Color col)
        {
            sb.Draw(texture, screenPos, sourceRect, col, 0.0f, centre, 1, SpriteEffects.None, 0);
        }

        public Vector2 getPos()
        {
            return screenPos;
        }

        public void setVelocityX(int x)
        {
            this.velocity.X = x;
        }

        public void incVelX()
        {
            this.velocity.X += 0.02f;
        }

        public void decVelX()
        {
            this.velocity.X -= 0.02f;
        }

        public void setVelocityY(int y)
        {
            this.velocity.Y = y;
        }
    }
}


