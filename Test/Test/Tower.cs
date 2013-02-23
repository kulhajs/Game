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

        int vision = 400;

        float reloadTime = 0f;

        Rectangle[] sources = new Rectangle[] {
            new Rectangle(0, 0, 64, 64),
            new Rectangle(64, 0, 64, 64), 
            new Rectangle(128, 0, 64, 64)
        };

        List<Bullet> bullets;
        Bullet newBullet;

        Vector2 bulletDirection;

        public Tower(Vector2 position)
        {
            this.Position = position;
            this.Scale = 0.667f;
            bullets = new List<Bullet>();
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
                this.Rotation = (float)Math.Atan((double)((dy - 10) / dx)); //(dy-10) so it doesn't aim on top of head
                lightColor = 2;
                this.SeePlayer = true;
            }
            else
            {
                lightColor = 0;
                this.SeePlayer = false;
            }

            if (reloadTime < 0.1f)
                reloadTime += (float)theGameTime.ElapsedGameTime.TotalSeconds;

            if(reloadTime > 0.1f && SeePlayer)
            {
                bulletDirection = new Vector2(p.X - this.X, (p.Y + 10) - this.Y); //(dy-10) so it doesn't shoot on top of head
                bulletDirection.Normalize();

                newBullet = new Bullet(this.Position, bulletDirection, this.Rotation);
                newBullet.LoadContent(contentManager);
                bullets.Add(newBullet);
                newBullet = null;
                reloadTime = 0f;
            }

            foreach (Bullet b in bullets)
                b.Update(theGameTime);

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
            theSpriteBatch.Draw(gun, 
                new Vector2(this.Position.X + 33 * this.Scale, this.Position.Y + 9 * this.Scale), 
                new Rectangle(0,0,64,64), Color.White, this.Rotation, 
                new Vector2(33, 9), this.Scale ,SpriteEffects.None, 0);

            theSpriteBatch.Draw(body, this.Position, this.Source, Color.White, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0.0f);

            foreach (Bullet b in bullets)
                b.Draw(theSpriteBatch);
        }
    }
}
