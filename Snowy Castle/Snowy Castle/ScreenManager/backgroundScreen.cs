using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Snowy_Castle
{

    class backgroundScreen : gameScreen
    {
        ContentManager content;
        Texture2D myBg;

        public backgroundScreen()
        {
            onTime = TimeSpan.FromSeconds(1);
            offTime = TimeSpan.FromSeconds(1);
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(SManager.Game.Services, "Content");
            }
            myBg = content.Load<Texture2D>("Textures\\Winter_Castle");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool focussedOtherScreen, bool coveredOtherScreen)
        {
            base.Update(gameTime, focussedOtherScreen, false);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = SManager.SpriteBatch;
            Viewport vp = SManager.GraphicsDevice.Viewport;
            Rectangle fullSize = new Rectangle(0, 0, vp.Width, vp.Height);

            sb.Begin();
            sb.Draw(myBg, fullSize, new Color(transAlpha, transAlpha, transAlpha));
            sb.End();
        }
    }
}
