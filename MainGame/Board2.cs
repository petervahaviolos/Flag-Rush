using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiplayerPlatform.Graphics;
using MultiplayerPlatform.Objects;
using System;

namespace MultiplayerPlatform
{
    public class Board2
    {
        private Tile[,] Tiles { get; set; }
        private int Columns { get; set; }
        private int Rows { get; set; }
        private Texture2D TileTexture { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private Random rand = new Random();
        private Vector2 tilePosition;

        public static Board2 CurrentBoard { get; private set; }

        public Board2(Texture2D texture, SpriteBatch batch, int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            TileTexture = texture;
            SpriteBatch = batch;
            InitializeBoard();
            SetAllBorderTilesBlocked();
            drawLevel();
            Board2.CurrentBoard = this;
           
        }

        private void InitializeBoard()
        {
            Tiles = new Tile[Columns, Rows];
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    tilePosition = new Vector2(x * TileTexture.Width, y * TileTexture.Height);
                    Tiles[x, y] = new Tile(TileTexture, tilePosition, SpriteBatch, false);
                }
            }

        }

        private void drawLevel() {
            for (int x = 1; x <= 6; x++) {
                Tiles[x, 3].IsBlocked = true;
            }
            for (int x = 53; x <= 58; x++)
            {
                Tiles[x, 3].IsBlocked = true;
            }
            for (int x = 21; x <= 38; x++)
            {
                Tiles[x, 5].IsBlocked = true;
            }
            for (int x = 1; x <= 12; x++)
            {
                Tiles[x, 7].IsBlocked = true;
            }
            for (int x = 17; x <= 18; x++)
            {
                Tiles[x, 7].IsBlocked = true;
            }
            for (int x = 41; x <= 42; x++)
            {
                Tiles[x, 7].IsBlocked = true;
            }
            for (int x = 47; x <= 58; x++)
            {
                Tiles[x, 7].IsBlocked = true;
            }
            for (int x = 9; x <= 50; x++)
            {
                Tiles[x, 10].IsBlocked = true;
            }
            for (int x = 1; x <= 7; x++)
            {
                Tiles[x, 13].IsBlocked = true;
            }
            for (int x = 52; x <= 58; x++)
            {
                Tiles[x, 13].IsBlocked = true;
            }
            for (int x = 10; x <= 14; x++)
            {
                Tiles[x, 16].IsBlocked = true;
            }
            for (int x = 19; x <= 20; x++)
            {
                Tiles[x, 16].IsBlocked = true;
            }
            for (int x = 39; x <= 40; x++)
            {
                Tiles[x, 16].IsBlocked = true;
            }
            for (int x = 45; x <= 49; x++)
            {
                Tiles[x, 16].IsBlocked = true;
            }
            for (int x = 25; x <= 34; x++)
            {
                Tiles[x, 18].IsBlocked = true;
            }
            for (int x = 1; x <= 7; x++)
            {
                Tiles[x, 19].IsBlocked = true;
            }
            for (int x = 52; x <= 58; x++)
            {
                Tiles[x, 19].IsBlocked = true;
            }
            Tiles[10, 20].IsBlocked = true;
            Tiles[49, 20].IsBlocked = true;
            for (int x = 9; x <= 50; x++)
            {
                Tiles[x, 23].IsBlocked = true;
            }
            for (int x = 5; x <= 6; x++)
            {
                Tiles[x, 24].IsBlocked = true;
            }
            for (int x = 53; x <= 54; x++)
            {
                Tiles[x, 24].IsBlocked = true;
            }
            for (int x = 1; x <= 12; x++)
            {
                Tiles[x, 27].IsBlocked = true;
            }
            for (int x = 17; x <= 18; x++)
            {
                Tiles[x, 27].IsBlocked = true;
            }
            for (int x = 41; x <= 42; x++)
            {
                Tiles[x, 27].IsBlocked = true;
            }
            for (int x = 47; x <= 58; x++)
            {
                Tiles[x, 27].IsBlocked = true;
            }
            for (int x = 21; x <= 38; x++)
            {
                Tiles[x, 29].IsBlocked = true;
            }
            for (int x = 1; x <= 3; x++)
            {
                Tiles[x, 30].IsBlocked = true;
            }
            for (int x = 56; x <= 58; x++)
            {
                Tiles[x, 30].IsBlocked = true;
            }
            for (int x = 17; x <= 18; x++)
            {
                Tiles[x, 31].IsBlocked = true;
            }
            for (int x = 41; x <= 42; x++)
            {
                Tiles[x, 31].IsBlocked = true;
            }
        }

        private void SetAllBorderTilesBlocked()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (x == 0 || x == Columns - 1 || y == 0 || y == Rows - 1)
                    {
                        Tiles[x, y].IsBlocked = true;
                    }
                }
            }
        }

        public bool HasRoomForRectangle(Rectangle rectangle)
        {
            foreach (var tile in Tiles)
            {
                if (tile.IsBlocked && tile.Bounds.Intersects(rectangle))
                {
                    return false;
                }
            }
            return true;
        }

        public Vector2 WhereCanIGetTo(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
        {
            Vector2 movementToTry = destination - originalPosition;
            Vector2 furthestAvailableLocationSoFar = originalPosition;
            int numberOfStepsToBreakMovementInto = (int)(movementToTry.Length() * 2) + 1;
            Vector2 oneStep = movementToTry / numberOfStepsToBreakMovementInto;

            for (int i = 1; i < numberOfStepsToBreakMovementInto; i++)
            {
                Vector2 positionToTry = originalPosition + oneStep * i;
                Rectangle newBoundary = CreateRectangleAtPosition(positionToTry, boundingRectangle.Width, boundingRectangle.Height);
                if (HasRoomForRectangle(newBoundary))
                {
                    furthestAvailableLocationSoFar = positionToTry;
                }
                else
                {
                    bool isDiagonalMove = movementToTry.X != 0 && movementToTry.Y != 0;
                    if (isDiagonalMove)
                    {
                        int stepsLeft = numberOfStepsToBreakMovementInto - (i - 1);
                        Vector2 remainingHorizontalMovement = oneStep.X * Vector2.UnitX * stepsLeft;
                        Vector2 finalPositionIfMovingHorizontally = furthestAvailableLocationSoFar + remainingHorizontalMovement;
                        furthestAvailableLocationSoFar =
                            WhereCanIGetTo(furthestAvailableLocationSoFar, finalPositionIfMovingHorizontally, boundingRectangle);

                        Vector2 remainingVerticalMovement = oneStep.Y * Vector2.UnitY * stepsLeft;
                        Vector2 finalPositionIfMovingVertically = furthestAvailableLocationSoFar + remainingVerticalMovement;
                        furthestAvailableLocationSoFar =
                            WhereCanIGetTo(furthestAvailableLocationSoFar, finalPositionIfMovingVertically, boundingRectangle);
                    }
                    break;
                }
            }
            return furthestAvailableLocationSoFar;
        }

        private Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }

        public void Draw()
        {
            foreach (var tile in Tiles)
            {
                tile.Draw();
            }
        }
    }
}
