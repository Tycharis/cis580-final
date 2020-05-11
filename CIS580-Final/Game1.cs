using System.Collections.Generic;
using System;
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

        Vector2 mousePosition;
        
        MouseState mouseState;
        MouseState prevMouseState;

        //Textures

        Texture2D background;
        Texture2D test;
        Texture2D bitCoinTexture;


        //Buttons
        Button testButton;
        Button bitCoinButton;

        //Text
        SpriteFont font;

        

        // Math constants for buildings
        private const double BtcPerClick = 0.00011d;
        private const double BtcPerCpu = 0.000011d;
        private const double BtcPerGpu = 0.00011d;
        private const double BtcPerServer = 0.00088d;
        private const double BtcPerMiner = 0.00517d;
        private const double BtcPerSupercomputer = 0.0286;

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

            base.Initialize();
            IsMouseVisible = true;

            //Window settings
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.ApplyChanges();

            //Sprite font
            font = Content.Load<SpriteFont>("text");

            mousePosition = new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2, _graphics.GraphicsDevice.Viewport.Height / 2);
            //Intialize textures
            background = this.Content.Load<Texture2D>("GameBG");
            test = this.Content.Load<Texture2D>("pixel");
            bitCoinTexture = this.Content.Load<Texture2D>("Bitcoin_Coin");

            //Buttons
            testButton = new Button(724, 300, 300, 100, test);
            bitCoinButton = new Button(0, 200, 400, 400, bitCoinTexture);

            prevMouseState = Mouse.GetState();

            

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

            mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // BitCoin per second updates
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

            //Mouse Controls

            mousePosition.X = mouseState.X;
            mousePosition.Y = mouseState.Y;

            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released && bitCoinButton.IsClicked(mouseState) == true)
            {

                Bitcoin += BtcPerClick;
                Console.WriteLine("Bitcoin Button clicked");
            }
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released && testButton.IsClicked(mouseState) == true)
            {
                Console.WriteLine("Button clicked");
            }
            prevMouseState = mouseState;


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            _spriteBatch.Begin();
      

            _spriteBatch.Draw(background, new Rectangle (0,0,1024,768), Color.White);
            _spriteBatch.Draw(test, new Rectangle(50, 150, 300, 40), Color.Wheat);


            _spriteBatch.DrawString(font, "Score: " + Bitcoin, new Vector2(50, 150), Color.Black);




            //Button draws
            testButton.Draw(_spriteBatch);
            bitCoinButton.Draw(_spriteBatch);
            _spriteBatch.Draw(test, new Rectangle(954, 315, 60, 30), Color.DarkRed);
            _spriteBatch.Draw(test, new Rectangle(954, 355, 60, 30), Color.DarkRed);




            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
