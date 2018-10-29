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

namespace MultiplayerPlatform.States
{
    public class MenuState : State
    {
        private List<Component> components;
        private int screenWidth;
        private int screenHeight;
        private int padding = 16;

        SpriteFont titleFont;
        SpriteFont titleFontOutline;
        SpriteBatch spriteBatch;
        private Background background;
        private Texture2D tileTexture;
        private Texture2D backgroundTexture;
        private Texture2D buttonTexture;
        

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            
            spriteBatch = new SpriteBatch(graphicsDevice);
            buttonTexture = content.Load<Texture2D>("Button");
            titleFont = content.Load<SpriteFont>("Title");
            titleFontOutline = content.Load<SpriteFont>("TitleOutline");
            var buttonFont = content.Load<SpriteFont>("Font");
            tileTexture = content.Load<Texture2D>("Tiles/TileDark");
            backgroundTexture = content.Load<Texture2D>("Tiles/Background");
            background = new Background(backgroundTexture, spriteBatch, 60, 34);

            screenWidth = graphicsDevice.Viewport.Width;
            screenHeight = graphicsDevice.Viewport.Height;
            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                
                Position = new Vector2((screenWidth / 2) - (buttonTexture.Width / 2),(screenHeight / 2) - (buttonTexture.Height / 2)),
                Text = "Start",
            };

            newGameButton.Click += NewGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((screenWidth / 2) - (buttonTexture.Width / 2), (screenHeight / 2) + (buttonTexture.Height / 2) + padding),
                Text = "Quit",
            };

            quitGameButton.Click += QuitGameButton_Click;

            components = new List<Component>() {
                newGameButton,
                quitGameButton,
            };

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            background.Draw();
            spriteBatch.DrawString(titleFont, "Flag", new Vector2(screenWidth / 2 - (titleFont.Texture.Width / 8) - 25, 2), Color.Black);           
            spriteBatch.DrawString(titleFont, "Rush", new Vector2(screenWidth / 2 - (titleFont.Texture.Width / 8) - 25, titleFont.Texture.Height / 3+2), Color.Black);
            
            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(new SelectionState(game, graphicsDevice, content, 1, new List<int>(), new List<Keys>(), new List<Keys>(), new List<Keys>(), new List<Keys>()));
        }


        public override void PostUpdate(GameTime gameTime)
        {
           
        }

        public override void Update(GameTime gameTime)
        {
            
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            game.Exit();
        }
    }
}
