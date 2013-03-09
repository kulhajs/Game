using System.Collections.Generic;
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

        public EnemyHandler()
        {
            towers = new List<Tower>();
            doors = new List<FlashDoor>();
            zombies = new List<ZombieDispenser>();
        }

        public void Initiliaze()
        {

            //_________________________TOWERS________________________________
            towers.Add(new Tower(new Vector2(1024, 256 + 24 * 1 / 0.667f)));
            towers.Add(new Tower(new Vector2(1536, 64 + 24 * 1 / 0.667f)));
            towers.Add(new Tower(new Vector2(1664, 256 + 24 * 1 / 0.667f)));
            towers.Add(new Tower(new Vector2(2176, 128 + 24 * 1 / 0.667f)));
            //_________________________DOORS_________________________________
            doors.Add(new FlashDoor(new Vector2(576, 128 + 4), new Vector2(384, 192 + 16)));
            doors.Add(new FlashDoor(new Vector2(2432, 128 + 4), new Vector2(1152, 64 + 16)));
            //________________________ZOMBIES_________________________________
            zombies.Add(new ZombieDispenser(new Vector2(1472, -16)));
            zombies.Add(new ZombieDispenser(new Vector2(2496, 64 - 16)));
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

        public void Update(Player player, GameTime theGameTime, ExplosionHandler explosions)
        {
            foreach (Tower t in towers)
                t.Update(player, theGameTime, explosions);

            foreach (FlashDoor f in doors)
                f.Update(theGameTime);

            foreach (ZombieDispenser z in zombies)
                z.Update(theGameTime);
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
