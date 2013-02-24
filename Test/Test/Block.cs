﻿using System;
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
            new Rectangle(0,0,64,24), 
            new Rectangle(64,0,64,24),
            new Rectangle(128,0,64,24),
            new Rectangle(192,0,64,24)
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
            base.LoadContent(contentManager, "blocks_3_noise");
            this.Source = sources[id];
        }

        public void Draw(SpriteBatch theSpritebatch)
        {
            base.Draw(theSpritebatch, Vector2.Zero, this.Position, Color.White, 0.0f);
        }
    }
}
