﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snowy_Castle
{
    class L2Player : L2Sprite
    {
        public L2Player(Texture2D tex, Vector2 centre, Vector2 pos, Rectangle sourceRect, Vector2 vel) : base(tex, centre, pos, sourceRect, vel)
        {
            health = 100;
        }

        public override void Update(GameTime gameTime, Rectangle viewportRect)
        {
            ProcessInput();
            base.Update(gameTime, viewportRect);
            velocity *= 0.90f;
        }

        protected void ProcessInput()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            velocity.X += (gamePadState.ThumbSticks.Left.X / 2);


            #if !XBOX
            KeyboardState keyboardState = Keyboard.GetState();
            
            if (keyboardState.IsKeyDown(Keys.A))
            {
                velocity.X -= 0.5f;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                velocity.X += 0.5f;

            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb, Color col, float rot)
        {
            sb.Draw(texture, screenPos, sourceRect, col, rot, centre, this.size, SpriteEffects.None, 0);
        }
    }
}

#endif