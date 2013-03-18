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
        Rectangle[] sources;

        int currentFrame = 0;
        int frame = 0;
        int animationLength;
        int frameSize;
        int frameTime;

        string explosionType;

        public bool Visible { get; set; }

        public Explosion(Vector2 position, int sourceCount, int animationLength, string explosionType, int frameSize)
        {
            sources = new Rectangle[sourceCount];
            this.Position = position;
            this.Visible = true;
            this.explosionType = explosionType;
            this.animationLength = animationLength;
            this.frameSize = frameSize;
            frameTime = animationLength / sourceCount;

            for (int i = 0; i < sources.Length; i++)
                sources[i] = new Rectangle(i * frameSize, 0, frameSize, frameSize);
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, explosionType);
        }

        public void Animate()
        {
            if (currentFrame < sources.Length - 1)
                this.Source = sources[currentFrame];
            else
                this.Visible = false;

            frame++;
            if(frame > frameTime)
            {
                currentFrame++;
                frame = 0;
            }
            
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch, Vector2.Zero, this.Position, Color.White, 0.0f);
        }
    }
}
