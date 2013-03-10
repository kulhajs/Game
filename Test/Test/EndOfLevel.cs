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

namespace Test
{
    class EndOfLevel : Sprite
    {
        Texture2D back;
        Texture2D front;

        Rectangle backRec = new Rectangle(0, 0, 64, 64);
        Rectangle frontRec = new Rectangle(64, 0, 64, 64);

        public EndOfLevel(Vector2 position)
        {
            this.Position = position;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            back = contentManager.Load<Texture2D>("eol");
            front = contentManager.Load<Texture2D>("eol");
        }

        public void DrawBack(SpriteBatch theSpritebatch)
        {
            theSpritebatch.Draw(back, this.Position, backRec, Color.White, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0.0f);
        }

        public void DrawFront(SpriteBatch theSpritebatch)
        {
            theSpritebatch.Draw(front, this.Position, frontRec, Color.White, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0.0f);
        }
    }
}
