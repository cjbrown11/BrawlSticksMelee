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

        private KeyboardState currentKeyboardState;

        private KeyboardState previousKeyboardState;

        private MouseState currentMouseState;

        private MouseState previousMouseState;

        private Texture2D stanceOne;

        private Texture2D stanceTwo;

        private Texture2D punchOne;

        private Texture2D punchTwo;

        private Texture2D punchThree;

        private Texture2D block;

        private Texture2D[] stanceFrame;

        private Texture2D front;

        private Texture2D back;

        public Vector2 position;

        private HealthBar healthBar;

        public bool flipped;

        public bool poweredUp = false;

        public int health = 100;

        public int healthMax;

        private double animationTimer;

        public short animationFrame = 0;

        public float rotation;

        public int hit = 0;

        public bool blocked = false;


        public StickmanSprite(Vector2 startingPosition)
        {
            position = startingPosition;

            healthMax = health;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content, bool poweredUp)
        {
            if(!poweredUp)
            {
                stanceFrame = new Texture2D[]
                {
                stanceOne = content.Load<Texture2D>("StickmanSprite-StanceOne"),
                stanceTwo = content.Load<Texture2D>("StickmanSprite-StanceTwo"),
                punchOne = content.Load<Texture2D>("StickmanSprite-PunchOne"),
                punchTwo = content.Load<Texture2D>("StickmanSprite-StanceOne"),
                punchThree = content.Load<Texture2D>("StickmanSprite-PunchOne"),
                block = content.Load<Texture2D>("StickmanSprite-Block")
                };
            }
            else
            {
                stanceFrame = new Texture2D[]
                {
                stanceOne = content.Load<Texture2D>("StickmanGloveSprite-StanceOne"),
                stanceTwo = content.Load<Texture2D>("StickmanGloveSprite-StanceTwo"),
                punchOne = content.Load<Texture2D>("StickmanGloveSprite-PunchOne"),
                punchTwo = content.Load<Texture2D>("StickmanGloveSprite-StanceOne"),
                punchThree = content.Load<Texture2D>("StickmanGloveSprite-PunchOne"),
                block = content.Load<Texture2D>("StickmanGloveSprite-Block")
                };
            }

            back = content.Load<Texture2D>("healthbg");
            front = content.Load<Texture2D>("healthfg");

            healthBar = new(back, front, healthMax, new(100, 50));
        }

        public void TakeDamage(int dmg)
        {
            health -= dmg;
            if (health < 0) health = 0;
            hit = 1;
        }


        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime, bool poweredUp, ContentManager content)
        {
            LoadContent(content, poweredUp);

            keyboardState = Keyboard.GetState();

            // Apply keyboard movement
            if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) && position.X > 0)
            {
                position += new Vector2(-2, 0);
                flipped = true;
            }
            if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.S)) && position.X < 670)
            {
                position += new Vector2(2, 0);
                flipped = false;
            }
            if (keyboardState.IsKeyDown(Keys.Q) && poweredUp)
            {
                animationFrame = 5;
                blocked = true;
            }    
            if(keyboardState.IsKeyUp(Keys.Q))
            {
                blocked = false;
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
            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();

            // Update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            // Update animation frame
        
            if(previousKeyboardState.IsKeyUp(Keys.W) && keyboardState.IsKeyDown(Keys.W))
            {
                animationFrame = 2;
                animationTimer = 0.3;
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

            if(position.X > 200 && position.X < 350 && animationFrame == 2)
            {
                poweredUp = true;
            }
     
            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            if(!poweredUp)
            {
                spriteBatch.Draw(stanceFrame[animationFrame], position, null, Color.White, 0, new Vector2(0, 0), 2, spriteEffects, 0);
            }
            /*
            if (!poweredUp && hit == 1)
            {
                position.X -= 5;
                spriteBatch.Draw(stanceFrame[animationFrame], position, null, Color.White, -0.5f, new Vector2(0, 0), 2, spriteEffects, 0);
                hit++;
            }
            if (!poweredUp && hit == 2)
            {
                position.X -= 10;
                spriteBatch.Draw(stanceFrame[animationFrame], position, null, Color.White, -0.5f, new Vector2(0, 0), 2, spriteEffects, 0);
                hit = 0;
            }
            */
            if (poweredUp)
            {
                spriteBatch.Draw(stanceFrame[animationFrame], position, null, Color.White, 0, new Vector2(0, 0), 3.5f, spriteEffects, 0);
            }
            healthBar.Draw(spriteBatch);
        }
    }
}
