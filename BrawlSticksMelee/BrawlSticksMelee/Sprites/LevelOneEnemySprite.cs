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

        public Vector2 position;

        private Texture2D front;

        private Texture2D back;

        public HealthBar healthBar;

        public int health;

        public Vector2 healtBarPos;

        public int healthMax;

        private StickmanSprite player;

        private bool flipped = true;

        private double animationTimer;

        public short animationFrame = 0;

        public float scale = 2;

        public LevelOneEnemySprite(Vector2 startingPosition, StickmanSprite gamePlayer, Vector2 healthPosition, int maxHealth)
        {
            position = startingPosition;
            player = gamePlayer;
            health = maxHealth;
            healthMax = maxHealth;
            healtBarPos = healthPosition;
        }

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

            back = content.Load<Texture2D>("healthbg");
            front = content.Load<Texture2D>("healthfg");

            healthBar = new(back, front, healthMax, healtBarPos);
        }

        public void TakeDamage(int dmg)
        {
            health -= dmg;
            if (health < 0) health = 0;
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
                position += new Vector2(-1.5f, 0);
                flipped = true;
            }
            if (position.X - 50 < player.position.X)
            {
                position += new Vector2(1.5f, 0);
                flipped = false;
            }

            healthBar.Update(health);
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Update animation timerd
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            // Update animation frame


            if (animationTimer > 0.4)
            {
                animationFrame++;
                if (animationFrame > 1) animationFrame = 0;
                animationTimer = 0;
            }
                if ((position.X < player.position.X + 150) && (position.X > player.position.X))
                {
                    if (player.animationFrame == 2)
                    {
                        health -= 10;
                        TakeDamage(10);
                    }
                    if (animationFrame != 2 && player.blocked == false)
                    {
                        animationFrame = 2;
                        animationTimer = 0.2;
                        player.health -= 1;
                        player.TakeDamage(1);
                    }
                }
                else if (animationFrame == 2)
                {
                    animationFrame++;
                    animationTimer = 0.2;
                }
                else if (animationFrame == 3)
                {
                    animationFrame++;
                    animationTimer = 0.2;
                }
                else if (animationFrame == 4)
                {
                    animationFrame = 0;
                    animationTimer = 0.2;
                }
            

            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(stanceFrame[animationFrame], position, null, Color.White, 0, new Vector2(0, 0), scale, spriteEffects, 0);
            healthBar.Draw(spriteBatch);
        }
    }
}
