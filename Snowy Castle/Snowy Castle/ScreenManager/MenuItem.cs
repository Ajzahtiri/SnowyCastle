using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snowy_Castle
{
    class menuItem
    {
        string text;
        float fade;
        Vector2 position;
        public event EventHandler<playerEvent> chosen;

        public menuItem(string text)
        {
            this.text = text;
        }
        public string Text
        {
            get 
            { 
                return text; 
            }
            set 
            { 
                text = value; 
            }
        }
        public Vector2 Position
        {
            get 
            { 
                return position; 
            }
            set 
            { 
                position = value; 
            }
        }        
        protected internal virtual void onSelect(PlayerIndex pIndex)
        {
            if (chosen != null)
            {
                chosen(this, new playerEvent(pIndex));
            }
        }        
        public virtual void Update(menuScreen screen, bool isChosen, GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isChosen)
            {
                fade = Math.Min(fade + fadeSpeed, 1);
            }
            else
            {
                fade = Math.Max(fade - fadeSpeed, 0);
            }
        }
        public virtual void Draw(menuScreen screen, bool isChosen, GameTime gameTime)
        {
            Color color = isChosen ? Color.DarkGray : Color.Black;

            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulse = (float)Math.Sin(time * 6) + 1;
            float scale = 1 + pulse * 0.05f * fade;

            color *= screen.transAlpha;

            screenManager screenManager = screen.SManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, color, 0, origin, scale, SpriteEffects.None, 0);
        }
        public virtual int getHeight(menuScreen screen)
        {
            return screen.SManager.Font.LineSpacing;
        }
        public virtual int getWidth(menuScreen screen)
        {
            return (int)screen.SManager.Font.MeasureString(Text).X;
        }
    }
}
