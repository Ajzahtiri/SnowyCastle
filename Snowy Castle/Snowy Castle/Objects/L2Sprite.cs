using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snowy_Castle
{
    public class L2Sprite
    {
        protected Texture2D texture;
        protected Vector2 centre, velocity, screenPos;
        protected Rectangle sourceRect;
        public float rotation;
        protected float size;
        protected int health = 10;
        private bool facingBackwards = true;
        protected bool hasCollided, hasLanded, hasPlayed, toDie;

        public L2Sprite(Texture2D tex, Vector2 centre, Vector2 pos, Rectangle sourceRect, Vector2 vel)
        {
            texture = tex;
            this.centre = centre;
            this.screenPos = pos;
            rotation = 0;
            this.sourceRect = sourceRect;
            this.velocity = vel;
            this.size = 1;
            toDie = false;
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

        #region Get/sets
        public bool getDie()
        {
            return toDie;
        }

        public void setRotation(float f)
        {
            this.rotation += f;
        }

        public float getRotation()
        {
            return rotation;
        }

        public void minusHealth()
        {
            health--;
        }

        public int getHealth()
        {
            return health;
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

        public Vector2 getVel()
        {
            return velocity;
        }
        #endregion

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
                velocity.Y *= 0;
                velocity.X *= 0;
                screenPos.Y = viewportRect.Height + 50;
                toDie = true;
            }

            //left
            if (screenPos.X < viewportRect.Left + sourceRect.Width / 2)
            {
                screenPos.X = viewportRect.Right - sourceRect.Width / 2;
            }

            //top
            if (screenPos.Y < viewportRect.Top + sourceRect.Width / 2)
            {
                velocity.Y *= -1;
                screenPos.Y = viewportRect.Top + sourceRect.Width / 2;
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch sb, Color col)
        {
            sb.Draw(texture, screenPos, sourceRect, col, rotation, centre, this.size, SpriteEffects.None, 0);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch sb, Color col, float rot)
        {
            sb.Draw(texture, screenPos, sourceRect, col, rot, centre, this.size, SpriteEffects.None, 0);
        }
    }
}
