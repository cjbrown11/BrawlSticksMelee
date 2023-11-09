using BrawlSticksMelee.Screens;
using BrawlSticksMelee.Sprites;
using BrawlSticksMelee.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BrawlSticksMelee
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private readonly ScreenManager screenManager;

        private SpriteBatch spriteBatch;
        private StickmanSprite stickman;
        private LevelOneEnemySprite enemy;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenu(), null);
        }

        protected override void Initialize()
        {
            stickman = new StickmanSprite(new Vector2(0, graphics.PreferredBackBufferHeight - 100), enemy = new LevelOneEnemySprite(new Vector2(670, graphics.PreferredBackBufferHeight - 100), stickman));
            enemy = new LevelOneEnemySprite(new Vector2(670, graphics.PreferredBackBufferHeight - 100), stickman);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            stickman.LoadContent(Content);
            enemy.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            stickman.Update(gameTime);
            enemy.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            stickman.Draw(gameTime, spriteBatch);
            if(enemy.health != 0)
            {
                enemy.Draw(gameTime, spriteBatch);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}