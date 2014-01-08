using System;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

namespace Snowy_Castle
{
#if WINDOWS || XBOX
    public class Program : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        screenManager screenManager;

        public Program()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 648;
            graphics.PreferredBackBufferHeight = 486;
            screenManager = new screenManager(this);
            Components.Add(screenManager);
            screenManager.AddScreen(new backgroundScreen(), null);
            screenManager.AddScreen(new MainMenu(), null);
        }

        protected override void LoadContent()
        {
         
        }

        protected override void Initialize()
        {
            base.Initialize();
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

