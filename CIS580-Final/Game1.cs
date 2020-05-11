using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CIS580_Final
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        // Math constants for buildings
        private const double BtcPerClick = 0.00011d;
        private const double BtcPerCpu = 0.000011d;
        private const double BtcPerGpu = 0.00011d;
        private const double BtcPerServer = 0.00088d;
        private const double BtcPerMiner = 0.00517d;
        private const double BtcPerSupercomputer = 0.0286;
        MouseState mouseState;
        MouseState prevMouseState;
        private Button button1 = new Button(100, 100, 100, 100);

        /// <summary>
        /// A list of all items the user has built
        /// </summary>
        private List<Building> buildings = new List<Building>();

        /// <summary>
        /// The number of Bitcoin the user has collected
        /// </summary>
        private double Bitcoin { get; set; }
        
        /// <summary>
        /// Constructs a new instance of the game
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            IsMouseVisible = true;
            prevMouseState = Mouse.GetState();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            mouseState = Mouse.GetState();

            
            // TODO: Add your update logic here
            double bps = 0.0d;

            buildings.ForEach(building =>
            {
                switch (building.Type)
                {
                    case BuildingType.Cpu:
                        bps += BtcPerCpu;
                        break;
                    case BuildingType.Gpu:
                        bps += BtcPerGpu;
                        break;
                    case BuildingType.Server:
                        bps += BtcPerServer;
                        break;
                    case BuildingType.Miner:
                        bps += BtcPerMiner;
                        break;
                    case BuildingType.Supercomputer:
                        bps += BtcPerSupercomputer;
                        break;
                }
            });

            Bitcoin += bps * gameTime.ElapsedGameTime.Seconds;
            
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released && button1.IsClicked(mouseState) == true)
            {
                Console.WriteLine("Button clicked");
            }
            base.Update(gameTime);
            prevMouseState = mouseState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
