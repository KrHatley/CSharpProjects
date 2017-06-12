using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.State;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels.GameStates
{
    public interface ICombatState : IGameState { }
    class CombatState:BaseGameState,ICombatState
    {
        private string title;
        private Texture2D combattexture;
        private SpriteFont font;
        private SpriteFont combatfont;
        public Hero hero;
        public Antagonist opponnent;
        private int moveselect;
        private int defenseuse;
        public bool outofmana;
        private double successchance;
        private double chance;
      //  private bool cooldown;
        private int miliseconds;
        private int prevgametime;
        private int time;
        Random ran;

        InputHandler input;

        public CombatState(Game game,IGameStateManager manager,Hero h, Antagonist a):base (game,manager)
        {
            this.chance = 0.6;
            this.ran = new Random();
            this.hero = h;
            this.opponnent = a;
            
            input = (InputHandler)this.Game.Services.GetService<IInputHandler>();
            if (input == null)
            {
                input = new InputHandler(this.Game);
                this.Game.Components.Add(input);
            }
        }
        protected override void LoadContent()
        {
            this.miliseconds = 5;
            title = "FIGHT!";
            this.combattexture = this.Game.Content.Load<Texture2D>("battleground");
            font = this.Game.Content.Load<SpriteFont>("Xanadu");
            combatfont = this.Game.Content.Load<SpriteFont>("combatfont");
            base.LoadContent();
        }
        public override void Initialize()
        {
            this.successchance = 0.0;
            // this.cooldown = false;
            this.outofmana = false;
            this.defenseuse = 3;
            this.moveselect = 0;

            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            successchance = chance * ran.Next(10);
           
            this.time = gameTime.ElapsedGameTime.Milliseconds;
            hero.Visible = true;
            hero.Location = hero.combatpos;
            opponnent.Location = opponnent.combatpos;
         
                Battle(hero, opponnent);
           
            
            if (opponnent.HP <= 0 ||hero.HP <=0 || input.KeyboardState.HasReleasedKey(Microsoft.Xna.Framework.Input.Keys.L))
            {
                foreach (Antagonist a in hero._enemies)
                {
                    if (a != opponnent)
                    {
                        a.Location = a.prevLoc;
                        a.Enabled = true;
                        
                    }
                    else
                    {
                        if (hero.HP>0)
                        {
                            opponnent.Location = opponnent.prevLoc;
                            opponnent.defeated = true;
                            opponnent.Texture = this.Game.Content.Load<Texture2D>("Dead");
                        }
                       
                    }

                   
                }
                hero.EXP += opponnent.EXP;
                hero.expUntilNextLvl -= opponnent.EXP;
                hero.Location = hero.prevpos;
                GameManager.PopState();//pop myself off the stack

            }
            base.Update(gameTime);
        }

        

        private void Battle(Hero hero, Antagonist opponnent)
        {
            attackselect();


            switch (moveselect)
            {
                case 1:
                    opponnent.HP = (opponnent.HP - (hero.ATK - opponnent.DEF));
                    moveselect = 0;
                    OpponentAtK(this.hero);
                    break;
                case 2:
                    if (outofmana ==false)
                    {
                        opponnent.HP = (opponnent.HP - (hero.SPL - opponnent.DEF));
                        outofmana = true;
                        moveselect = 0;
                        OpponentAtK(this.hero);
                    }
                    break;
                case 3:
                    if (defenseuse > 0)
                    {
                        hero.isdefending = true;
                        defenseuse--;
                        moveselect = 0;
                       
                    }
                    OpponentAtK(this.hero);
                    break;
                case 0:
                    attackselect();
                    break;
                default:

                    break;
            }
            
            
            moveselect = 0;
            prevgametime = time;
            

        }

        private void attackselect()
        {
            if (input.KeyboardState.HasReleasedKey(Microsoft.Xna.Framework.Input.Keys.NumPad1))
            {
                moveselect = 1;
               
            }
            if (input.KeyboardState.HasReleasedKey(Microsoft.Xna.Framework.Input.Keys.NumPad2))
            {
                moveselect = 2;
                
            }
            if (input.KeyboardState.HasReleasedKey(Microsoft.Xna.Framework.Input.Keys.NumPad3))
            {
                moveselect = 3;
                
            }
            
                
                
            
            
        }

        private void OpponentAtK(Hero hero)
        {
            if (successchance>2.0)
            {
                if (hero.isdefending ==true)
                {
                    hero.HP +=  (opponnent.ATK -hero.DEF);
                }
                else
                {
                    hero.HP -= opponnent.ATK;
                }

            }
            
        }

        public override void Draw(GameTime gameTime)
        {
            spritebatch.Begin();
            spritebatch.Draw(this.combattexture, new Rectangle(0, 0, this.Game.GraphicsDevice.Viewport.Width, this.Game.GraphicsDevice.Viewport.Height), Color.White);
            spritebatch.DrawString(combatfont, "Remaining Health:" + opponnent.HP, new Vector2((opponnent.Location.X) - (opponnent.Texture.Width / 2), (opponnent.Location.Y) - (opponnent.Texture.Height / 2)), Color.Red);
            spritebatch.DrawString(font, title, new Vector2((this.Game.GraphicsDevice.Viewport.Width / 2) - title.Length, this.Game.GraphicsDevice.Viewport.Height / 18), Color.Red);
            spritebatch.DrawString(combatfont, "HP:" + this.hero.HP, new Vector2(hero.combatpos.X, hero.combatpos.Y - (hero.combattexture.Height)),Color.Red);
            spritebatch.DrawString(combatfont, "Press '1' to attack", new Vector2(this.Game.GraphicsDevice.Viewport.Width / 3, this.Game.GraphicsDevice.Viewport.Height - (this.GraphicsDevice.Viewport.Height / 8) * 3), Color.Red);
            spritebatch.DrawString(combatfont, "Press '2' to use magic(one use per battle)", new Vector2(this.Game.GraphicsDevice.Viewport.Width / 3, this.Game.GraphicsDevice.Viewport.Height - (this.GraphicsDevice.Viewport.Height / 8) * 2), Color.Red);
            spritebatch.DrawString(combatfont, "Press '3' to Defend(three uses per battle)", new Vector2(this.Game.GraphicsDevice.Viewport.Width / 3, this.Game.GraphicsDevice.Viewport.Height - (this.GraphicsDevice.Viewport.Height / 8)), Color.Red);
            spritebatch.Draw(this.hero.combattexture, this.hero.Location, Color.White);
            spritebatch.Draw(this.opponnent.Texture, this.opponnent.combatpos, Color.White);
            spritebatch.End();
            base.Draw(gameTime);
        }
    }
}
