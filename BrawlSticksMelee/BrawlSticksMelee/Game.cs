using BrawlSticksMelee.Screens;
using BrawlSticksMelee.Sprites;
using BrawlSticksMelee.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Net.Security;

namespace BrawlSticksMelee
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private readonly ScreenManager screenManager;

        private SpriteBatch spriteBatch;
        private StickmanSprite stickman;
        private LevelOneEnemySprite enemy;
        private Cube powerUp;
        private QuestionMark question;
        private MainMenu mainMenu;
        private Texture2D background;
        private SoundEffect airPunch;
        private SoundEffect bodyPunch;
        private Song backgroundMusic;
        private SpriteFont orbitron;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
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
            var menuBackground = new BackgroundScreen();
            mainMenu = new MainMenu(menuBackground);
            screenManager.AddScreen(menuBackground, null);
            screenManager.AddScreen(mainMenu, null);
        }

        protected override void Initialize()
        {
            stickman = new StickmanSprite(new Vector2(0, graphics.PreferredBackBufferHeight - 100));
            enemy = new LevelOneEnemySprite(new Vector2(670, graphics.PreferredBackBufferHeight - 100), stickman);
            question = new QuestionMark();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>("gameBackground");
            stickman.LoadContent(Content, stickman.poweredUp);
            enemy.LoadContent(Content);
            powerUp = new Cube(this);
            question.LoadContent(Content);
            airPunch = Content.Load<SoundEffect>("airpunch");
            bodyPunch = Content.Load<SoundEffect>("bodyPunch");
            backgroundMusic = Content.Load<Song>("backgroundMusic");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
            orbitron = Content.Load<SpriteFont>("MenuFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            stickman.Update(gameTime, stickman.poweredUp, Content);
            enemy.Update(gameTime);
            powerUp.Update(gameTime);

            if(stickman.animationFrame == 2)
            {
                airPunch.Play();
                if((stickman.position.X < enemy.position.X + 50 && stickman.position.X > enemy.position.X - 50))
                {
                    bodyPunch.Play();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if(!mainMenu.isActive)
            {
                GraphicsDevice.Clear(Color.Pink);

                spriteBatch.Begin();

                //spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                if (stickman.poweredUp == false)
                {
                    powerUp.Draw();
                    question.Draw(gameTime, spriteBatch);
                }

                if (stickman.health > 0)
                {
                    stickman.Draw(gameTime, spriteBatch);
                }
                else
                {
                    spriteBatch.DrawString(orbitron, $"You Lose\nTo Be Continued...", new Vector2(100, 50), Color.Aqua, 0, new Vector2(0, 0), 4, SpriteEffects.None, 0);
                }
                if (enemy.health > 0)
                {
                    enemy.Draw(gameTime, spriteBatch);
                }
                else
                {
                    spriteBatch.DrawString(orbitron, $"You Win!\nTo Be Continued...", new Vector2(100, 50), Color.Aqua, 0, new Vector2(0, 0), 4, SpriteEffects.None, 0);
                }

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}