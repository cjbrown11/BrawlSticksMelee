using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BrawlSticksMelee
{
    public static class Globals
    {
        public static float Time { get; private set; }

        public static ContentManager Content { get; set; }

        public static SpriteBatch SpriteBatch{ get; set; }

        public static void Update(GameTime gameTime)
        {
            Time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
