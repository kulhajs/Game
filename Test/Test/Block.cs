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
    class Block : Sprite
    {
        Rectangle[] sources = new Rectangle[]{
            new Rectangle(0,0,66,64), //22 -> 72
            new Rectangle(66,0,64,64),
            new Rectangle(130,0,66,64)
        };

        int id;

        public Block(int id, Vector2 initPosition)
        {
            this.id = id;
            this.Position = initPosition;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, "blocks_2");
            this.Source = sources[id];
        }

        public void Draw(SpriteBatch theSpritebatch)
        {
            base.Draw(theSpritebatch, Vector2.Zero, this.Position, Color.White, 0.0f);
        }
    }
}
