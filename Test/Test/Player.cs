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
    enum Facing
    {
        Left,
        Right
    }

    class Player : Sprite
    {
        Texture2D body;
        Texture2D gun;
        Texture2D crosshair;

        int WIDTH = 800;
        int HEIGHT = 500;

        Rectangle[] sources = new Rectangle[] {
            new Rectangle(0, 0, 64, 64), //IDLE
            new Rectangle(64, 0, 64, 64), //JUMP
            new Rectangle(0, 64, 64, 64), //Running
            new Rectangle(64, 64, 64, 64),
            new Rectangle(128, 64, 64, 64), 
            new Rectangle(192, 64, 64, 64), 
            new Rectangle(256, 64, 64, 64), 
            new Rectangle(320, 64, 64, 64),
            new Rectangle(384, 64, 64, 64), 
            new Rectangle(448, 64, 64, 64) //END Running
        };

        List<Bullet> bullets;
        Bullet newBullet;
        
        Facing currentFacing = Facing.Right;

        int animationLenght = 40;
        int currentFrame = 0;

        const float gravity = 8f;
        float reloadTime = 0f;
        
        Vector2 velocity = new Vector2(150, 0);

        Vector2 direction;

        Vector2 gunPosition = Vector2.Zero;

        Vector2 crosshairPosition = Vector2.Zero;

        public Vector2 Direction 
            {
                get { return this.direction; }
                set { this.direction = value; }
        }

        public float DX { get { return this.direction.X; }
            set { this.direction.X = value; }
        }
        public float DY { get { return this.direction.Y; }
            set { this.direction.Y = value; }
        }

        public float VY { get { return velocity.Y; }
            set { velocity.Y = value; }
        }

        public bool Falling { get; set; }

        public bool Jumping { get; set; }
        
        public Player(Vector2 position, float initRotation=0.0f)
        {
            this.Position = position;
            this.Rotation = initRotation;
            this.Scale = 0.667f;
            this.Falling = true;
            this.Jumping = false;
            this.bullets = new List<Bullet>();
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager theContentManager)
        {
            contentManager = theContentManager;
            body = contentManager.Load<Texture2D>("body_anim_2");
            gun = contentManager.Load<Texture2D>("gun");
            crosshair = contentManager.Load<Texture2D>("crosshair");
            this.Source = sources[0];
        }

        public void Update(MouseState mouseState, KeyboardState currentKeyboardState, KeyboardState oldKeyboardState,  GameTime theGameTime, Camera camera)
        {
            float dx = (mouseState.X + camera.origin.X) - this.X;
            float dy = mouseState.Y - this.Y;
            this.Rotation = (float)Math.Atan((double)(dy / dx));

            this.UpdateMovement(currentKeyboardState, oldKeyboardState, theGameTime);
            this.UpdateAnimation();

            if (mouseState.LeftButton == ButtonState.Pressed)
                this.UpdateAttack(mouseState, camera);

            foreach (Bullet b in bullets)
                b.Update(theGameTime);

            if (reloadTime < 0.2f)
                reloadTime += (float)theGameTime.ElapsedGameTime.TotalSeconds;

            if (currentFacing == Facing.Right)
                gunPosition = new Vector2(X + 26 * this.Scale, Y + 20 * this.Scale);
            else
                gunPosition = new Vector2(X + 38 * this.Scale, Y + 20 * this.Scale);

            crosshairPosition = new Vector2(mouseState.X + camera.origin.X, mouseState.Y + camera.origin.Y);
        }

        private void UpdateAttack(MouseState mouseState, Camera camera)
        {
            if (reloadTime > 0.2f)
            {
                //direction of new bullet
                float x = mouseState.X - (this.X - camera.origin.X);
                float y = mouseState.Y - (this.Y - camera.origin.Y);
                //if mouse is pointing in opposite direction than player, it won't shoot
                if ((x < 0 && currentFacing == Facing.Right) || (x > 0 && currentFacing == Facing.Left))
                    return;
                Vector2 dir = new Vector2(x, y);
                dir.Normalize();

                //coordinates of new bullet position
                float dist = (float)Math.Sqrt((double)(30 * Scale * 30 * Scale) + (7 * Scale * 7 * Scale));
                float xx;
                float yy;
                if (currentFacing == Facing.Right)
                {
                    xx = gunPosition.X + (float)Math.Cos((double)Rotation) * dist;
                    yy = gunPosition.Y + (float)Math.Sin((double)Rotation) * dist;
                }
                else
                {
                    xx = gunPosition.X - (float)Math.Cos((double)Rotation) * dist;
                    yy = gunPosition.Y - (float)Math.Sin((double)Rotation) * dist;
                }

                newBullet = new Bullet(new Vector2(xx, yy), dir, this.Rotation);
                newBullet.LoadContent(contentManager);
                bullets.Add(newBullet);
                newBullet = null;
                reloadTime = 0f;
            }

        }

        private void UpdateAnimation()
        {
            if (Jumping || Falling)
                this.Source = sources[1];
            else if (DX == 0)
                this.Source = sources[0];
            else
            {
                if (currentFrame < animationLenght / 8)
                    Source = sources[2];
                else if (currentFrame < animationLenght * 2 / 8)
                    Source = sources[3];
                else if (currentFrame < animationLenght * 3 / 8)
                    Source = sources[4];
                else if (currentFrame < animationLenght * 4 / 8)
                    Source = sources[5];
                else if (currentFrame < animationLenght * 5 / 8)
                    Source = sources[6];
                else if (currentFrame < animationLenght * 6 / 8)
                    Source = sources[7];
                else if (currentFrame < animationLenght * 7 / 8)
                    Source = sources[8];
                else if (currentFrame < animationLenght)
                    Source = sources[9];
                else
                    currentFrame = 0;

                currentFrame++;
            }
        }

        private void UpdateMovement(KeyboardState currentKeyboardState, KeyboardState oldKeyboardState, GameTime theGameTime)
        {
            DY = 0;

            if (currentKeyboardState.IsKeyDown(Keys.D) && DX == 0 && !Jumping && !Falling)
            {
                DX = 1;
                currentFacing = Facing.Right;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.A) && DX == 0 && !Jumping && !Falling)
            {
                DX = -1;
                currentFacing = Facing.Left;
            }

            if (currentKeyboardState.IsKeyUp(Keys.D) && oldKeyboardState.IsKeyDown(Keys.D) && !Jumping && !Falling)
                DX = 0f;
            else if (currentKeyboardState.IsKeyUp(Keys.A) && oldKeyboardState.IsKeyDown(Keys.A) && !Jumping && !Falling)
                DX = 0f;
            else if (currentKeyboardState.IsKeyUp(Keys.D) && currentKeyboardState.IsKeyUp(Keys.A) && !Jumping && !Falling)
                DX = 0f;

            if(currentKeyboardState.IsKeyDown(Keys.Space) && !oldKeyboardState.IsKeyDown(Keys.Space) && !Falling)
            {
                velocity.Y = 270f; //200f
                Jumping = true;
            }

            if (Jumping)
            {
                velocity.Y -= gravity; //5f
                DY = -1f;
                if (velocity.Y < 0)
                    Jumping = false;
            }
            else if (Falling)
            {
                velocity.Y += gravity; //5f
                DY = 1f;
                Jumping = false;
            }
            else
                velocity.Y = 0;

            //if (this.X > WIDTH / 2)
            //    this.X = WIDTH / 2;
            
            this.Position += Direction * velocity * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            //draw body
            theSpriteBatch.Draw(body,  this.Position, this.Source, Color.White, 0.0f, Vector2.Zero, this.Scale, 
                currentFacing == Facing.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);
            
            //draw gun
            theSpriteBatch.Draw(gun, gunPosition,
                new Rectangle(0, 0, 64, 64), Color.White, this.Rotation, 
                currentFacing == Facing.Right ? new Vector2(25, 19) : new Vector2(39,19), this.Scale, 
                currentFacing == Facing.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);

            //drawcrosshair
            theSpriteBatch.Draw(crosshair, crosshairPosition, new Rectangle(0, 0, 16, 16), Color.LimeGreen);

            foreach (Bullet b in bullets)
                b.Draw(theSpriteBatch);
        }
    }
}
