using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiplayerPlatform;
using MultiplayerPlatform.Graphics;
using MultiplayerPlatform.Objects;
using MultiplayerPlatform.States;
using MultiplayerPlatformGame.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerPlatformGame.Objects
{
    public class Bullet
    {
        private Texture2D bulletTexture;
        public Vector2 bulletPosition;
        private int bulletFacing;
        private Vector2 bulletVelocity;
        public bool isActive;
        private float moveSpeed;
        public Rectangle bulletRectangle;
        private List<Player> Players = GameMode2State.currentGameState.players;
        
        private Random rand = new Random();

        public Bullet()
        {
            isActive = false;
        }

        public void ActivateBullet(Vector2 center, Texture2D texture) {
            bulletFacing = Player.currentPlayer.facing;
            bulletPosition = center;
            bulletTexture = texture;
            moveSpeed = 1500;
            isActive = true;
            SetVelocity();
        }

        private void SetVelocity() {
            bulletVelocity = -(bulletPosition);
            bulletVelocity.Normalize();
        }

        public void Update(GameTime gameTime) {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (bulletPosition.X < 0) {
                isActive = false;
            }
            if (bulletPosition.X > 1920) {
                isActive = false;
            }
            if (bulletFacing == 0) {
                bulletPosition.X += (bulletVelocity.X * moveSpeed * elapsedTime);
            }
            else if (bulletFacing == 1)
            {
    
                bulletPosition.X -= (bulletVelocity.X * moveSpeed * elapsedTime);

            }
            for (int i = 0; i < Players.Count; i++) {
                if (bulletRectangle.Intersects(Players[i].Bounds)) {
                    Players[i].Position = GameMode2State.currentGameState.respawnPositions[rand.Next(0,GameMode2State.currentGameState.respawnPositions.Count)];
                }
            }

            bulletRectangle = new Rectangle((int)bulletPosition.X, (int)bulletPosition.Y, bulletTexture.Width, bulletTexture.Height);
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(bulletTexture, bulletPosition, null, Color.White, 0f, new Vector2(bulletTexture.Width / 2, bulletTexture.Height / 2), 1.0f, SpriteEffects.None, 0f);

        }
    }
}
