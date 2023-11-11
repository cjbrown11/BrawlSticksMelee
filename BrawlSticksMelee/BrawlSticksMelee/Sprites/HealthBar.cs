using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BrawlSticksMelee.Sprites
{
    public class HealthBar
    {
        private Texture2D background;

        private Texture2D foreground;

        private Vector2 positionFront;

        private Vector2 positionBack;

        private float maxValue;

        public float currentValue;

        public Rectangle part;

        public HealthBar(Texture2D bg, Texture2D fg, float max, Vector2 pos)
        {
            background = bg;
            foreground = fg;
            maxValue = max;
            currentValue = max;
            positionFront = pos;
            positionBack = pos;
            positionBack.Y -= 33;
            part = new(0, 0, foreground.Width, foreground.Height);
        }

        public virtual void Update(float value)
        {
            currentValue = value;
            part.Width = (int)(currentValue / maxValue * foreground.Width);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, positionBack, Color.White);
            spriteBatch.Draw(foreground, positionFront, part, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }
    }
}
