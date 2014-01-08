using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snowy_Castle
{
    abstract class menuScreen : gameScreen
    {
        List<menuItem> menuEntries = new List<menuItem>();
        int selected = 0;
        string menuTitle;
        Texture2D rect;

        protected IList<menuItem> MenuEntries
        {
            get 
            { 
                return menuEntries; 
            }
        }
        public menuScreen(string menuTitle)
        {
            this.menuTitle = menuTitle;
            onTime = TimeSpan.FromSeconds(0.5);
            offTime = TimeSpan.FromSeconds(0.5);
        }
        public override void HandleInput(inputState input)
        {
            if (input.isMenuUp(thisPlayer))
            {
                selected--;

                if (selected < 0)
                    selected = menuEntries.Count - 1;
            }

            if (input.isMenuDown(thisPlayer))
            {
                selected++;

                if (selected >= menuEntries.Count)
                    selected = 0;
            }

            PlayerIndex pIndex;

            if (input.isSelect(thisPlayer, out pIndex))
            {
                OnSelectEntry(selected, pIndex);
            }
            else if (input.isCancel(thisPlayer, out pIndex))
            {
                OnCancel(pIndex);
            }
        }
        protected virtual void OnSelectEntry(int eIndex, PlayerIndex pIndex)
        {
            menuEntries[eIndex].onSelect(pIndex);
        }
        protected virtual void OnCancel(PlayerIndex pIndex)
        {
            exitScreen();
        }
        protected void OnCancel(object sender, playerEvent e)
        {
            OnCancel(e.pIndex);
        }
        protected virtual void UpdateMenuEntryLocations()
        {
            float offset = (float)Math.Pow(transPos, 2);
            Vector2 position = new Vector2(0f, 175f);

            for (int i = 0; i < menuEntries.Count; i++)
            {
                menuItem menuEntry = menuEntries[i];
                position.X = SManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.getWidth(this) / 2;

                if (state == screenState.On)
                {
                    position.X -= offset * 256;
                }
                else
                {
                    position.X += offset * 512;
                }

                menuEntry.Position = position;

                position.Y += menuEntry.getHeight(this);
            }
        }
        public override void LoadContent()
        {
            GraphicsDevice graphics = SManager.GraphicsDevice;
            rect = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
            rect.SetData(new[] { Color.White });
        }
        public override void Update(GameTime gameTime, bool otherScreenFocus, bool coveredOtherScreen)
        {
            base.Update(gameTime, otherScreenFocus, coveredOtherScreen);

            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = active && (i == selected);

                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            UpdateMenuEntryLocations();
            GraphicsDevice graphics = SManager.GraphicsDevice;
            SpriteBatch spriteBatch = SManager.SpriteBatch;
            SpriteFont font = SManager.Font;
            spriteBatch.Begin();

            spriteBatch.Draw(rect, new Rectangle(124, 65, 400, 200), Color.White);

            for (int i = 0; i < menuEntries.Count; i++)
            {
                menuItem menuItem = menuEntries[i];
                bool isSelected = active && (i == selected);
                menuItem.Draw(this, isSelected, gameTime);
            }

            float offset = (float)Math.Pow(transPos, 2);

            Vector2 tPosition = new Vector2(graphics.Viewport.Width / 2, 100);
            Vector2 tCenter = font.MeasureString(menuTitle) / 2;
            Color tColor = new Color();
            tColor = new Color(0, 0, 0) * transAlpha; 
            float titleScale = 1.25f;
            tPosition.Y -= offset * 100;

            spriteBatch.DrawString(font, menuTitle, tPosition, tColor, 0, tCenter, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
