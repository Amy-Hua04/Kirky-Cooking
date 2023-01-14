/*
 * Author: Amy Hua
 * File Name: Game1.cs
 * Project Name: Kirky Cooking
 * Creation Date: May 17, 2022
 * Modified Date: 
 * Description: Lets user to play a 2D platformer cooking game.
 */

//Course Concepts
//Lists: OrderQueue (orderList)
//File I/O: High Score
//OOP: Implemented classes in program. Encapsulation in all classes
//Stacks: IngredientManager, ProjectileManager
//Queues: OrderQueue
//Linked Lists: IngredientManager using Ingredient class as nodes, ProjectileManager using Projectile class as nodes

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;

namespace Kirky_Cooking
{
    public class Game1 : Game
    {
        int counter = 1;

        //store random number generation variable
        static Random rng = new Random();

        //store constants for gamestates
        const int MENU = 1;
        const int GAMEPLAY = 2;
        const int PAUSE = 3;
        const int GAMEOVER = 4;

        //store constants for screen overlay
        const int INSTRUCTIONS = 1;

        //store overlay opacity
        const float OVERLAYOPACITY = 0.7f;

        //store button dimensionconstants
        const int BUTTONSPACING = 20;
        const int BUTTONLENGTH = 50; 
        const int BORDER = 10;        

        //store constant for slot spacing
        const int SLOTSPACING = 40;        

        //store constant for movement
        const int PROJECTILESPEED = 17;

        //store constant for ground
        const int GROUND = 450;

        //store constant for time per round
        const int GAMETIME = 240;

        //store constant for max number of orders spawned
        const int MAXORDERS = 4;

        //store variable for graphics
        private GraphicsDeviceManager graphics;

        //stors variable for spritbatch
        private SpriteBatch spriteBatch;

        //store variable for 
        float volume = 1f;

        //store input variables
        MouseState prevMouse, mouse;
        KeyboardState prevKb, kb;

        //store font variables
        SpriteFont titleFont;
        SpriteFont generalFont;

        //store file input/output variables
        StreamReader inFile;
        StreamWriter outFile;

        //store song variables
        Song menuSong;
        Song gameplaySong;
        Song gameOverSong;
        
        //store timer variables
        Timer gameTimer;
        Vector2 gameTimerLoc;

        //store screen dimensions
        int screenWidth, screenHeight;

        //define variable for game state;
        int gameState;

        //store cursor image variable
        Texture2D cursor;
        Rectangle cursorRec;

        //store instruction text
        string instrText;
        string instrText2;
        Vector2 instrTextLoc;

        //store gamestate text
        string pausedText;
        Vector2 pausedTextLoc;
        string gameOverText;
        Vector2 gameOverTextLoc;

        //store transparent colour overlay
        Texture2D colourOverlay;
        Rectangle colourOverlayRec;

        //store title image variable
        Texture2D title;
        Vector2 titleLoc;

        //store ui square button variables
        Texture2D exit, exitClick;
        Rectangle exitRec;
        Texture2D paused, playing;
        Rectangle pausedRec;
        Texture2D instr;
        Rectangle instrRec;

        //store menu ui buttons variables
        Texture2D startImg, startImgHover;
        Rectangle startImgRec;
        Button startButton;

        //stor exit button variables
        Texture2D exitImg, exitImgHover;
        Rectangle exitImgRec;
        Button exitButton;

        //store resume button variables
        Texture2D resumeImg, resumeImgHover;
        Rectangle resumeImgRec;
        Button resumeButton;

        //store retry button variables 
        Texture2D retryImg, retryImgHover;
        Rectangle retryImgRec;
        Button retryButton;

        //store return button variables
        Texture2D returnImg, returnImgHover;
        Rectangle returnImgRec;
        Button returnButton;

        //store game state buttons
        Button[] menuButtons;
        Button[] pausedButtons;
        Button[] gameOverButtons;

        //store background 
        Texture2D backgroundImg, groundImg;
        Vector2 backgroundImgLoc, groundImgLoc;

        //store ingredient sprites and variables
        Texture2D lettuceImg, tomatoImg, cucumberImg;
        Rectangle lettuceRec, tomatoRec, cucumberRec;
        Ingredient lettuce, tomato, cucumber;

        //store ingredient slot sprites and variables
        Texture2D lettuceSlotImg, tomatoSlotImg, cucumberSlotImg;
        Rectangle lettuceSlotRec, tomatoSlotRec, cucumberSlotRec;                
        IngredientSlot lettuceSlot, tomatoSlot, cucumberSlot;
        IngredientSlot[] ingrSlots;
        Timer slotTimer;
        
        //store slot dimensions
        int slotWidth, slotHeight;        

        //store character variables
        Texture2D charImg;
        Rectangle characterDefaultRec, characterRec;
        Character character;        

        //store pipe variables
        Texture2D pipe;
        Rectangle pipeRec;

        //store portal variables
        Texture2D portal;
        Rectangle portalRec;

        //store overlay variables
        bool showOverlay;
        int overlay;
        
        //store projectile variables
        Rectangle projectileRec;
        ProjectileManager projectileList;
        IngredientManager ingredientList;

        //store plate variables
        Texture2D plateImg, plateImgFilled;
        Rectangle plateRec;
        Plate plate;

        //store plate overlay
        Texture2D circleOverlay;
        Rectangle circleOverlayRec;

        //store salad order sprites and variables
        Texture2D saladImg1, saladImg2, saladImg3;
        Rectangle saladRec;
        Order saladVar1, saladVar2, saladVar3;

        //store order variables
        Texture2D orderBg, orderSlip;
        Rectangle orderBgRec, orderSlipRec;
        Order[] orderList;
        OrderQueue orderQueue;
        Timer orderTimer;

        //store score
        int score;
        int highScore;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //set initial game state
            gameState = MENU;

            //set width and height of screen
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 550;
            graphics.ApplyChanges();

            try
            {
                inFile = File.OpenText("Score.txt");
                highScore = Convert.ToInt32(inFile.ReadLine());
                inFile.ReadLine();
            }
            catch
            {
                highScore = 0;
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //create spritebatch variable
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //set screen dimensions
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;

            //set volume
            MediaPlayer.Volume = volume;
            SoundEffect.MasterVolume = volume;

            //load songs
            menuSong = Content.Load<Song>("Audio/MenuSong");
            gameplaySong = Content.Load<Song>("Audio/GameplaySong");
            gameOverSong = Content.Load<Song>("Audio/ResultsSong");

            //load fonts
            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");
            generalFont = Content.Load<SpriteFont>("Fonts/GeneralFont");

            //set timers
            gameTimer = new Timer(GAMETIME, false);
            gameTimerLoc = new Vector2(670, 500);

            //load cursor
            cursor = Content.Load<Texture2D>("Images/Cursor");
            cursorRec = new Rectangle(0, 0, (int)(cursor.Width * 0.2), (int)(cursor.Height * 0.2));

            //load screen colour overlay 
            colourOverlay = Content.Load<Texture2D>("Images/ColourOverlay");
            colourOverlayRec = new Rectangle(0, 0, screenWidth, screenHeight);

            //load title text and text locations
            instrText = "INSTRUCTIONS";
            instrTextLoc = new Vector2(CenterX(instrText, titleFont), 50);
            pausedText = "PAUSED";
            pausedTextLoc = new Vector2(CenterX(pausedText, titleFont), 150);
            gameOverText = "GAME OVER";
            gameOverTextLoc = new Vector2(CenterX(gameOverText, titleFont), 150);

            //load title logo
            title = Content.Load<Texture2D>("Images/Title");
            titleLoc = new Vector2(CenterX(title), 110);

            //load ui buttons
            exit = Content.Load<Texture2D>("Images/Exit");
            exitClick = Content.Load<Texture2D>("Images/ExitClick");
            exitRec = new Rectangle(screenWidth - 2* BORDER - 2* BUTTONLENGTH, BORDER, BUTTONLENGTH, BUTTONLENGTH);
            paused = Content.Load<Texture2D>("Images/Paused");
            playing = Content.Load<Texture2D>("Images/Playing");
            pausedRec = new Rectangle(screenWidth - BORDER - BUTTONLENGTH, BORDER, BUTTONLENGTH, BUTTONLENGTH);
            instr = Content.Load<Texture2D>("Images/Instructions");
            instrRec = new Rectangle(BORDER, BORDER, BUTTONLENGTH, BUTTONLENGTH);

            //load menu ui buttons
            startImg = Content.Load<Texture2D>("Images/StartMenu");
            startImgHover = Content.Load<Texture2D>("Images/StartMenuHover");
            startImgRec = new Rectangle(300, 220, (int)(startImg.Width * 0.4), (int)(startImg.Height * 0.4));
            startButton = new Button(startImgRec, startImg, startImgHover, spriteBatch);
            
            //load exit ui buttons
            exitImg = Content.Load<Texture2D>("Images/ExitMenu");
            exitImgHover = Content.Load<Texture2D>("Images/ExitMenuHover");
            exitImgRec = new Rectangle(startImgRec.X, startImgRec.Y + startImgRec.Height + BUTTONSPACING, startImgRec.Width, startImgRec.Height);
            exitButton = new Button(exitImgRec, exitImg, exitImgHover, spriteBatch);

            //load resume ui buttons
            resumeImg = Content.Load<Texture2D>("Images/Resume");
            resumeImgHover = Content.Load<Texture2D>("Images/ResumeHover");
            resumeImgRec = startImgRec;
            resumeButton = new Button(startImgRec, resumeImg, resumeImgHover, spriteBatch);

            //load return ui buttons
            returnImg = Content.Load<Texture2D>("Images/Return");
            returnImgHover = Content.Load<Texture2D>("Images/ReturnHover");
            returnImgRec = exitImgRec;
            returnButton = new Button(exitImgRec, returnImg, returnImgHover, spriteBatch);

            //load retry ui buttons
            retryImg = Content.Load<Texture2D>("Images/Retry");
            retryImgHover = Content.Load<Texture2D>("Images/RetryHover");
            retryImgRec = startImgRec;
            retryButton = new Button(startImgRec, retryImg, retryImgHover, spriteBatch);

            //create button arrays for each game state
            menuButtons = new Button[] { startButton, exitButton };
            pausedButtons = new Button[] { resumeButton, returnButton };
            gameOverButtons = new Button[] {retryButton, returnButton };

            //load background image
            backgroundImg = Content.Load<Texture2D>("Images/Background");
            backgroundImgLoc = new Vector2(0, 0);

            //load ground image
            groundImg = Content.Load<Texture2D>("Images/Ground");
            groundImgLoc = new Vector2(0, 440);

            //load character
            charImg = Content.Load<Texture2D>("Images/Character");
            characterDefaultRec = new Rectangle(350, GROUND - charImg.Height * 2, charImg.Width * 2, charImg.Height * 2);
            characterRec = characterDefaultRec;
            character = new Character(charImg, characterDefaultRec, spriteBatch);

            //load projectile
            projectileRec = new Rectangle(0, 0, charImg.Width, charImg.Height);
            projectileList = new ProjectileManager(colourOverlayRec);

            //load pipe
            pipe = Content.Load<Texture2D>("Images/Pipe");
            pipeRec = new Rectangle(650, 120, (int)(pipe.Width * 0.3), (int)(pipe.Height * 0.3));

            //load portal
            portal = Content.Load<Texture2D>("Images/Portal");
            portalRec = new Rectangle(0, 0, (int)(portal.Width * 0.3), (int)(portal.Height * 0.3));
            portalRec.X = screenWidth / 2 - portalRec.Width / 2;
            portalRec.Y = (int)groundImgLoc.Y - portalRec.Height;

            //load plate
            plateImg = Content.Load<Texture2D>("Images/Plate");
            plateImgFilled = Content.Load<Texture2D>("Images/PlateFilled");
            plateRec = new Rectangle(250, 0, (int)(plateImg.Width * 0.4), (int)(plateImg.Height * 0.4));
            plateRec.Y = GROUND - plateRec.Height;

            //loate plate overlay
            circleOverlay = Content.Load<Texture2D>("Images/CircleOverlay");
            circleOverlayRec = new Rectangle(0, 0, (int)(circleOverlay.Width * 0.3), (int)(circleOverlay.Height * 0.3));

            //load lettuce slot images
            lettuceSlotImg = Content.Load<Texture2D>("Images/LettuceSlot");
            tomatoSlotImg = Content.Load<Texture2D>("Images/TomatoSlot");
            cucumberSlotImg = Content.Load<Texture2D>("Images/CucumberSlot");

            //slot dimensions
            slotWidth = lettuceSlotImg.Width * 2;
            slotHeight = lettuceSlotImg.Height * 2;

            //create slot rectangle
            cucumberSlotRec = new Rectangle(0, 75, slotWidth, slotHeight);
            tomatoSlotRec = new Rectangle(0, cucumberSlotRec.Y + slotHeight + SLOTSPACING, slotWidth, slotHeight);
            lettuceSlotRec = new Rectangle(0, tomatoSlotRec.Y + slotHeight + SLOTSPACING, slotWidth, slotHeight);

            //set slot timer
            slotTimer = new Timer(1, false);

            //load ingredient images
            lettuceImg = Content.Load<Texture2D>("Images/Lettuce");
            tomatoImg = Content.Load<Texture2D>("Images/Tomato");
            cucumberImg = Content.Load<Texture2D>("Images/Cucumber");

            //load ingredient rectangles
            lettuceRec = new Rectangle(0, 0, (int)(lettuceImg.Width * 0.3), (int)(lettuceImg.Height * 0.3));
            tomatoRec = new Rectangle(0, 0, (int)(tomatoImg.Width * 0.3), (int)(tomatoImg.Height * 0.3));
            cucumberRec = new Rectangle(0, 0, (int)(cucumberImg.Width * 0.4), (int)(cucumberImg.Height * 0.4));
            
            //set ingredient variables
            lettuce = new Ingredient("Lettuce", lettuceImg, lettuceRec, spriteBatch, GROUND - lettuceRec.Height);
            tomato = new Ingredient("Tomato", tomatoImg, tomatoRec, spriteBatch, GROUND - tomatoRec.Height);
            cucumber = new Ingredient("Cucumber", cucumberImg, cucumberRec, spriteBatch, GROUND - cucumberRec.Height);

            //load salad order images
            saladImg1 = Content.Load<Texture2D>("Images/SaladVar1");
            saladImg2 = Content.Load<Texture2D>("Images/SaladVar2");
            saladImg3 = Content.Load<Texture2D>("Images/SaladVar3");

            //set salad order rectangle
            saladRec = new Rectangle(0, 0, (int)(saladImg1.Width * 0.6), (int)(saladImg1.Height * 0.6));

            //load order background images
            orderBg = Content.Load<Texture2D>("Images/Order");
            orderBgRec = new Rectangle(0, 0, (int)(orderBg.Width * 0.5), (int)(orderBg.Height * 0.5));
            orderSlip = Content.Load<Texture2D>("Images/OrderSlip");
            orderSlipRec = new Rectangle(0, 0, (int)(orderSlip.Width * 0.6), (int)(orderSlip.Height * 0.6));

            //set order queue
            orderQueue = new OrderQueue(spriteBatch, orderBg, orderBgRec, orderSlip, orderSlipRec);

            //create salad order variables
            saladVar1 = new Order(new Ingredient[] { lettuce }, saladImg1, saladRec, 1);
            saladVar2 = new Order(new Ingredient[] { lettuce, tomato }, saladImg2, saladRec, 2);
            saladVar3 = new Order(new Ingredient[] { lettuce, tomato, cucumber }, saladImg3, saladRec, 3);

            //create order list with order variables
            orderList = new Order[] { saladVar1, saladVar2, saladVar3 };

            //set order timer
            orderTimer = new Timer(5, false);

            //create ingredient manager variable
            ingredientList = new IngredientManager();

            //set ingredient slot variables
            lettuceSlot = new IngredientSlot("Lettuce", lettuceSlotImg, lettuceSlotRec, spriteBatch);
            tomatoSlot = new IngredientSlot("Tomato", tomatoSlotImg, tomatoSlotRec, spriteBatch);
            cucumberSlot = new IngredientSlot("Cucumber", cucumberSlotImg, cucumberSlotRec, spriteBatch);

            //set array holding ingredient slots
            ingrSlots = new IngredientSlot[] { lettuceSlot, tomatoSlot, cucumberSlot };

            ResetGame();
        }

        protected override void UnloadContent()
        {}

        protected override void Update(GameTime gameTime)
        {
            //recieve user inputs
            GetInput();

            //update the game based on the gamestate
            switch (gameState)
            {
                case (MENU):
                    //start menu music if music is not playing
                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(menuSong);
                    }

                    //update menu buttons based on input
                    UpdateMenuButtons();
                    break;
                case (GAMEPLAY):
                    //start gameplay music if music is not playing
                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(gameplaySong);
                    }

                    //start game timer if it has not started
                    if (gameTimer.IsActive() == false)
                    {
                        gameTimer.Activate();
                    }

                    //change to game over gamestate if timer is over
                    if (gameTimer.IsFinished())
                    {
                        MediaPlayer.Stop();
                        SaveScore();
                        gameState = GAMEOVER;
                        break;
                    }

                    //generate new order if timer is up or if there are no other orders
                    if (orderQueue.GetSize() == 0 || (orderTimer.IsFinished() && orderQueue.GetSize() < MAXORDERS))
                    {
                        orderQueue.Enqueue(orderList[rng.Next(0, orderList.Length)]);
                        orderTimer.ResetTimer(true);
                    }
                    
                    //pause game if instructed
                    if (kb.IsKeyDown(Keys.Escape) && prevKb.IsKeyDown(Keys.Escape) == false)
                    {
                        gameState = PAUSE;
                        break;
                    }

                    //interact with game if left button is pressed
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        //pause game if mouse is pause button
                        if (pausedRec.Contains(mouse.Position))
                        {
                            gameState = PAUSE;
                            break;
                        }

                        if (character.IsHolding())
                        {
                            if (character.CheckIngr())
                            {
                                Ingredient tempIngr = (character.RemoveIngr());
                                if (tempIngr.GetRec().Intersects(plate.GetRec()))
                                {
                                    plate.AddIngr(tempIngr);
                                }
                            }
                        }
                        else
                        {
                            character.AddIngr(ingredientList.Remove(character.GetRec()));
                        }
                    }

                    //spawn projectile if right mouse button is released
                    if (mouse.RightButton != ButtonState.Pressed && prevMouse.RightButton == ButtonState.Pressed)
                    {
                        projectileList.Add(new Projectile(true, charImg, projectileRec, character.GetRec().Location, mouse.Position, PROJECTILESPEED, spriteBatch));
                    }

                    //move character depending on keyboard buttons
                    if (kb.IsKeyDown(Keys.A) && characterRec.X > 0)
                    {
                        character.MoveLeft();
                    }
                    if (kb.IsKeyDown(Keys.D) && characterRec.Right < screenWidth)
                    {
                        character.MoveRight();
                    }
                    if (kb.IsKeyDown(Keys.Space) && prevKb.IsKeyDown(Keys.Space) == false && character.IsGrounded())
                    {
                        character.Jump();                        
                    }

                    //apply gravity if character is not on the ground
                    if (character.IsGrounded() == false)
                    {
                        character.ApplyGravity();
                    }

                    //spawn ingredients if slot timer if over
                    if (slotTimer.IsFinished() || slotTimer.IsInactive())
                    {
                        //spawns ingredients depending on projectile interaction with slots
                        SpawnIngredients();
                    }

                    //update timers
                    gameTimer.Update(gameTime.ElapsedGameTime.TotalSeconds);
                    orderTimer.Update(gameTime.ElapsedGameTime.TotalSeconds);
                    if (slotTimer.IsActive())
                    {
                        slotTimer.Update(gameTime.ElapsedGameTime.TotalSeconds);
                    }

                    if (orderQueue.Remove(plate.GetIngr()))
                    {
                        score += 50;
                        plate.Clear();
                    }

                    //moves all objects
                    character.MoveObject();
                    projectileList.MoveAll();
                    ingredientList.MoveAll(colourOverlayRec);
                    break;

                case (PAUSE):
                    //pause music if playing
                    if (MediaPlayer.State == MediaState.Playing)
                    {
                        MediaPlayer.Pause();
                    }
                    
                    //resumes game if instructed
                    if (kb.IsKeyDown(Keys.Escape) && prevKb.IsKeyDown(Keys.Escape) == false)
                    {
                        MediaPlayer.Resume();
                        gameState = GAMEPLAY;
                        break;
                    }

                    //updates pause buttons
                    UpdatePauseButtons();
                    break;

                case (GAMEOVER):
                    //play game over music if music is not playing
                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(gameOverSong);
                    }

                    //update game over buttons
                    UpdateGameOverButtons();
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            //start spritebatch
            spriteBatch.Begin();

            //draw game based on gamestate
            switch (gameState)
            {
                case (MENU):
                    DrawBg();

                    spriteBatch.Draw(title, titleLoc, Color.White);
                    
                    //draw all menu buttons
                    for (int i = 0; i < menuButtons.Length; i++)
                    {
                        menuButtons[i].DrawButtons();
                    }

                    //draw instruction button
                    spriteBatch.Draw(instr, instrRec, Color.White);

                    //draw overlays
                    if (showOverlay == true)
                    {
                        //draw colour overlay
                        spriteBatch.Draw(colourOverlay, colourOverlayRec, Color.Black * OVERLAYOPACITY);
                        switch (overlay)
                        {
                            //draw instructions
                            case (INSTRUCTIONS):
                                spriteBatch.DrawString(titleFont, instrText, instrTextLoc, Color.White);
                                break;
                        }
                    }
                    break;

                case (GAMEPLAY):
                    //draw game
                    DrawGame();
                    spriteBatch.Draw(playing, pausedRec, Color.White);
                    break;

                case (PAUSE):
                    //draw game
                    DrawGame();

                    //draw pause screen ui
                    spriteBatch.Draw(paused, pausedRec, Color.White);
                    spriteBatch.Draw(colourOverlay, colourOverlayRec, Color.Black * OVERLAYOPACITY);
                    spriteBatch.DrawString(titleFont, pausedText, pausedTextLoc, Color.White);
                    for (int i = 0; i < pausedButtons.Length; i++)
                    {
                        pausedButtons[i].DrawButtons();
                    }
                    break;

                case (GAMEOVER):
                    //draw game
                    DrawGame();

                    //draw game over ui
                    spriteBatch.Draw(colourOverlay, colourOverlayRec, Color.Black * OVERLAYOPACITY);
                    spriteBatch.DrawString(titleFont, gameOverText, gameOverTextLoc, Color.White);
                    for (int i = 0; i < gameOverButtons.Length; i++)
                    {
                        gameOverButtons[i].DrawButtons();
                    }
                    break;
            }

            //draw cursor
            spriteBatch.Draw(cursor, cursorRec, Color.White);

            //end spritebatch
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void GetInput()
        {
            prevMouse = mouse;
            mouse = Mouse.GetState();
            prevKb = kb;
            kb = Keyboard.GetState();

            cursorRec.X = mouse.X;
            cursorRec.Y = mouse.Y;
        }

        private void UpdateMenuButtons()
        {
            if (showOverlay == false)
            {
                for (int i = 0; i < menuButtons.Length; i++)
                {
                    menuButtons[i].CheckHover(mouse.Position);
                }
            }

            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
            {
                if (showOverlay == true)
                {
                    showOverlay = !showOverlay;
                }
                else if (startImgRec.Contains(mouse.Position))
                {
                    gameState = GAMEPLAY;
                    MediaPlayer.Stop();
                    ResetGame();
                }
                else if (exitImgRec.Contains(mouse.Position))
                {
                    Exit();
                }
                else if (instrRec.Contains(mouse.Position))
                {
                    showOverlay = true;
                    overlay = INSTRUCTIONS;
                }
            }
        }

        private void UpdatePauseButtons()
        {
            for (int i = 0; i < pausedButtons.Length; i++)
            {
                pausedButtons[i].CheckHover(mouse.Position);
            }

            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
            {
                if (resumeImgRec.Contains(mouse.Position))
                {
                    MediaPlayer.Resume();
                    gameState = GAMEPLAY;
                }
                if (returnImgRec.Contains(mouse.Position))
                {
                    MediaPlayer.Stop();
                    gameState = MENU;
                }
            }

        }

        private void UpdateGameOverButtons()
        {
            for (int i = 0; i < gameOverButtons.Length; i++)
            {
                gameOverButtons[i].CheckHover(mouse.Position);
            }
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
            {
                if (retryImgRec.Contains(mouse.Position))
                {
                    ResetGame();
                    MediaPlayer.Stop();
                    gameState = GAMEPLAY;
                }
                if (returnImgRec.Contains(mouse.Position))
                {
                    MediaPlayer.Stop();
                    gameState = MENU;
                }
            }
        }


        private void SpawnIngredients()
        {
            for (int i = 0; i < ingrSlots.Length; i++)
            {
                if (projectileList.SlotInteract(ingrSlots[i]))
                {
                    if (ingrSlots[i].GetType() == "Tomato")
                    {
                        ingredientList.Add(new Ingredient("Tomato", tomatoImg, tomatoRec, spriteBatch, GROUND - tomatoRec.Height), pipeRec);
                    }
                    else if (ingrSlots[i].GetType() == "Lettuce")
                    {
                        ingredientList.Add(new Ingredient("Lettuce", lettuceImg, lettuceRec, spriteBatch, GROUND - lettuceRec.Height), pipeRec);
                    }
                    else if (ingrSlots[i].GetType() == "Cucumber")
                    {
                        ingredientList.Add(new Ingredient("Cucumber", cucumberImg, cucumberRec, spriteBatch, GROUND - cucumberRec.Height), pipeRec);
                    }

                    slotTimer.ResetTimer(true);
                    break;
                }
            }
        }

        private void ResetGame()
        {
            gameTimer.ResetTimer(true);
            slotTimer.ResetTimer(false);
            ingredientList.Reset();
            projectileList.Reset();
            orderQueue.Reset();
            character.Reset();

            plate = new Plate(plateImg, plateImgFilled, plateRec, circleOverlay, circleOverlayRec, spriteBatch);

            counter++;
        }

        private void SaveScore()
        {
            if (score > highScore)
            {
                highScore = score;
            }

            outFile = File.CreateText("Score.txt");
            outFile.WriteLine(highScore);
            outFile.Close();
        }

        private void DrawGame()
        {
            DrawBg();
            spriteBatch.Draw(pipe, pipeRec, Color.White);
            spriteBatch.DrawString(titleFont, GetGameTime(), gameTimerLoc, Color.Black);
            projectileList.DrawAll();
            for (int i = 0; i < ingrSlots.Length; i++)
            {
                ingrSlots[i].Draw();
            }
            character.Draw();
            orderQueue.DrawAll();
            ingredientList.DrawAll();
            plate.Draw();
        }

        private void DrawBg()
        {
            spriteBatch.Draw(backgroundImg, backgroundImgLoc, Color.White);
            spriteBatch.Draw(groundImg, groundImgLoc, Color.White);
            spriteBatch.Draw(portal, portalRec, Color.White);
        }

        private string GetGameTime()
        {
            int time = (int)gameTimer.GetTimeRemaining();
            if (time % 60 < 10)
            {
                return (int)(time / 60) + ":0" + time % 60;
            }
            return (int)(time / 60) + ":" + time % 60;
        }

        private int CenterX(Texture2D img)
        {
            return (int)(screenWidth / 2 - img.Width / 2);
        }

        private int CenterX(string text, SpriteFont font)
        {
            return (int)(screenWidth / 2 - font.MeasureString(text).X / 2);
        }
    }
}
