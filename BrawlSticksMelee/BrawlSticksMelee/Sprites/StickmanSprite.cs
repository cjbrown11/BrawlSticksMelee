using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Security.Cryptography.X509Certificates;

namespace BrawlSticksMelee.Sprites
{
    public class StickmanSprite
    {
        private KeyboardState keyboardState;

        private MouseState currentMouseState;

        private MouseState previousMouseState;

        private Texture2D stanceOne;

        private Texture2D stanceTwo;

        private Texture2D punchOne;

        private Texture2D punchTwo;

        private Texture2D punchThree;

        private Texture2D[] stanceFrame;

        public Vector2 position;

        public LevelOneEnemySprite enemySprite;

        private bool flipped;

        private double animationTimer;

        private short animationFrame = 0;

        public StickmanSprite(Vector2 startingPosition, LevelOneEnemySprite enemy)
        {
            position = startingPosition;
            enemySprite = enemy;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            stanceFrame = new Texture2D[]
            {
                stanceOne = content.Load<Texture2D>("StickmanSprite-StanceOne"),
                stanceTwo = content.Load<Texture2D>("StickmanSprite-StanceTwo"),
                punchOne = content.Load<Texture2D>("StickmanSprite-PunchOne"),
                punchTwo = content.Load<Texture2D>("StickmanSprite-StanceOne"),
                punchThree = content.Load<Texture2D>("StickmanSprite-PunchOne")
            };
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            // Apply keyboard movement
            if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) && position.X > 0)
            {
                position += new Vector2(-2, 0);
                flipped = true;
            }
            if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) && position.X < 670)
            {
                position += new Vector2(2, 0);
                flipped = false;
            }
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            previousMouseState = currentMouseState;

            currentMouseState = Mouse.GetState();

            // Update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            // Update animation frame
            
            
            if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                animationFrame = 2;
                animationTimer = 0.3;
                if ((position.X < enemySprite.position.X + 50 && position.X > enemySprite.position.X - 50))
                {
                    enemySprite.health -= 10;
                }
            }
            else if (animationTimer > 0.4)
            {
                if (animationFrame == 2)
                {
                    animationFrame++;
                    animationTimer = 0.3;
                }
                else if (animationFrame == 3)
                {
                    animationFrame++;
                    animationTimer = 0.3;
                }
                else if (animationFrame == 4)
                {
                    animationFrame = 0;
                    animationTimer = 0.3;
                }
                else
                {
                    animationFrame++;
                    if (animationFrame > 1) animationFrame = 0;
                    animationTimer = 0;
                }
                
            }
            

            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(stanceFrame[animationFrame], position, null, Color.White, 0, new Vector2(0, 0), 2, spriteEffects, 0);
        }
    }
}
