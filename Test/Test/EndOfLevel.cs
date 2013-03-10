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

        int frame;
        int currentFrame = 0;
        int animationLength = 4;

        Rectangle[] backRec = new Rectangle[] {
            new Rectangle(0, 0, 64, 64),
            new Rectangle(0, 64, 64, 64)
        };

        Rectangle[] frontRec = new Rectangle[] {
            new Rectangle(64, 0, 64, 64),
            new Rectangle(64, 64, 64, 64)
        };

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

        public void Update()
        {
            if (currentFrame < animationLength / 2)
                frame = 0;
            else if (currentFrame < animationLength)
                frame = 1;
            else
                currentFrame = 0;

            currentFrame++;
        }

        public void DrawBack(SpriteBatch theSpritebatch)
        {
            theSpritebatch.Draw(back, this.Position, backRec[frame], Color.White, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0.0f);
        }

        public void DrawFront(SpriteBatch theSpritebatch)
        {
            theSpritebatch.Draw(front, this.Position, frontRec[frame], Color.White, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0.0f);
        }
    }
}
