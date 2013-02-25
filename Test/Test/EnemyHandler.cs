using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    class EnemyHandler
    {
        List<Tower> towers;
        List<FlashDoor> doors;

        public EnemyHandler()
        {
            towers = new List<Tower>();
            doors = new List<FlashDoor>();
        }

        public void Initiliaze()
        {

            //_________________________TOWERS________________________________
            towers.Add(new Tower(new Vector2(1024, 256 + 24 * 1 / 0.667f)));
            towers.Add(new Tower(new Vector2(1536, 64 + 24 * 1 / 0.667f)));
            towers.Add(new Tower(new Vector2(1664, 256 + 24 * 1 / 0.667f)));
            towers.Add(new Tower(new Vector2(2176, 128 + 24 * 1 / 0.667f)));
            //_________________________DOORS_________________________________
            doors.Add(new FlashDoor(new Vector2(1088, 256 - 48), new Vector2(100, 100)));
        }

        public void LoadContent(ContentManager theContentManager)
        {
            foreach (Tower t in towers)

                if (t.GetType() == typeof(Tower))
                    t.LoadContent(theContentManager);

            foreach (FlashDoor f in doors)
                f.LoadContent(theContentManager);
        }

        public void Update(Player player, GameTime theGameTime)
        {
            foreach (Tower t in towers)
                t.Update(player, theGameTime);
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Tower t in towers)
                t.Draw(theSpriteBatch);

            foreach (FlashDoor f in doors)
                f.Draw(theSpriteBatch);
        }
    }
}
