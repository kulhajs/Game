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
    class MacroBlock
    {
        int[] id;
        Vector2 initPos;

        public List<Block> blocks;
        
        public MacroBlock(int[] id, int layer, int column)
        {
            this.id = id;
            this.initPos = new Vector2(column * 64, layer * 66);
            blocks = new List<Block>();
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)initPos.X, (int)initPos.Y + 2, blocks.Count * 63, 8);
        }

        public void Initialize()
        {
            for (int i = 0; i < id.Length; i++)
                blocks.Add(new Block(id[i], new Vector2(initPos.X + i * 63, initPos.Y)));
        }

        public void LoadContent(ContentManager theContentManager)
        {
            foreach (Block b in blocks)
                b.LoadContent(theContentManager);
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Block b in blocks)
                b.Draw(theSpriteBatch);
        }
    }
}
