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

        // Textures
        private Texture2D _background;
        private Texture2D _bitcoinTexture;
        private Texture2D _buyButton;
        private Texture2D _upgradeButton;

        private Texture2D _cpuIcon;
        private Texture2D _gpuIcon;
        private Texture2D _minerIcon;
        private Texture2D _serverIcon;
        private Texture2D _supercomputerIcon;

        private Texture2D _upgradeWindow;
        private Texture2D _textWindow;

        // Buttons
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
        private Button _bitcoinButton;
        private Button _exitButton;

        // Text
        private SpriteFont _font;
        private SpriteFont _quantities;

        // Math properties for buildings
        private static double BtcPerCpu => 0.1 * BtcPerGpu;
        private static double BtcPerGpu { get; set; }
        private static double BtcPerServer => 8 * BtcPerGpu;
        private static double BtcPerMiner => 47 * BtcPerGpu;
        private static double BtcPerSupercomputer => 260 * BtcPerGpu;

        // Numerical stuff
        private double _bps;

        // Building costs
        private static double CpuCost => 15 * BtcPerGpu;
        private static double GpuCost => 100 * BtcPerGpu;
        private static double ServerCost => 1100 * BtcPerGpu;
        private static double MinerCost => 12000 * BtcPerGpu;
        private static double SupercomputerCost => 130000 * BtcPerGpu;

        // Upgrade counters
        private int _cpuUpgrades;
        private int _gpuUpgrades;
        private int _serverUpgrades;
        private int _minerUpgrades;
        private int _supercomputerUpgrades;

        // Number of buildings
        private int _numberOfCpus;
        private int _numberOfGpus;
        private int _numberOfMiners;
        private int _numberOfServers;
        private int _numberOfSupercomputers;

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

            // Add your initialization logic here
            IsMouseVisible = true;
            Window.IsBorderless = true;

            _bps = 0.0d;
            // Window settings
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.ApplyChanges();

            // Spritefonts
            _font = Content.Load<SpriteFont>("text");
            _quantities = Content.Load<SpriteFont>("numberSize");

            // Buttons
            _bitcoinButton = new Button(0, 200, 400, 400, _bitcoinTexture);
            
            // Buy buttons
            _cpuBuy = new Button(954, 275, 60, 30, _buyButton);
            _gpuBuy = new Button(954, 375, 60, 30, _buyButton);
            _serverBuy = new Button(954, 475, 60, 30, _buyButton);
            _minerBuy = new Button(954, 575, 60, 30, _buyButton);
            _supercomputerBuy = new Button(954, 675, 60, 30, _buyButton);

            // Upgrade buttons
            _cpuUpgrade = new Button(954, 315, 60, 30, _upgradeButton);
            _gpuUpgrade = new Button(954, 415, 60, 30, _upgradeButton);
            _serverUpgrade = new Button(954, 515, 60, 30, _upgradeButton);
            _minerUpgrade = new Button(954, 615, 60, 30, _upgradeButton);
            _supercomputerUpgrade = new Button(954, 715, 60, 30, _upgradeButton);

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

            // Use this.Content to load your game content here

            // Initialize textures
            _background = Content.Load<Texture2D>("GameBG");
            _bitcoinTexture = Content.Load<Texture2D>("Bitcoin_Coin");
            _buyButton = Content.Load<Texture2D>("Buy_Button");
            _upgradeButton = Content.Load<Texture2D>("Upgrade_Button");
            _upgradeWindow = Content.Load<Texture2D>("Upgrade_Window");
            _textWindow = Content.Load<Texture2D>("Text_Window");

            _cpuIcon = Content.Load<Texture2D>("CPU_Icon");
            _gpuIcon = Content.Load<Texture2D>("GPU_Icon");
            _serverIcon = Content.Load<Texture2D>("Server_Icon");
            _minerIcon = Content.Load<Texture2D>("Miner_Icon");
            _supercomputerIcon = Content.Load<Texture2D>("SuperComputer_Icon");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unload any non ContentManager content here
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

            // Bitcoin per second updates
            double bps = 0.0d;

            bps += _numberOfCpus * BtcPerCpu * (2 * _cpuUpgrades == 0 ? 1 : 2 * _cpuUpgrades);
            bps += _numberOfGpus * BtcPerGpu * (2 * _gpuUpgrades == 0 ? 1 : 2 * _gpuUpgrades);
            bps += _numberOfServers * BtcPerServer * (2 * _serverUpgrades == 0 ? 1 : 2 *_serverUpgrades);
            bps += _numberOfMiners * BtcPerMiner * (2 * _minerUpgrades == 0 ? 1 : 2 * _serverUpgrades);
            bps += _numberOfSupercomputers * BtcPerSupercomputer * (2 * _supercomputerUpgrades == 0 ? 1 : 2 * _supercomputerUpgrades);

            _bps = bps;

            if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released)
            {
                if (_exitButton.IsClicked(_mouseState))
                {
                    Exit();
                }

                if (_bitcoinButton.IsClicked(_mouseState))
                {
                    Bitcoin += BtcPerGpu;
                    Console.WriteLine("Bitcoin Button clicked");
                }

                //Buy buttons

                if (_cpuBuy.IsClicked(_mouseState) && Bitcoin >= CpuCost *  Math.Pow(1.15,_numberOfCpus))
                {
                    _numberOfCpus++;
                    Bitcoin -= CpuCost * Math.Pow(1.15, _numberOfCpus);
                    Console.WriteLine("CPU Buy Button clicked");
                }

                if (_gpuBuy.IsClicked(_mouseState) && Bitcoin >= GpuCost * Math.Pow(1.15, _numberOfGpus))
                {
                    _numberOfGpus++;
                    Bitcoin -= (GpuCost * Math.Pow(1.15, _numberOfGpus));
                    Console.WriteLine("GPU Buy Button clicked");
                }

                if (_minerBuy.IsClicked(_mouseState) && Bitcoin >= MinerCost * Math.Pow(1.15, _numberOfMiners))
                {
                    _numberOfMiners++;
                    Bitcoin -= MinerCost * Math.Pow(1.15, _numberOfMiners);
                    Console.WriteLine("Miner Buy Button clicked");
                }

                if (_serverBuy.IsClicked(_mouseState) && Bitcoin >= ServerCost * Math.Pow(1.15, _numberOfServers))
                {
                    _numberOfServers++;
                    Bitcoin -= ServerCost * Math.Pow(1.15, _numberOfServers);
                    Console.WriteLine("Server Buy Button clicked");
                }

                if (_supercomputerBuy.IsClicked(_mouseState) && Bitcoin >= SupercomputerCost * Math.Pow(1.15, _numberOfServers))
                {
                    _numberOfSupercomputers++;
                    Bitcoin -= SupercomputerCost * Math.Pow(1.15, _numberOfSupercomputers);
                    Console.WriteLine("SuperComputer Buy Button clicked");
                }

                //Upgrade buttons

                if (_cpuUpgrade.IsClicked(_mouseState) && Bitcoin >= CpuCost * Math.Pow(5, _cpuUpgrades))
                {
                    _cpuUpgrades++;
                    Console.WriteLine("CPU Upgrade Button clicked");
                }

                if (_gpuUpgrade.IsClicked(_mouseState) && Bitcoin >= GpuCost * Math.Pow(5, _gpuUpgrades))
                {
                    _gpuUpgrades++;
                    Console.WriteLine("GPU Upgrade Button clicked");
                }

                if (_minerUpgrade.IsClicked(_mouseState) && Bitcoin >= MinerCost * Math.Pow(5, _minerUpgrades))
                {
                    _minerUpgrades++;
                    Console.WriteLine("Miner Upgrade Button clicked");
                }

                if (_serverUpgrade.IsClicked(_mouseState) && Bitcoin >= ServerCost * Math.Pow(5, _serverUpgrades))
                {
                    _serverUpgrades++;
                    Console.WriteLine("Server Upgrade Button clicked");
                }

                if (_supercomputerUpgrade.IsClicked(_mouseState) && Bitcoin >= SupercomputerCost * Math.Pow(5, _supercomputerUpgrades))
                {
                    _supercomputerUpgrades++;
                    Console.WriteLine("SuperComputer Upgrade Button clicked");
                }
            }

            _prevMouseState = _mouseState;

            Bitcoin += bps * gameTime.ElapsedGameTime.TotalSeconds;

            if (Bitcoin < 0.0d)
            {
                Bitcoin = 0.0d;
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            // Add your drawing code here
            _spriteBatch.Begin();

            // Display draws
            _spriteBatch.Draw(_background, new Rectangle (0,0,1024,768), Color.White);
            _spriteBatch.Draw(_textWindow, new Rectangle(50, 100, 300, 120), Color.White);
            _spriteBatch.Draw(_upgradeWindow, new Rectangle(600, 200, 424, 568), Color.White);

            // Icon Draws
            _spriteBatch.Draw(_cpuIcon, new Rectangle(675, 260, 100, 100), Color.Wheat);
            _spriteBatch.Draw(_gpuIcon, new Rectangle(675, 360, 100, 100), Color.Wheat);
            _spriteBatch.Draw(_serverIcon, new Rectangle(675, 460, 100, 100), Color.Wheat);
            _spriteBatch.Draw(_minerIcon, new Rectangle(675, 560, 100, 100), Color.Wheat);
            _spriteBatch.Draw(_supercomputerIcon, new Rectangle(675, 660, 100, 100), Color.Wheat);

            _spriteBatch.DrawString(_font, $"Bitcoins: {Bitcoin:0.#####}\nBPS: {_bps:0.#####}\n\nUSD: ${Bitcoin / BtcPerGpu:0.#####}", new Vector2(60, 145), Color.White);

            // Building cost draws
            _spriteBatch.DrawString(_font, $"Buy Cost: {CpuCost * Math.Pow(1.15, _numberOfCpus):0.#####}", new Vector2(783, 280), Color.White);
            _spriteBatch.DrawString(_font, $"Buy Cost: {GpuCost * Math.Pow(1.15, _numberOfGpus):0.#####}", new Vector2(783, 380), Color.White);
            _spriteBatch.DrawString(_font, $"Buy Cost: {ServerCost * Math.Pow(1.15, _numberOfServers):0.#####}", new Vector2(783, 480), Color.White);
            _spriteBatch.DrawString(_font, $"Buy Cost: {MinerCost * Math.Pow(1.15, _numberOfMiners):0.#####}", new Vector2(783, 580), Color.White);
            _spriteBatch.DrawString(_font, $"Buy Cost: {SupercomputerCost * Math.Pow(1.15, _numberOfSupercomputers):0.#####}", new Vector2(778, 680), Color.White);

            // Upgrade cost draws
            _spriteBatch.DrawString(_font, $"Upgrade: {CpuCost * Math.Pow(10, _cpuUpgrades):0.#####}", new Vector2(783, 320), Color.White);
            _spriteBatch.DrawString(_font, $"Upgrade: {GpuCost * Math.Pow(10, _gpuUpgrades):0.#####}", new Vector2(783, 420), Color.White);
            _spriteBatch.DrawString(_font, $"Upgrade: {ServerCost * Math.Pow(10, _serverUpgrades):0.#####}", new Vector2(783, 520), Color.White);
            _spriteBatch.DrawString(_font, $"Upgrade: {MinerCost * Math.Pow(10, _minerUpgrades):0.#####}", new Vector2(783, 620), Color.White);
            _spriteBatch.DrawString(_font, $"Upgrade: {SupercomputerCost * Math.Pow(10, _supercomputerUpgrades):0.#####}", new Vector2(783, 720), Color.White);

            // Window title draws
            _spriteBatch.DrawString(_font, "Statistics", new Vector2(125, 110), Color.Black);
            _spriteBatch.DrawString(_font, "Shop", new Vector2(775, 210), Color.Black);
            _spriteBatch.DrawString(_font, "Total Buildings", new Vector2(610, 240), Color.White);

            // Building quantity draws
            _spriteBatch.DrawString(_quantities, $"{_numberOfCpus}", new Vector2(635, 295), Color.White);
            _spriteBatch.DrawString(_quantities, $"{_numberOfGpus}", new Vector2(635, 395), Color.White);
            _spriteBatch.DrawString(_quantities, $"{_numberOfServers}", new Vector2(635, 495), Color.White);
            _spriteBatch.DrawString(_quantities, $"{_numberOfMiners}", new Vector2(635, 595), Color.White);
            _spriteBatch.DrawString(_quantities, $"{_numberOfSupercomputers}", new Vector2(635, 695), Color.White);

            // Button draws
            _bitcoinButton.Draw(_spriteBatch);

            // Buy button draws
            _cpuBuy.Draw(_spriteBatch);
            _gpuBuy.Draw(_spriteBatch);
            _serverBuy.Draw(_spriteBatch);
            _minerBuy.Draw(_spriteBatch);
            _supercomputerBuy.Draw(_spriteBatch);

            // Upgrade button draws
            _cpuUpgrade.Draw(_spriteBatch);
            _gpuUpgrade.Draw(_spriteBatch);
            _serverUpgrade.Draw(_spriteBatch);
            _minerUpgrade.Draw(_spriteBatch);
            _supercomputerUpgrade.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
