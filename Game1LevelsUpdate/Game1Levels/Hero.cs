using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels
{
    public enum movement { walkingLeft,walkingRight,walkingDown,walkingUp,dead,paused, combat }
    public class Hero : Protagonist, ISubject, IDamageable
    {
        Random ran;
        public Texture2D combattexture;
        public Vector2 prevpos;
        public Vector2 combatpos;
        public Rectangle Dir;
        protected List<IObserver> _observers;
        public List<Antagonist> _enemies;
        public List<Camp> _Camps;
        public int expUntilNextLvl;
        public int prevneededExp;
        public int Maxhitpoints;
        public int lvl;
        public bool isdefending;
        public SpriteAnimation previousanimation;
        SpriteAnimation HeroAnimationLeft, HeroAnimationRight;
        SpriteAnimation dead;
        SpriteAnimation HeroAnimationUp, HeroAnimationDown;/// <summary>
        public List<IObserver> observers { get { return _observers; } set { _observers = value; } }
        public List<Antagonist> enemies { get { return _enemies; } set { _enemies = value; } }
        
        movement state;
        public movement State { get { return this.state; } }
        internal Antagonist currentEnemy;

        public Hero(Game game) : base(game)
        {

            this.ran = new Random();
            this._observers = new List<IObserver>();
            this._enemies = new List<Antagonist>();
            this._Camps = new List<Camp>();
            this.hitpoints = 20;
            this.defense = 5;
            this.attack = 5;
            this.spelldamage = 10;
            this.Speed = 3;
            this.lvl = 1;
            this.Experience = 0;

            this.expUntilNextLvl = 20;
            this.prevneededExp = expUntilNextLvl;
            

            
        }
        
        public override void Initialize()
        {
            this.isdefending = false;
            this.Maxhitpoints = 20;
            this.combattexture = this.Game.Content.Load<Texture2D>("combatstance");
            this.combatpos = new Vector2(this.Game.GraphicsDevice.Viewport.Width - (this.Game.GraphicsDevice.Viewport.Width / 3), (this.Game.GraphicsDevice.Viewport.Height / 3));// texture hieght
            this.Location = new Vector2((this.Game.GraphicsDevice.Viewport.Width/2)-200,(this.Game.GraphicsDevice.Viewport.Height/2)-200);
            this.Scale = 1.0f;
           
            base.Initialize();
        }

        protected override void LoadContent()
        {
            

            HeroAnimationLeft = new SpriteAnimation("HeroAnimationLeft", "WalkLeft.gif",5, 3, 3);
            spriteAnimationAdapter.AddAnimation(HeroAnimationLeft);

            HeroAnimationRight = new SpriteAnimation("HeroAnimationRight", "WalkRight.gif", 5, 3, 3);
            spriteAnimationAdapter.AddAnimation(HeroAnimationRight);

            dead = new SpriteAnimation("dead", "Dead", 1, 1, 1);
            spriteAnimationAdapter.AddAnimation(dead);
            HeroAnimationDown = new SpriteAnimation("HeroAnimationDown", "WalkDown.gif", 5, 3, 3);
            spriteAnimationAdapter.AddAnimation(HeroAnimationDown);

            HeroAnimationUp = new SpriteAnimation("HeroAnimationUp", "WalkUp.gif", 5, 3, 3);
            spriteAnimationAdapter.AddAnimation(HeroAnimationUp);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            if (this.expUntilNextLvl<=0)
            {
                LvlUp();
            }

            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            MovementUpdate();
            BoarderTouch();
            CreatureTouch();
            if (this.HP < this.Maxhitpoints)
            {
                
                Heal();
            }
           
            if (this.HP <= 0)
            {
                this.HP = 0;
                this.state = movement.dead;
                this.Enabled = false;
            }
            base.Update(gameTime);
        }

        private void LvlUp()
        {
            this.expUntilNextLvl = prevneededExp + 10;
            this.lvl++;
            this.Maxhitpoints += 5;
            this.attack += 5;
            this.defense += 5;
            this.Experience = 0;
            this.spelldamage += 3;
            this.prevneededExp = expUntilNextLvl;
            this.hitpoints = Maxhitpoints;
            EnemyPowerUp();
        }

        private void EnemyPowerUp()
        {
            foreach (Antagonist a in this._enemies)
            {
                if (a.isBoss != true)
                {
                    a.ATK += 2;
                    a.DEF += 3;
                    a.HP += 2;
                    a.SPL += 2;
                }
            }
        }

        public void Heal()
        {

            foreach (Camp f in _Camps)
            {
                if(this.locationRect.Intersects(f.location))
                {
                    this.hitpoints = Maxhitpoints;
                    this.Notify("Heal");

                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            
            base.Draw(gameTime);
        }
        public void MovementUpdate()
        {
            this.state = movement.paused;
            if (Keyboard.GetState().IsKeyDown(Keys.W) == true)
            {
                this.Location.Y -= (int)this.Speed;
                this.state = movement.walkingUp;
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S) == true)
            {
                this.Location.Y += (int)this.Speed;
                this.state = movement.walkingDown;
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D) == true)
            {
                this.Location.X += (int)this.Speed;
                this.state = movement.walkingRight;
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A) == true)
            {
                this.Location.X -= (int)this.Speed;
                this.state = movement.walkingLeft;
                
            }
            //changes the animation
            switch (state)
            {
                case movement.walkingLeft:
                    this.spriteAnimationAdapter.CurrentAnimation = HeroAnimationLeft;
                    this.spriteAnimationAdapter.ResumeAmination(spriteAnimationAdapter.CurrentAnimation);
                    break;
                case movement.walkingRight:
                    this.spriteAnimationAdapter.CurrentAnimation = HeroAnimationRight;
                    this.spriteAnimationAdapter.ResumeAmination(spriteAnimationAdapter.CurrentAnimation);
                    break;
                case movement.walkingDown:
                    this.spriteAnimationAdapter.CurrentAnimation = HeroAnimationUp;
                    this.spriteAnimationAdapter.ResumeAmination(spriteAnimationAdapter.CurrentAnimation); // correct animation, named them wrong
                    break;
                case movement.walkingUp:
                    this.spriteAnimationAdapter.CurrentAnimation = HeroAnimationDown; // correct animation, named them wrong
                    this.spriteAnimationAdapter.ResumeAmination(spriteAnimationAdapter.CurrentAnimation);
                    break;
                case movement.dead:
                    this.spriteAnimationAdapter.CurrentAnimation = dead;
                    break;
                case movement.paused:
                    this.spriteAnimationAdapter.PauseAnimation(spriteAnimationAdapter.CurrentAnimation);
                    break;
                default:
                    this.spriteAnimationAdapter.PauseAnimation(spriteAnimationAdapter.CurrentAnimation);
                    break;
            }

        }
        public void BoarderTouch()
        {
            if (this.Location.Y < (this.GraphicsDevice.Viewport.Height-(this.GraphicsDevice.Viewport.Height/8)*9))
            {
                this.Enabled = false;
                this.Notify("Touched Top");
                this.Location = new Vector2(Location.X, this.GraphicsDevice.Viewport.Height-(this.GraphicsDevice.Viewport.Height/6));
                this.Enabled = true;
            }
            if (this.Location.Y > this.GraphicsDevice.Viewport.Height-(this.GraphicsDevice.Viewport.Height/7))
            {
                this.Enabled = false;
                this.Notify("Touched Bottom");
                
                this.Location = new Vector2(Location.X, (this.GraphicsDevice.Viewport.Height - (this.GraphicsDevice.Viewport.Height / 8) * 9) + 5);
                this.Enabled = true;
            }
            if (this.Location.X > this.GraphicsDevice.Viewport.Width -(this.GraphicsDevice.Viewport.Width/8))
            {
                this.Enabled = false;
                this.Notify("Touched Right");
                
                this.Location = new Vector2((this.GraphicsDevice.Viewport.Width-(this.GraphicsDevice.Viewport.Width/8)*9)+50, Location.Y);
                this.Enabled = true;
            }
            if (this.Location.X < (this.GraphicsDevice.Viewport.Width - (this.GraphicsDevice.Viewport.Width / 8) * 9)+25)
            {
                this.Enabled = false;
                this.Notify("Touched Left");
                this.Location = new Vector2((this.GraphicsDevice.Viewport.Width - (this.GraphicsDevice.Viewport.Width / 8)-5), Location.Y);
                this.Enabled = true;
            }
        }
        public void CreatureTouch()
        {

            foreach (Antagonist a in _enemies)
            {
                if (a.Enabled == true && a.defeated == false && a.Visible == true)
                {
                    if (this.locationRect.Intersects(a.DestRec))
                    {
                        {
                            this.previousanimation = this.spriteAnimationAdapter.CurrentAnimation;
                            this.spriteAnimationAdapter.CurrentAnimation = HeroAnimationLeft;
                            a.prevLoc = a.Location;
                            this.prevpos = this.Location;
                            this.Notify("Combat");
                            

                            this.currentEnemy = a;
                            this.state = movement.combat;
                           
                        }
                    }
                }
                


            }

        }
        
        public void Attach(IObserver o)
        {
            this._observers.Add(o);
        }

        public void Deatach(IObserver o)
        {
            this._observers.Remove(o);
        }

        public void Notify(string s)
        {
            foreach (IObserver o in _observers)
            {
                o.ObserverUpdate(this, s);
            }
        }

        public void Notify(Rectangle r)
        {
            foreach (IObserver o in _observers)
            {
                o.ObserverUpdate(this, r);
            }
        }
        public void Combat(Antagonist a)
        {
            //this.hitpoints -= 2;
           
        }
    }
}
