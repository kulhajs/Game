using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Test
{
    class ZombieDispenser : Sprite
    {

        Rectangle[] sources = new Rectangle[] { 
            new Rectangle(0,0,64,64),
            new Rectangle(64,0,64,64),
            new Rectangle(128,0,64,64),
            new Rectangle(192,0,64,64)
        };

        int currentFrame = 0;
        int animationLength = 16;

        public ZombieDispenser(Vector2 position)
        {
            this.Position = position;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, "zombie_dispenser");
            Source = sources[0];
        }

        public void Update()
        {
            this.Animate();
        }

        private void Animate()
        {
            if (currentFrame < animationLength / 4)
                this.Source = sources[0];
            else if (currentFrame < 2 * animationLength / 4)
                this.Source = sources[1];
            else if (currentFrame < 3 * animationLength / 4)
                this.Source = sources[2];
            else if (currentFrame < animationLength)
                this.Source = sources[3];
            else
                currentFrame = 0;

            currentFrame++;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch, Vector2.Zero, this.Position, Color.White, 0.0f);
        }

    }
}
