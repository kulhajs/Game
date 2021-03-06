﻿using System;
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

        Random random = new Random();

        int currentFrame = 0;
        int animationLength = 24;

        int lightColor = 0;

        int vision = 375;

        float reloadTime = 0f;
        float initRealoadTime; 

        const int rocketAnimationLength = 15;
        const int explosionFrameCount = 5;

        public bool OnScreen { get; set; }

        Rectangle[] sources = new Rectangle[] {
            new Rectangle(0, 0, 64, 64),
            new Rectangle(64, 0, 64, 64), 
            new Rectangle(128, 0, 64, 64)
        };

        public List<Rocket> rockets;
        Rocket newRocket;

        Vector2 rocketDirection;

        public Tower(Vector2 position)
        {
            this.Position = position;
            this.Scale = 0.667f;
            this.initRealoadTime = random.Next(100, 125) / 100;
            rockets = new List<Rocket>(10);
        }

        public bool SeePlayer { get; set; }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            gun = contentManager.Load<Texture2D>("turretGun_rowan");
            body = contentManager.Load<Texture2D>("turretBody_rowan");
            Source = sources[0]; //Source = sources[1]; using my assets
        }

        public void Update(Player p, GameTime theGameTime, ExplosionHandler explosions, SoundHandler sounds, Camera c)
        {

            foreach (Rocket r in rockets)
                if (r.Visible)
                    r.Update(theGameTime, new Vector2((p.X + 21 * p.Scale) - r.X, (p.Y + 3 * p.Scale) - r.Y)); //(p.x+21); (p.y+3) so it doesn't aim at player.origin 


            if (reloadTime < initRealoadTime)
                reloadTime += (float)theGameTime.ElapsedGameTime.TotalSeconds;

            //this.Animate();
            this.RemoveRocket(explosions, sounds, p);

            this.OnScreen = this.X - c.origin.X < 800 ? true : false;
            
            if (!OnScreen)
                return;

            float dx = this.Position.X - p.X;
            float dy = this.Position.Y - p.Y;
            float dist = dx * dx + dy * dy;     //distance from player to tower

            if (dx > 64 && dist < vision * vision && (dy < 100 && dy > -30)) //if distance.X < 64 and distance y < 100 && > 30 then Rotate - facing player
            {
                this.Rotation = FAtan((dy + 2) / dx); //(y+2) so it doesn't aim at very top of head
                lightColor = 2;
                this.SeePlayer = true;
            }
            else
            {
                lightColor = 0;
                this.SeePlayer = false;
            }

            if (reloadTime > initRealoadTime && SeePlayer)
            {
                rocketDirection = new Vector2((p.X + 21 * p.Scale) - this.X, (p.Y + 3 * p.Scale) - this.Y); //(p.Y + 3) so it doesn't shoot on top of head
                rocketDirection.Normalize();
                
                float xx = this.X + FCos(Rotation) * Fsqrt((32 * 32 * Scale * Scale) + (9 * 9 * Scale * Scale)); //x coordinate of new rocket

                newRocket = new Rocket(new Vector2(xx, Y + 5 * Scale), rocketDirection, this.Rotation);
                newRocket.LoadContent(contentManager);
                rockets.Add(newRocket);
                newRocket = null;
                reloadTime = 0f;
            }

        }

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)(X + 15 * Scale), (int)(Y + 5 * Scale), (int)(15 * Scale), (int)(50 * Scale));
        }

        private void RemoveRocket(ExplosionHandler explosions, SoundHandler sounds, Player p)
        {
            foreach(Rocket r in this.rockets)
            {
                if (!r.Visible)
                {
                    sounds.PlayExplosion(p, r);
                    explosions.AddExplosion(r.Position, contentManager, 5, 15, "explosion", 32); //each time rocket is removed, explosion is created
                    rockets.Remove(r);
                    break;
                }
            }
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
            foreach (Rocket r in rockets)
                if (r.Visible)
                    r.Draw(theSpriteBatch);

            theSpriteBatch.Draw(gun, 
                new Vector2(this.Position.X + 28 * this.Scale, this.Position.Y + 12 * this.Scale),  //X+33; Y+9 using own assets
                new Rectangle(0,0,64,64), Color.White, this.Rotation,
                new Vector2(28, 12), this.Scale, SpriteEffects.None, 0);  //Vector2(33,9)

            theSpriteBatch.Draw(body, this.Position, this.Source, Color.White, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0.0f);
        }
    }
}
