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
        
        int FPS = 0;
        int frame = 0;
        float TIME = 0.0f;
        

        const int WIDTH = 800;
        const int HEIGHT = 500;

        SpriteFont font;

        KeyboardState currentKeyboardState;
        KeyboardState oldKeyboardState;

        Player p;
        Level l;
        Tower t;

        Camera camera;

        CollisionHandler ch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            //IsMouseVisible = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            p = new Player(new Vector2(0, 0));
            l = new Level();
            t = new Tower(new Vector2(1024, 256 + 24 * 1 / 0.667f)); //0.667f = t.Scale

            camera = new Camera(graphics.GraphicsDevice.Viewport);

            ch = new CollisionHandler();

            l.Initialize(0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Font_1");

            p.LoadContent(this.Content);
            l.LoadContent(this.Content);
            t.LoadContent(this.Content);
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            currentKeyboardState = Keyboard.GetState();

            ch.HandleMovingCollision(p, l);

            p.Update(Mouse.GetState(), currentKeyboardState, oldKeyboardState, gameTime, camera);
            t.Update(p);
            
            oldKeyboardState = currentKeyboardState;

            TIME += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(TIME > 1.0f)
            {
                TIME = 0.0f;
                FPS = frame;
                frame = 0;
            }

            camera.Update(p);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                null, null, null, null,
                camera.transform);

            p.Draw(this.spriteBatch);
            l.Draw(this.spriteBatch);
            t.Draw(this.spriteBatch);

            spriteBatch.DrawString(font, "FPS " + FPS, new Vector2(camera.origin.X + 10, camera.origin.Y + 10), Color.Black);
            frame++;
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
