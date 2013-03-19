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
    class HealthBar : Sprite
    {
        Rectangle frameSource;
        Rectangle barSource;

        Texture2D frame;
        Texture2D bar;

        int val = 0;

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            frame = contentManager.Load<Texture2D>("healthbar");
            frameSource = new Rectangle(0, 0, 122, 14);
            bar = contentManager.Load<Texture2D>("healthbar");
            barSource = new Rectangle(0, 16, val, 10);
        }

        public void Update(Camera c, int val)
        {
            this.val = val;
            barSource = new Rectangle(0, 16, val, 10);
        }

        public void Draw(SpriteBatch theSpriteBatch, Vector2 position)
        {
            theSpriteBatch.Draw(frame, position, frameSource, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            theSpriteBatch.Draw(bar, new Vector2(position.X + 1, position.Y + 2), barSource, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
