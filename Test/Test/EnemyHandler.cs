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
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("#"))
                        continue;
                    else
                    {
                        objParams = line.Split('\t');
                        if (objParams[0] == "T")
                            towers.Add(new Tower(new Vector2(float.Parse(objParams[1]) * 64, float.Parse(objParams[2]) * 64 + 24 * 1 / 0.667f)));
                        else if (objParams[0] == "D")
                            doors.Add(new FlashDoor(new Vector2(float.Parse(objParams[1]) * 64, float.Parse(objParams[2]) * 64 + 4),
                                                    new Vector2(float.Parse(objParams[3]) * 64, float.Parse(objParams[4]) * 64 + 16)));
                        else if (objParams[0] == "Z")
                            zombies.Add(new ZombieDispenser(new Vector2(float.Parse(objParams[1]) * 64, float.Parse(objParams[2]) * 64 + 16)));
                    }
                }
            }

            //    if (level == 0)
            //    {
            //        //_________________________TOWERS________________________________
            //        towers.Add(new Tower(new Vector2(12 * 64, 4 * 64 + 24 * 1 / 0.667f)));
            //        towers.Add(new Tower(new Vector2(20 * 64, 1 * 64 + 24 * 1 / 0.667f)));
            //        towers.Add(new Tower(new Vector2(22 * 64, 4 * 64 + 24 * 1 / 0.667f)));
            //        towers.Add(new Tower(new Vector2(30 * 64, 2 * 64 + 24 * 1 / 0.667f)));
            //        towers.Add(new Tower(new Vector2(46 * 64, 2 * 64 + 24 * 1 / 0.667f)));
            //        //_________________________DOORS_________________________________
            //        doors.Add(new FlashDoor(new Vector2(5 * 64, 2 * 64 + 4), new Vector2(2 * 64, 3 * 64 + 16)));
            //        doors.Add(new FlashDoor(new Vector2(33 * 64, 2 * 64 + 4), new Vector2(14 * 64, 1 * 64 + 16)));
            //        doors.Add(new FlashDoor(new Vector2(50 * 64, 1 * 64 + 4), new Vector2(40 * 64, 1 * 64 + 16)));
            //        //________________________ZOMBIES_________________________________
            //        zombies.Add(new ZombieDispenser(new Vector2(19 * 64, 0 * 64 - 16)));
            //        zombies.Add(new ZombieDispenser(new Vector2(35 * 64, 0 * 64 - 16)));
            //        zombies.Add(new ZombieDispenser(new Vector2(41 * 64, 0 * 64 - 16)));
            //    }
            //    else if (level == 1)
            //    {
            //        //_________________________TOWERS_________________________________
            //        towers.Add(new Tower(new Vector2(6 * 64, 4 * 64 + 24 * 1 / 0.667f)));
            //        towers.Add(new Tower(new Vector2(15 * 64, 2 * 64 + 24 * 1 / 0.667f)));
            //        towers.Add(new Tower(new Vector2(24 * 64, 3 * 64 + 24 * 1 / 0.667f)));
            //        towers.Add(new Tower(new Vector2(32 * 64, 3 * 64 + 24 * 1 / 0.667f)));
            //        //_________________________DOORS__________________________________
            //        doors.Add(new FlashDoor(new Vector2(17 * 64, 3 * 64 + 6), new Vector2(11 * 64, 1 * 64 + 16)));
            //        doors.Add(new FlashDoor(new Vector2(36 * 64, 1 * 64 + 4), new Vector2(18 * 64, 3 * 64 + 16)));
            //        //________________________ZOMBIES_________________________________
            //        zombies.Add(new ZombieDispenser(new Vector2(17 * 64, 0 * 64 - 16)));
            //        zombies.Add(new ZombieDispenser(new Vector2(30 * 64, 0 * 64 - 16)));
            //    }
            //    else if (level == 2)
            //    {
            //        //_________________________TOWERS_________________________________
            //        towers.Add(new Tower(new Vector2(10 * 64, 4 * 64 + 22 * 1 / 0.667f)));
            //        towers.Add(new Tower(new Vector2(12 * 64 + 6, 18 * 1 / 0.667f))); //TODO: Normalize to * 64 format
            //        towers.Add(new Tower(new Vector2(23 * 64 + 6, 2 * 64 + 24 * 1 / 0.667f)));
            //        towers.Add(new Tower(new Vector2(33 * 64 + 6, 4 * 64 + 24 * 1 / 0.667f)));
            //        towers.Add(new Tower(new Vector2(40 * 64 + 6, 2 * 64 + 24 * 1 / 0.667f)));
            //        towers.Add(new Tower(new Vector2(55 * 64 + 6, 2 * 64 + 24 * 1 / 0.667f)));
            //        //_________________________DOORS__________________________________
            //        doors.Add(new FlashDoor(new Vector2(13 * 64, 1 * 64 + 2), new Vector2(6 * 64, 0 * 64 + 16)));
            //        doors.Add(new FlashDoor(new Vector2(15 * 64, 4 * 64 + 6), new Vector2(14 * 64, 2 * 64 + 16)));
            //        doors.Add(new FlashDoor(new Vector2(15 * 64, 1 * 64 + 2), new Vector2(16 * 64, 5 * 64 + 16)));
            //        doors.Add(new FlashDoor(new Vector2(19 * 64, 0 * 64 + 2), new Vector2(23 * 64, 4 * 64 + 16)));
            //        doors.Add(new FlashDoor(new Vector2(35 * 64, 0 * 64 + 2), new Vector2(30 * 64, -1 * 64 + 16)));
            //        doors.Add(new FlashDoor(new Vector2(49 * 64, -1 * 64 + 2), new Vector2(52 * 64, 3 * 64 + 16)));
            //        //________________________ZOMBIES_________________________________
            //        zombies.Add(new ZombieDispenser(new Vector2(9 * 64, -1 * 64)));
            //        zombies.Add(new ZombieDispenser(new Vector2(26 * 64, 0)));
            //        zombies.Add(new ZombieDispenser(new Vector2(36 * 64, -1 * 64)));
            //        zombies.Add(new ZombieDispenser(new Vector2(46 * 64, -1 * 64)));
            //    }
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

        public void Update(Player player, GameTime theGameTime, ExplosionHandler explosions, ItemHandler ih)
        {
            foreach (Tower t in towers)
                t.Update(player, theGameTime, explosions);

            foreach (FlashDoor f in doors)
                f.Update(theGameTime);

            foreach (ZombieDispenser z in zombies)
                z.Update(theGameTime, explosions, ih);
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
