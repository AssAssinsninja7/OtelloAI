using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Otello
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D otelloPieceTex;
        Texture2D lineTex;
        LineDrawer lineDrawer;

        GraphPlayingfield playingField;
        GameManager gameManager;

        //players
        Agent agentMinMax;
        HumanPlayer hPlayer;

        Rectangle whiteButtonRect;
        Rectangle blackButtonRect;

        MouseState mouseState, oldMouseState; //temp används bara för menyknapparna
        int mouseX, posY;

        bool started = false;

        enum GameState
        {
            MainMenu,
            GamePlay,
            GameOver
        }

        GameState state;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
 
        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 400;
            graphics.PreferredBackBufferWidth = 400;
            graphics.ApplyChanges();

            this.IsMouseVisible = true;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            otelloPieceTex = Content.Load<Texture2D>("OtelloTile");
            lineTex = Content.Load<Texture2D>("1x1px");

            whiteButtonRect = new Rectangle((Window.ClientBounds.Width / 2) - otelloPieceTex.Width, Window.ClientBounds.Height / 2, otelloPieceTex.Width, otelloPieceTex.Height);
            blackButtonRect = new Rectangle(Window.ClientBounds.Width / 2 , Window.ClientBounds.Height / 2, otelloPieceTex.Width, otelloPieceTex.Height);

            playingField = new GraphPlayingfield(otelloPieceTex);
            playingField.PlaceStartPositios();

            lineTex = new Texture2D(graphics.GraphicsDevice, 1, 1);
            lineDrawer = new LineDrawer(lineTex);

            agentMinMax = new Agent(playingField);
            hPlayer = new HumanPlayer(playingField);

            gameManager = new GameManager(playingField, hPlayer, agentMinMax);

           
        }
       
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Acts as the logic for the main menu so as to move on once the player has picked a color.
        /// White = start (player 1), Black = second (player 2).
        /// </summary>
        /// <returns></returns>
        private bool CheckStartSelection()//bad name
        {
            bool moveOn = false;
            mouseState = Mouse.GetState();
            mouseX = mouseState.X;
            posY = mouseState.Y;

            if (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                if (whiteButtonRect.Contains(mouseX, posY))
                {
                    hPlayer.AssignStarter(true);
                    agentMinMax.AssignStarter(false);
                    moveOn = true;
                }
                else if (blackButtonRect.Contains(mouseX, posY))
                {
                    hPlayer.AssignStarter(false);
                    agentMinMax.AssignStarter(true);
                    moveOn = true;
                }               
            }
            oldMouseState = mouseState;
            return moveOn;
        }

       
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            started = CheckStartSelection();

            base.Update(gameTime);
            switch (state)
            {
                case GameState.MainMenu:
                    if (started)
                    {
                       
                        state = GameState.GamePlay;                       
                    }
                    break;
                case GameState.GamePlay:
                    //gameManager.StartGame(true);

                    gameManager.Update();
                 
                    break;
                case GameState.GameOver:
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            base.Draw(gameTime);
            switch (state)
            {
                case GameState.MainMenu:
                    GraphicsDevice.Clear(Color.Green);
                    spriteBatch.Draw(otelloPieceTex, whiteButtonRect, Color.White);
                    spriteBatch.Draw(otelloPieceTex, blackButtonRect, Color.Black);
                    break;
                case GameState.GamePlay:
                    gameManager.Draw(spriteBatch);
                    lineDrawer.DrawGrid(spriteBatch);
                    GraphicsDevice.Clear(Color.Green);
                    break;
                case GameState.GameOver:
                    break;
                default:
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
