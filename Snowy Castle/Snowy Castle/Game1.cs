using System;
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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //logic
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle viewportRect;
        private int score;

        //time
        private int elapsedTime = 0;
        private int spawnTime = 2000;

        //background
        Texture2D background;

        //player
        Texture2D pTex;
        AnimatedSprite pSprite;

        //snowballs
        private List<Sprite> sbs;
        private Texture2D sbTex;

        //sounds
        private SoundEffect impact;
        private SoundEffect ouch;
        private SoundEffect lose;

        //text
        SpriteFont periclesFont;
        Vector2 textPosition = new Vector2(10, 10);
        String textString = "Hit by: ";
        SoundEffectInstance impactInstance;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //background
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("Textures\\Winter_Castle");
            viewportRect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            //snowballs
            sbs = new List<Sprite>(1000);
            sbTex = Content.Load<Texture2D>("Textures\\Snowball");
            sbs.Add(CreateSb());
            
            //player
            pTex = Content.Load<Texture2D>("Textures\\Run");
            pSprite = new AnimatedSprite(pTex,
                new Vector2(pTex.Height / 2, pTex.Height / 2),
                new Vector2(300, 420),
                new Rectangle(0, 0, 64, pTex.Height),                
                new Vector2(0, 0),
                1, 11, 10
                );
            sbs.Add(pSprite);

            //sounds
            impact = Content.Load<SoundEffect>("Sounds\\impact");
            ouch = Content.Load<SoundEffect>("Sounds\\ow");
            lose = Content.Load<SoundEffect>("Sounds\\u_lose");
            impactInstance = impact.CreateInstance();

            //text
            periclesFont = Content.Load<SpriteFont>("Fonts\\Pericles");

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

        protected override void UnloadContent()
        {
            
        }
   
        protected override void Update(GameTime gameTime)
        {
            Random rand = new Random();
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime > spawnTime)
            {
                elapsedTime -= spawnTime;
                sbs.Add(CreateSb());
                spawnTime = rand.Next(500, 3000);
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            foreach (Sprite s in sbs)
            {
                s.Update(gameTime, viewportRect);
                if (s != pSprite)
                {
                    if (s.getLanded())
                    {
                        //doesn't stop playing
                      //  impactInstance.Play();
                    }

                    if (s.CollidesWith(pSprite))
                    {
                        if (!s.getLanded() && !s.getCollided())
                        {
                            score++;
                            ouch.Play();
                            s.setCollided();
                        }  
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, viewportRect, Color.White);
            spriteBatch.DrawString(periclesFont, (textString + score + " snowballs"), textPosition, Color.Red);

            foreach (Sprite s in sbs)
            {
                s.Draw(gameTime, spriteBatch, Color.White);
            }  
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
