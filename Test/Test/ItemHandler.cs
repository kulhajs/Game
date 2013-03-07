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

        public ItemHandler()
        {
            coins = new List<Coin>();
        }

        public void Initialize()
        {
            coins.Add(new Coin(new Vector2(128, 196 - 16), Color.White, 0.0f));
        }

        public void LoadContent(ContentManager theContentManager)
        {
            foreach (Coin c in coins)
                c.LoadContent(theContentManager);
        }

        public void Update()
        {
            foreach (Coin c in coins)
                c.Update();
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Coin c in coins)
                c.Draw(theSpriteBatch);
        }
    }
}
