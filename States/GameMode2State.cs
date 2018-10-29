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
using MultiplayerPlatform;
using MultiplayerPlatform.Objects;
using MultiplayerPlatform.States;
using MultiplayerPlatformGame.MainGame;

namespace MultiplayerPlatformGame.States
{
    public class GameMode2State : State
    {
        GraphicsDeviceManager GraphicsDeviceManager;
        SpriteBatch spriteBatch;
        public List<Player> players = new List<Player>();
        public List<Vector2> respawnPositions = new List<Vector2>();
        private Board2 board;
        private Background background;
        private Vector2 startingPosition;
        private List<Keys> jumpKeys;
        private List<Keys> leftKeys;
        private List<Keys> rightKeys;
        private List<Keys> shootKeys;

        private Texture2D tileTexture, backgroundTexture, flagTexture, enemyTexture;
        private Texture2D bulletTexture;
        private Flag flag;
        private SpriteFont font;
        private SoundEffect win, death;

        public int WindowWidth = 1920;
        public int WindowHeight = 1080;
        private int columns = 60;
        private int rows = 34;

        private Random rand = new Random();

        public static GameMode2State currentGameState { get; private set; }

        public GameMode2State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, int numberOfPlayers, List<int> chosenTexture, List<Texture2D> textures, List<Keys> jumpKeys, List<Keys> leftKeys, List<Keys> rightKeys, List<Keys> shootKeys) : base(game, graphicsDevice, content)
        {
            this.jumpKeys = jumpKeys;
            this.leftKeys = leftKeys;
            this.rightKeys = rightKeys;
            this.shootKeys = shootKeys;
            GraphicsDeviceManager = Game1.graphics;
            spriteBatch = new SpriteBatch(graphicsDevice);
            respawnPositions.Add(new Vector2(80, 50));
            

            startingPosition = new Vector2(80, 50);


            tileTexture = content.Load<Texture2D>("Tiles/TileDark");
            Console.WriteLine("Loaded tile texture");
            backgroundTexture = content.Load<Texture2D>("Tiles/Background");
            Console.WriteLine("Loaded background texture");
            flagTexture = content.Load<Texture2D>("Objects/Flag");
            Console.WriteLine("Loaded flag texture");
            font = content.Load<SpriteFont>("font");
            Console.WriteLine("Loaded font");
            enemyTexture = content.Load<Texture2D>("Enemies/Crab");
            bulletTexture = content.Load<Texture2D>("Objects/Bullet");


            board = new Board2(tileTexture, spriteBatch, columns, rows);
            Console.WriteLine("Loaded board");
            flag = new Flag(flagTexture, new Vector2(960,544), spriteBatch);
            Console.WriteLine("Loaded flag");
            background = new Background(backgroundTexture, spriteBatch, columns, rows);
            Console.WriteLine("Loaded background");

            Console.WriteLine("Number of players: " + numberOfPlayers);
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players.Add(new Player(textures[chosenTexture[i]], startingPosition, spriteBatch,bulletTexture));
                Console.WriteLine("Loaded player " + (i + 1));
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
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Update(gameTime, jumpKeys[i], leftKeys[i], rightKeys[i], shootKeys[i], 2, players);
            }
            flag.Update(gameTime, players, startingPosition, win);
        }
    }
}
