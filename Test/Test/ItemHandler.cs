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

        public List<Acid> acidBalls;
        Acid newAcid;

        ContentManager contentManager;

        Random random = new Random();

        const int coinCount = 15;

        public ItemHandler(ContentManager theContentmanager)
        {
            coins = new List<Coin>(coinCount);
            firstAids = new List<FirstAid>(25);
            acidBalls = new List<Acid>(25);
            contentManager = theContentmanager;
        }

        public void AddFirstAid(Vector2 position, float dx)
        {
            newFirstAid = new FirstAid(position, dx, 0.075f);
            newFirstAid.LoadContent(contentManager);
            firstAids.Add(newFirstAid);
            newFirstAid = null;
        }

        public void AddAcid(Vector2 position, float dx)
        {
            newAcid = new Acid(position, dx, 0.025f);
            newAcid.LoadContent(contentManager);
            acidBalls.Add(newAcid);
            newAcid = null;
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

            if (acidBalls.Count > 0)
                foreach (Acid a in acidBalls)
                    a.Update(theGameTime);

            this.RemoveInvisibleCoins();
            this.RemoveInvisibleFirstAids();
            this.RemoveInvisibleAcids();
        }

        private void RemoveInvisibleAcids()
        {
            foreach(Acid a in acidBalls)
                if(!a.Visible)
                {
                    acidBalls.Remove(a);
                    break;
                }
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

            foreach (Acid a in acidBalls)
                a.Draw(theSpriteBatch);
        }
    }
}
