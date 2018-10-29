using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MultiplayerPlatform.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerPlatform.Objects
{
    public class Player : Sprite
    {
        private Vector2 Movement { get; set; }
        private Vector2 oldPosition;
        public int score;
        private SpriteEffects s = SpriteEffects.None;
        private int gameMode;

        public Player(Texture2D texture, Vector2 position, SpriteBatch batch) : base(texture, position, batch) {

        }

        public int GetScore()
        {
            return score;
        }

        public void Update(GameTime gameTime, Keys jump, Keys left, Keys right, int GameMode)
        {
            gameMode = GameMode;
            CheckKeyboardInput(jump, left, right);
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

        private void CheckKeyboardInput(Keys jump, Keys left, Keys right)
        {

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(left))
            {
                Movement += new Vector2(-0.8f, 0);
                s = SpriteEffects.None;
            }
            if (keyboardState.IsKeyDown(right))
            {
                Movement += new Vector2(0.8f, 0);
                s = SpriteEffects.FlipHorizontally;
            }
            if (keyboardState.IsKeyDown(jump) && IsOnGround(gameMode))
            {
                Movement = new Vector2(0, -25f);
            }

        }

        public override void Draw()
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(Texture, Bounds, null, Color.White, 0, new Vector2(0, 0), s, 0);
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
