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
        public EndOfLevel endOflevel;

        public List<List<MacroBlock>> levels = new List<List<MacroBlock>>() {
            new List<MacroBlock>(){
                new MacroBlock(new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 }, 4, 0),
                new MacroBlock(new int[] { 0, 1, 2 }, 2, 10),
                new MacroBlock(new int[] { 0, 1, 2 }, 3, 13),
                new MacroBlock(new int[] { 0, 1, 1, 1, 2 }, 5, 12),
                new MacroBlock(new int[] { 3 }, 2, 18),
                new MacroBlock(new int[] { 0, 1, 2 }, 4, 18),
                new MacroBlock(new int[] { 0, 1, 1, 2 }, 2, 21),
                new MacroBlock(new int[] { 0, 1, 2 }, 4, 22),
                new MacroBlock(new int[] { 0, 1, 2 }, 3, 26),
                new MacroBlock(new int[] { 3 }, 4, 30),
                new MacroBlock(new int[] { 0, 1, 1, 1, 2 }, 5, 26),
                new MacroBlock(new int[] { 3 }, 2, 29),
                new MacroBlock(new int[] { 0, 1, 2 }, 3, 32),
                new MacroBlock(new int[] { 0, 1, 1, 2 }, 5, 32),
                new MacroBlock(new int[] { 0, 1, 1, 1, 2 }, 4, 36),
                new MacroBlock(new int[] { 0, 2}, 3, 39),
                new MacroBlock(new int[] { 3 }, 2, 40)
                //new MacroBlock(new int[] {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2}, 6, 0) //debug block
            },
            new List<MacroBlock>(){
                new MacroBlock(new int[] { 0, 1, 1, 1, 2 }, 4, 0), 
                new MacroBlock(new int[] { 3 }, 3, 6),
                new MacroBlock(new int[] { 3 }, 5, 6), 
                new MacroBlock(new int[] { 0, 2 }, 2, 8), 
                new MacroBlock(new int[] { 0, 2 }, 4, 8),
                new MacroBlock(new int[] { 3 }, 2, 11),
                new MacroBlock(new int[] { 0, 2 }, 4, 11), 
                new MacroBlock(new int[] { 0, 1, 2 }, 3, 14), 
                new MacroBlock(new int[] { 0, 1, 1, 2 }, 5, 14), 
                new MacroBlock(new int[] { 0, 2 }, 2, 17), 
                new MacroBlock(new int[] { 3 }, 4, 18), 
                new MacroBlock(new int[] { 0, 1, 1, 1, 2 }, 4, 20), 
                new MacroBlock(new int[] { 0, 2 }, 3, 22), 
                new MacroBlock(new int[] { 0, 2 }, 5, 25), 
                new MacroBlock(new int[] { 3 }, 4, 27), 
                new MacroBlock(new int[] { 3 }, 3, 28), 
                new MacroBlock(new int[] { 3 }, 2, 29), 
                new MacroBlock(new int[] { 3 }, 5, 29), 
                new MacroBlock(new int[] { 0, 1, 2 }, 2, 31),
                new MacroBlock(new int[] { 0, 1, 1, 2 }, 4, 30),
                //finish, maybe will be one layer lower later
                new MacroBlock(new int[] { 0, 1, 1, 1, 2 }, 3, 35), 
                new MacroBlock(new int[] { 0, 2 }, 2, 38), 
                new MacroBlock(new int[] { 3 }, 1, 39),
                new MacroBlock(new int[] {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2}, 6, 0) //debug block
            }
        };

        public List<EndOfLevel> ends = new List<EndOfLevel>() {
            new EndOfLevel(new Vector2(40 * 64, 64 + 3)),
            new EndOfLevel(new Vector2(39 * 64, 0 + 3))
        };

        public Level()
        {
            levelBlocks = new List<MacroBlock>();
        }

        public void Initialize(int level)
        {
            levelBlocks = levels[level];
            endOflevel = ends[level];

            foreach (MacroBlock mb in levelBlocks)
                mb.Initialize();
        }

        public void LoadContent(ContentManager theContentManager)
        {
            foreach (MacroBlock mb in levelBlocks)
                mb.LoadContent(theContentManager);

            endOflevel.LoadContent(theContentManager);
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (MacroBlock mb in levelBlocks)
                mb.Draw(theSpriteBatch);
        }
    }
}
