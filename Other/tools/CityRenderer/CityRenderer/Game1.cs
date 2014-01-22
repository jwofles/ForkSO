using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CityRenderer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        //Which city are we loading?
        public const int CITY_NUMBER = 3;

        private Terrain m_Terrain;
        private Effect m_VertexShader, m_PixelShader;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
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
            this.IsMouseVisible = true;
            GraphicsDevice.VertexDeclaration = new VertexDeclaration(GraphicsDevice, MeshVertex.VertexElements);
            GraphicsDevice.RenderState.CullMode = CullMode.None;

            GraphicsDevice.DeviceResetting += new EventHandler(GraphicsDevice_DeviceResetting);

            base.Initialize();
        }

        private void GraphicsDevice_DeviceResetting(object sender, EventArgs e)
        {
            GraphicsDevice.RenderState.CullMode = CullMode.None;
            SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);
            m_Terrain.ClearOldData();
            m_Terrain.GenerateCityMesh(GraphicsDevice);
            m_Terrain.CreateTextureAtlas(spriteBatch);
            m_Terrain.CreateTransparencyAtlas(spriteBatch);
            m_Terrain.RoadAtlas = m_Terrain.CreateRoadAtlas(m_Terrain.m_Roads, spriteBatch);
            m_Terrain.RoadCAtlas = m_Terrain.CreateRoadAtlas(m_Terrain.m_RoadCorners, spriteBatch);
            spriteBatch.Dispose();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            m_VertexShader = Content.Load<Effect>("VerShader");
            m_PixelShader = Content.Load<Effect>("PixShader");

            CityDataRetriever cityData = new CityDataRetriever();
            m_Terrain = new Terrain(GraphicsDevice, CITY_NUMBER, cityData);
            m_Terrain.Shader2D = Content.Load<Effect>("colorpoly2d");
            m_Terrain.Initialize();
            m_Terrain.GenerateCityMesh(GraphicsDevice);
            m_Terrain.CreateTextureAtlas(spriteBatch);
            m_Terrain.CreateTransparencyAtlas(spriteBatch);
            m_Terrain.RoadAtlas = m_Terrain.CreateRoadAtlas(m_Terrain.m_Roads, spriteBatch);
            m_Terrain.RoadCAtlas = m_Terrain.CreateRoadAtlas(m_Terrain.m_RoadCorners, spriteBatch);


            //Shadow configuration. Very Low quality res: 512, Low quality: 1024, high quality: 2048
            m_Terrain.ShadowsEnabled = true;
            m_Terrain.ShadowRes = 2048;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            m_Terrain.UnloadEverything();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            m_Terrain.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.RenderState.DepthBufferEnable = true;
            GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
            GraphicsDevice.RenderState.AlphaBlendEnable = true;

            // TODO: Add your drawing code here


            /*spriteBatch.Draw(m_Terrain.TransAtlas, new Rectangle(0, 0, m_Terrain.TransAtlas.Width, 
                m_Terrain.TransAtlas.Height), Color.White);*/
            m_Terrain.Draw(m_VertexShader, m_PixelShader);

            base.Draw(gameTime);
        }
    }
}
