using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels
{

    public enum TransitionDirection { none, left, right, up, down }
    class LevelManager : DrawableGameComponent,IObserver
    {
        
        TransitionDirection currentTransitionDirection;
        public GoblinKing King;
        public Level[,] levels;
        Level currentLevel, nextlevel, previousLevel;
        private Random ran;
        float transTime = 500;  //transition time in miliseconds
        float currentTransTime = 0; //if a tranistion is active this is the transition time

        

        InputHandler input;
        GameConsole console;

        Vector2 LevelTranstionOffset = Vector2.Zero;
        SpriteBatch spriteBatch;

        Hero hero;

        public LevelManager(Game game,Hero h): base (game)
        {
            ran = new Random();
            input = (InputHandler)this.Game.Services.GetService<IInputHandler>();
            if (input == null)
            {
                input = new InputHandler(game);
                this.Game.Components.Add(input);
            }
            console = (GameConsole)this.Game.Services.GetService<IGameConsole> ();
            if(console == null)
            { 
                console = new GameConsole(game);
                this.Game.Components.Add(console);
            }

            this.hero = h;
            h.Attach(this);

            
         }
        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            if (spriteBatch == null)
            {
                spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            }


            base.Initialize();
        }
        public Level CurrentLevel
        {
            get { return this.currentLevel; }
            set
            {
                if (this.currentLevel != value)
                {
                    
                    this.currentLevel.LevelScreenState = LevelScreenState.TransitionFrom;
                    this.nextlevel = value;
                    this.nextlevel.LevelScreenState = LevelScreenState.TransitionFrom;

                }
            }

        }

        private void LoadLevels()
        {
            int width, height;
            width = height = 3;
            Level l;

           levels = new Level[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    l = new Level(this.Game, x, y, this.hero);
                    l.Initialize();
                    levels[x, y] = l;
                    if (currentLevel == null)
                    {
                        currentLevel = l;
                    }
                }
            }
            currentLevel = levels[(int)currentLevel.GridIndex.X, (int)currentLevel.GridIndex.Y];
            currentLevel.LevelScreenState = LevelScreenState.CurrentLevel;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadLevels();
            King = new GoblinKing(this.Game, this.hero);
            this.levels[ran.Next(3), ran.Next(3)].em.enemies.Add(King);
            this.hero._enemies.Add(King);
            King.Initialize();
            

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (currentTransitionDirection == TransitionDirection.none)
            {
                currentTransitionDirection = GetTranistion();
                switch (currentTransitionDirection)
                {
                    case TransitionDirection.none:
                        //nothing
                        break;
                    default:
                        SetupTransition();
                        break;
                }
            }

            else
            {
                //DO Transition
                if (currentTransTime > 0)
                {
                    //trans lefttim
                    currentTransTime -= gameTime.ElapsedGameTime.Milliseconds;
                    console.GameConsoleWrite(string.Format("currentTransTime: {0} dir:{1}", currentTransTime, currentTransitionDirection));
                }
                else
                {
                    //Transition Done
                    currentTransTime = 0;
                    this.currentLevel.DrawOffset = Vector2.Zero;
                    this.currentLevel.LevelScreenState = LevelScreenState.NotOnScreen;
                    
                    this.previousLevel = currentLevel;
                    this.currentLevel = nextlevel;
                    this.currentLevel.DrawOffset = Vector2.Zero;
                    this.currentLevel.LevelScreenState = LevelScreenState.CurrentLevel;
                    currentTransitionDirection = TransitionDirection.none;
                    this.currentLevel.em.Enabled = true;
                    this.currentLevel.em.Visible = true;
                    
                    this.previousLevel.em.Enabled = false ;
                    this.previousLevel.em.Visible = false;
                }
            }


            base.Update(gameTime);
        }

        private void SetupTransition()
        {
            Level testNextLevel = GetNextLevel(currentTransitionDirection);
            if (testNextLevel != null)
            {
                //for(int i = 0; i< levels.Length;i++)
                //{
                //    if()
                //}
                console.GameConsoleWrite(string.Format("nextLevel {0}, {1}", testNextLevel.GridIndex.X, testNextLevel.GridIndex.Y));
                currentLevel.LevelScreenState = LevelScreenState.TransitionFrom;

                nextlevel = testNextLevel;
                nextlevel.LevelScreenState = LevelScreenState.TransitionTo;
                currentTransTime = transTime; //set transition time

            }

        }

        private Level GetNextLevel(TransitionDirection currentTransition)
        {
            Level next = null;
            switch (currentTransition)
            {
                case TransitionDirection.up:
                    //test x - 1
                    next = GetLevel((int)currentLevel.GridIndex.X - 1, (int)currentLevel.GridIndex.Y);
                    break;
                case TransitionDirection.down:
                    //test x + 1
                    next = GetLevel((int)currentLevel.GridIndex.X + 1, (int)currentLevel.GridIndex.Y);
                    break;
                case TransitionDirection.right:
                    //test y + 1
                    next = GetLevel((int)currentLevel.GridIndex.X, (int)currentLevel.GridIndex.Y + 1);
                    break;
                case TransitionDirection.left:
                    //test y - 1
                    next = GetLevel((int)currentLevel.GridIndex.X, (int)currentLevel.GridIndex.Y - 1);
                    break;
            }
            return next;
        }

        private Level GetLevel(int x, int y)
        {
            Level next = null;
            try
            {
                next = levels[x, y];
            }
            catch(IndexOutOfRangeException ex)
            {
                //will throw execption if out of bounds
                console.GameConsoleWrite("can't move that way out of bounds!");
                console.GameConsoleWrite(ex.ToString());
            }
            return next;
        }

        private TransitionDirection GetTranistion()
        {
           
            return TransitionDirection.none;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            switch (currentLevel.LevelScreenState)
            {
                case LevelScreenState.CurrentLevel:
                    //spriteBatch.Draw(currentLevel.Texture, Vector2.Zero, Color.White);
                    currentLevel.Draw(gameTime);
                    break;
                case LevelScreenState.TransitionFrom:
                    switch (currentTransitionDirection)
                    {
                        case TransitionDirection.down:
                            //current
                            this.currentLevel.DrawOffset = new Vector2(0, MathHelper.Lerp(currentLevel.Texture.Height * -1, 0, currentTransTime / transTime));
                            //next
                            
                            this.nextlevel.DrawOffset = new Vector2(0, MathHelper.Lerp(nextlevel.Texture.Height * -1 + nextlevel.Texture.Height, nextlevel.Texture.Height, currentTransTime / transTime));
                            break;
                        case TransitionDirection.up:
                            //current
                            this.currentLevel.DrawOffset = new Vector2(0, MathHelper.Lerp(currentLevel.Texture.Height, 0, currentTransTime / transTime));
                            //next
                            this.nextlevel.DrawOffset = new Vector2(0, MathHelper.Lerp(0, nextlevel.Texture.Height * -1, currentTransTime / transTime));
                            break;
                        case TransitionDirection.left:
                            //current
                            this.currentLevel.DrawOffset = new Vector2(MathHelper.Lerp(currentLevel.Texture.Width, 0, currentTransTime / transTime),0);
                            //next
                            this.nextlevel.DrawOffset = new Vector2(MathHelper.Lerp(0, nextlevel.Texture.Width * -1, currentTransTime / transTime),0);
                            break;
                        case TransitionDirection.right:
                            //current 
                            this.currentLevel.DrawOffset = new Vector2(MathHelper.Lerp(currentLevel.Texture.Width * -1, 0, currentTransTime / transTime),0);
                            //next
                            this.nextlevel.DrawOffset = new Vector2(MathHelper.Lerp(nextlevel.Texture.Width * -1 + nextlevel.Texture.Width, nextlevel.Texture.Width, currentTransTime / transTime),0);
                            break;
                    }
                    currentLevel.Draw(gameTime);
                    nextlevel.Draw(gameTime);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void ObserverUpdate(object sender, object message)
        {

            
                if (sender is Hero && message is string)
                {
                foreach (Antagonist a in hero._enemies)
                {
                    if (a.defeated == true)
                    {
                        a.Location = a.defeatedpos;
                    }
                }
                    if ((string)message == "Touched Top")
                    {
                        currentTransitionDirection = TransitionDirection.up;
                        SetupTransition();
                }

                    if ((string)message == "Touched Bottom")
                    {
                        currentTransitionDirection = TransitionDirection.down;
                        SetupTransition();
                }
                    if ((string)message == "Touched Left")
                    {
                        currentTransitionDirection = TransitionDirection.left;
                        SetupTransition();

                }
                    if ((string)message == "Touched Right")
                    {
                        //if (levels[,] )
                        //{
                            currentTransitionDirection = TransitionDirection.right;
                            SetupTransition();
                        //}
                    }
                }
            
        }
    }
}
