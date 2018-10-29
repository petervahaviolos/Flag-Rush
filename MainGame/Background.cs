using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiplayerPlatform.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerPlatformGame.MainGame
{
    class Background
    {
        private Tile[,] Tiles { get; set; }
        private int Columns { get; set; }
        private int Rows { get; set; }
        private Texture2D TileTexture { get; set; }
        private SpriteBatch SpriteBatch { get; set; }

        public Background(Texture2D texture, SpriteBatch batch, int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            TileTexture = texture;
            SpriteBatch = batch;
            InitializeBackground();
        }

        private void InitializeBackground()
        {
            Tiles = new Tile[Columns, Rows];
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Vector2 tilePosition = new Vector2(x * TileTexture.Width, y * TileTexture.Height);
                    Tiles[x, y] = new Tile(TileTexture, tilePosition, SpriteBatch, true);
                }
            }
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
