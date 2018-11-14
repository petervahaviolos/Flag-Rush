using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MultiplayerPlatform.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerPlatformGame.MainGame
{
    public class HUD
    {
        
        private List<Player> players;
        private SpriteFont font;

        public HUD(List<Player> player, ContentManager content) {
            players = player;
            font = content.Load<SpriteFont>("Font");
        }

        public void Draw(SpriteBatch spriteBatch) {
            int x = 32;
            for (int i = 0; i < players.Count; i++) {
                spriteBatch.DrawString(font, "Player " + (i+1) + ": " + players[i].score, new Vector2(x, 0), Color.White);
                x += 128;
            }
        }
    }
}
