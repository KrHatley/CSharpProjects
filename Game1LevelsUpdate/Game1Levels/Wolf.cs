using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels
{
    public class Wolf : Antagonist, IDamageable, IObserver
    {
        internal Hero hero;
        public Wolf(Game game, Hero h) : base(game)
        {
            this.hero = h;
            this.hitpoints = 8;
            this.attack = 5;
            this.defence = 1;
            this.spelldamage = 0;
            this.experience = 20;
        }
        public override void Initialize()
        {
            this.Texture = this.Game.Content.Load<Texture2D>("wolf");
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            if (this.defeated == true)
            {
                hero._enemies.Remove(this);
            }
            base.Update(gameTime);
        }
        
        
        public void ObserverUpdate(object sender, object message)
        {
            if (sender is Hero && message is string)
            {
                //nothing
            }
        }
    }
}