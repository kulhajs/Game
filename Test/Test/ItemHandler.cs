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
    class ItemHandler
    {
        public List<Coin> coins;

        Random random = new Random();

        const int coinCount = 15;

        public ItemHandler()
        {
            coins = new List<Coin>();
        }

        public void Initialize()
        {
            for (int i = 0; i < coinCount; i++)
            {
                coins.Add(new Coin(new Vector2(random.Next(36) * 64 - 24, random.Next(3, 6) * 64 - 16), Color.White, 0.0f));
            }
        }

        public void LoadContent(ContentManager theContentManager)
        {
            foreach (Coin c in coins)
                c.LoadContent(theContentManager);
        }

        public void Update()
        {
            foreach (Coin c in coins)
                if (c.Visible)
                    c.Update();

            this.RemoveInvisible();
        }

        private void RemoveInvisible()
        {
            foreach(Coin c in coins)
                if(!c.Visible)
                {
                    coins.Remove(c);
                    break;
                }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Coin c in coins)
                if (c.Visible)
                    c.Draw(theSpriteBatch);
        }
    }
}
