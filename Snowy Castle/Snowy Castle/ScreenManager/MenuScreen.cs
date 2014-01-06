using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snowy_Castle
{
    abstract class MenuScreen : GameScreen
    {
        List<MenuItem> menuEntries = new List<MenuItem>();
        int selectedEntry = 0;
        int bow;
        string menuTitle;

        protected IList<MenuItem> MenuEntries
        {
            get 
            { 
                return menuEntries; 
            }
        }

        public MenuScreen(string menuTitle, int w)
        {
            this.menuTitle = menuTitle;
            this.bow = w;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsMenuUp(ControllingPlayer))
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }

            if (input.IsMenuDown(ControllingPlayer))
            {
                selectedEntry++;

                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
            }

            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                OnSelectEntry(selectedEntry, playerIndex);
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                OnCancel(playerIndex);
            }
        }

        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }

        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }

        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }

        protected virtual void UpdateMenuEntryLocations()
        {
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 position = new Vector2(0f, 175f);

            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuItem menuEntry = menuEntries[i];

                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                if (ScreenState == ScreenState.TransitionOn)
                {
                    position.X -= transitionOffset * 256;
                }
                else
                {
                    position.X += transitionOffset * 512;
                }

                menuEntry.Position = position;

                position.Y += menuEntry.GetHeight(this);
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuItem menuItem = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuItem.Draw(this, isSelected, gameTime);
            }

            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 100);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color();
            if (bow == 1)
            {
                titleColor = new Color(0, 0, 0) * TransitionAlpha;
            }
            else if (bow == 2)
            {
                titleColor = new Color(255, 255, 255) * TransitionAlpha;
            }
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
