using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    class Level
    {
        public List<MacroBlock> levelBlocks;
        public EndOfLevel endOflevel;

        string[] eolParams;
        string[] newBlockParams;
        int newBlockLength;
        int newBlockColumn;
        int newBlockLayer;
        int[] newBlock;


        public Level()
        {
            levelBlocks = new List<MacroBlock>();
        }

        public void Initialize(int level, string levelType)
        {
            string path = "Content/Levels/blocks_" + (level+1) + ".level";
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while((line = reader.ReadLine()) != null) //block length \t layer \t column
                {
                    if (line.StartsWith("#"))
                        continue;
                    else if (line.StartsWith("E"))
                    {
                        eolParams = line.Split('\t');
                        endOflevel = new EndOfLevel(new Vector2(float.Parse(eolParams[1]) * 64, float.Parse(eolParams[2]) * 64 + 3));
                    }
                    else
                    {
                        newBlockParams = line.Split('\t');
                        newBlockLength = int.Parse(newBlockParams[0]);
                        newBlockLayer = int.Parse(newBlockParams[1]);
                        newBlockColumn = int.Parse(newBlockParams[2]);
                        newBlock = new int[newBlockLength];

                        if (newBlockLength == 1)
                            newBlock[0] = 3;
                        else
                        {
                            for(int i = 0; i < newBlockLength; i++)
                            {
                                if (i == 0)
                                    newBlock[i] = 0;
                                else if (i == newBlockLength - 1)
                                    newBlock[i] = 2;
                                else
                                    newBlock[i] = 1;
                            }
                        }

                        levelBlocks.Add(new MacroBlock(newBlock, newBlockLayer, newBlockColumn));
                    }
                }
            }

            foreach (MacroBlock mb in levelBlocks)
                mb.Initialize(levelType);
        }

        public void Update()
        {
            endOflevel.Update();
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
