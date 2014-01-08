using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Snowy_Castle
{
    public class screenManager : DrawableGameComponent
    {
        List<gameScreen> screens = new List<gameScreen>();
        List<gameScreen> screensToUpdate = new List<gameScreen>();

        inputState input = new inputState();
        SpriteBatch sB;
        Texture2D background;
        public SpriteFont f;
        bool isInitialized, traceEnabled;

        public screenManager(Game game) : base(game)
        {

        }
        public SpriteBatch SpriteBatch
        {
            get 
            { 
                return sB; 
            }
        }
        public SpriteFont Font
        {
            get 
            { 
                return f; 
            }
        }
        public bool TraceEnabled
        {
            get 
            { 
                return traceEnabled; 
            }
            set 
            { 
                traceEnabled = value; 
            }
        }        
        public override void Initialize()
        {
            base.Initialize();
            
            isInitialized = true;
        }
        protected override void LoadContent()
        {
            ContentManager content = Game.Content;
            sB = new SpriteBatch(GraphicsDevice);
            f = content.Load<SpriteFont>("Fonts\\Pericles");
            background = content.Load<Texture2D>("Textures\\Winter_Castle");

            foreach (gameScreen screen in screens)
            {
                screen.LoadContent();
            }
        }
        protected override void UnloadContent()
        {
            foreach (gameScreen screen in screens)
            {
                screen.UnloadContent();
            }
        }
        public override void Update(GameTime gameTime)
        {
            input.Update();
            screensToUpdate.Clear();
            foreach (gameScreen s in screens)
            {
                screensToUpdate.Add(s);
            }

            bool otherScreenFocus = !Game.IsActive;
            bool coveredOtherScreen = false;

            while (screensToUpdate.Count > 0)
            {
                gameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                screen.Update(gameTime, otherScreenFocus, coveredOtherScreen);

                if (screen.state == screenState.On || screen.state == screenState.Active)
                {
                    if (!otherScreenFocus)
                    {
                        screen.HandleInput(input);

                        otherScreenFocus = true;
                    }

                    if (!screen.popsUp)
                    {
                        coveredOtherScreen = true;
                    }
                }
            }

            if (traceEnabled)
            {
                TraceScreens();
            }
        }
        void TraceScreens()
        {
            List<string> screenNames = new List<string>();
            foreach (gameScreen screen in screens)
            {
                screenNames.Add(screen.GetType().Name);
            }

            Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
        }
        public override void Draw(GameTime gameTime)
        {
            foreach (gameScreen screen in screens)
            {
                if (screen.state == screenState.Hidden)
                {
                    continue;
                }
                screen.Draw(gameTime);
            }
        }
        public void AddScreen(gameScreen s, PlayerIndex? thisPlayerr)
        {
            s.thisPlayer = thisPlayerr;
            s.SManager = this;
            s.exiting = false;

            if (isInitialized)
            {
                s.LoadContent();
            }

            screens.Add(s);

        }
        public void RemoveScreen(gameScreen s)
        {
            if (isInitialized)
            {
                s.UnloadContent();
            }

            screens.Remove(s);
            screensToUpdate.Remove(s);
        }
        public gameScreen[] GetScreens()
        {
            return screens.ToArray();
        }
       
    }
}
