using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snowy_Castle
{
    class L2Sprite
    {
        protected Texture2D texture;
        protected Vector2 centre, velocity, screenPos;
        protected Rectangle sourceRect;
        protected float size;
        private bool facingBackwards = true;
        protected bool hasCollided;
        protected bool hasLanded;
        protected bool hasPlayed;


        public L2Sprite(Texture2D tex, Vector2 centre, Vector2 pos, Rectangle sourceRect, Vector2 vel)
        {
            texture = tex;
            this.centre = centre;
            this.screenPos = pos;
            this.sourceRect = sourceRect;
            this.velocity = vel;
            this.size = 1;
        }

        public L2Sprite(Texture2D tex, Vector2 centre, Vector2 pos, Rectangle sourceRect, Vector2 vel, float size)
        {
            texture = tex;
            this.centre = centre;
            this.screenPos = pos;
            this.sourceRect = sourceRect;
            this.velocity = vel;
            this.size = size;
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

        public void setBackwards(bool b)
        {
            this.facingBackwards = b;
        }

        public bool getBackwards()
        {
            return facingBackwards;
        }

        public bool getCollided()
        {
            return hasCollided;
        }

        public bool getPlayed()
        {
            return hasPlayed;
        }

        public void setPlayed()
        {
            this.hasPlayed = true;
        }

        public bool getLanded()
        {
            return hasLanded;
        }

        public void setCollided()
        {
            this.hasCollided = true;
        }

        public void setLanded(Boolean b)
        {
            this.hasLanded = b;
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
               // velocity.X *= -1;
                screenPos.X = viewportRect.Left + sourceRect.Width / 2;
            }

            //bottom
            if (screenPos.Y + sourceRect.Height / 2 > viewportRect.Bottom)
            {
                velocity.Y *= 0;
                velocity.X *= 0;
                screenPos.Y = viewportRect.Bottom - sourceRect.Height / 2;
                setLanded(true);
                velocity.X = 0;

            }

            //left
            if (screenPos.X < viewportRect.Left + sourceRect.Width / 2)
            {
              //  velocity.X *= -1;
                screenPos.X = viewportRect.Right - sourceRect.Width / 2;
            }

            //top
            if (screenPos.Y < viewportRect.Top + sourceRect.Width / 2)
            {
                velocity.Y *= -1;
                screenPos.Y = viewportRect.Top + sourceRect.Width / 2;
            }


        }

        public virtual void Draw(GameTime gameTime, SpriteBatch sb, Color col, float rotation)
        {
            if (!facingBackwards)
            {
                sb.Draw(texture, screenPos, sourceRect, col, rotation, centre, this.size, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                sb.Draw(texture, screenPos, sourceRect, col, rotation, centre, this.size, SpriteEffects.None, 0);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch sb, Color col)
        {
            if (!facingBackwards)
            {
                sb.Draw(texture, screenPos, sourceRect, col, 0.0f, centre, this.size, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                sb.Draw(texture, screenPos, sourceRect, col, 0.0f, centre, this.size, SpriteEffects.None, 0);
            }
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
