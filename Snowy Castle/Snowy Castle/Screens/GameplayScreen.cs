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
    public class GameplayScreen : GameScreen
    {
        //logic
        GraphicsDeviceManager graphics;        
        SpriteBatch spriteBatch;
        Rectangle viewportRect;
        ContentManager content;
        private int score;
        private bool death;

        //time
        private int counterTime = 0;
        private int elapsedTime = 0;
        private int secondTime = 1000;
        private int spawnTime = 2000;

        //background
        Texture2D background;

        //player
        Texture2D pTex;
        AnimatedSprite pSprite;

        //snowballs
        private List<Sprite> sbs;
        private List<Sprite> inactive;
        private List<Sprite> hit;
        private Texture2D sbTex;

        //sounds
        private SoundEffect impact;
        private SoundEffect ouch;
        private SoundEffect lose;

        //text
        SpriteFont periclesFont;
        Vector2 textPosition = new Vector2(10, 10);
        Vector2 textPosition2 = new Vector2(10, 40);
        String textString = "Hit by: ";
        String textString2 = "Time left: "; 
        int countdown = 20;
        SoundEffectInstance impactInstance;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            //background
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            background = content.Load<Texture2D>("Textures\\Winter_Castle");
            viewportRect = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);

            //snowballs
            sbs = new List<Sprite>(1000);
            inactive = new List<Sprite>(1000);
            hit = new List<Sprite>(1000);
            sbTex = content.Load<Texture2D>("Textures\\Snowball");
            sbs.Add(CreateSb());
            
            //player
            pTex = content.Load<Texture2D>("Textures\\Run");
            pSprite = new AnimatedSprite(pTex,
                new Vector2(pTex.Height / 2, pTex.Height / 2),
                new Vector2(300, 420),
                new Rectangle(0, 0, 64, pTex.Height),                
                new Vector2(0, 0),
                1, 11, 10
                );
            sbs.Add(pSprite);

            //sounds
            impact = content.Load<SoundEffect>("Sounds\\impact");
            ouch = content.Load<SoundEffect>("Sounds\\ow");
            lose = content.Load<SoundEffect>("Sounds\\u_lose");
            impactInstance = impact.CreateInstance();

            //text
            periclesFont = content.Load<SpriteFont>("Fonts\\Pericles");

        }

        private Sprite CreateSb()
        {
            Random rand = new Random();
            return new Sprite(sbTex,
                new Vector2(15, 15),
                new Vector2(
                    (float)rand.Next(0, viewportRect.Width),
                    (float)0),
                new Rectangle(0, 0, 29, 29),
                new Vector2(0, rand.Next(1, 8)),
                rand.Next(2, 10) * 0.1f);
        }

        public override void UnloadContent()
        {
            
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            Random rand = new Random();
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            counterTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime > spawnTime)
            {
                elapsedTime -= spawnTime;
                sbs.Add(CreateSb());
                spawnTime = rand.Next(300, 2000);
            }

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

            foreach (Sprite s in sbs)
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
                        }  
                    }

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

            foreach (Sprite s in hit)
            {
                sbs.Remove(s);
            }

            foreach (Sprite s in inactive)
            {
               
            }

            if (!death)
            {
                if (score >= 5)
                {
                    lose.Play();
                    death = true;
                }
            }

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, viewportRect, Color.White);
            spriteBatch.DrawString(periclesFont, (textString + score + " snowballs"), textPosition, Color.Red);
            spriteBatch.DrawString(periclesFont, (textString2 + countdown), textPosition2, Color.Red);

            foreach (Sprite s in sbs)
            {
                s.Draw(gameTime, spriteBatch, Color.White);
            }  
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
