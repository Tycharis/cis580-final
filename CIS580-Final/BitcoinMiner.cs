using System.Collections.Generic;
using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CIS580_Final
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class BitcoinMiner : Game
    {
        readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Vector2 _mousePosition;
        
        private MouseState _mouseState;
        private MouseState _prevMouseState;

        //Textures
        private Texture2D _background;
        private Texture2D _test;
        private Texture2D _bitcoinTexture;


        //Buttons
          //Button testButton;
        Button bitCoinButton;
        Button cpuBuy;
        Button cpuUpgrade;
        Button gpuBuy;
        Button gpuUpgrade;
        Button serverBuy;
        Button serverUpgrade;
        Button minerBuy;
        Button minerUpgrade;
        Button superComputerBuy;
        Button superComputerUpgrade;
        private Button _testButton;
        private Button _bitcoinButton;
        private Button _exitButton;

        

        //Text
        private SpriteFont _font;

        // Math constants for buildings
        private const double BtcPerClick = 0.00012d;

        private static double BtcPerCpu => 0.1 * BtcPerGpu;
        private static double BtcPerGpu { get; set; }
        private static double BtcPerServer => 8 * BtcPerGpu;
        private static double BtcPerMiner => 47 * BtcPerGpu;
        private static double BtcPerSupercomputer => 260 * BtcPerGpu;
        //Numerical stuff
        double bps;


        private static double CpuCost => 15 * BtcPerGpu;
        private static double GpuCost => 100 * BtcPerGpu;
        private static double ServerCost => 1100 * BtcPerGpu;
        private static double MinerCost => 12000 * BtcPerGpu;
        private static double SupercomputerCost => 130000 * BtcPerGpu;

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
        public BitcoinMiner()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            const string uri = "https://blockchain.info/tobtc?currency=USD&value=1";

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebClient client = new WebClient {UseDefaultCredentials = true};
                string data = client.DownloadString(uri);

                BtcPerGpu = Convert.ToDouble(data);
            }
            catch (WebException exception)
            {
                Console.WriteLine(exception.Message);
                BtcPerGpu = 0.00012;
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            Window.IsBorderless = true;

            bps = 0.0d;
            //Window settings
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.ApplyChanges();

            //Sprite font
            _font = Content.Load<SpriteFont>("text");

            // ReSharper disable PossibleLossOfFraction
            _mousePosition = new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2, _graphics.GraphicsDevice.Viewport.Height / 2);
            // ReSharper restore PossibleLossOfFraction

            //Buttons
              //testButton = new Button(724, 300, 300, 100, test);
            bitCoinButton = new Button(0, 200, 400, 400, _bitcoinTexture);

            //Buy buttons
            cpuBuy = new Button(954, 275, 60, 30, _test);
            gpuBuy = new Button(954, 375, 60, 30, _test);
            minerBuy = new Button(954, 475, 60, 30, _test);
            serverBuy = new Button(954, 575, 60, 30, _test);
            superComputerBuy = new Button(954, 675, 60, 30, _test);

            //Upgrade buttons
            cpuUpgrade = new Button(954, 315, 60, 30, _test);
            gpuUpgrade = new Button(954, 415, 60, 30, _test);
            minerUpgrade = new Button(954, 515, 60, 30, _test);
            serverUpgrade = new Button(954, 615, 60, 30, _test);
            superComputerUpgrade = new Button(954, 715, 60, 30, _test);





            _prevMouseState = Mouse.GetState();

            
            _testButton = new Button(724, 300, 300, 100, _test);
            _bitcoinButton = new Button(0, 200, 400, 400, _bitcoinTexture);
            _exitButton = new Button(983, 6, 23, 20, null);

            _prevMouseState = Mouse.GetState();
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

            // Initialize textures
            _background = Content.Load<Texture2D>("GameBG");
            _test = Content.Load<Texture2D>("pixel");
            _bitcoinTexture = Content.Load<Texture2D>("Bitcoin_Coin");
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

            _mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // BitCoin per second updates

            buildings.ForEach(building =>
            {
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
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


            //Mouse Controls

            _mousePosition.X = _mouseState.X;
            _mousePosition.Y = _mouseState.Y;

            if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released && bitCoinButton.IsClicked(_mouseState) == true)
            {

                Bitcoin += BtcPerClick;
                Console.WriteLine("Bitcoin Button clicked");
            }
            /*if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released && testButton.IsClicked(mouseState) == true)
            {
                Console.WriteLine("Button clicked");
            }
            */

            
            //Buy button
            if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released && cpuBuy.IsClicked(_mouseState) == true)
            {
                buildings.Add(new Building { Type = BuildingType.Cpu });
                Console.WriteLine("CPU Buy Button clicked");
            }
            if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released && gpuBuy.IsClicked(_mouseState) == true)
            {
                buildings.Add(new Building { Type = BuildingType.Gpu });

                Console.WriteLine("GPU Buy Button clicked");
            }
            if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released && minerBuy.IsClicked(_mouseState) == true)
            {
                buildings.Add(new Building { Type = BuildingType.Miner });

                Console.WriteLine("Miner Buy Button clicked");
            }
            if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released && serverBuy.IsClicked(_mouseState) == true)
            {
                buildings.Add(new Building { Type = BuildingType.Server });

                Console.WriteLine("Server Buy Button clicked");
            }
            if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released && superComputerBuy.IsClicked(_mouseState) == true)
            {
                buildings.Add(new Building { Type = BuildingType.Supercomputer });

                Console.WriteLine("SuperComputer Buy Button clicked");
            }

            //Upgrade button
            if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released && cpuUpgrade.IsClicked(_mouseState) == true)
            {
                Console.WriteLine("CPU Upgarde Button clicked");
            }
            if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released && gpuUpgrade.IsClicked(_mouseState) == true)
            {
                Console.WriteLine("GPU Upgrade Button clicked");
            }
            if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released && minerUpgrade.IsClicked(_mouseState) == true)
            {
                Console.WriteLine("Miner Upgrade Button clicked");
            }
            if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released && serverUpgrade.IsClicked(_mouseState) == true)
            {
                Console.WriteLine("Server Upgrade Button clicked");
            }
            if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released && superComputerUpgrade.IsClicked(_mouseState) == true)
            {
               
                Console.WriteLine("SuperComputer Upgrade Button clicked");
            }

            _prevMouseState = _mouseState;

            _prevMouseState = _mouseState;

            Bitcoin += bps * gameTime.ElapsedGameTime.TotalSeconds;
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
      
            //Display draws
            _spriteBatch.Draw(_background, new Rectangle (0,0,1024,768), Color.White);
            _spriteBatch.Draw(_test, new Rectangle(50, 150, 300, 40), Color.Wheat);
            _spriteBatch.DrawString(_font, "Score: " + Bitcoin, new Vector2(50, 150), Color.Black);



            _spriteBatch.DrawString(_font, $"Score: {Bitcoin:0.#####}\nUSD: ${Bitcoin * BtcPerGpu:0.#####}", new Vector2(50, 150), Color.Black);

            //Button draws
          
            bitCoinButton.Draw(_spriteBatch);

            //Test Draws
                //_spriteBatch.Draw(test, new Rectangle(954, 315, 60, 30), Color.DarkRed);
                // _spriteBatch.Draw(test, new Rectangle(954, 355, 60, 30), Color.DarkRed);
                // testButton.Draw(_spriteBatch);

            //Buy button draws
            cpuBuy.Draw(_spriteBatch);
            gpuBuy.Draw(_spriteBatch);
            minerBuy.Draw(_spriteBatch);
            serverBuy.Draw(_spriteBatch);
            superComputerBuy.Draw(_spriteBatch);

            //Upgrade button draws
            cpuUpgrade.Draw(_spriteBatch);
            gpuUpgrade.Draw(_spriteBatch);
            minerUpgrade.Draw(_spriteBatch);
            serverUpgrade.Draw(_spriteBatch);
            superComputerUpgrade.Draw(_spriteBatch);






            _spriteBatch.Draw(_test, new Rectangle(954, 315, 60, 30), Color.DarkRed);
            _spriteBatch.Draw(_test, new Rectangle(954, 355, 60, 30), Color.DarkRed);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
