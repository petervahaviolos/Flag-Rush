using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiplayerPlatform.Graphics;
using MultiplayerPlatform.Objects;
using System;

namespace MultiplayerPlatform
{
    public class Board
    {
        private Tile[,] Tiles { get; set; }
        private int Columns { get; set; }
        private int Rows { get; set; }
        private Texture2D TileTexture { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private Random rand = new Random();
        private Vector2 tilePosition;

        public static Board CurrentBoard { get; private set; }

        public Board(Texture2D texture, SpriteBatch batch, int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            TileTexture = texture;
            SpriteBatch = batch;
            InitializeBoard();
            SetAllBorderTilesBlocked();
            SetObjectivesTileUnblocked();
            Board.CurrentBoard = this;
           
        }

        private void InitializeBoard()
        {
            Tiles = new Tile[Columns, Rows];
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    tilePosition = new Vector2(x * TileTexture.Width, y * TileTexture.Height);
                    Tiles[x, y] = new Tile(TileTexture, tilePosition, SpriteBatch, rand.Next(3) == 0);
                }
            }

        }

        private void ClearBoard()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    tilePosition = new Vector2(x * TileTexture.Width, y * TileTexture.Height);
                    Tiles[x, y] = new Tile(TileTexture, tilePosition, SpriteBatch, false);
                }
            }
        }

        private void SetObjectivesTileUnblocked()
        {
            for (int x = 1; x < 5; x++)
            {
                for (int y = 1; y < 5; y++)
                {
                    Tiles[x, y].IsBlocked = false;
                }
            }

            for (int x = Columns - 4; x < Columns - 1; x++)
            {
                for (int y = Rows - 4; y < Rows - 1; y++)
                {
                    Tiles[x, y].IsBlocked = false;
                }
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

        public void RedrawBoard()
        {
            ClearBoard();
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    tilePosition = new Vector2(x * TileTexture.Width, y * TileTexture.Height);
                    Tiles[x, y] = new Tile(TileTexture, tilePosition, SpriteBatch, rand.Next(3) == 0);

                }

            }
            SetAllBorderTilesBlocked();
            SetObjectivesTileUnblocked();
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
