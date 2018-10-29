using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiplayerPlatform.Graphics
{
    public class Sprite
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public SpriteBatch SpriteBatch { get; set; }


        public Sprite(Texture2D texture, Vector2 position, SpriteBatch batch)
        {
            Texture = texture;
            Position = position;
            SpriteBatch = batch;
        }

        public virtual void Draw()
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(Texture, Position, Color.White);
            SpriteBatch.End();
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            }
        }


    }
}
