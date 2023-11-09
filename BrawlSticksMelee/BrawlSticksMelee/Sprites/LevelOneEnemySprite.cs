using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace BrawlSticksMelee.Sprites
{
    public class LevelOneEnemySprite
    {
        private Texture2D stanceOne;

        private Texture2D stanceTwo;

        private Texture2D punchOne;

        private Texture2D punchTwo;

        private Texture2D punchThree;

        private Texture2D[] stanceFrame;

        private Vector2 position;

        private StickmanSprite player;

        private bool flipped = true;

        private double animationTimer;

        private short animationFrame = 0;

        public LevelOneEnemySprite(Vector2 startingPosition, StickmanSprite gamePlayer)
        {
            position = startingPosition;
            player = gamePlayer;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            stanceFrame = new Texture2D[]
            {
                stanceOne = content.Load<Texture2D>("LevelOneEnemySprite-StanceOne"),
                stanceTwo = content.Load<Texture2D>("LevelOneEnemySprite-StanceTwo"),
                punchOne = content.Load<Texture2D>("LevelOneEnemySprite-PunchOne"),
                punchTwo = content.Load<Texture2D>("LevelOneEnemySprite-StanceOne"),
                punchThree = content.Load<Texture2D>("LevelOneEnemySprite-PunchOne")
            };
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            // Automated Movement
            if (position.X + 50 > player.position.X)
            {
                position += new Vector2(-2, 0);
                flipped = true;
            }
            if (position.X - 50 < player.position.X)
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
            // Update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            // Update animation frame

            if (animationTimer > 0.4)
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
