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

        private MouseState _mouseState;
        private MouseState _prevMouseState;

        //Textures
        private Texture2D _background;
        private Texture2D _test;
        private Texture2D _bitcoinTexture;
        private Texture2D _buyButton;
        private Texture2D _upgradeButton;
        private Texture2D _textBackground;

        private Texture2D _cpuIcon;
        private Texture2D _gpuIcon;
        private Texture2D _minerIcon;
        private Texture2D _serverIcon;
        private Texture2D _superComputerIcon;



        //Buttons
        private Button _cpuBuy;
        private Button _cpuUpgrade;
        private Button _gpuBuy;
        private Button _gpuUpgrade;
        private Button _serverBuy;
        private Button _serverUpgrade;
        private Button _minerBuy;
        private Button _minerUpgrade;
        private Button _supercomputerBuy;
        private Button _supercomputerUpgrade;
        //private Button _testButton;
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

            //Buttons
              //testButton = new Button(724, 300, 300, 100, test);
            _bitcoinButton = new Button(0, 200, 400, 400, _bitcoinTexture);
            

            //Buy buttons
            _cpuBuy = new Button(954, 275, 60, 30, _buyButton);
            _gpuBuy = new Button(954, 375, 60, 30, _buyButton);
            _minerBuy = new Button(954, 475, 60, 30, _buyButton);
            _serverBuy = new Button(954, 575, 60, 30, _buyButton);
            _supercomputerBuy = new Button(954, 675, 60, 30, _buyButton);

            //Upgrade buttons
            _cpuUpgrade = new Button(954, 315, 60, 30, _upgradeButton);
            _gpuUpgrade = new Button(954, 415, 60, 30, _upgradeButton);
            _minerUpgrade = new Button(954, 515, 60, 30, _upgradeButton);
            _serverUpgrade = new Button(954, 615, 60, 30, _upgradeButton);
            _supercomputerUpgrade = new Button(954, 715, 60, 30, _upgradeButton);

            //_testButton = new Button(724, 300, 300, 100, _test);
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
            _buyButton = Content.Load<Texture2D>("Buy_Button");
            _upgradeButton = Content.Load<Texture2D>("Upgrade_Button");
            _textBackground = Content.Load<Texture2D>("Text_Background");

            _cpuIcon = Content.Load<Texture2D>("CPU_Icon");
            _gpuIcon = Content.Load<Texture2D>("GPU_Icon");
            //_minerIcon = Content.Load<Texture2D>("Miner_Icon");
            _serverIcon = Content.Load<Texture2D>("Server_Icon");
            _superComputerIcon = Content.Load<Texture2D>("SuperComputer_Icon");




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
            double bps = 0.0d;

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

            this.bps = bps;

            /*if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released && testButton.IsClicked(mouseState) == true)
            {
                Console.WriteLine("Button clicked");
            }
            */

            if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released)
            {
                if (_exitButton.IsClicked(_mouseState))
                {
                    Exit();
                }

                //Buy buttons

                if (_bitcoinButton.IsClicked(_mouseState))
                {
                    Bitcoin += BtcPerClick;
                    Console.WriteLine("Bitcoin Button clicked");
                }

                if (_cpuBuy.IsClicked(_mouseState) && Bitcoin >= 0.00165   )
                {
                    buildings.Add(new Building { Type = BuildingType.Cpu });
                    Console.WriteLine("CPU Buy Button clicked");
                }

                if (_gpuBuy.IsClicked(_mouseState) && Bitcoin >= 0.011 )
                {
                    buildings.Add(new Building { Type = BuildingType.Gpu });
                    Console.WriteLine("GPU Buy Button clicked");
                }

                if (_minerBuy.IsClicked(_mouseState) && Bitcoin >= 0.121 )
                {
                    buildings.Add(new Building { Type = BuildingType.Miner });
                    Console.WriteLine("Miner Buy Button clicked");
                }

                if (_serverBuy.IsClicked(_mouseState) && Bitcoin >= 1.32 )
                {
                    buildings.Add(new Building { Type = BuildingType.Server });
                    Console.WriteLine("Server Buy Button clicked");
                }

                if (_supercomputerBuy.IsClicked(_mouseState) && Bitcoin >= 14.3)
                {
                    buildings.Add(new Building { Type = BuildingType.Supercomputer });
                    Console.WriteLine("SuperComputer Buy Button clicked");
                }

                //Upgrade buttons

                if (_cpuUpgrade.IsClicked(_mouseState))
                {
                    Console.WriteLine("CPU Upgrade Button clicked");
                }

                if (_gpuUpgrade.IsClicked(_mouseState))
                {
                    Console.WriteLine("GPU Upgrade Button clicked");
                }

                if (_minerUpgrade.IsClicked(_mouseState))
                {
                    Console.WriteLine("Miner Upgrade Button clicked");
                }

                if (_serverUpgrade.IsClicked(_mouseState))
                {
                    Console.WriteLine("Server Upgrade Button clicked");
                }

                if (_supercomputerUpgrade.IsClicked(_mouseState))
                {
                    Console.WriteLine("SuperComputer Upgrade Button clicked");
                }
            }

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
            _spriteBatch.Draw(_textBackground, new Rectangle(50, 150, 300, 40), Color.Wheat);



            //Test Draws
                _spriteBatch.Draw(_test, new Rectangle(650, 200, 374, 568), Color.DarkRed);
                // _spriteBatch.Draw(test, new Rectangle(954, 355, 60, 30), Color.DarkRed);
                // testButton.Draw(_spriteBatch);
                //_spriteBatch.Draw(_test, new Rectangle(954, 315, 60, 30), Color.DarkRed);
                //_spriteBatch.Draw(_test, new Rectangle(954, 355, 60, 30), Color.DarkRed);

            //Icon Draws
            _spriteBatch.Draw(_cpuIcon, new Rectangle(675, 260, 100, 100), Color.Wheat);
            _spriteBatch.Draw(_gpuIcon, new Rectangle(675, 360, 100, 100), Color.Wheat);
            //_spriteBatch.Draw(_minerIcon, new Rectangle(675, 460, 100, 100), Color.Wheat);
            _spriteBatch.Draw(_serverIcon, new Rectangle(675, 560, 100, 100), Color.Wheat);
            _spriteBatch.Draw(_superComputerIcon, new Rectangle(675, 660, 100, 100), Color.Wheat);


            //_spriteBatch.DrawString(_font, "Score: " + Bitcoin, new Vector2(50, 150), Color.Black);

            _spriteBatch.DrawString(_font, $"Bitcoins: {Bitcoin:0.#####}\nUSD: ${Bitcoin * BtcPerGpu:0.#####}", new Vector2(60, 153), Color.Black);

            //Button draws
          
            _bitcoinButton.Draw(_spriteBatch);

           

            //Buy button draws
            _cpuBuy.Draw(_spriteBatch);
            _gpuBuy.Draw(_spriteBatch);
            _minerBuy.Draw(_spriteBatch);
            _serverBuy.Draw(_spriteBatch);
            _supercomputerBuy.Draw(_spriteBatch);

            //Upgrade button draws
            _cpuUpgrade.Draw(_spriteBatch);
            _gpuUpgrade.Draw(_spriteBatch);
            _minerUpgrade.Draw(_spriteBatch);
            _serverUpgrade.Draw(_spriteBatch);
            _supercomputerUpgrade.Draw(_spriteBatch);

          



            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
