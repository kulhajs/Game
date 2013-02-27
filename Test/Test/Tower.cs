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
    class Tower : Sprite
    {
        Texture2D gun;
        Texture2D body;

        int currentFrame = 0;
        int animationLength = 24;

        int lightColor = 0;

        int vision = 375;

        float reloadTime = 0f;

        Rectangle[] sources = new Rectangle[] {
            new Rectangle(0, 0, 64, 64),
            new Rectangle(64, 0, 64, 64), 
            new Rectangle(128, 0, 64, 64)
        };

        List<Rocket> bullets;
        Rocket newBullet;

        Vector2 bulletDirection;

        public Tower(Vector2 position)
        {
            this.Position = position;
            this.Scale = 0.667f;
            bullets = new List<Rocket>();
        }

        public bool SeePlayer { get; set; }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            gun = contentManager.Load<Texture2D>("enemy_tower_gun");
            body = contentManager.Load<Texture2D>("enemy_tower_body");
            Source = sources[1];
        }

        public void Update(Player p, GameTime theGameTime)
        {
            float dx = this.Position.X - p.X;
            float dy = this.Position.Y - p.Y;
            float dist = dx * dx + dy * dy;

            if (dx > 32 && dist < vision * vision && (dy < 100 && dy > -30)) //TODO: Light Color / complete vision
            {
                this.Rotation = (float)Math.Atan((double)((dy - 2) / dx)); //(dy-10) so it doesn't aim on top of head
                lightColor = 2;
                this.SeePlayer = true;
            }
            else
            {
                lightColor = 0;
                this.SeePlayer = false;
            }

            if (reloadTime < 0.8f)
                reloadTime += (float)theGameTime.ElapsedGameTime.TotalSeconds;

            if(reloadTime > 0.8f && SeePlayer)
            {
                bulletDirection = new Vector2(p.X - this.X, (p.Y + 2) - this.Y); //(dy-10) so it doesn't shoot on top of head
                bulletDirection.Normalize();
                
                float xx = this.X + (float)Math.Cos((double)Rotation) * (float)Math.Sqrt((double)(32 * 32 * Scale * Scale) + (9 * 9 * Scale * Scale));

                newBullet = new Rocket(new Vector2(xx, Y + 5 * Scale), bulletDirection, this.Rotation);
                newBullet.LoadContent(contentManager);
                bullets.Add(newBullet);
                newBullet = null;
                reloadTime = 0f;
            }

            foreach (Rocket b in bullets)
                b.Update(theGameTime, new Vector2(p.X - b.X, (p.Y + 2) - b.Y));

            this.Animate();
        }

        private void Animate()
        {
            if (currentFrame < animationLength / 2)
                Source = sources[1];
            else if (currentFrame < animationLength)
                Source = sources[lightColor];
            else
                currentFrame = 0;

            currentFrame++;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Rocket b in bullets)
                b.Draw(theSpriteBatch);

            theSpriteBatch.Draw(gun, 
                new Vector2(this.Position.X + 33 * this.Scale, this.Position.Y + 9 * this.Scale), 
                new Rectangle(0,0,64,64), Color.White, this.Rotation, 
                new Vector2(33, 9), this.Scale ,SpriteEffects.None, 0);

            theSpriteBatch.Draw(body, this.Position, this.Source, Color.White, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0.0f);
        }
    }
}
