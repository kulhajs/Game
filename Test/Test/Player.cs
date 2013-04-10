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

        public HealthBar hb;

        Rectangle[] sources = new Rectangle[] {
            new Rectangle(0, 0, 64, 64), //IDLE         //0
            new Rectangle(64, 0, 64, 64), //JUMP        //1
            new Rectangle(0, 64, 64, 64), //Running     //2
            new Rectangle(64, 64, 64, 64),              //3
            new Rectangle(128, 64, 64, 64),             //4
            new Rectangle(192, 64, 64, 64),             //5
            new Rectangle(256, 64, 64, 64),             //6
            new Rectangle(320, 64, 64, 64),             //7
            new Rectangle(384, 64, 64, 64),             //8
            new Rectangle(448, 64, 64, 64), //END Running 9
            new Rectangle(128, 0, 64, 64) //Crouch      //10
        };

        public List<Bullet> bullets;
        Bullet newBullet;
        
        public Facing currentFacing = Facing.Right;

        int animationLenght = 35; //40
        int currentFrame = 0;
        int frame = 0;
        int frameTime = 35 / 8;

        float jumpHeight = 300f;        

        const float gravity = 9.81f;   
        float reloadTime = 0.2f;

        const int maxHealth = 100;
        
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

        public int Hitpoints { get; set; }

        public int Score { get; set; }

        public bool Falling { get; set; }

        public bool Jumping { get; set; }

        public bool Crouching { get; set; }

        public bool Sliding { get; set; }

        public bool Push { get; set; }

        public float RelativeX { get; private set; }

        public FlashDoor IntersectWithSwitch { get; set; }

        public Player(Vector2 position, float initRotation=0.0f)
        {
            this.Position = position;
            this.Rotation = initRotation;
            this.Scale = 0.667f;
            this.Hitpoints = maxHealth;
            this.Score = 0;
            this.Color = Color.White;
            this.Falling = true;
            this.Jumping = false;
            this.Crouching = false;
            this.Sliding = false;
            this.Push = false;
            this.bullets = new List<Bullet>(100);
            hb = new HealthBar();
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager theContentManager)
        {
            contentManager = theContentManager;
            body = contentManager.Load<Texture2D>("body_anim_2");
            gun = contentManager.Load<Texture2D>("gun");
            crosshair = contentManager.Load<Texture2D>("crosshair");
            this.Source = sources[0];
            hb.LoadContent(contentManager);
        }

        public void Update(MouseState mouseState, KeyboardState currentKeyboardState, KeyboardState oldKeyboardState, GameTime theGameTime, Camera camera, ExplosionHandler explosions)
        {
            hb.Update(camera, (int)((double)Hitpoints * 1.18));

            if ((mouseState.X + camera.origin.X) - this.X > 0)
                currentFacing = Facing.Right;
            else
                currentFacing = Facing.Left;

            //aiming
            float dx = (mouseState.X + camera.origin.X) - this.X;
            float dy = (mouseState.Y + camera.origin.Y) - this.Y;
            if (dx > 15 || dx < 15)
                this.Rotation = FAtan((dy / 10) / (dx / 10));

            //door switch interaction
            if (IntersectWithSwitch != null && IntersectWithSwitch.Switch && UseKeyPressed(currentKeyboardState, oldKeyboardState))
                IntersectWithSwitch.Switch = false;
            else if (IntersectWithSwitch != null && !IntersectWithSwitch.Switch && UseKeyPressed(currentKeyboardState, oldKeyboardState))
                IntersectWithSwitch.Switch = true;

            this.UpdateMovement(currentKeyboardState, oldKeyboardState, theGameTime);
            this.UpdateAnimation();

            if (mouseState.LeftButton == ButtonState.Pressed && reloadTime < 0.0f)
                this.UpdateAttack(mouseState, camera);

            foreach (Bullet b in bullets)
                if (b.Visible)
                    b.Update(theGameTime);

            this.RemoveBullets(explosions);

            if (reloadTime > 0.0f)
                reloadTime -= (float)theGameTime.ElapsedGameTime.TotalSeconds;

            if (this.Y > 500)
                this.Hitpoints = 0;

            if (currentFacing == Facing.Right)
                gunPosition = Crouching ? new Vector2(X + 27 * this.Scale, Y + 32 * this.Scale) : new Vector2(X + 26 * this.Scale, Y + 20 * this.Scale);
            else
                gunPosition = Crouching ? new Vector2(X + 36 * this.Scale, Y + 32 * this.Scale) : new Vector2(X + 38 * this.Scale, Y + 20 * this.Scale);

            crosshairPosition = new Vector2(mouseState.X - 4 + camera.origin.X, mouseState.Y - 4 + camera.origin.Y);
        }

        private void RemoveBullets(ExplosionHandler explosions)
        {
            foreach (Bullet b in bullets)
                if (!b.Visible)
                {
                    bullets.Remove(b);
                    if (b.SprayBlood)
                        explosions.AddExplosion(b.Position, contentManager, 4, 15, "blood", 32);
                    break;
                }
        }

        private void UpdateAttack(MouseState mouseState, Camera camera)
        {
            //direction of new bullet
            float x = mouseState.X - (this.X - camera.origin.X);
            float y = mouseState.Y - (this.Y - camera.origin.Y);
            
            Vector2 dir = new Vector2(x, y);
            dir.Normalize();

            //coordinates of new bullet position
            float dist = Fsqrt((30 * 30 * Scale * Scale) + (7 * 7 * Scale * Scale)); //distance from Vector2.origin to position where new bullet appears
            float xx;
            float yy;
            if (currentFacing == Facing.Right)
            {
                xx = gunPosition.X + FCos(Rotation) * dist;
                yy = gunPosition.Y + FSin(Rotation) * dist;
            }
            else
            {
                xx = gunPosition.X - FCos(Rotation) * dist;
                yy = gunPosition.Y - FSin(Rotation) * dist;
            }

            newBullet = new Bullet(new Vector2(xx, yy), dir, this.Rotation);
            newBullet.LoadContent(contentManager);
            bullets.Add(newBullet);
            newBullet = null;
            reloadTime = 0.2f;
        }


        private void UpdateAnimation()
        {
            if (Jumping || Falling)        
                this.Source = sources[1];
            else if (DX == 0 && !Crouching)
                this.Source = sources[0];
            else if (Crouching)
                this.Source = sources[10];
            else
            {
                if (currentFrame < sources.Length - 1)
                    this.Source = sources[currentFrame];
                else
                    currentFrame = 2;

                if (currentFrame < 2)
                    currentFrame = 9;

                frame++;
                if(frame > frameTime)
                {
                    if ((DX > 0 && currentFacing == Facing.Right) || (DX < 0 && currentFacing == Facing.Left))
                        currentFrame++;
                    else if ((DX < 0 && currentFacing == Facing.Right) || (DX > 0 && currentFacing == Facing.Left))
                        currentFrame--;
                    frame = 0;
                }
            }
        }

        private void UpdateMovement(KeyboardState currentKeyboardState, KeyboardState oldKeyboardState, GameTime theGameTime)
        {
            DY = 0;
            
            if(!Push && !Sliding) //if player is not pushed away from doors (you cannot control your movement during that)
            { 
                if (currentKeyboardState.IsKeyDown(Keys.D) && oldKeyboardState.IsKeyUp(Keys.A)  && !Crouching) //!jumping && !falling so you cannot modify horizontal movement while jumping/falling
                {
                    DX = 1;
                }
                else if (currentKeyboardState.IsKeyDown(Keys.A) && oldKeyboardState.IsKeyUp(Keys.D) && !Crouching)
                {
                    DX = -1;
                }
                else //if (!Jumping && !Falling) //if not jumping or falling, horizontal velocity = 0 no matter the previous key
                    DX = 0.0f;
            }

            if(Push)
            {
                if (DX < -1) //push to left
                    DX += 0.25f; //addition to given direction (-4), till it's standard direction (-1)
                else if (DX > 1) //push to right           |
                    DX -= 0.25f; //                      <-/ the same
                else
                    Push = false;
            }

            if (Sliding)
            {
                if (DX < -1) //push to left
                    DX += 0.05f; //addition to given direction , till it's standard direction abs(1)
                else if (DX > 1) //push to right
                    DX -= 0.05f;
                else
                    Sliding = false; 
            }

            if(currentKeyboardState.IsKeyDown(Keys.W) && !oldKeyboardState.IsKeyDown(Keys.W) && !Falling && !Crouching)
            {
                velocity.Y = jumpHeight; 
                Jumping = true;
            }


            if (Jumping)
            {
                this.Crouching = false;
                velocity.Y -= gravity;
                DY = -1f;
                if (velocity.Y < 0)
                    Jumping = false;
            }
            else if (Falling)
            {
                this.Crouching = false;
                velocity.Y += gravity;
                DY = 1f;
            }
            else
                velocity.Y = 0;

            //if not in air or already sliding then crouch, if crouch while running then slide!  - can't slide while running backwards
            if (currentKeyboardState.IsKeyDown(Keys.S) && ((currentFacing == Facing.Right && DX > 0) || (currentFacing == Facing.Left && DX < 0)) && !Falling && !Jumping && !Crouching)
            {
                this.Crouching = true;
                if (DX != 0)
                {
                    this.Sliding = true;
                    DX *= 2; //multiplication of current direction (-2 or 2)
                }
            }
            else if (currentKeyboardState.IsKeyUp(Keys.S) && !Sliding)
                Crouching = false;

            
            this.Position += Direction * velocity * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        private bool UseKeyPressed(KeyboardState currentKeyboardState, KeyboardState oldKeyboardState)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Space) && !oldKeyboardState.IsKeyDown(Keys.Space))
                return true;
            else if (currentKeyboardState.IsKeyDown(Keys.E) && !oldKeyboardState.IsKeyDown(Keys.E))
                return true;
            else
                return false;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            //draw body
            theSpriteBatch.Draw(body, this.Position, this.Source, this.Color, 0.0f, Vector2.Zero, this.Scale, 
                currentFacing == Facing.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);
            
            //draw gun
            theSpriteBatch.Draw(gun, gunPosition,
                new Rectangle(0, 0, 64, 64), this.Color, this.Rotation, 
                currentFacing == Facing.Right ? new Vector2(25, 19) : new Vector2(39,19), this.Scale, 
                currentFacing == Facing.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);

            //drawcrosshair
            theSpriteBatch.Draw(crosshair, crosshairPosition, new Rectangle(0, 0, 16, 16), Color.Green);
            
            foreach (Bullet b in bullets)
                if (b.Visible)
                    b.Draw(theSpriteBatch);
        }
    }
}
