using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    class Level
    {
        List<MacroBlock> levelBlocks;

        public List<List<MacroBlock>> levels = new List<List<MacroBlock>>() {
            new List<MacroBlock>(){
                new MacroBlock(new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 }, 4, 0),
                new MacroBlock(new int[] { 0, 1, 2 }, 2, 10),
                new MacroBlock(new int[] { 0, 1, 2 }, 3, 13),
                new MacroBlock(new int[] { 0, 1, 1, 1, 2 }, 5, 12),
                new MacroBlock(new int[] { 1 }, 2, 18),
                new MacroBlock(new int[] { 0, 1, 2 }, 4, 18),
                new MacroBlock(new int[] { 0, 1, 1, 2 }, 2, 21),
                new MacroBlock(new int[] { 0, 1, 2 }, 4, 22),
                new MacroBlock(new int[] { 0, 1, 2 }, 3, 26),
                new MacroBlock(new int[] { 1 }, 4, 30),
                new MacroBlock(new int[] { 0, 1, 1, 1, 2 }, 5, 26),
                new MacroBlock(new int[] { 1 }, 2, 29),
                new MacroBlock(new int[] { 0, 1, 2 }, 3, 32),
                new MacroBlock(new int[] { 0, 1, 1, 2 }, 5, 32),
                new MacroBlock(new int[] { 0, 1, 1, 1, 2 }, 4, 36),
                new MacroBlock(new int[] {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2}, 6, 0)
            }
        };

        public Level()
        {
            levelBlocks = new List<MacroBlock>();
        }

        public void Initialize(int level)
        {
            levelBlocks = levels[level];

            foreach (MacroBlock mb in levelBlocks)
                mb.Initialize();
        }

        public void LoadContent(ContentManager theContentManager)
        {
            foreach (MacroBlock mb in levelBlocks)
                mb.LoadContent(theContentManager);
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (MacroBlock mb in levelBlocks)
                mb.Draw(theSpriteBatch);
        }
    }
}
