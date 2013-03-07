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
    class Coin : Sprite
    {
        int currentFrame = 0;
        int animationLength = 15;

        Rectangle[] sources = new Rectangle[] { 
            new Rectangle(0,0,16,16),
            new Rectangle(16,0,16,16),
            new Rectangle(32,0,16,16),
            new Rectangle(48,0,16,16),
            new Rectangle(64,0,16,16),
            new Rectangle(0,16,16,16),
            new Rectangle(16,16,16,16),
            new Rectangle(32,16,16,16),
            new Rectangle(48,16,16,16),
            new Rectangle(64,16,16,16),
            new Rectangle(0,32,16,16),
            new Rectangle(16,32,16,16),
            new Rectangle(32,32,16,16),
            new Rectangle(48,32,16,16),
            new Rectangle(64,32,16,16)
        };

        public bool Visible { get; set; }

        public Coin(Vector2 position, Color color, float rotation)
        {
            this.Position = position;
            this.Color = color;
            this.Rotation = rotation;
            this.Visible = true;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, "coin");
            this.Source = sources[0];
        }

        public void Update()
        {
            this.Animate();
        }

        private void Animate()
        {
            if (currentFrame < animationLength / 15)
                this.Source = sources[0];
            else if (currentFrame < 2 * animationLength / 15)
                this.Source = sources[1];
            else if (currentFrame < 3 * animationLength / 15)
                this.Source = sources[2];
            else if (currentFrame < 4 * animationLength / 15)
                this.Source = sources[3];
            else if (currentFrame < 5 * animationLength / 15)
                this.Source = sources[4];
            else if (currentFrame < 6 * animationLength / 15)
                this.Source = sources[5];
            else if (currentFrame < 7 * animationLength / 15)
                this.Source = sources[6];
            else if (currentFrame < 8 * animationLength / 15)
                this.Source = sources[7];
            else if (currentFrame < 9 * animationLength / 15)
                this.Source = sources[8];
            else if (currentFrame < 10 * animationLength / 15)
                this.Source = sources[9];
            else if (currentFrame < 11 * animationLength / 15)
                this.Source = sources[10];
            else if (currentFrame < 12 * animationLength / 15)
                this.Source = sources[11];
            else if (currentFrame < 13 * animationLength / 15)
                this.Source = sources[12];
            else if (currentFrame < 14 * animationLength / 15)
                this.Source = sources[13];
            else if (currentFrame < animationLength / 15)
                this.Source = sources[14];
            else
                currentFrame = 0;

            currentFrame++;
        }

        public void Draw(SpriteBatch theSpritebatch)
        {
            base.Draw(theSpritebatch, Vector2.Zero, this.Position, this.Color, this.Rotation);
        }
    }
}
