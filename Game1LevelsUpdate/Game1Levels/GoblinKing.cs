using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels
{
   public class GoblinKing:Goblin,IObserver
    {
        private Random ran;

        public GoblinKing(Game game, Hero h):base(game,h) 
        {
            this.ran = new Random();
            h.Attach(this);
            this.hero = h;
            this.hitpoints = 70;
            this.defence = 30;
            this.attack = 15;
            this.spelldamage = 20;

        }

        public override void Initialize()
        {
            base.Initialize();
            this.Texture = this.Game.Content.Load<Texture2D>("GoblinKing");
            this.isBoss = true;
            this.Location = new Vector2(ran.Next(this.Game.GraphicsDevice.Viewport.Width), ran.Next(this.Game.GraphicsDevice.Viewport.Height));
        }
        public override void Update(GameTime gameTime)
        {
            if (hero._enemies.Count <2)
            {
                this.Enabled = true;
                this.Visible = true;
            }
            base.Update(gameTime);
        }
    }
}
