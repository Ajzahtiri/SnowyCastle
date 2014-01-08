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
    public class Level1 : gameScreen
    {
        #region Initialisation
        //logic 
        SpriteBatch spriteBatch;
        Rectangle viewportRect;
        ContentManager content;
        private int score;
        private bool death;
        float pauseGradient;
        Texture2D fadeRect;

        //time
        private int counterTime = 0;
        private int elapsedTime = 0;
        private int secondTime = 1000;
        private int spawnTime = 2000;

        //background
        Texture2D background;

        //player
        Texture2D pTex;
        L1Player pSprite;

        //snowballs
        private List<L1Sprite> sbs;
        private List<L1Sprite> inactive;
        private List<L1Sprite> hit;
        private Texture2D sbTex;

        //sounds
        private SoundEffect impact, ouch, lose;
        SoundEffectInstance impactInstance;

        //text
        SpriteFont periclesFont;
        Vector2 textPosition = new Vector2(10, 10);
        Vector2 textPosition2 = new Vector2(10, 40);
        String textString = "Hit by: ";
        String textString2 = "Time left: "; 
        int countdown = 20;
        
        public Level1()
        {
            onTime = TimeSpan.FromSeconds(1.5);
            offTime = TimeSpan.FromSeconds(0.5);
        }
        #endregion
        #region Load Content
        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(SManager.Game.Services, "Content");
            }

            //background
            spriteBatch = new SpriteBatch(SManager.GraphicsDevice);
            background = content.Load<Texture2D>("Textures\\Winter_Castle");
            viewportRect = new Rectangle(0, 0, SManager.GraphicsDevice.Viewport.Width, SManager.GraphicsDevice.Viewport.Height);
            fadeRect = new Texture2D(SManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            fadeRect.SetData(new[] { Color.White });

            //snowballs
            sbs = new List<L1Sprite>(100000);
            inactive = new List<L1Sprite>(100000);
            hit = new List<L1Sprite>(100000);
            sbTex = content.Load<Texture2D>("Textures\\Snowball");
            sbs.Add(CreateSb());
            
            //player
            pTex = content.Load<Texture2D>("Textures\\Run");
            pSprite = new L1Player(pTex, new Vector2(pTex.Height / 2, pTex.Height / 2), new Vector2(300, 420), new Rectangle(0, 0, 64, pTex.Height), new Vector2(0, 0), 1, 11, 10);
            sbs.Add(pSprite);

            //sounds
            impact = content.Load<SoundEffect>("Sounds\\impact");
            ouch = content.Load<SoundEffect>("Sounds\\ow");
            lose = content.Load<SoundEffect>("Sounds\\u_lose");
            impactInstance = impact.CreateInstance();

            //text
            periclesFont = content.Load<SpriteFont>("Fonts\\Pericles");
        }

        private L1Sprite CreateSb()
        {
            Random rand = new Random();
            return new L1Sprite(sbTex, new Vector2(15, 15), new Vector2((float)rand.Next(0, viewportRect.Width), (float)0), new Rectangle(0, 0, 29, 29), new Vector2(0, rand.Next(1, 8)), rand.Next(2, 10) * 0.1f);
        }

        public override void UnloadContent()
        {
            
        }
        #endregion
        #region Updates
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            #region Pause Gradient
            if (coveredByOtherScreen)
            {
                pauseGradient = Math.Min(pauseGradient + 1f / 32, 1);
            }
            else
            {
                pauseGradient = Math.Max(pauseGradient - 1f / 32, 0);
            }
            #endregion
            #region Timer Updates
            if (active)
            {
                Random rand = new Random();
                elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                counterTime += gameTime.ElapsedGameTime.Milliseconds;

            #endregion
                #region Spawn Snowballs
                if (elapsedTime > spawnTime)
                {
                    elapsedTime -= spawnTime;
                    sbs.Add(CreateSb());
                    spawnTime = rand.Next(300, 1500);
                }
                #endregion
                #region Countdown Timer
                if (counterTime > secondTime)
                {
                    counterTime -= secondTime;
                    if (countdown > 0)
                    {
                        countdown--;
                    }
                    else
                    {
                        countdown = 0;
                    }
                }
                #endregion
                #region Check collision, land, vibrate, sound play
                foreach (L1Sprite s in sbs)
                {
                    s.Update(gameTime, viewportRect);
                    if (s != pSprite)
                    {
                        if (s.CollidesWith(pSprite))
                        {
                            if (!s.getLanded() && !s.getCollided())
                            {
                                score++;
                                ouch.Play();
                                s.setCollided();
                                hit.Add(s);
                                GamePad.SetVibration(PlayerIndex.One, 1, 1);                                
                            }
                        }                       
                        GamePad.SetVibration(PlayerIndex.One, 0, 0);

                        if (s.getLanded() && !s.getPlayed())
                        {
                            impactInstance.Play();
                            inactive.Add(s);
                            s.setPlayed();
                        }

                        if (!s.getLanded())
                        {
                            if (s.getPos().X < pSprite.getPos().X)
                            {
                                s.incVelX();
                            }

                            if (s.getPos().X > pSprite.getPos().X)
                            {
                                s.decVelX();
                            }
                        }
                    }

                }
                #endregion
                #region Remove Dead Snowballs
                foreach (L1Sprite s in hit)
                {
                    sbs.Remove(s);
                }
                #endregion
                #region Check for Death
                if (!death)
                {
                    if (score >= 5)
                    {
                        lose.Play();
                        death = true;
                        SManager.AddScreen(new Loss1(), null);
                    }
                }
                #endregion
                #region Fade to Black
                if (countdown == 4)
                {
                    fadeBlack(10.0f);
                }
                #endregion
                #region Check for Win
                if (countdown == 0 && score < 5)
                {
                    SManager.AddScreen(new Victory1(), null);
                }
                #endregion
            }
        }

        public override void HandleInput(inputState input)
        {
            KeyboardState keyboardState = input.currentKeyboardState;
            GamePadState gamePadState = input.currentGamePadState;

            if (input.isPause(thisPlayer))
            {
                SManager.AddScreen(new PauseMenu(), thisPlayer);
            }           
        }
        #endregion
        #region Draw
        public override void Draw(GameTime gameTime)
        {
            SManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, viewportRect, Color.White);
            spriteBatch.DrawString(periclesFont, (textString + score + " snowballs"), textPosition, Color.Red);
            spriteBatch.DrawString(periclesFont, (textString2 + countdown), textPosition2, Color.Red);

            foreach (L1Sprite s in sbs)
            {
                s.Draw(gameTime, spriteBatch, Color.White);
            }  
            spriteBatch.End();

            base.Draw(gameTime);
        }
        
        public void fadeBlack(float alpha)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, viewportRect.Width, viewportRect.Height), Color.Black * alpha);
            spriteBatch.End();
        }
        #endregion
    }
}
