using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels
{
    public abstract class Antagonist : GameComponent, IDamageable
    {
        public bool isBoss;
        public Vector2 defeatedpos;
        public bool defeated;
        public Texture2D Texture;
        public Vector2 Location;
        public Rectangle DestRec;
        protected Vector2 Direction;
        protected float Speed;
        
        public Vector2 combatpos;
        //public Vector2 inactiveLoc = new Vector2(-1000, -1000);
        public  Vector2 prevLoc;
        
        protected int hitpoints;
        protected int defence;
        protected int attack;
        protected int spelldamage;
        protected int skillpoints;
        protected int experience;

        public bool Visible;

        Random ran;

        public int EXP { get { return experience; } set { experience = value; } }
        public int HP { get { return hitpoints; } set { hitpoints = value; } }

        public int DEF { get { return defence; } set { defence = value; } }

        public int ATK { get { return attack; } set { attack = value; } }

        public int SPL { get { return spelldamage; } set { spelldamage = value; } }

        public Antagonist(Game game) : base(game)
        {
            ran = new Random(); 
        }
        public override void Initialize()
        {
            this.isBoss = false;
            this.Location = new Vector2(ran.Next(this.Game.GraphicsDevice.Viewport.Width), ran.Next(this.Game.GraphicsDevice.Viewport.Height));
            if (this.Location.Y > (this.Game.GraphicsDevice.Viewport.Height - this.Texture.Height))
            {
                this.Location.Y -= this.Texture.Height;
            }
            if (this.Location.Y < 0 + this.Texture.Height)
            {
                this.Location.Y = this.Texture.Height;
            }
            if (this.Location.X > this.Game.GraphicsDevice.Viewport.Width - this.Texture.Width)
            {
                this.Location.X -= this.Texture.Width;
            }
            if (this.Location.X < 0 + this.Texture.Width)
            {
                this.Location.X += this.Texture.Width;
            }
                this.defeated = false;
            //this.defeatedpos = new Vector2(-9000, -9000);
            this.combatpos = new Vector2((this.Game.GraphicsDevice.Viewport.Width / 3)-(this.Texture.Width), (this.Game.GraphicsDevice.Viewport.Height / 2)-(this.Texture.Height));
            this.DestRec = new Rectangle((int)this.Location.X, (int)this.Location.Y, this.Texture.Width, this.Texture.Height);
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
           
            this.DestRec = new Rectangle((int)this.Location.X, (int)this.Location.Y, this.Texture.Width, this.Texture.Height);
            base.Update(gameTime);
        }
    }
}
