using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels
{
    public class Goblin : Antagonist, IDamageable, IObserver
    {

       internal Hero hero;
        
        public Goblin(Game game, Hero h) : base(game)
        {
            h.Attach(this);
            this.hero = h;
            
            this.attack = 4;
            this.defence = 3;
            this.hitpoints = 15;
            this.spelldamage = 8;
            this.experience = 10;
        }
        public override void Initialize()
        {
            this.Texture = this.Game.Content.Load<Texture2D>("goblin");
            
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
                if ((string)message == "Combat")
                {

                    //nothing
                }
            }
        }
    }
}
