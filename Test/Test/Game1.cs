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
    enum State{
        Menu,
        GameRunning,
        GamePaused,
        GameOver,
        Credits
    };

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const string industrial = "industrial_rowan"; //blocks_industrial
        const string stone = "stone_rowan"; //blocks_stone
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

        ItemHandler items;

        EnemyHandler enemies;
        ExplosionHandler explosions;
        SoundHandler sounds;

        Camera camera;

        CollisionHandler ch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }
        
        protected override void Initialize()
        {
            p = new Player(new Vector2(0, 0));
            l = new Level();
            l.Initialize(currentLevel, currentLevelType);

            items = new ItemHandler(this.Content);
            items.Initialize(currentLevel);

            enemies = new EnemyHandler();
            enemies.Initiliaze(currentLevel);
            explosions = new ExplosionHandler();
            sounds = new SoundHandler();

            camera = new Camera(graphics.GraphicsDevice.Viewport);

            ch = new CollisionHandler();


            this.ChangeLevel = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Font_1");
            backgroundIndustrial = Content.Load<Texture2D>("background_industrial_rowan");
            clouds = Content.Load<Texture2D>("clouds");

            p.LoadContent(this.Content);
            l.LoadContent(this.Content);


            items.LoadContent(this.Content);

            enemies.LoadContent(this.Content);
            sounds.LoadContent(this.Content);
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            
            currentKeyboardState = Keyboard.GetState();

            ch.HandleMovingCollision(p, l, camera);
            ch.HandleZombiesMovingCollision(enemies, l);
            ch.HandleZombiePlayerCollision(p, enemies, explosions, sounds);
            ch.HandleBulletZombieCollision(p, enemies);
            ch.HandleBulletTurretCollision(p, enemies, explosions);
            ch.HandleDoorCollision(p, enemies);
            ch.HandleRocketCollision(p, enemies);
            ch.HandleItemCollision(p, items, explosions, l, sounds);
            ch.HandleEndLevel(this, p, l);

            p.Update(Mouse.GetState(), currentKeyboardState, oldKeyboardState, gameTime, camera, explosions);

            enemies.Update(p, gameTime, explosions, items, sounds, camera);

            explosions.Update();

            items.Update(gameTime);

            l.Update();

            camera.Update(p);

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

            if(ChangeLevel)
            {
                if (currentLevel < 4)
                {
                    currentLevel++;
                }
                else
                {
                    currentLevel = 0;
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
            items.Draw(this.spriteBatch);
            l.endOflevel.DrawBack(this.spriteBatch);
            p.Draw(this.spriteBatch);
            l.endOflevel.DrawFront(this.spriteBatch);
            explosions.Draw(this.spriteBatch);

            this.spriteBatch.Draw(clouds, new Vector2(camera.origin.X, -320), new Rectangle(0, 0, 800, 800), Color.White);

            spriteBatch.DrawString(font, FPS + " FPS ", new Vector2(camera.origin.X + 10, camera.origin.Y + 10), Color.Black);
            spriteBatch.DrawString(font, "SCORE: " + p.Score, new Vector2(camera.origin.X + 350, camera.origin.Y + 10), Color.Black);
            p.hb.Draw(this.spriteBatch, new Vector2(camera.origin.X + 670, camera.origin.Y + 10));

            frame++;
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
