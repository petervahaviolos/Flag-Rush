using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MultiplayerPlatform.Graphics;
using MultiplayerPlatformGame.States;
using System;
using System.Collections.Generic;

namespace MultiplayerPlatform.Objects
{
    class Flag : Sprite
    {
        private Random rand = new Random();

        public Flag(Texture2D texture, Vector2 position, SpriteBatch batch) : base(texture, position, batch)
        {

        }

        public void Update(GameTime gametime, List<Player> Players, Board board, Vector2 originalPlayerPosition, SoundEffect win)
        {
                if (!Board.CurrentBoard.HasRoomForRectangle(Bounds))
                {
                    Position = new Vector2(rand.Next(1920), rand.Next(1080));
                }
                for (int i = 0; i < Players.Count; i++)
                {
                    if (Bounds.Intersects(Players[i].Bounds))
                    {
                        win.Play();
                        Players[i].score += 1;
                        board.RedrawBoard();
                        Position = new Vector2(rand.Next(1920), rand.Next(1080));
                        for (int j = 0; j < Players.Count; j++)
                        {
                            Players[j].Position = originalPlayerPosition;
                        }

                    }
                }
        }

        public void Update(GameTime gametime, List<Player> Players, Vector2 originalPlayerPosition, SoundEffect win)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (Bounds.Intersects(Players[i].Bounds))
                {
                    win.Play();
                    Players[i].score += 1;                  
                    for (int j = 0; j < Players.Count; j++)
                    {
                        Players[j].Position = GameMode2State.currentGameState.respawnPositions[rand.Next(0, GameMode2State.currentGameState.respawnPositions.Count)];
                    }

                }
            }
        }
    }
}
