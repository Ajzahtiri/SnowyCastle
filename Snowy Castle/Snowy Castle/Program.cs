using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snowy_Castle
{
#if WINDOWS || XBOX
    public class Program : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        public Program()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 648;
            graphics.PreferredBackBufferHeight = 486;
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenu(), null);
        }

        protected override void LoadContent()
        {
         
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
        
        static void Main(string[] args)
        {
            using (Program game = new Program())
            {
                game.Run();
            }
        }
    }
#endif
}

