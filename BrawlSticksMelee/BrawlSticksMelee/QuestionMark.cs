using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Security.Cryptography.X509Certificates;

namespace BrawlSticksMelee.Sprites
{
    public class QuestionMark
    {
        
        private Texture2D texture;

        public Vector2 position = new Vector2(333, 380);

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("questionmark");
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
          
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {    
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(0,0), new Vector2(0.7f, 0.7f), SpriteEffects.None, 0);
        }
    }
}
