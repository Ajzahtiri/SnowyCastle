using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snowy_Castle
{
    class L1Player : L1Sprite
    {
        private int rows;
        private int columns;
        private int frames;
        private int currentFrame;
        private int elapsedTime = 0;
        private int setTime = 75;
        private int level = 1;

        //level 2
        public L1Player(Texture2D tex, Vector2 centre, Vector2 pos, Rectangle sourceRect, Vector2 vel) : base(tex, centre, pos, sourceRect, vel)
        {
            level = 2;
        }

        //level 1
        public L1Player(Texture2D tex, Vector2 centre, Vector2 pos, Rectangle sourceRect, Vector2 vel, int rows, int columns, int frames):  base(tex, centre, pos, sourceRect, vel)
        {
            this.rows = rows;
            this.columns = columns;
            this.frames = frames;
            currentFrame = -1;
            level = 1;
        }

        public override void Update(GameTime gameTime, Rectangle viewportRect)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (level == 1)
            {
                if (elapsedTime > setTime)
                {
                    elapsedTime -= setTime;
                    currentFrame++;
                    currentFrame %= frames;
                }

                sourceRect.X = (currentFrame % columns) * sourceRect.Width;
                sourceRect.Y = (currentFrame / columns) * sourceRect.Height;
            }
            
            ProcessInput();
            base.Update(gameTime, viewportRect);
            velocity *= 0.95f;            
        }

        protected void ProcessInput()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            if (gamePadState.ThumbSticks.Left.X > 0)
            {
                setBackwards(false);
            }
            else if (gamePadState.ThumbSticks.Left.X < 0)
            {
                setBackwards(true);
            }

            velocity.X += gamePadState.ThumbSticks.Left.X;
                

        #if !XBOX
            KeyboardState keyboardState = Keyboard.GetState();
          

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                velocity.X -= 1.0f;
                setBackwards(true);
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                velocity.X += 1.0f;
                setBackwards(false);
                
            }
        }
    }
}

#endif