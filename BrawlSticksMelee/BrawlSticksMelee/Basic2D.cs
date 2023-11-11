using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BrawlSticksMelee
{
    public class Basic2D
    {
        public float rot;

        public Vector2 pos;

        public Vector2 dims;

        public Texture2D myModel;

        public Basic2D(string path, Vector2 POS, Vector2 DIMS)
        {
            pos = POS;
            dims = DIMS;

            myModel = Globals.Content.Load<Texture2D>(path);
        }
    }
}
