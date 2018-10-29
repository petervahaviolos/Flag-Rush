using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MultiplayerPlatform.Controls;
using MultiplayerPlatformGame.MainGame;
using MultiplayerPlatformGame.States;

namespace MultiplayerPlatform.States
{
    public class SelectionState : State
    {
        private SpriteBatch spriteBatch;
        private List<Component> components = new List<Component>();
        private List<Button> chooseCharacter = new List<Button>();
        private List<Texture2D> playerTextures = new List<Texture2D>();
        private List<int> chosenTexture;
        private List<Keys> jumpKeys;
        private List<Keys> leftKeys;
        private List<Keys> rightKeys;

        private Texture2D buttonTexture;
        private Texture2D backgroundTexture;
        private SpriteFont buttonFont;
        private SpriteFont bigFont;
        
        private Background background;
        private Button setJumpKeyButton;
        private Button setLeftKeyButton;
        private Button setRightKeyButton;
        private Button setGameModeButton;

        private int screenWidth;
        private int screenHeight;
        private int paddingX = 50;
        private int paddingY = 25;
        private int playerNumber;
        private int characterChosen;
        private int currentGameMode;
        

        public SelectionState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, int playerNumber, List<int> chosenTexture, List<Keys> jumpKeys, List<Keys> leftKeys, List<Keys> rightKeys) : base(game, graphicsDevice, content)
        {
          
            currentGameMode = 2;
            spriteBatch = new SpriteBatch(graphicsDevice);
            this.playerNumber = playerNumber;
            this.chosenTexture = chosenTexture;
            this.jumpKeys = jumpKeys;
            this.leftKeys = leftKeys;
            this.rightKeys = rightKeys;

            screenWidth = graphicsDevice.Viewport.Width;
            screenHeight = graphicsDevice.Viewport.Height;

            buttonTexture = content.Load<Texture2D>("Button");
            backgroundTexture = content.Load<Texture2D>("Tiles/Background");
            buttonFont = content.Load<SpriteFont>("Font");
            bigFont = content.Load<SpriteFont>("BigFont");

            background = new Background(backgroundTexture, spriteBatch, 60, 34);
            

            

            for (int i = 0; i < 8; i++) {
                playerTextures.Add(content.Load<Texture2D>("Sprites/Char" + (i + 1)));
            }

            for (int i = 0; i < playerTextures.Count; i++) {
                chooseCharacter.Add(new Button(playerTextures[i], buttonFont));
                

                components.Add(chooseCharacter[i]);
            }

            chooseCharacter[0].Position = new Vector2(screenWidth / 2 - 250, bigFont.Texture.Height);

            for (int i = 1; i < chooseCharacter.Count; i++) {
                chooseCharacter[i].Position = new Vector2(chooseCharacter[i - 1].Position.X + chooseCharacter[i].Rectangle.Width + paddingX, bigFont.Texture.Height);
            }

            var startGameButton = new Button(buttonTexture, buttonFont) {
                Position = new Vector2((screenWidth / 2) - buttonTexture.Width / 2,screenHeight - buttonTexture.Height - paddingY),
                Text = "Start"
            };

            var nextPlayerButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(startGameButton.Position.X, startGameButton.Position.Y - (paddingY*2)),
                Text = "Next player"
            };

            setLeftKeyButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((screenWidth / 2) - buttonTexture.Width / 2 - (paddingX*5), screenHeight - (buttonTexture.Height * 2) - (paddingY * 10)),
                Text = "Set Left Key"
            };

            setJumpKeyButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((screenWidth / 2) - (buttonTexture.Width / 2), screenHeight - (buttonTexture.Height * 2) - (paddingY * 10)),
                Text = "Set Jump Key"
            };

            setRightKeyButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((screenWidth / 2) - buttonTexture.Width / 2 + (paddingX * 5), screenHeight - (buttonTexture.Height * 2) - (paddingY * 10)),
                Text = "Set Right Key"
            };

            setGameModeButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(nextPlayerButton.Position.X, nextPlayerButton.Position.Y - paddingY*3),
                Text = "Select Game Mode"
            };



            startGameButton.Click += StartGameButton_Click;
            nextPlayerButton.Click += NextPlayerButton_Click;
            setJumpKeyButton.Click += SetJumpKeyButton_Click;
            setLeftKeyButton.Click += SetLeftKeyButton_Click;
            setRightKeyButton.Click += SetRightKeyButton_Click;
            setGameModeButton.Click += SetGameModeButton_Click;
            
            components.Add(startGameButton);
            components.Add(nextPlayerButton);
            components.Add(setGameModeButton);
            components.Add(setJumpKeyButton);
            components.Add(setLeftKeyButton);
            components.Add(setRightKeyButton);
            
        }

        private void SetGameModeButton_Click(object sender, EventArgs e)
        { 
            if (currentGameMode ==  1)
            {        
                setGameModeButton.Text = "King of the Flag";
                currentGameMode = 2;
                Console.WriteLine(currentGameMode);
            }
            else if (currentGameMode == 2) {
                
                setGameModeButton.Text = "Find the Flag";
                currentGameMode = 1;
                Console.WriteLine(currentGameMode);
            }
        }

        private void SetRightKeyButton_Click(object sender, EventArgs e)
        {
            KeyboardState currentKey = Keyboard.GetState();

            for (int i = 0; i < currentKey.GetPressedKeys().Length; i++) {
                rightKeys.Add(currentKey.GetPressedKeys()[i]);
                rightKeys[playerNumber-1] = currentKey.GetPressedKeys()[i];
                setRightKeyButton.Text = rightKeys[playerNumber - 1].ToString();
            }

            
        }

        private void SetLeftKeyButton_Click(object sender, EventArgs e)
        {
            KeyboardState currentKey = Keyboard.GetState();

            for (int i = 0; i < currentKey.GetPressedKeys().Length; i++)
            {
                leftKeys.Add(currentKey.GetPressedKeys()[i]);
                leftKeys[playerNumber - 1] = currentKey.GetPressedKeys()[i];
                setLeftKeyButton.Text = leftKeys[playerNumber - 1].ToString();
            }
        }

        private void SetJumpKeyButton_Click(object sender, EventArgs e)
        {
            KeyboardState currentKey = Keyboard.GetState();

            for (int i = 0; i < currentKey.GetPressedKeys().Length; i++)
            {
                jumpKeys.Add(currentKey.GetPressedKeys()[i]);
                jumpKeys[playerNumber - 1] = currentKey.GetPressedKeys()[i];
                setJumpKeyButton.Text = jumpKeys[playerNumber - 1].ToString();
            }
        }

        private void NextPlayerButton_Click(object sender, EventArgs e)
        {
            chosenTexture.Add(characterChosen);
            game.ChangeState(new SelectionState(game, graphicsDevice, content, playerNumber + 1, chosenTexture, jumpKeys, leftKeys, rightKeys));
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            chosenTexture.Add(characterChosen);

            Console.WriteLine(currentGameMode);
            if (currentGameMode == 1)
            {
                game.ChangeState(new GameMode1State(game, graphicsDevice, content, playerNumber, chosenTexture, playerTextures, jumpKeys, leftKeys, rightKeys));
            }
            else if (currentGameMode == 2) {
                game.ChangeState(new GameMode2State(game, graphicsDevice, content, playerNumber, chosenTexture, playerTextures, jumpKeys, leftKeys, rightKeys));
            }
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            background.Draw();
            graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            spriteBatch.DrawString(bigFont, "Player " + playerNumber + ", Choose your character", new Vector2(chooseCharacter[0].Position.X - 25, chooseCharacter[0].Position.Y - 50), Color.Black);
            spriteBatch.DrawString(bigFont, "You choose: ", new Vector2(setLeftKeyButton.Position.X, screenHeight / 2), Color.Black);
            spriteBatch.Draw(playerTextures[characterChosen], new Vector2((screenWidth / 2) - 100 + playerTextures[characterChosen].Width, screenHeight / 2), null, null, Vector2.Zero, 0f, new Vector2(5f,5f), Color.White);
            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
                
            }
            
            spriteBatch.End();
        }


        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {

            for (int i = 0; i < chooseCharacter.Count; i++)
            {
                int index = i;
                chooseCharacter[i].Click += delegate (object sender, EventArgs e)
                {
                    characterChosen = index;                   
                };        
            }


            foreach (var component in components)
            {
                component.Update(gameTime);

            }
        }

      
    }
}
