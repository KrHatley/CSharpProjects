using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.State;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels.GameStates
{
    public interface IPlayingState : IGameState { }
    class PlayingState:BaseGameState,IPlayingState
    {
        internal Hero hero;
        //A new enemy manager lives in each level;
        private Vector2 enemycount;
        SpriteFont font;
        LevelManager lm;
       // public GoblinKing King;
        public  Hero Celebrimbor;
        Camp fire;
        SpriteFont combatfont;
        InventoryState inventorystate;
        CombatState combatstate;
        WinScreen wonthegame;
        InputHandler input;

        IGameStateManager manager;
        private Random ran;

        public PlayingState(Game game, IGameStateManager manager, IinventoryState inventoryState)
           : base(game, manager)
        {

            ran = new Random();
            game.Services.AddService(typeof(IPlayingState), this);
            //hero character initialize
            Celebrimbor = new Hero(this.Game);
            this.hero = Celebrimbor;
            Celebrimbor.DrawOrder = 5;
            this.Game.Components.Add(Celebrimbor);
            //levelmanager initialize
            lm = new LevelManager(this.Game, Celebrimbor);
            this.Game.Components.Add(lm);
            
            //healing camp initialize
            fire = new Camp(this.Game, Celebrimbor);
            this.Game.Components.Add(fire);
            fire.DrawOrder = 3;
           //stat screen initialize
            this.inventorystate = (InventoryState)inventoryState;
            //input manager initialize
            input = (InputHandler)this.Game.Services.GetService<IInputHandler>();
            if(input == null)
            {
                input = new InputHandler(this.Game);
                this.Game.Components.Add(input);
            }
            //game state manager assignment
            this.manager = manager;
        }
        public override void Update(GameTime gameTime)
        {
            inventorystate.dummy = Celebrimbor;
            
            if(input.KeyboardState.HasReleasedKey(Keys.I)==true )
            {
                
                GameManager.PushState(inventorystate.Value);
            }
            if (Celebrimbor._enemies.Count ==1)
            {
                enemycount = new Vector2(-1001, -1000);
            }
            if(Celebrimbor.State == movement.combat)
            {
                combatstate = new CombatState(this.Game, this.manager, Celebrimbor, Celebrimbor.currentEnemy);
                this.manager.PushState(combatstate);

                foreach (Antagonist a in Celebrimbor._enemies)
                {
                    if (a !=Celebrimbor.currentEnemy)
                    {
                        a.Enabled = false;

                    }
                    else
                    {
                        a.Enabled = true;
                    }
                }
                
                
            }
            if (lm.King.defeated ==true)
            {
                wonthegame = new WinScreen(this.Game, this.manager, Celebrimbor);
                this.manager.PushState(wonthegame);
            }
            
            
                
            
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            spritebatch.Begin();
            if (Celebrimbor.HP >0)
            {
                spritebatch.DrawString(font, "Life Points:" + this.Celebrimbor.HP, new Vector2((this.Game.GraphicsDevice.Viewport.Width / 3), 10), Color.Red);
                spritebatch.DrawString(combatfont, "Minions Left:" + (Celebrimbor.enemies.Count-1), enemycount, Color.Red);
            }
            if (lm.King.Enabled == true && lm.King.Visible == true)
            {
                spritebatch.DrawString(combatfont, "Kill the Goblin King!", new Vector2(this.Game.GraphicsDevice.Viewport.Width / 3, 45), Color.Red);
            }
            if (Celebrimbor.HP <= 0)
            {
                spritebatch.DrawString(font, "Game Over", new Vector2((this.GraphicsDevice.Viewport.Width / 3), (this.GraphicsDevice.Viewport.Height / 3)), Color.Red);
                spritebatch.DrawString(font, "Press escape to close", new Vector2((this.GraphicsDevice.Viewport.Width / 3)-30, (this.GraphicsDevice.Viewport.Height / 3)+30), Color.Red);

            }
            spritebatch.End();
            base.Draw(gameTime);
        }
        protected override void StateChanged(object sender, EventArgs e)
        {

            base.StateChanged(sender, e);
            if (GameManager.State == inventorystate)
            {
                this.Enabled = false;
                this.Visible = false;
                Celebrimbor.Enabled = false;
                Celebrimbor.Visible = false;
                lm.Enabled = false;
                lm.Visible = false;
                fire.Enabled = false;
                fire.Visible = false;
            }
            else if (GameManager.State != this.Value )
            {
                Visible = false;
                Enabled = false;
                Celebrimbor.Enabled = false;
                Celebrimbor.Visible = false;
                lm.Enabled = false;
                lm.Visible = false;
                fire.Enabled = false;
                fire.Visible = false;
            }
            else
            {
                Celebrimbor.Enabled = true;
                Celebrimbor.Visible = true;
                lm.Enabled = true;
                lm.Visible = true;
                lm.CurrentLevel.em.Enabled = true;
                lm.CurrentLevel.em.Visible = true;
                fire.Enabled = true;
                fire.Visible = true;
            }
            
        }
        protected override void LoadContent()
        {
            enemycount = new Vector2(this.Game.GraphicsDevice.Viewport.Width / 3, 45);
            combatfont = this.Game.Content.Load<SpriteFont>("combatfont");
            font = this.Game.Content.Load<SpriteFont>("Xanadu");
            base.LoadContent();
        }
    }
}
