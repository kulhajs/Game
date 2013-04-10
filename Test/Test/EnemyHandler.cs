using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    class EnemyHandler
    {
        public List<Tower> towers;
        public List<FlashDoor> doors;
        public List<ZombieDispenser> zombies;

        string[] objParams;

        public EnemyHandler()
        {
            towers = new List<Tower>(10);
            doors = new List<FlashDoor>(5);
            zombies = new List<ZombieDispenser>(5);
        }

        public void Initiliaze(int level)
        {
            string path = "Content/Levels/enemies_" + (level + 1) + ".level";
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)  //type of enemy (T, D, Z) \t column \t layer
                {
                    if (line.StartsWith("#"))
                        continue;
                    else
                    {
                        objParams = line.Split('\t');
                        if (objParams[0] == "T")
                            towers.Add(new Tower(new Vector2(float.Parse(objParams[1]) * 64, float.Parse(objParams[2]) * 64 + 16 * 1 / 0.667f))); 
                        else if (objParams[0] == "D")
                            doors.Add(new FlashDoor(new Vector2(float.Parse(objParams[1]) * 64, float.Parse(objParams[2]) * 64 + 4),
                                                    new Vector2(float.Parse(objParams[3]) * 64, float.Parse(objParams[4]) * 64 + 16)));
                        else if (objParams[0] == "Z")
                            zombies.Add(new ZombieDispenser(new Vector2(float.Parse(objParams[1]) * 64, float.Parse(objParams[2]) * 64 + 16)));
                    }
                }
            }
        }

        public void LoadContent(ContentManager theContentManager)
        {
            foreach (Tower t in towers)
                    t.LoadContent(theContentManager);

            foreach (FlashDoor f in doors)
                f.LoadContent(theContentManager);

            foreach (ZombieDispenser z in zombies)
                z.LoadContent(theContentManager);
        }

        public void Update(Player player, GameTime theGameTime, ExplosionHandler explosions, ItemHandler ih, SoundHandler sounds, Camera c)
        {
            foreach (Tower t in towers)
                t.Update(player, theGameTime, explosions, sounds, c);

            foreach (FlashDoor f in doors)
                f.Update(theGameTime);

            foreach (ZombieDispenser z in zombies)
                z.Update(theGameTime, explosions, ih, c);
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Tower t in towers)
                t.Draw(theSpriteBatch);

            foreach (FlashDoor f in doors)
                f.Draw(theSpriteBatch);

            foreach (ZombieDispenser z in zombies)
                z.Draw(theSpriteBatch);
        }
    }
}
