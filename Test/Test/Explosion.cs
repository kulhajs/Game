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
    class Explosion : Sprite
    {
        Rectangle[] sources = new Rectangle[] {
            new Rectangle(0,0,32,32),
            new Rectangle(32,0,32,32),
            new Rectangle(64,0,32,32),
            new Rectangle(96,0,32,32),
            new Rectangle(128,0,32,32)
        };

        string explosionType;

        const int animationLength = 15;
        int currentFrame = 0;

        public bool Visible { get; private set; }

        public Explosion(Vector2 position, string explosionType)
        {
            this.Position = position;
            this.Visible = true;
            this.explosionType = explosionType;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, explosionType);
        }

        public void Animate()
        {
            if (currentFrame < animationLength / 5)
                Source = sources[0];
            else if (currentFrame < 2 * animationLength / 5)
                Source = sources[1];
            else if (currentFrame < 3 * animationLength / 5)
                Source = sources[2];
            else if (currentFrame < 4 * animationLength / 5)
                Source = sources[3];
            else if (currentFrame < animationLength)
                Source = sources[4];
            else
                Visible = false;

            currentFrame++;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch, Vector2.Zero, this.Position, Color.White, 0.0f);
        }
    }
}
