using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Snowy_Castle
{
    public class Level2 : GameScreen
    {
        #region Initialisation
        //logic 
        SpriteBatch spriteBatch;
        Rectangle viewportRect;
        ContentManager content;
        private int score;
        private int lives = 5;
        private bool death;
        float pauseGradient;

        //time
        private int counterTime = 0;
        private int elapsedTime = 0;
        private int secondTime = 1000;
        private int spawnTime = 2000;

        //background
        Texture2D background;

        //player
        Texture2D pTex;
        Sprite pSprite;

        //snowballs
        private List<Sprite> sbs, inactive, hit;
        private Texture2D eTex, e1, e2;

        //sounds
        private SoundEffect impact, ouch, lose;

        //text
        SpriteFont periclesFont;
        Vector2 textPosition = new Vector2(10, 10);
        Vector2 textPosition2 = new Vector2(10, 40);
        String scoreString = "Score: ";
        String livesString = "Lives: ";
        int countdown = 20;
        SoundEffectInstance impactInstance;

        public Level2()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }
        #endregion
        #region Load Content
        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            //background
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            background = content.Load<Texture2D>("Textures\\space");
            viewportRect = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);

            //enemies
            sbs = new List<Sprite>(1000);
            inactive = new List<Sprite>(1000);
            hit = new List<Sprite>(1000);
            e1 = content.Load<Texture2D>("Textures\\rock1");
            e2 = content.Load<Texture2D>("Textures\\rock2");
            for (int i = 0; i < 10; i++)
            {
                sbs.Add(CreateEnemy());
            }

            //player
            pTex = content.Load<Texture2D>("Textures\\pSnowball");
            pSprite = new AnimatedSprite(pTex,
                new Vector2(pTex.Height / 2, pTex.Height / 2),
                new Vector2(300, 420),
                new Rectangle(0, 0, pTex.Width, pTex.Height),
                new Vector2(0, 0));
            sbs.Add(pSprite);

            //sounds
            impact = content.Load<SoundEffect>("Sounds\\impact");
            ouch = content.Load<SoundEffect>("Sounds\\ow");
            lose = content.Load<SoundEffect>("Sounds\\u_lose");
            impactInstance = impact.CreateInstance();

            //text
            periclesFont = content.Load<SpriteFont>("Fonts\\Pericles");

        }

        private Sprite CreateEnemy()
        {
            Random rand = new Random();
            int whichEnemy;
            whichEnemy = rand.Next(0, 2);

            if (whichEnemy == 0)
            {
                eTex = e1;
            }
            else if (whichEnemy == 1)
            {
                eTex = e2;
            }

            return new Sprite(eTex,
                new Vector2(15, 15),
                new Vector2(
                    (float)rand.Next(0, viewportRect.Width),
                    (float)rand.Next(0, viewportRect.Height)),
                new Rectangle(0, 0, eTex.Width, eTex.Height),
                new Vector2(0, 0),
                rand.Next(1, 2));
        }

        public override void UnloadContent()
        {

        }
        #endregion
        #region Updates
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (coveredByOtherScreen)
            {
                pauseGradient = Math.Min(pauseGradient + 1f / 32, 1);
            }
            else
            {
                pauseGradient = Math.Max(pauseGradient - 1f / 32, 0);
            }

            if (IsActive)
            {
                Random rand = new Random();
                elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                counterTime += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsedTime > spawnTime)
                {
                    elapsedTime -= spawnTime;
                    sbs.Add(CreateEnemy());
                    spawnTime = rand.Next(300, 2000);
                }

                if (counterTime > secondTime)
                {
                    counterTime -= secondTime;
                    score++;
                }

                foreach (Sprite s in sbs)
                {
                    s.Update(gameTime, viewportRect);
                    if (s != pSprite)
                    {
                        if (s.CollidesWith(pSprite))
                        {
                            if (!s.getLanded() && !s.getCollided())
                            {
                                lives--;
                                ouch.Play();
                                s.setCollided();
                                hit.Add(s);
                            }

                            if (lives <= 0)
                            {
                                lose.Play();
                                death = true;
                                ScreenManager.AddScreen(new Loss(), null);
                            }
                        }

                        if (s.getLanded() && !s.getPlayed())
                        {
                            impactInstance.Play();
                            inactive.Add(s);
                            s.setPlayed();
                        }                        
                    }
                }

                foreach (Sprite s in hit)
                {
                    sbs.Remove(s);
                }

                foreach (Sprite s in inactive)
                {

                }
            }
        }

        public override void HandleInput(InputState input)
        {
            KeyboardState keyboardState = input.CurrentKeyboardState;
            GamePadState gamePadState = input.CurrentGamePadState;

            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseMenu(), ControllingPlayer);
            }
        }
        #endregion
        #region Draw
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, viewportRect, Color.White);
            spriteBatch.DrawString(periclesFont, (scoreString + score), textPosition, Color.Red);
            spriteBatch.DrawString(periclesFont, (livesString + lives), textPosition2, Color.Red);

            foreach (Sprite s in sbs)
            {
                s.Draw(gameTime, spriteBatch, Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}
