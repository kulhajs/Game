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

        public List<FirstAid> firstAids;
        FirstAid newFirstAid;

        ContentManager contentManager;

        Random random = new Random();

        const int coinCount = 15;

        public ItemHandler(ContentManager theContentmanager)
        {
            coins = new List<Coin>();
            firstAids = new List<FirstAid>();
            contentManager = theContentmanager;
        }

        public void AddFirstAid(Vector2 position)
        {
            newFirstAid = new FirstAid(position);
            newFirstAid.LoadContent(contentManager);
            firstAids.Add(newFirstAid);
            newFirstAid = null;
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

        public void Update(GameTime theGameTime)
        {
            foreach (Coin c in coins)
                if (c.Visible)
                    c.Update();

            if (firstAids.Count > 0)
                foreach (FirstAid f in firstAids)
                    f.Update(theGameTime);

            this.RemoveInvisibleCoins();
            this.RemoveInvisibleFirstAids();
        }

        private void RemoveInvisibleFirstAids()
        {
            foreach(FirstAid f in firstAids)
                if(!f.Visible)
                {
                    firstAids.Remove(f);
                    break;
                }
        }

        private void RemoveInvisibleCoins()
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

                foreach (FirstAid f in firstAids)
                    f.Draw(theSpriteBatch);
        }
    }
}
