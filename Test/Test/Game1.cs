using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Test
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const string industrial = "blocks_industrial";
        const string stone = "blocks_stone";
        const string grass = "blocks_grass";
        
        int FPS = 0;
        int frame = 0;
        float TIME = 0.0f;
                
        const int WIDTH = 800;
        const int HEIGHT = 480;

        int currentLevel = 0;
        string currentLevelType = industrial;

        public bool ChangeLevel { get; set; }

        SpriteFont font;

        Texture2D backgroundIndustrial;
        Texture2D clouds;

        KeyboardState currentKeyboardState;
        KeyboardState oldKeyboardState;

        Player p;
        Level l;

        ItemHandler ih;

        EnemyHandler enemies;
        ExplosionHandler explosions;

        Camera camera;

        CollisionHandler ch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }
        
        protected override void Initialize()
        {
            p = new Player(new Vector2(0, 0));
            l = new Level();

            ih = new ItemHandler(this.Content);
            ih.Initialize();

            enemies = new EnemyHandler();
            enemies.Initiliaze(currentLevel);
            explosions = new ExplosionHandler();

            camera = new Camera(graphics.GraphicsDevice.Viewport);

            ch = new CollisionHandler();

            l.Initialize(currentLevel, currentLevelType);

            this.ChangeLevel = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Font_1");
            backgroundIndustrial = Content.Load<Texture2D>("background_industrial");
            clouds = Content.Load<Texture2D>("clouds");

            p.LoadContent(this.Content);
            l.LoadContent(this.Content);

            ih.LoadContent(this.Content);

            enemies.LoadContent(this.Content);
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            
            currentKeyboardState = Keyboard.GetState();

            ch.HandleMovingCollision(p, l, camera, currentLevel);
            ch.HandleZombiesMovingCollision(enemies, l, currentLevel);
            ch.HandleZombiePlayerCollision(p, enemies, explosions);
            ch.HandleBulletZombieCollision(p, enemies);
            ch.HandleDoorCollision(p, enemies);
            ch.HandleRocketCollision(p, enemies);
            ch.HandleItemCollision(p, ih, explosions, l, currentLevel);
            ch.HandleEndLevel(this, p, l);

            p.Update(Mouse.GetState(), currentKeyboardState, oldKeyboardState, gameTime, camera, explosions);

            enemies.Update(p, gameTime, explosions, ih);

            explosions.Update();

            ih.Update(gameTime);

            l.Update();

            if (p.Hitpoints <= 0)
                this.Initialize();

            oldKeyboardState = currentKeyboardState;

            TIME += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(TIME > 1.0f)
            {
                TIME = 0.0f;
                FPS = frame;
                frame = 0;
            }

            camera.Update(p);

            if(ChangeLevel)
            {
                if (currentLevel < 1)
                {
                    currentLevel++;
                    currentLevelType = stone;
                }
                else
                {
                    currentLevel = 0;
                    currentLevelType = industrial;
                }
                this.Initialize();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                null, null, null, null,
                camera.transform);

            this.spriteBatch.Draw(backgroundIndustrial, new Vector2(camera.origin.X, -320), new Rectangle(0, 0, 800, 800), Color.White);

            l.Draw(this.spriteBatch);
            enemies.Draw(this.spriteBatch);
            ih.Draw(this.spriteBatch);
            l.endOflevel.DrawBack(this.spriteBatch);
            p.Draw(this.spriteBatch);
            l.endOflevel.DrawFront(this.spriteBatch);
            explosions.Draw(this.spriteBatch);

            this.spriteBatch.Draw(clouds, new Vector2(camera.origin.X, -320), new Rectangle(0, 0, 800, 800), Color.White);

            spriteBatch.DrawString(font, FPS + " FPS ", new Vector2(camera.origin.X + 10, camera.origin.Y + 10), Color.Black);
            spriteBatch.DrawString(font, "SCORE: " + p.Score, new Vector2(camera.origin.X + 350, camera.origin.Y + 10), Color.Black);
            p.hb.Draw(this.spriteBatch, new Vector2(camera.origin.X + 670, camera.origin.Y + 10));
            //spriteBatch.DrawString(font, p.Hitpoints + " HP", new Vector2(camera.origin.X + 720, camera.origin.Y + 10), Color.Black);

            frame++;
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
