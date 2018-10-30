using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MultiplayerPlatform.Objects;
using MultiplayerPlatformGame.MainGame;

namespace MultiplayerPlatform.States
{
    public class GameMode1State : State
    {
        GraphicsDeviceManager GraphicsDeviceManager;
        SpriteBatch spriteBatch;
        private List<Player> players = new List<Player>();
        private Board board;
        private Background background;
        private Vector2 startingPosition;
        private List<Keys> jumpKeys;
        private List<Keys> leftKeys;
        private List<Keys> rightKeys;
        private List<Keys> shootKeys;

        private Texture2D tileTexture, backgroundTexture, flagTexture, enemyTexture, bulletTexture;
        private Flag flag;
        private SpriteFont font;
        private SoundEffect win, death;

        public int WindowWidth = 1920;
        public int WindowHeight = 1080;
        private int columns = 60;
        private int rows = 34;

        private Random rand = new Random();


        public GameMode1State(Game1 game, GraphicsDevice graphics, ContentManager content, int numberOfPlayers, List<int> chosenTexture, List<Texture2D> textures, List<Keys> jumpKeys, List<Keys> leftKeys, List<Keys> rightKeys, List<Keys> shootKeys) : base(game, graphics, content)
        {
            this.jumpKeys = jumpKeys;
            this.leftKeys = leftKeys;
            this.rightKeys = rightKeys;
            this.shootKeys = shootKeys;
            GraphicsDeviceManager = Game1.graphics;
            spriteBatch = new SpriteBatch(graphics);
            
            
            startingPosition = new Vector2(80, 80);

            
            tileTexture = content.Load<Texture2D>("Tiles/TileDark");
            Console.WriteLine("Loaded tile texture");
            backgroundTexture = content.Load<Texture2D>("Tiles/Background");
            Console.WriteLine("Loaded background texture");
            flagTexture = content.Load<Texture2D>("Objects/Flag");
            Console.WriteLine("Loaded flag texture");
            font = content.Load<SpriteFont>("font");
            Console.WriteLine("Loaded font");
            bulletTexture = content.Load<Texture2D>("Objects/Bullet");
            enemyTexture = content.Load<Texture2D>("Enemies/Crab");
        

            board = new Board(tileTexture, spriteBatch, columns, rows);
            Console.WriteLine("Loaded board");
            flag = new Flag(flagTexture, new Vector2(WindowWidth - 60, WindowHeight - 50), spriteBatch);
            Console.WriteLine("Loaded flag");
            background = new Background(backgroundTexture, spriteBatch, columns, rows);
            Console.WriteLine("Loaded background");

            Console.WriteLine("Number of players: " + numberOfPlayers);
            for (int i = 0; i < numberOfPlayers; i++) {
                players.Add(new Player(textures[chosenTexture[i]], startingPosition, spriteBatch, bulletTexture, 0));
                Console.WriteLine("Loaded player " + (i+1));
            }
            

            Song song = content.Load<Song>("Sound/background");
            Console.WriteLine("Loaded background music");
            win = content.Load<SoundEffect>("Sound/win");
            death = content.Load<SoundEffect>("Sound/death");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.3f;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            background.Draw();
            board.Draw();
            foreach (var player in players) {
                player.Draw();
            }
            flag.Draw();
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Home))
            {
                board.RedrawBoard();
                foreach (var player in players)
                {
                    player.Position = startingPosition;
                }
                flag.Position = new Vector2(rand.Next(1920), rand.Next(1080));
            }

            if (state.IsKeyDown(Keys.End))
            {
                foreach (var player in players)
                {
                    player.score = 0;
                }
            }

            for (int i = 0; i < players.Count; i++) {
                players[i].Update(gameTime, jumpKeys[i], leftKeys[i], rightKeys[i], shootKeys[i], 1, players);
            }

            flag.Update(gameTime, players, board, new Vector2(80, 80), win);
        }
    }
}
