using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snowy_Castle
{
    public class L2Enemy : L2Sprite
    {
        public L2Enemy(Texture2D tex, Vector2 centre, Vector2 pos, Rectangle sourceRect, Vector2 vel) : base(tex, centre, pos, sourceRect, vel)
        {
            this.texture = tex;
            this.centre = centre;
            this.screenPos = pos;
            this.rotation = 0.0f;
            this.sourceRect = sourceRect;
            this.velocity = vel;
            this.size = 1;
            this.toDie = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb, Color col, float rotation)
        {
            sb.Draw(texture, screenPos, sourceRect, col, rotation, centre, this.size, SpriteEffects.None, 0);
        }
    }
}
