using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MineTris
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        bool adjustBoxEffect = true;

        //ParticleEngine particle1;
        ParticleEngine[] particle_1 = new ParticleEngine[12];
        List<Texture2D> particleTexture = new List<Texture2D>();

        #region Variables

        // gameboard array variables
        GameBoard GameBoardField;
        GameBoard GameBoard3DEffect;
        List<Texture2D> boxTexture3D = new List<Texture2D>();
        List<Texture2D> blockTexture = new List<Texture2D>();

        // background images
        Texture2D bgTexture;
        Texture2D mainMenuTex;
        Texture2D gameOverTex;


        // Keyboard input variables
        KeyboardState oldState;
        KeyboardState constantState;
        KeyboardState oldSwitchState;

        KeyboardState leftState;
        KeyboardState rightState;
        KeyboardState downState;
        KeyboardState spaceState;

        // shape variables
        public int blockCount = 0;                           // shapes from 0 - 6
        public int rotationCount = 0;                         // rotation of the shape
        Vector2 position;                                   // temp position holder  
        Vector2 startingPosition = new Vector2(178, 48);    // starting position of the current playing piece
        Vector2 previewPosition = new Vector2(474, 152);    // position of the next piece to be played

        Shapes Shape1;                  // shape the player is controlling
        Shapes Shape2;                  // shape of the next piece
        ArrayBlock currentShapeArray;   // array for Shape1
        ArrayBlock nextShapeArray;      // array for Shape2

        Shapes Shape3;                  // shape for the preview piece
        ArrayBlock previewShapeArray;   // array for shape3

        Shapes Shape4;                  // shape for checking if rotation is possible
        ArrayBlock rotationShapeArray;  // array for shape4

        Random random;                  

        // variables
        bool gameEnded = false;             // if the top has been reached - game over
        bool movePossible = true;           // if a playing piece can move in a direction
        bool[] lineCheck = new bool[18];    // array to keep track of completed lines
        bool previewMovePossible = true;    // is the preview piece visible -- if false, it is visible
        bool rotationMovePossible = true;   // if TRUE, current shape (Shape1) can rotate

        int randomBlockStartingIndex = 3;   // random index for the starting piece of a game

        int numberOfLines = 0;              // total number of lines
        int baseNumberofLines = 10;         // number of lines before game speeds up
        int levelNumber = 0;                // used to display level
        int baseScoreNumber = 112;

        public int indexOfColor1 = 0;       // stores the index of a color
        public int indexOfColor2 = 0;       // stores the index of a color


        //FPS counter variables
        SpriteFont sprFont;
        float elapsedTime = 0.0f;   // gameplay timer
        float GameSpeed = 1000.0f;   // game speed

        float elapsedTimeFPS = 0f;  // framerate timer

        float timerRanOut = 100f;
        

        // menu variables
        bool isMainMenu;
        bool isGameOver;
        bool IsGamePlaying;
        bool isGamePaused;

        int buttonMenuSelect = 0;
        int buttonPauseSelect = 0;

        // main menu textures
        Texture2D startBtnTexOn, startBtnTexOff, optionBtnTexOn,  optionBtnTexOff, 
                    exitBtnTexOn, exitBtnTexOff, titleTex;

        // pause game textures
        Texture2D returnBtnTexOn, returnBtnTexOff, mainMenuBtnTexOff, mainMenuBtnOn, pauseMenuBg, restartBtnOff, restartBtnOn;

        //gameplay textures
        Texture2D leftBorder, rightBorder, topBorder, gameboardTexture, leftHandWallTexture, backgroundTexture;

        Texture2D box1, box2, box3, box4;

        // score and piece variables
        int gameScore = 0;
        int Lpiece = 0;
        int Jpiece = 0;
        int Tpiece = 0;
        int Ipiece = 0;
        int Zpiece = 0;
        int Spiece = 0;
        int Opiece = 0;


        // Read and Write High Score Variables
        FileStream theFileRead;
        FileStream theFileWrite;
        StreamWriter theScoreWrite;
        StreamReader theScoreRead;

        String[] textHighScore_1;
        String[] textHighScore_2;
        int maxHighScores = 5;
        bool highScoreRun = false;

        String textHighScore;

        Vector2 textHighScorePos_1;
        Vector2 textHighScorePos_2;
        Vector2 textHighScorePos_3;
        Vector2 textHighScorePos_4;
        Vector2 textHighScorePos_5;


        // sounds
        public SoundEffect soundRotate;
        public SoundEffect soundDrop;
        public SoundEffect soundSweep;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 680;
            IsMouseVisible = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            #region KeyBoard States
            // Initialize keyboard states for player input
            oldState = Keyboard.GetState();
            constantState = Keyboard.GetState();
            oldSwitchState = Keyboard.GetState();

            leftState = Keyboard.GetState();
            rightState = Keyboard.GetState();
            downState = Keyboard.GetState();
            spaceState = Keyboard.GetState();
            #endregion

            currentShapeArray = new ArrayBlock(startingPosition);
            nextShapeArray = new ArrayBlock(previewPosition);
            previewShapeArray = new ArrayBlock(new Vector2(256, 41));
            rotationShapeArray = new ArrayBlock(startingPosition);

            textHighScore_1 = new String[maxHighScores];
            textHighScore_2 = new String[maxHighScores];

            textHighScore = "High Scores";

            textHighScorePos_1 = new Vector2(330, 310);
            textHighScorePos_2 = new Vector2(330, 330);
            textHighScorePos_3 = new Vector2(330, 350);
            textHighScorePos_4 = new Vector2(330, 370);
            textHighScorePos_5 = new Vector2(330, 390);

            

            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            #region MainMenu Textures
            startBtnTexOff = Content.Load<Texture2D>("mainMenu/startButtonOff");
            startBtnTexOn = Content.Load<Texture2D>("mainMenu/startButtonOn");
            optionBtnTexOff = Content.Load<Texture2D>("mainMenu/optionsButtonOff");
            optionBtnTexOn = Content.Load<Texture2D>("mainMenu/optionsButtonOn");
            exitBtnTexOff = Content.Load<Texture2D>("mainMenu/exitButtonOff");
            exitBtnTexOn = Content.Load<Texture2D>("mainMenu/exitButtonOn");

            titleTex = Content.Load<Texture2D>("mainGame/topBorderTex");
            
            #endregion

            #region PauseMenu Textures
            returnBtnTexOff = Content.Load<Texture2D>("pauseGame/returnButtonOff");
            returnBtnTexOn = Content.Load<Texture2D>("pauseGame/returnButtonOn");
            mainMenuBtnTexOff = Content.Load<Texture2D>("pauseGame/mainMenuButtonOff");
            mainMenuBtnOn = Content.Load<Texture2D>("pauseGame/mainMenuButtonOn");
            pauseMenuBg = Content.Load<Texture2D>("pauseGame/pausedGameTex");
            restartBtnOff = Content.Load<Texture2D>("pauseGame/restartButtonOff");
            restartBtnOn = Content.Load<Texture2D>("pauseGame/restartButtonOn");

            #endregion

            #region GamePlay Textures
            blockTexture.Add(Content.Load<Texture2D>("mainGame/blue"));     // 0
            blockTexture.Add(Content.Load<Texture2D>("mainGame/green"));    // 1
            blockTexture.Add(Content.Load<Texture2D>("mainGame/purple"));   // 2
            blockTexture.Add(Content.Load<Texture2D>("mainGame/red"));      // 3
            blockTexture.Add(Content.Load<Texture2D>("mainGame/yellow"));   // 4
            blockTexture.Add(Content.Load<Texture2D>("mainGame/gray"));     // 5
            
            leftBorder = Content.Load<Texture2D>("mainGame/leftBorderTex");
            rightBorder = Content.Load<Texture2D>("mainGame/rightBorderTex");
            topBorder = Content.Load<Texture2D>("mainGame/topBorderTex");

            gameboardTexture = Content.Load<Texture2D>("mainGame/frameOutline2");
            leftHandWallTexture = Content.Load<Texture2D>("mainGame/wallFrame2");
            backgroundTexture = Content.Load<Texture2D>("mainGame/backgroundTetris");

            box1 = Content.Load<Texture2D>("mainGame/3dBoxEffect");
            box2 = Content.Load<Texture2D>("mainGame/3dBoxEffect");
            box3 = Content.Load<Texture2D>("mainGame/3dBoxEffect");
            box4 = Content.Load<Texture2D>("mainGame/3dBoxEffect");

            boxTexture3D.Add(Content.Load<Texture2D>("mainGame/3DBoxEffect"));

            sprFont = Content.Load<SpriteFont>("mainGame/FPSCounter");
            #endregion

            #region Temp Textures
            bgTexture = Content.Load<Texture2D>("mainGame/layout background");
            mainMenuTex = Content.Load<Texture2D>("mainMenu/mainMenu");
            gameOverTex = Content.Load<Texture2D>("mainGame/gameoverScreen");
            #endregion

            GameBoardField = new GameBoard(blockTexture);
            GameBoard3DEffect = new GameBoard(boxTexture3D);
            GameBoard3DEffect.StartingPos = new Vector2(50, 64);
            Shape1 = new Shapes(currentShapeArray.Block, blockTexture );
            Shape2 = new Shapes(nextShapeArray.Block, blockTexture);
            Shape3 = new Shapes(previewShapeArray.Block, blockTexture);
            Shape4 = new Shapes(rotationShapeArray.Block, blockTexture);

            Shape1.LoadContent(Content);
            Shape2.LoadContent(Content);
            Shape3.LoadContent(Content);
            Shape4.LoadContent(Content);

            particleTexture.Add(Content.Load<Texture2D>("mainGame/smoke"));
            ParticleInitialize();
            
            isMainMenu = true;
            isGameOver = false;
            IsGamePlaying = false;
            isGamePaused = false;

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // randomize the starting blocks of the game
            #region StartingBlocks Randomizer
            if (randomBlockStartingIndex > 0)
            {
                ChangePlayingPiece();
                randomBlockStartingIndex--;
            }
            #endregion

            // managing gamestates
            #region GameState Management
            
            if (isMainMenu)
            {
                if (timerRanOut >= 0)
                {
                    timerRanOut -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                UpdateMainMenu();
            }
            else if (IsGamePlaying)
            {
                #region Timer
                // time has passed (1 sec)
                elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                elapsedTimeFPS += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (elapsedTimeFPS >= 1000.0f)
                {
                    //fps = totalFrame;
                    //totalFrame = 0;
                    elapsedTimeFPS = 0f;
                }
                if (elapsedTime >= GameSpeed)
                {
                    elapsedTime = 0f;
                }
                
                #endregion
                if (adjustBoxEffect == true)
                {
                    Move3DGameBoardPosition();
                    adjustBoxEffect = false;
                }

                UpdateGamePlaying();
                
            }
            else if (isGameOver)
            {
                UpdateGameOver();
            }
            else if (isGamePaused)
            {
                UpdateGamePaused();
            }
            #endregion
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            #region GameState Draw
            if (isMainMenu)
            {
                DrawMainMenu();
            }
            else if (IsGamePlaying)
            {
                DrawGamePlaying();
            }
            else if (isGameOver)
            {
                DrawGameOver();
            }
            else if (isGamePaused)
            {
                DrawGamePused();
            }
            #endregion

            //#region FPS Counter
            //spriteBatch.DrawString(sprFont, string.Format("FPS= " + (int)(1000/gameTime.ElapsedGameTime.TotalMilliseconds)), new Vector2(10.0f, 20.0f), Color.White);
            //#endregion

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region  Game Play Processes

        // player input class
        public void PlayerInput()
        {
            // variables for keyboard input
            KeyboardState newState = Keyboard.GetState();
            KeyboardState switchState = Keyboard.GetState();
            KeyboardState leftMoveState = Keyboard.GetState();
            KeyboardState rightMoveState = Keyboard.GetState();
            KeyboardState downMoveState = Keyboard.GetState();
            KeyboardState spaceSetState = Keyboard.GetState();

            /*
             * keyboard input for rotating shape
             */
            #region UpKey (rotation)
            if (newState.IsKeyDown(Keys.Up))
            {
                if (!oldState.IsKeyDown(Keys.Up))
                {
                    RotationMovePossible();
                    if (rotationMovePossible)
                    {
                        Shape1.rotationCount++;
                        Shape3.blockCount = Shape1.blockCount;
                        Shape3.rotationCount = Shape1.rotationCount;

                        for (int row = 0; row < 4; row++)
                        {
                            for (int col = 0; col < 4; col++)
                            {
                                previewShapeArray.Block[row, col] = currentShapeArray.Block[row, col];
                            }
                        }
                    }

                }
                else if (oldState.IsKeyDown(Keys.Up))
                {
                    
                }
                oldState = newState;
            }

            if (newState.IsKeyUp(Keys.Up))
            {
                rotationMovePossible = true;
                previewMovePossible = true;
                oldState = constantState;
            }
            #endregion

            /*
             * keyboard input for changing shape
             */
            #region N Key (new piece)
            if (switchState.IsKeyDown(Keys.N))
            {
                if (!oldSwitchState.IsKeyDown(Keys.N))
                {
                    ChangePlayingPiece();
                }
                else if (oldSwitchState.IsKeyDown(Keys.N))
                {
                }
                oldSwitchState = switchState;
            }
            if (switchState.IsKeyUp(Keys.N))
            {
                oldSwitchState = constantState;
            }
            #endregion

            /*
             * Keyboard Left movement
             */
            #region LeftKey
            if (leftMoveState.IsKeyDown(Keys.Left))
            {
                if (!leftState.IsKeyDown(Keys.Left))
                {
                    LeftMovePossible();
                    if (movePossible)
                    {
                        if ((Shape1.posBlock1.X > 50 && Shape1.posBlock2.X > 50 && Shape1.posBlock3.X > 50 && Shape1.posBlock4.X > 50))
                        {
                            for (int row = 0; row < 4; row++)
                            {
                                for (int col = 0; col < 4; col++)
                                {
                                    currentShapeArray.Block[row, col] = new Vector2(currentShapeArray.Block[row, col].X - 32, currentShapeArray.Block[row, col].Y);
                                    previewShapeArray.Block[row, col] = currentShapeArray.Block[row, col];

                                }
                            }
                        }
                    }
                }
                else if (leftState.IsKeyDown(Keys.Left))
                {
                }
                leftState = leftMoveState;
            }
            if (leftMoveState.IsKeyUp(Keys.Left))
            {
                previewMovePossible = true;
                leftState = constantState;
                movePossible = true;
            }
            #endregion

            /*
             * Keyboard Right movement
             */
            #region RightKey
            if (rightMoveState.IsKeyDown(Keys.Right))
            {
                if (!rightState.IsKeyDown(Keys.Right))
                {
                    RightMovePossible();
                    if (movePossible)
                    {
                        if ((Shape1.posBlock1.X < 384 && Shape1.posBlock2.X < 384 && Shape1.posBlock3.X < 384 && Shape1.posBlock4.X < 384))
                        {
                            for (int row = 0; row < 4; row++)
                            {
                                for (int col = 0; col < 4; col++)
                                {
                                    currentShapeArray.Block[row, col] = new Vector2(currentShapeArray.Block[row, col].X + 32, currentShapeArray.Block[row, col].Y);
                                    previewShapeArray.Block[row, col] = currentShapeArray.Block[row, col];
                                }
                            }
                        }
                    }
                }
                else if (rightState.IsKeyDown(Keys.Right))
                {
                }
                rightState = rightMoveState;
            }
            if (rightMoveState.IsKeyUp(Keys.Right))
            {
                previewMovePossible = true;
                rightState = constantState;
                movePossible = true;
            }
            #endregion

            /*
             *  Keyboard single Down Movement
             */
            #region DownKey
            if (downMoveState.IsKeyDown(Keys.Down))
            {
                if (!downState.IsKeyDown(Keys.Down))
                {
                    elapsedTime = 1;
                    DownMovePossible();
                    if (movePossible)
                    {
                        if (Shape1.posBlock1.Y + 32 <= 624 && Shape1.posBlock2.Y + 32 <= 624 && Shape1.posBlock3.Y + 32 <= 624 && Shape1.posBlock4.Y + 32 <= 624)
                        {
                            for (int row = 0; row < 4; row++)
                            {
                                for (int col = 0; col < 4; col++)
                                {
                                    currentShapeArray.Block[row, col] = new Vector2(currentShapeArray.Block[row, col].X, currentShapeArray.Block[row, col].Y + 32);
                                }
                            }
                        }
                    }
                    else if (!movePossible)
                    {
                        //SetPieceIntoPlace();
                        //ChangePlayingPiece();
                        movePossible = true;
                    }
                    if (Shape1.posBlock1.Y + 32 > 624 || Shape1.posBlock2.Y + 32 > 624 || Shape1.posBlock3.Y + 32 > 624 || Shape1.posBlock4.Y + 32 > 624)
                    {
                        //SetPieceIntoPlace();
                        //ChangePlayingPiece();
                        movePossible = true;
                    }
                }
                else if (downState.IsKeyDown(Keys.Down))
                {
                }
                downState = downMoveState;
            }
            if (downMoveState.IsKeyUp(Keys.Down))
            {
                downState = constantState;
                movePossible = true;
            }
            #endregion

            /*
             *  Keyboard Set Piece Input
             */
            #region SpaceKey
            if (spaceSetState.IsKeyDown(Keys.Space))
            {
                if (!spaceState.IsKeyDown(Keys.Space))
                {
                    for (int row = 0; row < 4; row++)
                    {
                        for (int col = 0; col < 4; col++)
                        {
                            currentShapeArray.Block[row, col] = previewShapeArray.Block[row, col];
                        }
                    }

                }
                else if (spaceState.IsKeyDown(Keys.Space))
                {
                }
                spaceState = spaceSetState;
            }
            if (spaceSetState.IsKeyUp(Keys.Space))
            {
                spaceState = constantState;
            }
            #endregion
        }
        
        public void LeftMovePossible()
        {
            Vector2 leftConstant = new Vector2(32, 0);

            for (int x = 0; x < GameBoardField.blocks.Count; x++)
            {
                if (GameBoardField.blocks[x].isActive == true)
                {
                    if (GameBoardField.blocks[x].Position + leftConstant == Shape1.posBlock1)
                    {
                        movePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position + leftConstant == Shape1.posBlock2)
                    {
                        movePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position + leftConstant == Shape1.posBlock3)
                    {
                        movePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position + leftConstant == Shape1.posBlock4)
                    {
                        movePossible = false;
                    }
                }
            }

        }

        public void RightMovePossible()
        {
            Vector2 rightConstant = new Vector2(-32, 0);
            for (int x = 0; x < GameBoardField.blocks.Count; x++)
            {
                if (GameBoardField.blocks[x].isActive == true)
                {
                    if (GameBoardField.blocks[x].Position + rightConstant == Shape1.posBlock1)
                    {
                        movePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position + rightConstant == Shape1.posBlock2)
                    {
                        movePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position + rightConstant == Shape1.posBlock3)
                    {
                        movePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position + rightConstant == Shape1.posBlock4)
                    {
                        movePossible = false;
                    }
                }
            }

        }

        public void DownMovePossible()
        {
            Vector2 downConstant = new Vector2(0, -32);

            for (int x = 0; x < GameBoardField.blocks.Count; x++)
            {
                if (GameBoardField.blocks[x].isActive == true)
                {
                    if (GameBoardField.blocks[x].Position + downConstant == Shape1.posBlock1)
                    {
                        movePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position + downConstant == Shape1.posBlock2)
                    {
                        movePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position + downConstant == Shape1.posBlock3)
                    {
                        movePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position + downConstant == Shape1.posBlock4)
                    {
                        movePossible = false;
                    }
                }
            }

        }

        // checks to see if Shape1 can rotate
        public void RotationMovePossible()
        {
            Shape4.blockCount = Shape1.blockCount;
            Shape4.rotationCount = Shape1.rotationCount;

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    rotationShapeArray.Block[row, col] = currentShapeArray.Block[row, col];

                }
            }
            Shape4.rotationCount++;
            if (Shape4.rotationCount == 4)
            {
                Shape4.rotationCount = 0;
            }

            if(Shape4.blockCount == 2 || Shape4.blockCount == 3)
            {
                if (Shape4.rotationCount == 2)
                {
                    Shape4.rotationCount = 0;
                }
            }

            if (Shape4.posBlock1.X < 50 || Shape4.posBlock1.X > 402 || Shape4.posBlock1.Y + 32 > 624)
            {
                rotationMovePossible = false;
            }
            if (Shape4.posBlock2.X < 50 || Shape4.posBlock2.X > 402 || Shape4.posBlock2.Y + 32 > 624)
            {
                rotationMovePossible = false;
            }
            if (Shape4.posBlock3.X < 50 || Shape4.posBlock3.X > 402 || Shape4.posBlock3.Y + 32 > 624)
            {
                rotationMovePossible = false;
            }
            if (Shape4.posBlock4.X < 50 || Shape4.posBlock4.X > 402 || Shape4.posBlock4.Y + 32 > 624)
            {
                rotationMovePossible = false;
            }

            for (int x = 0; x < GameBoardField.blocks.Count; x++)
            {
                if (GameBoardField.blocks[x].isActive == true)
                {
                    if (GameBoardField.blocks[x].Position == Shape4.posBlock1)
                    {
                        rotationMovePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position == Shape4.posBlock2)
                    {
                        rotationMovePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position == Shape4.posBlock3)
                    {
                        rotationMovePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position == Shape4.posBlock4)
                    {
                        rotationMovePossible = false;
                    }
                }
            }

        }

        // as time runs, current shape is moving down
        public void TimerPieceMovement()
        {
            Vector2 downConstant = new Vector2(0, -32);
            bool itStoppped = false;
            
            if (elapsedTime == 0)
            {
                for (int x = 0; x < GameBoardField.blocks.Count; x++)
                {
                    if (GameBoardField.blocks[x].isActive == true)
                    {
                        if (GameBoardField.blocks[x].Position + downConstant == Shape1.posBlock1)
                        {
                            movePossible = false;
                        }
                        else if (GameBoardField.blocks[x].Position + downConstant == Shape1.posBlock2)
                        {
                            movePossible = false;
                        }
                        else if (GameBoardField.blocks[x].Position + downConstant == Shape1.posBlock3)
                        {
                            movePossible = false;
                        }
                        else if (GameBoardField.blocks[x].Position + downConstant == Shape1.posBlock4)
                        {
                            movePossible = false;
                        }
                    }
                }

                if (movePossible)
                {
                    if (Shape1.posBlock1.Y + 32 <= 624 && Shape1.posBlock2.Y + 32 <= 624 && Shape1.posBlock3.Y + 32 <= 624 && Shape1.posBlock4.Y + 32 <= 624)
                    {
                        for (int row = 0; row < 4; row++)
                        {
                            for (int col = 0; col < 4; col++)
                            {
                                currentShapeArray.Block[row, col] = new Vector2(currentShapeArray.Block[row, col].X, currentShapeArray.Block[row, col].Y + 32);
                            }
                        }
                    }
                }
                else if ((!movePossible && elapsedTime == 0))
                {
                    itStoppped = true;
                    //SetPieceIntoPlace();
                    //ChangePlayingPiece();
                }

                if (Shape1.posBlock1.Y + 32 > 624 || Shape1.posBlock2.Y + 32 > 624 || Shape1.posBlock3.Y + 32 > 624 || Shape1.posBlock4.Y + 32 > 624)
                {
                    itStoppped = true;
                    //SetPieceIntoPlace();
                    //ChangePlayingPiece();
                    //movePossible = true;
                }
            }

            if (itStoppped == true)
            {
                SetPieceIntoPlace();
                
                movePossible = true;
            }

            
        }

        // checks to see if the player has any moves left
        // if the top has been reached and current shape cannot move down
        public void GameOverCheck()
        {
            Vector2 downConstant = new Vector2(0, -32);

            for (int x = 0; x < 12; x++)
            {
                if (GameBoardField.blocks[x].isActive == true)
                {
                    if (GameBoardField.blocks[x].Position == Shape1.posBlock1)
                    {
                        gameEnded = true;

                    }
                    else if (GameBoardField.blocks[x].Position == Shape1.posBlock2)
                    {
                        gameEnded = true;
                    }
                    else if (GameBoardField.blocks[x].Position == Shape1.posBlock3)
                    {
                        gameEnded = true;
                    }
                    else if (GameBoardField.blocks[x].Position == Shape1.posBlock4)
                    {
                        gameEnded = true;
                    }
                }
            }
        }

        // updates the current shape with the new shape to be played
        public void ChangePlayingPiece()
        {                             
            position = startingPosition;
            int previousNumber;
            previousNumber = Shape2.blockCount;

            ShapeRandomizer();

            Shape1.rotationCount = 0;
            Shape1.blockCount = previousNumber;
            Shape3.blockCount = Shape1.blockCount;
            Shape3.rotationCount = Shape1.rotationCount;
            previewShapeArray.position = startingPosition;
            PrevColorOfBlock();
            PreviewDropPiece();

            movePossible = true;
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    currentShapeArray.Block[row, col] = new Vector2(position.X, position.Y);
                    previewShapeArray.Block[row, col] = currentShapeArray.Block[row, col];
                    position.X += 32;
                }
                position.X -= (4 * 32);
                position.Y += 32;
            }
            NewColorOfBlock();
            
        }

        // randomizes the next shape to be played
        public void ShapeRandomizer()
        {
            random = new Random();
            int number = random.Next(0, 21);

            if(number >= 0 && number < 3)
            {
                number = 1;
                Shape2.blockCount = number;
            }
            else if(number >= 3 && number < 6)
            {
                number = 3;
                Shape2.blockCount = number;
            }
            else if(number >= 6 && number < 9)
            {
                number = 6;
                Shape2.blockCount = number;
            }
            else if(number >= 9 && number < 12)
            {
                number = 4;
                Shape2.blockCount = number;
            }
            else if(number >= 12 && number < 15)
            {
                number = 0;
                Shape2.blockCount = number;
            }
            else if(number >= 15 && number < 18)
            {
                number = 5;
                Shape2.blockCount = number;
            }
            else if(number >= 18 && number < 21)
            {
                number = 2;
                Shape2.blockCount = number;
            }
            
        }
      
        // updates the gameboard with the current shapes position
        // reacts to gameover
        public void SetPieceIntoPlace()
        {
            CountingThePieces();

            for (int x = 0; x < GameBoardField.blocks.Count; x++)
            {
                if ((Shape1.posBlock1 == GameBoardField.blocks[x].Position) || (Shape1.posBlock2 == GameBoardField.blocks[x].Position)
                    || (Shape1.posBlock3 == GameBoardField.blocks[x].Position) || (Shape1.posBlock4 == GameBoardField.blocks[x].Position))
                {
                    GameBoardField.blocks[x].Texture = Shape1.block1;
                    GameBoardField.blocks[x].isActive = true;
                    GameBoard3DEffect.blocks[x].Texture = box1;
                    GameBoard3DEffect.blocks[x].isActive = true;
                }
            }

            GameOverCheck();

            if (gameEnded == true)
            {
                isMainMenu = false;
                IsGamePlaying = false;
                isGameOver = true;
                buttonPauseSelect = 0;
            }
            else if (gameEnded == false)
            {
                ChangePlayingPiece();
                CheckForLines();
                LineComplete();
            }     
            
        }

        // checks if a line is filled
        public void CheckForLines()
        {
            bool[,] lineArray = new bool[18,12];
            
            int n = 0;

            for (int y = 0; y < 18; y++)
            {
                for (int x = 0; x < 12; x++)
                {
                    if (GameBoardField.blocks[n].isActive == true)
                    {
                        lineArray[y, x] = true;
                    }
                    else
                    {
                        lineArray[y, x] = false;
                    }
                    n++;
                }
            }

            for (int a = 17; a >= 0; a--)
            {
                if (lineArray[a, 0] == true && lineArray[a, 1] == true && lineArray[a, 2] == true && lineArray[a, 3] == true && lineArray[a, 4] == true && lineArray[a, 5] == true && lineArray[a, 6] == true
                     && lineArray[a, 7] == true && lineArray[a, 8] == true && lineArray[a, 9] == true && lineArray[a, 10] == true && lineArray[a, 11] == true)
                {
                    lineCheck[a] = true;
                }
                else
                {
                    lineCheck[a] = false;
                }
            }
        }

        // destroys a completed line
        // updates the gameboard by moving any blocks above the destroyed line
        public void LineComplete()
        {
            bool lineComplete = false;

            for (int m = 0; m < 4; m++)
            {
                for (int d = 0; d < 18; d++)
                {
                    int p = 12 * d;

                    if (d <= 14 && lineCheck[d] == true && lineCheck[d + 1] == true && lineCheck[d + 2] == true && lineCheck[d + 3] == true)
                    {
                        gameScore += 400;
                    }

                    if (lineCheck[d] == true)
                    {
                        lineComplete = true;
                        for (int c = 0; c < 12; c++)
                        {
                            GameBoardField.blocks[p].isActive = false;
                            GameBoard3DEffect.blocks[p].isActive = false;

                            particle_1[c].EmitterLocation = new Vector2(GameBoardField.blocks[p].Position.X + 16, GameBoardField.blocks[p].Position.Y + 16);

                            p++;
                        }

                        if (lineComplete == true)
                        {
                            int g = (d - 1);
                            int nextRow = 12;
                            bool temp1;
                            Texture2D temTex;
                            
                            for (int z = g; z >= 0; z--)
                            {
                                int k = z * 12;

                                for (int f = 0; f < 12; f++)
                                {
                                    temp1 = GameBoardField.blocks[(k + f)].isActive;
                                    temTex = GameBoardField.blocks[(k + f)].Texture;
                                    GameBoardField.blocks[(k + f)].isActive = false;
                                    GameBoard3DEffect.blocks[k + f].isActive = false;
                                    GameBoardField.blocks[(k + f + nextRow)].isActive = temp1;
                                    GameBoard3DEffect.blocks[k + f + nextRow].isActive = temp1;
                                    GameBoardField.blocks[(k + f + nextRow)].Texture = temTex;                                    
                            
                                }

                            }

                            for (int h = 0; h < 12; h++)
                            {
                                particle_1[h].Active = true;
                            }

                            numberOfLines++;
                            lineComplete = false;
                            baseNumberofLines--;
                            GameSpeedAdjust();

                            if (levelNumber != 0)
                            {
                                gameScore += baseScoreNumber * (levelNumber + 1) ;
                            }
                            else
                            {
                                gameScore += 100;
                            }


                        }
                        lineCheck[d] = false;

                    }
                }
            }

        }

        // next shapes color
        public void PrevColorOfBlock()
        {
            random = new Random();
            int num = random.Next(0, 16);
            
            indexOfColor1 = indexOfColor2;
            NewColorOfBlock();
            

            if (num >= 0 && num < 3)
            {
                num = 0;
                Shape2.block1 = Shape2.TextureList[num];
                Shape2.block2 = Shape2.TextureList[num];
                Shape2.block3 = Shape2.TextureList[num];
                Shape2.block4 = Shape2.TextureList[num];
                indexOfColor2 = 0;
            }
            if (num >= 3 && num < 7)
            {
                num = 1;
                Shape2.block1 = Shape2.TextureList[num];
                Shape2.block2 = Shape2.TextureList[num];
                Shape2.block3 = Shape2.TextureList[num];
                Shape2.block4 = Shape2.TextureList[num];
                indexOfColor2 = 1;
            }
            if (num >= 7 && num < 10)
            {
                num = 2;
                Shape2.block1 = Shape2.TextureList[num];
                Shape2.block2 = Shape2.TextureList[num];
                Shape2.block3 = Shape2.TextureList[num];
                Shape2.block4 = Shape2.TextureList[num];
                indexOfColor2 = 2;
            }
            if (num >= 10 && num < 13)
            {
                num = 3;
                Shape2.block1 = Shape2.TextureList[num];
                Shape2.block2 = Shape2.TextureList[num];
                Shape2.block3 = Shape2.TextureList[num];
                Shape2.block4 = Shape2.TextureList[num];
                indexOfColor2 = 3;
            }
            if (num >= 13 && num < 16)
            {
                num = 4;
                Shape2.block1 = Shape2.TextureList[num];
                Shape2.block2 = Shape2.TextureList[num];
                Shape2.block3 = Shape2.TextureList[num];
                Shape2.block4 = Shape2.TextureList[num];
                indexOfColor2 = 4;
            }
           // NewColorOfBlock();
            
        }

        // setting the new shapes color
        public void NewColorOfBlock()
        {
            int c = indexOfColor1;

            Shape1.block1 = Shape1.TextureList[c];
            Shape1.block2 = Shape1.TextureList[c];
            Shape1.block3 = Shape1.TextureList[c];
            Shape1.block4 = Shape1.TextureList[c];

            Shape3.alphaColor = .6f;

            Shape3.block1 = blockTexture[5];
            Shape3.block2 = blockTexture[5];
            Shape3.block3 = blockTexture[5];
            Shape3.block4 = blockTexture[5];

        }

        // shows where the current shape will eventually stop
        public void PreviewDropPiece()
        {
            Vector2 lowerBlockMove = new Vector2(0, 32);

            Vector2 downConstant = new Vector2(0, -32);

            if (Shape3.posBlock1.Y + 32 > 624 || Shape3.posBlock2.Y + 32 > 624 || Shape3.posBlock3.Y + 32 > 624 || Shape3.posBlock4.Y + 32 > 624)
            {
                previewMovePossible = false;
            }

            for (int x = 0; x < GameBoardField.blocks.Count; x++)
            {
                if (GameBoardField.blocks[x].isActive == true)
                {
                    if (GameBoardField.blocks[x].Position + downConstant == Shape3.posBlock1)
                    {
                        previewMovePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position + downConstant == Shape3.posBlock2)
                    {
                        previewMovePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position + downConstant == Shape3.posBlock3)
                    {
                        previewMovePossible = false;
                    }
                    else if (GameBoardField.blocks[x].Position + downConstant == Shape3.posBlock4)
                    {
                        previewMovePossible = false;
                    }
                }
            }

            if (previewMovePossible == true)
            {
                if (Shape3.posBlock1.Y + 32 <= 624 && Shape3.posBlock2.Y + 32 <= 624 && Shape3.posBlock3.Y + 32 <= 624 && Shape3.posBlock4.Y + 32 <= 624)
                {
                    for (int row = 0; row < 4; row++)
                    {
                        for (int col = 0; col < 4; col++)
                        {
                            previewShapeArray.Block[row, col] = new Vector2(previewShapeArray.Block[row, col].X, previewShapeArray.Block[row, col].Y + 32);
                        }
                    }
                }
            }   
        }

        // increases the speed of the falling blocks every time 10 lines are completed
        public void GameSpeedAdjust()
        {
            if (baseNumberofLines == 0 && GameSpeed > 300)
            {
                GameSpeed -= 100;
                
            }
            if (baseNumberofLines == 0 && GameSpeed <= 300 & GameSpeed > 150)
            {
                GameSpeed -= 30;
            }
            if (baseNumberofLines == 0)
            {
                levelNumber++;
                baseNumberofLines = 10;
            }
   
        }

        // counts the number of shapes played
        public void CountingThePieces()
        {
            // L shape
            if (Shape1.blockCount == 0)
            {
                Lpiece++;
            }
            // T shape
            if (Shape1.blockCount == 1)
            {
                Tpiece++;
            }
            // Z shape
            if (Shape1.blockCount == 2)
            {
                Zpiece++;
            }
            // S shape
            if (Shape1.blockCount == 3)
            {
                Spiece++;
            }
            // J shape
            if (Shape1.blockCount == 4)
            {
                Jpiece++;
            }
            // I shape
            if (Shape1.blockCount == 5)
            {
                Ipiece++;
            }
            // O shape
            if (Shape1.blockCount == 6)
            {
                Opiece++;
            }
        }

        public void Move3DGameBoardPosition()
        {
            for (int i = 0; i < GameBoard3DEffect.blocks.Count; i++)
            {
                GameBoard3DEffect.blocks[i].Position -= new Vector2 (0, -16);
            }
        }

        public void BoxEffect()
        {
            spriteBatch.Draw(box1, new Vector2(Shape1.posBlock1.X, Shape1.posBlock1.Y - 16), Color.White);
            spriteBatch.Draw(box2, new Vector2(Shape1.posBlock2.X, Shape1.posBlock2.Y - 16), Color.White);
            spriteBatch.Draw(box3, new Vector2(Shape1.posBlock3.X, Shape1.posBlock3.Y - 16), Color.White);
            spriteBatch.Draw(box4, new Vector2(Shape1.posBlock4.X, Shape1.posBlock4.Y - 16), Color.White);
        }

        public void ResetGameBoard3DEffect()
        {
            for (int i = 0; i < GameBoard3DEffect.blocks.Count; i++)
            {
                GameBoard3DEffect.blocks[i].isActive = false;
            }
        }

        // reads and writes High Score to a text file
        private void HighScore()
        {
            Boolean boolWorkingFileIO = true;

            try
            {
                theFileRead = new FileStream("HighScores.txt", FileMode.OpenOrCreate, FileAccess.Read);

                theScoreRead = new StreamReader(theFileRead);

                for (int i = 0; i < maxHighScores; i++)
                {
                    textHighScore_1[i] = theScoreRead.ReadLine();

                    if (textHighScore_1[i] == null)
                    {
                        textHighScore_1[i] = "0";
                    }
                }

                theScoreRead.Close();
                theFileRead.Close();
            }
            catch
            {
                boolWorkingFileIO = false;
            }

            if (boolWorkingFileIO)
            {
                int j = 0;

                for (int i = 0; i < maxHighScores; i++)
                {
                    if (gameScore > Convert.ToInt32(textHighScore_1[i]) && i == j)
                    {
                        textHighScore_2[i] = gameScore.ToString();
                        i++;

                        if (i < maxHighScores)
                        {
                            textHighScore_2[i] = textHighScore_1[j];
                        }
                    }
                    else
                    {
                        textHighScore_2[i] = textHighScore_1[j];
                    }
                    j++;
                }
            }

            try
            {
                theFileWrite = new FileStream("HighScores.txt", FileMode.Create, FileAccess.Write);

                theScoreWrite = new StreamWriter(theFileWrite);

                for (int i = 0; i < maxHighScores; i++)
                {
                    theScoreWrite.WriteLine(textHighScore_2[i]);
                }

                theScoreWrite.Close();
                theFileWrite.Close();
            }
            catch
            {
                boolWorkingFileIO = false;
            }

            highScoreRun = true;
        }

        public void ParticleInitialize()
        {
            for (int i = 0; i < 12; i++)
            {
                particle_1[i] = new ParticleEngine(particleTexture, new Vector2(0,0), 2);
            }
        }

        #endregion

        #region Main Menu Processes

        // controls for selecting main menu buttons
        public void MainMenuButtonControl()
        {
            KeyboardState newState = Keyboard.GetState();
            KeyboardState downMoveState = Keyboard.GetState();
            KeyboardState enterKeyState = Keyboard.GetState();

            #region UpKey
            if (newState.IsKeyDown(Keys.Up))
            {
                if (!oldState.IsKeyDown(Keys.Up))
                {
                    buttonMenuSelect--;
                    if (buttonMenuSelect == -1)
                    {
                        buttonMenuSelect = 2;
                    }
                }
                else if (oldState.IsKeyDown(Keys.Up))
                {
                }
                oldState = newState;
            }

            if (newState.IsKeyUp(Keys.Up))
            {
                oldState = constantState;
            }
            #endregion

            #region DownKey
            if (downMoveState.IsKeyDown(Keys.Down))
            {
                if (!spaceState.IsKeyDown(Keys.Down))
                {
                    buttonMenuSelect++;
                    if (buttonMenuSelect == 3)
                    {
                        buttonMenuSelect = 0;
                    }
                }
                else if (spaceState.IsKeyDown(Keys.Down))
                {
                }
                spaceState = downMoveState;
            }
            if (downMoveState.IsKeyUp(Keys.Down))
            {
                spaceState = constantState;
            }
            #endregion

            #region Enter Key (Selecting)

            if (timerRanOut <= 0)
            {
                if (enterKeyState.IsKeyDown(Keys.Enter))
                {
                    if (!downState.IsKeyDown(Keys.Enter))
                    {
                        if (buttonMenuSelect == 0)
                        {
                            ResetGameBoard3DEffect();
                            GameBoardField = new GameBoard(blockTexture);
                            gameEnded = false;
                            randomBlockStartingIndex = 3;
                            numberOfLines = 0;
                            levelNumber = 0;
                            baseNumberofLines = 10;
                            GameSpeed = 1000f;
                            timerRanOut = 100f;
                            

                            gameScore = 0;
                            Lpiece = 0;
                            Jpiece = 0;
                            Tpiece = 0;
                            Ipiece = 0;
                            Zpiece = 0;
                            Spiece = 0;
                            Opiece = 0;

                            highScoreRun = false;

                            isMainMenu = false;
                            isGameOver = false;
                            IsGamePlaying = true;
                            isGamePaused = false;
                        }
                        if (buttonMenuSelect == 1)
                        {
                        }
                        if (buttonMenuSelect == 2)
                        {
                            Exit();
                        }
                    }
                    else if (downState.IsKeyDown(Keys.Enter))
                    {
                    }
                    downState = enterKeyState;
                }
                if (enterKeyState.IsKeyUp(Keys.Enter))
                {
                    downState = constantState;
                }

            #endregion
            }
        }

        // settings for drawing the main menu screen
        public void MainMenuButtonDraw()
        {
            Vector2 buttonPosition1 = new Vector2(170,330);
            Vector2 buttonPosition2 = new Vector2(170,440);
            Vector2 buttonPosition3 = new Vector2(170,550);

            if (buttonMenuSelect == 0)
            {
                spriteBatch.Draw(startBtnTexOn, buttonPosition1, Color.White);
                spriteBatch.Draw(optionBtnTexOff, buttonPosition2, Color.White);
                spriteBatch.Draw(exitBtnTexOff, buttonPosition3, Color.White);
            }
            else if (buttonMenuSelect == 1)
            {
                spriteBatch.Draw(startBtnTexOff, buttonPosition1, Color.White);
                spriteBatch.Draw(optionBtnTexOn, buttonPosition2, Color.White);
                spriteBatch.Draw(exitBtnTexOff, buttonPosition3, Color.White);
            }
            else if (buttonMenuSelect == 2)
            {
                spriteBatch.Draw(startBtnTexOff, buttonPosition1, Color.White);
                spriteBatch.Draw(optionBtnTexOff, buttonPosition2, Color.White);
                spriteBatch.Draw(exitBtnTexOn, buttonPosition3, Color.White);
            }
        }

        // settings for drawing the pause screen
        public void PausedGameButtonDraw()
        {
            Vector2 buttonPosition1 = new Vector2(170, 330);
            Vector2 buttonPosition2 = new Vector2(170, 440);
            Vector2 buttonPosition3 = new Vector2(170, 550);

            if (buttonPauseSelect == 0)
            {
                spriteBatch.Draw(returnBtnTexOn, buttonPosition1, Color.White);
                spriteBatch.Draw(restartBtnOff, buttonPosition2, Color.White);
                spriteBatch.Draw(mainMenuBtnTexOff, buttonPosition3, Color.White);
            }
            else if (buttonPauseSelect == 1)
            {
                spriteBatch.Draw(returnBtnTexOff, buttonPosition1, Color.White);
                spriteBatch.Draw(restartBtnOn, buttonPosition2, Color.White);
                spriteBatch.Draw(mainMenuBtnTexOff, buttonPosition3, Color.White);
            }
            else if (buttonPauseSelect == 2)
            {
                spriteBatch.Draw(returnBtnTexOff, buttonPosition1, Color.White);
                spriteBatch.Draw(restartBtnOff, buttonPosition2, Color.White);
                spriteBatch.Draw(mainMenuBtnOn, buttonPosition3, Color.White);
            }
            
        }

        // controls for selecting pause menu buttons
        public void PauseMenuButtonControl()
        {
            KeyboardState leftMoveState = Keyboard.GetState();
            KeyboardState rightMoveState = Keyboard.GetState();
            KeyboardState enterKeyState = Keyboard.GetState();

            #region LeftKey
            if (leftMoveState.IsKeyDown(Keys.Down))
            {
                if (!leftState.IsKeyDown(Keys.Down))
                {
                    buttonPauseSelect++;
                    if (buttonPauseSelect == 3)
                    {
                        buttonPauseSelect = 0;
                    }

                }
                else if (leftState.IsKeyDown(Keys.Down))
                {
                }
                leftState = leftMoveState;
            }
            if (leftMoveState.IsKeyUp(Keys.Down))
            {
                leftState = constantState;
            }
            #endregion

            #region RightKey
            if (rightMoveState.IsKeyDown(Keys.Up))
            {
                if (!rightState.IsKeyDown(Keys.Up))
                {
                    buttonPauseSelect--;
                    if (buttonPauseSelect == -1)
                    {
                        buttonPauseSelect = 2;
                    }

                }
                else if (rightState.IsKeyDown(Keys.Up))
                {
                }
                rightState = rightMoveState;
            }
            if (rightMoveState.IsKeyUp(Keys.Up))
            {
                rightState = constantState;
            }
            #endregion

            if (enterKeyState.IsKeyDown(Keys.Enter))
            {
                if (!downState.IsKeyDown(Keys.Enter))
                {
                    if (buttonPauseSelect == 0)
                    {
                        isMainMenu = false;
                        isGameOver = false;
                        IsGamePlaying = true;
                        isGamePaused = false;
                    }
                    if (buttonPauseSelect == 1)
                    {
                        ResetGameBoard3DEffect();
                        GameBoardField = new GameBoard(blockTexture);
                        gameEnded = false;
                        randomBlockStartingIndex = 3;
                        numberOfLines = 0;
                        levelNumber = 0;
                        baseNumberofLines = 10;
                        GameSpeed = 1000f;
                        timerRanOut = 1000f;
                        buttonMenuSelect = 0;

                        gameScore = 0;
                        Lpiece = 0;
                        Jpiece = 0;
                        Tpiece = 0;
                        Ipiece = 0;
                        Zpiece = 0;
                        Spiece = 0;
                        Opiece = 0;

                        highScoreRun = false;

                        isMainMenu = false;
                        isGameOver = false;
                        IsGamePlaying = true;
                        isGamePaused = false;

                    }
                    if (buttonPauseSelect == 2)
                    {
                        highScoreRun = false;

                        isMainMenu = true;
                        isGameOver = false;
                        IsGamePlaying = false;
                        isGamePaused = false;
                        gameEnded = false;

                    }
                }
                else if (downState.IsKeyDown(Keys.Enter))
                {
                }
                downState = enterKeyState;
            }
            if (enterKeyState.IsKeyUp(Keys.Enter))
            {
                downState = constantState;
            }
        }

        public void GameOverButtonControl()
        {
            KeyboardState leftMoveState = Keyboard.GetState();
            KeyboardState rightMoveState = Keyboard.GetState();
            KeyboardState enterKeyState = Keyboard.GetState();

            #region LeftKey
            if (leftMoveState.IsKeyDown(Keys.Down))
            {
                if (!leftState.IsKeyDown(Keys.Down))
                {
                    buttonPauseSelect++;
                    if (buttonPauseSelect == 2)
                    {
                        buttonPauseSelect = 0;
                    }

                }
                else if (leftState.IsKeyDown(Keys.Down))
                {
                }
                leftState = leftMoveState;
            }
            if (leftMoveState.IsKeyUp(Keys.Down))
            {
                leftState = constantState;
            }
            #endregion

            #region RightKey
            if (rightMoveState.IsKeyDown(Keys.Up))
            {
                if (!rightState.IsKeyDown(Keys.Up))
                {
                    buttonPauseSelect--;
                    if (buttonPauseSelect == -1)
                    {
                        buttonPauseSelect = 1;
                    }

                }
                else if (rightState.IsKeyDown(Keys.Up))
                {
                }
                rightState = rightMoveState;
            }
            if (rightMoveState.IsKeyUp(Keys.Up))
            {
                rightState = constantState;
            }
            #endregion

            if (enterKeyState.IsKeyDown(Keys.Enter))
            {
                if (!downState.IsKeyDown(Keys.Enter))
                {
                    
                    if (buttonPauseSelect == 0)
                    {
                        ResetGameBoard3DEffect();
                        GameBoardField = new GameBoard(blockTexture);
                        gameEnded = false;
                        randomBlockStartingIndex = 3;
                        numberOfLines = 0;
                        levelNumber = 0;
                        baseNumberofLines = 10;
                        GameSpeed = 1000f;
                        timerRanOut = 1000f;
                        buttonMenuSelect = 0;

                        gameScore = 0;
                        Lpiece = 0;
                        Jpiece = 0;
                        Tpiece = 0;
                        Ipiece = 0;
                        Zpiece = 0;
                        Spiece = 0;
                        Opiece = 0;

                        highScoreRun = false;

                        isMainMenu = false;
                        isGameOver = false;
                        IsGamePlaying = true;
                        isGamePaused = false;

                    }
                    if (buttonPauseSelect == 1)
                    {
                        highScoreRun = false;

                        isMainMenu = true;
                        isGameOver = false;
                        IsGamePlaying = false;
                        isGamePaused = false;
                        gameEnded = false;

                    }
                }
                else if (downState.IsKeyDown(Keys.Enter))
                {
                }
                downState = enterKeyState;
            }
            if (enterKeyState.IsKeyUp(Keys.Enter))
            {
                downState = constantState;
            }
        }

        public void GameOverButtonDraw()
        {
            Vector2 buttonPosition2 = new Vector2(170, 440);
            Vector2 buttonPosition3 = new Vector2(170, 550);

            if (buttonPauseSelect == 0)
            {
                spriteBatch.Draw(restartBtnOn, buttonPosition2, Color.White);
                spriteBatch.Draw(mainMenuBtnTexOff, buttonPosition3, Color.White);
            }
            if (buttonPauseSelect == 1)
            {
                spriteBatch.Draw(restartBtnOff, buttonPosition2, Color.White);
                spriteBatch.Draw(mainMenuBtnOn, buttonPosition3, Color.White);
            }
        }

        #endregion

        #region MenuUpdates

        private void UpdateMainMenu()
        {
           MainMenuButtonControl();           

            return;
        }

        private void UpdateGamePlaying()
        {            
            GameBoardField.Update();
            GameBoard3DEffect.Update();
            PlayerInput();
            RotationMovePossible();
            Shape4.Update();
            Shape3.Update();
            Shape1.Update();
            Shape2.Update();

            for (int i = 0; i < 12; i++)
            {
                particle_1[i].Update(10);
            }

            PreviewDropPiece();
            TimerPieceMovement();

            KeyboardState pauseKeyState = Keyboard.GetState();

            if (pauseKeyState.IsKeyDown(Keys.Escape))
            {
                if (!oldState.IsKeyDown(Keys.Escape))
                {
                    isMainMenu = false;
                    isGameOver = false;
                    IsGamePlaying = false;
                    isGamePaused = true;

                    buttonPauseSelect = 0;
                }
                else if (oldState.IsKeyDown(Keys.Escape))
                {
                }
                oldState = pauseKeyState;
            }

            if (pauseKeyState.IsKeyUp(Keys.Escape))
            {
            }
            
        }

        private void UpdateGameOver()
        {
            if (!highScoreRun)
            {
                HighScore();
            }

            GameOverButtonControl();

            
            #region Old controls
            // code for resetting all of the variables for a new game

            //if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            //{

            //    // resetting all the variables for a new game
            //    GameBoardField = new GameBoard(blockTexture);
            //    gameEnded = false;
            //    randomBlockStartingIndex = 3;
            //    numberOfLines = 0;
            //    levelNumber = 0;
            //    baseNumberofLines = 0;
            //    GameSpeed = 1000f;

            //    isMainMenu = false;
            //    isGameOver = false;
            //    IsGamePlaying = true;
            //    isGamePaused = false;

            //    gameScore = 0;
            //    Lpiece = 0;
            //    Jpiece = 0;
            //    Tpiece = 0;
            //    Ipiece = 0;
            //    Zpiece = 0;
            //    Spiece = 0;
            //    Opiece = 0;
                
            //    return;
            //}
            //else if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            //{
            //    isMainMenu = true;
            //    isGameOver = false;
            //    IsGamePlaying = false;
            //    return;
            //}
            #endregion
        }

        private void UpdateGamePaused()
        {
            PauseMenuButtonControl();

            return;
        }

        #endregion

        #region MenuDraws

        private void DrawGamePlaying()
        {
            Vector2 leftPos = new Vector2(110, 149);
            Vector2 rightPos = new Vector2(402,149);
            Vector2 topPos = new Vector2(128,0);
            //spriteBatch.Draw(bgTexture, Vector2.Zero, Color.White);

            spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(leftHandWallTexture, new Vector2(50, 80), Color.White);

            
            GameBoard3DEffect.Draw(spriteBatch);
            BoxEffect();

            //Shape4.Draw(spriteBatch);         // preview for the rotation --- not to be draw when playing

            if (previewMovePossible == false)
            {
                Shape3.Draw(spriteBatch);
            }
            
            Shape1.Draw(spriteBatch);
            
            GameBoardField.Draw(spriteBatch);

            spriteBatch.Draw(gameboardTexture, new Vector2(0, 0), Color.White);
            Shape2.Draw(spriteBatch);

            for (int i = 0; i < 12; i++)
            {
            particle_1[i].Draw(spriteBatch);
            }
            
            spriteBatch.DrawString(sprFont, "Lines: " + numberOfLines, new Vector2(474, 350), Color.White);
            spriteBatch.DrawString(sprFont, "Level: " + levelNumber, new Vector2(474, 370), Color.White);
            //spriteBatch.DrawString(sprFont, "L: " + Lpiece + "  J: " + Jpiece + "  T: " + Tpiece + "  Z: " + Zpiece + "  S: " + Spiece + "  I: " + Ipiece + "  O: " + Opiece, new Vector2(40, 765), Color.White);
            spriteBatch.DrawString(sprFont, "Score: " + gameScore, new Vector2(474, 390), Color.White);
            
            //spriteBatch.DrawString(sprFont, "MouseLoc: " + Mouse.GetState().X + ", " + Mouse.GetState().Y, new Vector2(20, 40), Color.White);

            //spriteBatch.Draw(leftBorder, leftPos, Color.White);
            //spriteBatch.Draw(rightBorder, rightPos, Color.White);
            //spriteBatch.Draw(topBorder, topPos, Color.White);

            //for (int u = 0; u < GameBoardField.blocks.Count; u++)
            //{
            //    if (GameBoardField.blocks[u].Texture != null)
            //    {
            //        spriteBatch.DrawString(sprFont, "X", GameBoardField.blocks[u].Position, Color.Black);
            //    }
            //}
        }

        private void DrawGameOver()
        {
            // spriteBatch.Draw(gameOverTex, new Vector2(0, 0), Color.White);
            GameOverButtonDraw(); 

            spriteBatch.DrawString(sprFont, "Lines: " + numberOfLines, new Vector2(310, 150), Color.White);
            spriteBatch.DrawString(sprFont, "Level: " + levelNumber, new Vector2(310, 170), Color.White);
           // spriteBatch.DrawString(sprFont, "L: " + Lpiece + "  J: " + Jpiece + "  T: " + Tpiece + "  Z: " + Zpiece + "  S: " + Spiece + "  I: " + Ipiece + "  O: " + Opiece, new Vector2(40, 765), Color.Black);
            spriteBatch.DrawString(sprFont, "Score: " + gameScore, new Vector2(310, 130), Color.White);

            if (highScoreRun)
            {
                
                spriteBatch.DrawString(sprFont, textHighScore, new Vector2(290, 280), Color.White);

                spriteBatch.DrawString(sprFont, "1 - ", new Vector2(290, 310), Color.White);
                spriteBatch.DrawString(sprFont, textHighScore_2[0], textHighScorePos_1, Color.White);
                spriteBatch.DrawString(sprFont, "2 - ", new Vector2(290, 330), Color.White);
                spriteBatch.DrawString(sprFont, textHighScore_2[1], textHighScorePos_2, Color.White);
                spriteBatch.DrawString(sprFont, "3 - ", new Vector2(290, 350), Color.White);
                spriteBatch.DrawString(sprFont, textHighScore_2[2], textHighScorePos_3, Color.White);
                spriteBatch.DrawString(sprFont, "4 - ", new Vector2(290, 370), Color.White);
                spriteBatch.DrawString(sprFont, textHighScore_2[3], textHighScorePos_4, Color.White);
                spriteBatch.DrawString(sprFont, "5 - ", new Vector2(290, 390), Color.White);
                spriteBatch.DrawString(sprFont, textHighScore_2[4], textHighScorePos_5, Color.White);
            }
        }

        private void DrawMainMenu()
        {
            Vector2 titlePos = new Vector2(190, 150);
            //spriteBatch.Draw(mainMenuTex, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(titleTex, titlePos, Color.White); 
            MainMenuButtonDraw();
        }

        private void DrawGamePused()
        {
            spriteBatch.Draw(pauseMenuBg, new Vector2(180,100), Color.White);

            PausedGameButtonDraw();
        }

        #endregion
    }
}
 