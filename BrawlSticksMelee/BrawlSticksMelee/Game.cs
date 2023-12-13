using BrawlSticksMelee.Screens;
using BrawlSticksMelee.Sprites;
using BrawlSticksMelee.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace BrawlSticksMelee
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private readonly ScreenManager screenManager;

        private SpriteBatch spriteBatch;
        private StickmanSprite playerLevelOne;
        private StickmanSprite playerLevelTwo;
        private StickmanSprite playerOneOne;
        private StickmanSprite playerOneTwo;
        private StickmanSprite playerOneThree;
        private PlayerTwo playerTwoOne;
        private PlayerTwo playerTwoTwo;
        private PlayerTwo playerTwoThree;
        private LevelOneEnemySprite boss;
        private StickmanSprite playerLevelThree;
        private LevelOneEnemySprite levelOneEnemy;
        private LevelOneEnemySprite[] levelTwoEnemies;
        private LevelOneEnemySprite[] levelThreeEnemies;
        private Cube powerUp;
        private QuestionMark question;
        private MainMenu mainMenu;
        private Texture2D background;
        private SoundEffect airPunch;
        private SoundEffect bodyPunch;
        private Song backgroundMusic;
        private SpriteFont orbitron;
        private int gameLevel;
        private int dead;
        private int transitionTimer;
        private bool restart;
        private string winner;
        private int playerOneWins = 0;
        private int playerTwoWins = 0;
        private bool newGame;

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
            restart = false;
            playerLevelOne = new StickmanSprite(new Vector2(0, graphics.PreferredBackBufferHeight - 100));
            levelOneEnemy = new LevelOneEnemySprite(new Vector2(670, graphics.PreferredBackBufferHeight - 100), playerLevelOne, new(550, 50), 1000);
            question = new QuestionMark();
            gameLevel = 1;
            playerLevelTwo = new StickmanSprite(new Vector2(0, graphics.PreferredBackBufferHeight - 100));
            playerLevelThree = new StickmanSprite(new Vector2(0, graphics.PreferredBackBufferHeight - 100));
            levelTwoEnemies = new LevelOneEnemySprite[]
            {
                new LevelOneEnemySprite(new Vector2(670, graphics.PreferredBackBufferHeight - 100), playerLevelTwo, new(550, 50), 1000),
                new LevelOneEnemySprite(new Vector2(630, graphics.PreferredBackBufferHeight - 100), playerLevelTwo, new(550, 70), 1000),
                new LevelOneEnemySprite(new Vector2(590, graphics.PreferredBackBufferHeight - 100), playerLevelTwo, new(550, 90), 1000)
            };
            levelThreeEnemies = new LevelOneEnemySprite[]
            {
                new LevelOneEnemySprite(new Vector2(670, graphics.PreferredBackBufferHeight - 100), playerLevelThree, new(550, 50), 1000),
                new LevelOneEnemySprite(new Vector2(630, graphics.PreferredBackBufferHeight - 100), playerLevelThree, new(550, 70), 1000),
                new LevelOneEnemySprite(new Vector2(590, graphics.PreferredBackBufferHeight - 100), playerLevelThree, new(550, 90), 1000)
            };
            boss = new LevelOneEnemySprite(new Vector2(670, graphics.PreferredBackBufferHeight - 100), playerLevelThree, new(550, 30), 2000);
            boss.scale = 3;
            boss.position.Y -= 35;
            playerOneOne = new StickmanSprite(new Vector2(0, graphics.PreferredBackBufferHeight - 100));
            playerOneTwo = new StickmanSprite(new Vector2(0, graphics.PreferredBackBufferHeight - 100));
            playerOneThree = new StickmanSprite(new Vector2(0, graphics.PreferredBackBufferHeight - 100));
            playerTwoOne = new PlayerTwo(new Vector2(670, graphics.PreferredBackBufferHeight - 100), playerOneOne);
            playerTwoTwo = new PlayerTwo(new Vector2(670, graphics.PreferredBackBufferHeight - 100), playerOneTwo);
            playerTwoThree = new PlayerTwo(new Vector2(670, graphics.PreferredBackBufferHeight - 100), playerOneThree);
            playerOneOne.poweredUp = true;
            playerOneTwo.poweredUp = true;
            playerOneThree.poweredUp = true;
            playerTwoOne.poweredUp = true;
            playerTwoTwo.poweredUp = true;
            playerTwoThree.poweredUp = true;
            newGame = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>("gameBackground");
            playerLevelOne.LoadContent(Content, playerLevelOne.poweredUp);
            levelOneEnemy.LoadContent(Content);
            playerLevelTwo.LoadContent(Content, playerLevelTwo.poweredUp);
            playerLevelThree.LoadContent(Content, playerLevelThree.poweredUp);
            playerOneOne.LoadContent(Content, playerOneOne.poweredUp);
            playerOneTwo.LoadContent(Content, playerOneTwo.poweredUp);
            playerOneThree.LoadContent(Content, playerOneThree.poweredUp);
            playerTwoOne.LoadContent(Content, playerTwoOne.poweredUp);
            playerTwoTwo.LoadContent(Content, playerTwoTwo.poweredUp);
            playerTwoThree.LoadContent(Content, playerTwoThree.poweredUp);
            foreach (var enemy in levelTwoEnemies) enemy.LoadContent(Content);
            foreach (var enemy in levelThreeEnemies) enemy.LoadContent(Content);
            boss.LoadContent(Content);
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

            if(!mainMenu.isActive && gameLevel == 1)
            {
                playerLevelOne.Update(gameTime, playerLevelOne.poweredUp, Content);
                levelOneEnemy.Update(gameTime);
                powerUp.Update(gameTime);

                if (playerLevelOne.animationFrame == 2)
                {
                    
                    if ((playerLevelOne.position.X < levelOneEnemy.position.X && playerLevelOne.position.X + 150  > levelOneEnemy.position.X))
                    {
                        bodyPunch.Play();
                    }
                    else airPunch.Play();
                }
            }

            if (!mainMenu.isActive && gameLevel == 2)
            {
                playerLevelTwo.Update(gameTime, playerLevelTwo.poweredUp, Content);
                foreach (var enemy in levelTwoEnemies) enemy.Update(gameTime);
                powerUp.Update(gameTime);
                
                if (playerLevelTwo.animationFrame == 2)
                {
                    foreach(var enemy in levelTwoEnemies)
                    {
                        if ((playerLevelTwo.position.X < enemy.position.X && playerLevelTwo.position.X + 150 > enemy.position.X))
                        {
                            bodyPunch.Play();
                        }
                        else airPunch.Play();
                    }
                    
                }
            }

            if (!mainMenu.isActive && gameLevel == 3)
            {
                playerLevelThree.Update(gameTime, playerLevelThree.poweredUp, Content);
                foreach (var enemy in levelThreeEnemies) enemy.Update(gameTime);
                boss.Update(gameTime);
                powerUp.Update(gameTime);

                if (playerLevelThree.animationFrame == 2)
                {
                    foreach (var enemy in levelThreeEnemies)
                    {
                        if ((playerLevelThree.position.X < enemy.position.X && playerLevelThree.position.X + 150 > enemy.position.X))
                        {
                            bodyPunch.Play();
                        }
                        else airPunch.Play();
                    }
                    if ((playerLevelThree.position.X < boss.position.X && playerLevelThree.position.X + 150 > boss.position.X))
                    {
                        bodyPunch.Play();
                    }
                    else airPunch.Play();
                }
            }

            if (!mainMenu.isActive && gameLevel == -3)
            {
                playerOneOne.Update(gameTime, playerOneOne.poweredUp, Content);
                playerTwoOne.Update(gameTime, playerTwoOne.poweredUp, Content);

                if (playerOneOne.animationFrame == 2)
                {
                    
                    if ((playerOneOne.position.X < playerTwoOne.position.X + 50 && playerOneOne.position.X > playerTwoOne.position.X - 50))
                    {
                        bodyPunch.Play();
                    }
                    else airPunch.Play();                 
                }
                if (playerTwoOne.animationFrame == 2)
                {

                    if ((playerTwoOne.position.X < playerOneOne.position.X + 50 && playerTwoOne.position.X > playerOneOne.position.X - 50))
                    {
                        bodyPunch.Play();
                    }
                    else airPunch.Play();
                }
            }

            if (!mainMenu.isActive && gameLevel == -2)
            {
                playerOneTwo.Update(gameTime, playerOneTwo.poweredUp, Content);
                playerTwoTwo.Update(gameTime, playerTwoTwo.poweredUp, Content);

                if (playerOneTwo.animationFrame == 2)
                {

                    if ((playerOneTwo.position.X < playerTwoTwo.position.X + 50 && playerOneTwo.position.X > playerTwoTwo.position.X - 50))
                    {
                        bodyPunch.Play();
                    }
                    else airPunch.Play();
                }
                if (playerTwoTwo.animationFrame == 2)
                {

                    if ((playerTwoTwo.position.X < playerOneTwo.position.X + 50 && playerTwoTwo.position.X > playerOneTwo.position.X - 50))
                    {
                        bodyPunch.Play();
                    }
                    else airPunch.Play();
                }
            }

            if (!mainMenu.isActive && gameLevel == -1)
            {
                playerOneThree.Update(gameTime, playerOneThree.poweredUp, Content);
                playerTwoThree.Update(gameTime, playerTwoThree.poweredUp, Content);

                if (playerOneThree.animationFrame == 2)
                {

                    if ((playerOneThree.position.X < playerTwoThree.position.X + 50 && playerOneThree.position.X > playerTwoThree.position.X - 50))
                    {
                        bodyPunch.Play();
                    }
                    else airPunch.Play();
                }
                if (playerTwoThree.animationFrame == 2)
                {

                    if ((playerTwoThree.position.X < playerOneThree.position.X + 50 && playerTwoThree.position.X > playerOneThree.position.X - 50))
                    {
                        bodyPunch.Play();
                    }
                    else airPunch.Play();
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            gameLevel = mainMenu.selection;
            if(!mainMenu.isActive && gameLevel == 10)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                spriteBatch.DrawString(orbitron, $"How to Play", new Vector2(250, 50), Color.Aqua, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
                spriteBatch.DrawString(orbitron, $"Movement - Arrow Keys\nPunch - W\n Block(Must have power up) - Q\n\nPress Space to Continue", new Vector2(275, 200), Color.Aqua, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    mainMenu.selection = 1;
                    restart = true;
                }

                    spriteBatch.End();

                if (restart) Initialize();
            }
            if (!mainMenu.isActive && gameLevel == 1)
            {

                spriteBatch.Begin();

                spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                if (playerLevelOne.poweredUp == false)
                {
                    powerUp.Draw();
                    question.Draw(gameTime, spriteBatch);
                }
                if (levelOneEnemy.health > 0)
                {
                    levelOneEnemy.Draw(gameTime, spriteBatch);
                }
                else
                {
                    spriteBatch.DrawString(orbitron, $"Level 2", new Vector2(250, 50), Color.Aqua, 0, new Vector2(0, 0), 4, SpriteEffects.None, 0);
                    spriteBatch.DrawString(orbitron, $"Press Enter to Continue", new Vector2(275, 200), Color.Aqua, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter)) mainMenu.selection++;
                }
                if (playerLevelOne.health > 0)
                {
                    playerLevelOne.Draw(gameTime, spriteBatch);
                }
                else
                {
                    spriteBatch.DrawString(orbitron, $"You Lose", new Vector2(225, 50), Color.Aqua, 0, new Vector2(0, 0), 4, SpriteEffects.None, 0);
                    spriteBatch.DrawString(orbitron, $"Press Enter to Try Again or Backspace to go to the Main Menu", new Vector2(125, 200), Color.Aqua, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        mainMenu.selection = 1;
                        restart = true;
                    };
                    if (Keyboard.GetState().IsKeyDown(Keys.Back))
                    {
                        mainMenu.selection = 0;
                        AddInitialScreens();
                    }
                }

                spriteBatch.End();

                if (restart) Initialize();
            }
            if(gameLevel == 2)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                if (playerLevelTwo.poweredUp == false)
                {
                    powerUp.Draw();
                    question.Draw(gameTime, spriteBatch);
                }
                dead = 0;
                foreach (var enemy in levelTwoEnemies)
                {
                    if (enemy.health > 0)
                    {
                        enemy.Draw(gameTime, spriteBatch);
                    }
                    else dead++;
                }
                if (dead == 3)
                {
                    spriteBatch.DrawString(orbitron, $"Level 3", new Vector2(250, 50), Color.Aqua, 0, new Vector2(0, 0), 4, SpriteEffects.None, 0);
                    spriteBatch.DrawString(orbitron, $"Press Enter to Continue", new Vector2(275, 200), Color.Aqua, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter)) mainMenu.selection++;
                }
                if (playerLevelTwo.health > 0)
                {
                    playerLevelTwo.Draw(gameTime, spriteBatch);
                }
                else
                {
                    spriteBatch.DrawString(orbitron, $"You Lose", new Vector2(250, 50), Color.Aqua, 0, new Vector2(0, 0), 4, SpriteEffects.None, 0);
                    spriteBatch.DrawString(orbitron, $"Press Enter to Try Again or Backspace to go to the Main Menu", new Vector2(125, 200), Color.Aqua, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        mainMenu.selection = 1;
                        restart = true;
                    };
                    if (Keyboard.GetState().IsKeyDown(Keys.Back))
                    {
                        mainMenu.selection = 0;
                        AddInitialScreens();
                    }
                }

                spriteBatch.End();

                if (restart) Initialize();
            }
            if (gameLevel == 3)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                if (playerLevelThree.poweredUp == false)
                {
                    powerUp.Draw();
                    question.Draw(gameTime, spriteBatch);
                }
                dead = 0;
                foreach (var enemy in levelThreeEnemies)
                {
                    if (enemy.health > 0)
                    {
                        enemy.Draw(gameTime, spriteBatch);
                    }
                    else dead++;
                }
                if (boss.health > 0)
                {
                    boss.Draw(gameTime, spriteBatch);
                }
                else dead++;
                if (dead == 4)
                {
                    spriteBatch.DrawString(orbitron, $"You Win", new Vector2(250, 50), Color.Aqua, 0, new Vector2(0, 0), 4, SpriteEffects.None, 0);
                    spriteBatch.DrawString(orbitron, $"Press Enter to Play Again or Backspace to go to the Main Menu", new Vector2(125, 200), Color.Aqua, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        mainMenu.selection = 1;
                        restart = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Back))
                    {
                        mainMenu.selection = 0;
                        AddInitialScreens();
                    }
                }
                if (playerLevelThree.health > 0)
                {
                    playerLevelThree.Draw(gameTime, spriteBatch);
                }
                else
                {
                    spriteBatch.DrawString(orbitron, $"You Lose", new Vector2(250, 50), Color.Aqua, 0, new Vector2(0, 0), 4, SpriteEffects.None, 0);
                    spriteBatch.DrawString(orbitron, $"Press Enter to Try Again or Backspace to go to the Main Menu", new Vector2(125, 200), Color.Aqua, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        mainMenu.selection = 1;
                        restart = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Back))
                    {
                        mainMenu.selection = 0;
                        AddInitialScreens();
                    }
                }

                spriteBatch.End();

                if (restart) Initialize();
            }

            if (!mainMenu.isActive && gameLevel == -4)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                spriteBatch.DrawString(orbitron, $"How to Play", new Vector2(250, 50), Color.Aqua, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
                spriteBatch.DrawString(orbitron, $"Player One:\nMovement - A & S\nPunch - W\n Block - Q\n\nPlayer Two:\nMovement - K & L\nPunch - O\n Block - I\n\nPress Space to Continue", new Vector2(275, 135), Color.Aqua, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    mainMenu.selection++;
                    restart = true;
                }

                spriteBatch.End();

                if (restart) Initialize();
            }
            if (!mainMenu.isActive && gameLevel == -3)
            {

                spriteBatch.Begin();

                spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                if (playerTwoOne.health > 0)
                {
                    playerTwoOne.Draw(gameTime, spriteBatch);
                }
                else
                {
                    spriteBatch.DrawString(orbitron, $"Round Winner: Player One", new Vector2(50, 50), Color.Aqua, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
                    spriteBatch.DrawString(orbitron, $"Press Enter to play Round 2", new Vector2(200, 200), Color.Aqua, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter)) mainMenu.selection++;
                    playerOneWins++;
                }
                if (playerOneOne.health > 0)
                {
                    playerOneOne.Draw(gameTime, spriteBatch);
                }
                else
                {
                    spriteBatch.DrawString(orbitron, $"Round Winner: Player Two", new Vector2(50, 50), Color.Aqua, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
                    spriteBatch.DrawString(orbitron, $"Press Enter to play Round 2", new Vector2(200, 200), Color.Aqua, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) ) mainMenu.selection++;
                    playerTwoWins++;
                }

                spriteBatch.End();
            }
            if (!mainMenu.isActive && gameLevel == -2)
            {

                spriteBatch.Begin();

                spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                if (playerTwoTwo.health > 0)
                {
                    playerTwoTwo.Draw(gameTime, spriteBatch);
                }
                else if(playerOneWins > 0)
                {
                    spriteBatch.DrawString(orbitron, $"Player One Wins!", new Vector2(175, 50), Color.Aqua, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
                    spriteBatch.DrawString(orbitron, $"Press Enter to Play Again or Backspace to go to the Main Menu", new Vector2(125, 200), Color.Aqua, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        mainMenu.selection = -3;
                        restart = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Back))
                    {
                        mainMenu.selection = 0;
                        AddInitialScreens();
                    }
                }
                else
                {
                    spriteBatch.DrawString(orbitron, $"Round Winner: Player One", new Vector2(50, 50), Color.Aqua, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
                    spriteBatch.DrawString(orbitron, $"Press Enter to Play Final Round", new Vector2(200, 200), Color.Aqua, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter)) mainMenu.selection++;
                }
                if (playerOneTwo.health > 0)
                {
                    playerOneTwo.Draw(gameTime, spriteBatch);
                }
                else if (playerTwoWins > 0)
                {
                    spriteBatch.DrawString(orbitron, $"Player Two Wins!", new Vector2(175, 50), Color.Aqua, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
                    spriteBatch.DrawString(orbitron, $"Press Enter to Play Again or Backspace to go to the Main Menu", new Vector2(125, 200), Color.Aqua, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        mainMenu.selection = -3;
                        restart = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Back))
                    {
                        mainMenu.selection = 0;
                        AddInitialScreens();
                    }
                }
                else
                {
                    spriteBatch.DrawString(orbitron, $"Round Winner: Player Two", new Vector2(50, 50), Color.Aqua, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
                    spriteBatch.DrawString(orbitron, $"Press Enter to Play Final Round", new Vector2(200, 200), Color.Aqua, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter)) mainMenu.selection++;
                }

                spriteBatch.End();

                if (restart) Initialize();
            }
            if (!mainMenu.isActive && gameLevel == -1)
            {

                spriteBatch.Begin();

                spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                if (playerTwoThree.health > 0)
                {
                    playerTwoThree.Draw(gameTime, spriteBatch);
                }
                else
                {
                    spriteBatch.DrawString(orbitron, $"Player One Wins!", new Vector2(175, 50), Color.Aqua, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
                    spriteBatch.DrawString(orbitron, $"Press Enter to Play Again or Backspace to go to the Main Menu", new Vector2(125, 200), Color.Aqua, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                    playerOneWins = 0;
                    playerTwoWins = 0;
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        mainMenu.selection = -3;
                        restart = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Back))
                    {
                        mainMenu.selection = 0;
                        AddInitialScreens();
                    }
                }
                if (playerOneThree.health > 0)
                {
                    playerOneThree.Draw(gameTime, spriteBatch);
                }
                else
                {
                    spriteBatch.DrawString(orbitron, $"Player Two Wins!", new Vector2(175, 50), Color.Aqua, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
                    spriteBatch.DrawString(orbitron, $"Press Enter to Play Again or Backspace to go to the Main Menu", new Vector2(125, 200), Color.Aqua, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                    playerOneWins = 0;
                    playerTwoWins = 0;
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        mainMenu.selection = -3;
                        restart = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Back))
                    {
                        mainMenu.selection = 0;
                        AddInitialScreens();
                    }
                }

                spriteBatch.End();

                if (restart) Initialize();
            }

            base.Draw(gameTime);
        }
    }
}