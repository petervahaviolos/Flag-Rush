using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MultiplayerPlatform.Graphics;
using MultiplayerPlatformGame.Objects;
using MultiplayerPlatformGame.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerPlatform.Objects
{
    public class Player : Sprite
    {
        public Bullet Bullet;
        private List<Bullet> bullets = new List<Bullet>();
        private Vector2 Movement { get; set; }
        private Vector2 oldPosition;
        private Texture2D bulletTexture;
        KeyboardState keyboardState = Keyboard.GetState();

        public int score;
        private SpriteEffects s = SpriteEffects.None;
        private int gameMode;
        public int facing;

        public static Player currentPlayer { get; private set; }

        public Player(Texture2D texture, Vector2 position, SpriteBatch batch, Texture2D bulletTexture, int bulletFacing) : base(texture, position, batch) {
            this.bulletTexture = bulletTexture;
            this.facing = bulletFacing;
            Player.currentPlayer = this;
            
        }

        public int GetScore()
        {
            return score;
        }

        public void Update(GameTime gameTime, Keys jump, Keys left, Keys right, Keys shoot, int GameMode, List<Player> players)
        {
            gameMode = GameMode;
            CheckKeyboardInput(jump, left, right, shoot, players);
            for (int i = 0; i < bullets.Count; i++) {
                if (bullets[i].isActive) {
                    bullets[i].Update(gameTime);
                }        
            }
            
            AffectWithGravity();
            SimulateFriction();
            MoveAsFarAsPossible(gameTime, GameMode);
            StopMovingIfBlocked();
        }

        public bool IsOnGround(int GameMode)
        {
            Rectangle onePixelLower = Bounds;
            onePixelLower.Offset(0, 1);
            if (GameMode == 1)
            {
                return !Board.CurrentBoard.HasRoomForRectangle(onePixelLower);
            }
            else if (GameMode == 2)
            {
                return !Board2.CurrentBoard.HasRoomForRectangle(onePixelLower);
            }
            else {
                return true;
            }
            
        }

        private void AffectWithGravity()
        {
            Movement += Vector2.UnitY * .5f;
        }

        private void CheckKeyboardInput(Keys jump, Keys left, Keys right, Keys shoot, List<Player> players)
        {

   
            KeyboardState previousKey = keyboardState;
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(left))
            {
                Movement += new Vector2(-0.8f, 0);
                s = SpriteEffects.None;
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].Equals(currentPlayer))
                    {
                        currentPlayer.facing = 0;
                    }
                }
            }
            if (keyboardState.IsKeyDown(right))
            {
                Movement += new Vector2(0.8f, 0);
                s = SpriteEffects.FlipHorizontally;
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].Equals(currentPlayer))
                    {
                        currentPlayer.facing = 1;
                    }
                }
            }
            if (keyboardState.IsKeyDown(jump) && IsOnGround(gameMode))
            {
                Movement = new Vector2(0, -25f);
            }
            if (keyboardState.IsKeyDown(shoot) && previousKey.IsKeyUp(shoot) && gameMode == 2) {
                
                bullets.Add(new Bullet());
               
                for (int i = 0; i < bullets.Count; i++) {
                    
                    if (!bullets[i].isActive && currentPlayer.facing == 1) {
                        bullets[i].ActivateBullet(this.Position + (Vector2.UnitX * Texture.Width) + (Vector2.UnitY * (Texture.Height / 2)), bulletTexture);
                    }
                    else if (!bullets[i].isActive && currentPlayer.facing == 0)
                    {
                        bullets[i].ActivateBullet(this.Position - (Vector2.UnitX * Texture.Width) + (Vector2.UnitY * (Texture.Height / 2)), bulletTexture);
                    }
                }
            }

        }

        public override void Draw()
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(Texture, Bounds, null, Color.White, 0, new Vector2(0, 0), s, 0);
            foreach (var bullet in bullets) {
                bullet.Draw(SpriteBatch);
            }
            SpriteBatch.End();
        }


        private void SimulateFriction()
        {
            Movement -= Movement * new Vector2(.15f, .15f);
        }

        private void UpdatePosition(GameTime gameTime)
        {
            Position += Movement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }

        private void MoveAsFarAsPossible(GameTime gameTime, int GameMode)
        {
            Vector2 oldPosition = Position;
            UpdatePosition(gameTime);
            if (GameMode == 1)
            {
                Position = (Board.CurrentBoard.WhereCanIGetTo(oldPosition, Position, Bounds));
            }
            else if (GameMode == 2) {
                Position = (Board2.CurrentBoard.WhereCanIGetTo(oldPosition, Position, Bounds));
            }
            
        }

        private void StopMovingIfBlocked()
        {
            Vector2 lastMovement = Position - oldPosition;
            if (lastMovement.X == 0) { Movement *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { Movement *= Vector2.UnitX; }
        }
    }
}
