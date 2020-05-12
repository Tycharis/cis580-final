using System;
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

        // Math
        private readonly MathHandler _mathHandler;

        /// <summary>
        /// Constructs a new instance of the game
        /// </summary>
        public BitcoinMiner()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _mathHandler = new MathHandler();
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

            // Window settings
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.ApplyChanges();

            // Math
            _mathHandler.Initialize();

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

            if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released)
            {
                if (_exitButton.IsClicked(_mouseState))
                {
                    Exit();
                }

                if (_bitcoinButton.IsClicked(_mouseState))
                {
                    _mathHandler.Click();
                    Console.WriteLine("Bitcoin Button clicked");
                }

                //Buy buttons

                if (_cpuBuy.IsClicked(_mouseState))
                {
                    _mathHandler.TryBuyBuilding(BuildingType.Cpu);
                    Console.WriteLine("CPU Buy Button clicked");
                }

                if (_gpuBuy.IsClicked(_mouseState))
                {
                    _mathHandler.TryBuyBuilding(BuildingType.Gpu);
                    Console.WriteLine("GPU Buy Button clicked");
                }

                if (_minerBuy.IsClicked(_mouseState))
                {
                    _mathHandler.TryBuyBuilding(BuildingType.Miner);
                    Console.WriteLine("Miner Buy Button clicked");
                }

                if (_serverBuy.IsClicked(_mouseState))
                {
                    _mathHandler.TryBuyBuilding(BuildingType.Server);
                    Console.WriteLine("Server Buy Button clicked");
                }

                if (_supercomputerBuy.IsClicked(_mouseState))
                {
                    _mathHandler.TryBuyBuilding(BuildingType.Supercomputer);
                    Console.WriteLine("SuperComputer Buy Button clicked");
                }

                //Upgrade buttons

                if (_cpuUpgrade.IsClicked(_mouseState))
                {
                    _mathHandler.TryBuyUpgrade(BuildingType.Cpu);
                    Console.WriteLine("CPU Upgrade Button clicked");
                }

                if (_gpuUpgrade.IsClicked(_mouseState))
                {
                    _mathHandler.TryBuyUpgrade(BuildingType.Gpu);
                    Console.WriteLine("GPU Upgrade Button clicked");
                }

                if (_minerUpgrade.IsClicked(_mouseState))
                {
                    _mathHandler.TryBuyUpgrade(BuildingType.Miner);
                    Console.WriteLine("Miner Upgrade Button clicked");
                }

                if (_serverUpgrade.IsClicked(_mouseState))
                {
                    _mathHandler.TryBuyUpgrade(BuildingType.Server);
                    Console.WriteLine("Server Upgrade Button clicked");
                }

                if (_supercomputerUpgrade.IsClicked(_mouseState))
                {
                    _mathHandler.TryBuyUpgrade(BuildingType.Supercomputer);
                    Console.WriteLine("SuperComputer Upgrade Button clicked");
                }
            }

            _prevMouseState = _mouseState;

            // Bitcoin per second updates
            _mathHandler.Update(gameTime);

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

            _spriteBatch.DrawString(_font, $"Bitcoins: {_mathHandler.Bitcoin:0.#####}\nBPS: {_mathHandler.Bps:0.#####}\n\nUSD: ${_mathHandler.Usd:0.##}", new Vector2(60, 145), Color.White);

            // Building cost draws
            _spriteBatch.DrawString(_font, $"Buy Cost: {_mathHandler.CpuCost:0.#####}", new Vector2(783, 280), Color.White);
            _spriteBatch.DrawString(_font, $"Buy Cost: {_mathHandler.GpuCost:0.#####}", new Vector2(783, 380), Color.White);
            _spriteBatch.DrawString(_font, $"Buy Cost: {_mathHandler.ServerCost:0.#####}", new Vector2(783, 480), Color.White);
            _spriteBatch.DrawString(_font, $"Buy Cost: {_mathHandler.MinerCost:0.#####}", new Vector2(783, 580), Color.White);
            _spriteBatch.DrawString(_font, $"Buy Cost: {_mathHandler.SupercomputerCost:0.#####}", new Vector2(778, 680), Color.White);

            // Upgrade cost draws
            _spriteBatch.DrawString(_font, $"Upgrade: {_mathHandler.CpuUpgradeCost:0.#####}", new Vector2(783, 320), Color.White);
            _spriteBatch.DrawString(_font, $"Upgrade: {_mathHandler.GpuUpgradeCost:0.#####}", new Vector2(783, 420), Color.White);
            _spriteBatch.DrawString(_font, $"Upgrade: {_mathHandler.ServerUpgradeCost:0.#####}", new Vector2(783, 520), Color.White);
            _spriteBatch.DrawString(_font, $"Upgrade: {_mathHandler.MinerUpgradeCost:0.#####}", new Vector2(783, 620), Color.White);
            _spriteBatch.DrawString(_font, $"Upgrade: {_mathHandler.SupercomputerUpgradeCost:0.#####}", new Vector2(783, 720), Color.White);

            // Window title draws
            _spriteBatch.DrawString(_font, "Statistics", new Vector2(125, 110), Color.Black);
            _spriteBatch.DrawString(_font, "Shop", new Vector2(775, 210), Color.Black);
            _spriteBatch.DrawString(_font, "Total Buildings", new Vector2(610, 240), Color.White);

            // Building quantity draws
            _spriteBatch.DrawString(_quantities, $"{_mathHandler.NumberOfCpus}", new Vector2(635, 295), Color.White);
            _spriteBatch.DrawString(_quantities, $"{_mathHandler.NumberOfGpus}", new Vector2(635, 395), Color.White);
            _spriteBatch.DrawString(_quantities, $"{_mathHandler.NumberOfServers}", new Vector2(635, 495), Color.White);
            _spriteBatch.DrawString(_quantities, $"{_mathHandler.NumberOfMiners}", new Vector2(635, 595), Color.White);
            _spriteBatch.DrawString(_quantities, $"{_mathHandler.NumberOfSupercomputers}", new Vector2(635, 695), Color.White);

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
