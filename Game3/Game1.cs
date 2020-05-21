using Game3.Components;
using Game3.Interfaces;
using Game3.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game3
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;
        UnitController unitController;
        UiController uiController;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsFixedTimeStep = false;
            this.IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new Camera(this, GraphicsDevice.Viewport, Content, GraphicsDevice);
            camera.Position = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

            unitController = new UnitController(this, Content, camera);
            Services.AddService(typeof(IUnitController), unitController);
            uiController = new UiController(this, Content, camera);
            Services.AddService(typeof(IUiController), uiController);

            unitController.LoadContent();
            uiController.LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            camera.Update(gameTime);
            unitController.Update(gameTime);
            uiController.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            camera.Draw(spriteBatch);
            unitController.Draw(spriteBatch,gameTime,camera);
            uiController.Draw(spriteBatch, gameTime, camera);
            base.Draw(gameTime);
        }
    }
}
