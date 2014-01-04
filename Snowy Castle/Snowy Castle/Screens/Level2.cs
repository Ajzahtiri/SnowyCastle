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
        private float rotation = 0;
        Rectangle viewportRect;
        ContentManager content;
        private int score;
        private int lives = 5;
        float pauseGradient;

        //time
        private int elapsedTime = 0;
        private int elapsedTime2 = 0;
        private int elapsedTime3 = 0;
        private int secondTime = 1000;
        private int spawnTime = 1;
        private int timeLeft = 600;
        private int rotationTime;

        //player
        Texture2D pTex;
        L2Sprite pSprite;

        //background
        private Background background;

        //textures        
        private Texture2D eTex;

        //lists 
        private List<L2Sprite> sbs, inactive, hit;
        private List<Bullet> allBullets = new List<Bullet>(100);
        private List<Bullet> deadBullets = new List<Bullet>(10000000);

        //sounds
        private SoundEffect impact, ouch, lose;

        //text
        SpriteFont periclesFont;
        Vector2 textPosition = new Vector2(10, 10);
        Vector2 textPosition2 = new Vector2(10, 40);
        Vector2 textPosition3 = new Vector2(10, 70);
        String scoreString = "Score: ";
        String livesString = "Lives: ";
        String timeString = "Time left: ";
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
            background = new Background();
            Texture2D bg = content.Load<Texture2D>("Textures\\space");
            background.Load(ScreenManager.GraphicsDevice, bg);
            viewportRect = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);


            //enemies
            sbs = new List<L2Sprite>(1000);
            inactive = new List<L2Sprite>(1000);
            hit = new List<L2Sprite>(1000);
            eTex = content.Load<Texture2D>("Textures\\eSpaceship");
            for (int i = 0; i < 10; i++)
            {
                sbs.Add(CreateEnemy());
            }

            //player
            pTex = content.Load<Texture2D>("Textures\\pSpaceship");
            pSprite = new L2Player(pTex,
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

        private L2Sprite CreateEnemy()
        {
            Random rand = new Random();
            return new L2Sprite(eTex, new Vector2(15, 15), new Vector2((float)rand.Next(0, viewportRect.Width), (float)-30), new Rectangle(0, 0, eTex.Width, eTex.Height), new Vector2(0, 1));
        }

        public override void UnloadContent()
        {

        }
        #endregion

        #region Updates
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            background.Update(2);

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
                elapsedTime2 += gameTime.ElapsedGameTime.Milliseconds;

                //spawn enemy timer
                if (elapsedTime > spawnTime)
                {
                    elapsedTime -= spawnTime;
                    sbs.Add(CreateEnemy());
                    spawnTime = rand.Next(timeLeft*5, timeLeft * 10);
                }

                //update time counter
                if (elapsedTime2 > secondTime)
                {
                    elapsedTime2 -= secondTime;
                    timeLeft--;
                    score++;
                }

                //check for collisions
                foreach (L2Sprite s in sbs)
                {
                    s.Update(gameTime, viewportRect);
                    elapsedTime3 += gameTime.ElapsedGameTime.Milliseconds;
                    rotationTime = rand.Next(1, 4);
                 

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
                        }

                        foreach (Bullet b in allBullets)
                        {
                            if (b.isVisible)
                            {
                                if (b.CollidesWith(s))
                                {
                                    s.minusHealth();
                                    if (s.getHealth() <= 0)
                                    {
                                        s.setCollided();
                                        hit.Add(s);
                                        score += 25;
                                    }
                                    deadBullets.Add(b);
                                }
                            }
                        }
                    }
                }

                //check for death
                if (lives <= 0)
                {
                    lose.Play();
                    ScreenManager.AddScreen(new Loss(), null);
                }

                foreach (L2Sprite s in hit)
                {
                    sbs.Remove(s);
                }

                foreach (L2Sprite s in inactive)
                {
                    sbs.Remove(s);
                }

                foreach (Bullet b in deadBullets)
                {
                    allBullets.Remove(b);
                }
            }

            UpdateBullets();
        }

        public void UpdateBullets()
        {
            foreach (Bullet b in allBullets)
            {
                b.screenPos += b.velocity;
                if (Vector2.Distance(b.screenPos, pSprite.getPos()) > 600)
                {
                    b.isVisible = false;
                }
            }

            for (int i = 0; i < allBullets.Count; i++)
            {
                if (!allBullets[i].isVisible)
                {
                    allBullets.RemoveAt(i);
                    i--;
                }
            }
        }

        public void PlayerShoot()
        {
            Bullet newBullet = new Bullet((content.Load<Texture2D>("Textures\\pBullet")));
            newBullet.velocity = new Vector2((float)Math.Sin(rotation), (float)Math.Cos(rotation)) * new Vector2(5f, -5f) + pSprite.getVel();
            newBullet.screenPos = pSprite.getPos() + newBullet.velocity * 5;
            newBullet.isVisible = true;

            if (allBullets.Count < 100)
            {
                allBullets.Add(newBullet);
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

            if (keyboardState.IsKeyDown(Keys.Space) || gamePadState.Triggers.Right > 0)
            {
                PlayerShoot();
            }

            if (keyboardState.IsKeyDown(Keys.Left) || gamePadState.ThumbSticks.Right.X < 0)
            {
                if (rotation < -0.8)
                {
                    rotation = (float)-0.8;
                }
                else
                {
                    rotation -= 0.05f;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Right) || gamePadState.ThumbSticks.Right.X > 0)
            {
                if (rotation > 0.8)
                {
                    rotation = (float)0.8;
                }
                else
                {
                    rotation += 0.05f;
                }
            }
        }
        #endregion

        #region Draw
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            background.Draw(spriteBatch);
            spriteBatch.DrawString(periclesFont, (scoreString + score), textPosition, Color.Red);
            spriteBatch.DrawString(periclesFont, (livesString + lives), textPosition2, Color.Red);
            spriteBatch.DrawString(periclesFont, (timeString + timeLeft), textPosition3, Color.Red);

            foreach (L2Sprite s in sbs)
            {
                if (s == pSprite)
                {
                    s.Draw(gameTime, spriteBatch, Color.White, rotation);
                }
                else
                {
                    s.Draw(gameTime, spriteBatch, Color.White);
                }
            }

            foreach (Bullet b in allBullets)
            {
                b.Draw(spriteBatch);
                b.Update(gameTime, viewportRect);                
            }
          
            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}
