using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MultiplayerPlatform.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerPlatform.Objects
{
    class Enemy : Sprite
    {

        Vector2 velocity;
        public bool isVisible = true;
        Random rand = new Random();
        int randX, randY;

        public Enemy(Texture2D texture, Vector2 position, SpriteBatch batch) : base(texture, position, batch)
        {
            randY = rand.Next(-7, 7);
            randX = rand.Next(-7, -3);

            velocity = new Vector2(randX, randY);
        }

        public void Update(List<Player> players, SoundEffect death)
        {

            foreach (var player in players)
            {
                if (Bounds.Intersects(player.Bounds))
                {
                    death.Play();
                    player.Position = new Vector2(80, 80);
                }
            }

            Position += velocity;

            if (Position.Y <= 0 || Position.Y >= 1080 - Texture.Height)
            {
                velocity.Y = -velocity.Y;
            }

            if (Position.X < 0 - Texture.Width)
            {
                isVisible = false;
            }
        }
    }
}
