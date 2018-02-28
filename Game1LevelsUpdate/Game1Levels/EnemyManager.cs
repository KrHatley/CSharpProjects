using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Game1Levels
{
    public class EnemyManager:DrawableGameComponent,IObserver
    {
        SpriteBatch spritebatch;
        public List<Antagonist> enemies;
        Random ran;
        

        Hero hero;
        public EnemyManager(Game game,Hero h):base(game)
        {
            
            ran = new Random();
            enemies = new List<Antagonist>();
            h.Attach(this);
            
            this.hero = h;
         }

        public void ObserverUpdate(object sender, object message)
        {
            if (sender is Hero && message is string)
            {
                if ((string)message == "Touched Top")
                {
                    
                }
                if ((string)message == "Touched Bottom")
                {

                }
                if ((string)message == "Touched Left")
                {

                }
                if ((string)message == "Touched Right")
                {

                }
            }
        }
        protected override void LoadContent()
        {
            if (spritebatch == null)
            {
                spritebatch = new SpriteBatch(this.Game.GraphicsDevice);
            }
            base.LoadContent();
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (this.Enabled)
            {
                foreach (Antagonist a in enemies)
                {

                    if (a.defeated == false && !a.isBoss)
                    {
                        a.Visible = true;
                        a.Enabled = true;
                    }
                }
            }
            else
            {
                foreach (Antagonist a in enemies)
                {

                        a.Visible = false;
                        a.Enabled = false;
                    
                }
            }
            base.OnEnabledChanged(sender, args);
        }
        public override void Initialize()
        {

            //Drawing enemies beneath the background screen draworder doesnt work, Slow to pop enemies out of the list

            for (int i = 0; i < ran.Next(6); i++)
            {
                if (i % 3 == 1)
                {
                    Wolf w = new Wolf(this.Game, hero);

                    w.Initialize();
                    //while(  !w.DestRec.Intersects(hero.LocationRect))
                    //{
                    //    w.Initialize();
                    //}
                    foreach (Antagonist a in enemies)
                    {
                        while (w.DestRec.Intersects(a.DestRec))
                        {
                            w.Initialize();
                        }
                    }
                    
                    enemies.Add(w);
                    hero._enemies.Add(w); //observer
                    
                    w.prevLoc = w.Location;
                }
                else
                {
                    Goblin g = new Goblin(this.Game, hero);
                    g.Initialize();
                    //while (!g.DestRec.Intersects(hero.LocationRect))
                    //{
                    //    g.Initialize();
                    //}
                    foreach (Antagonist a in enemies)
                    {
                        while (g.DestRec.Intersects(a.DestRec))
                        {
                            g.Initialize();
                        }
                    }
                    enemies.Add(g);
                    hero._enemies.Add(g);
                    
                    g.prevLoc = g.Location;
                }
            }
                
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.Enabled)
            {
                foreach (Antagonist a in enemies)
                {
                    if (a.Enabled)
                    {
                        a.Update(gameTime);
                    }
                    
                }
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (this.Visible)
            {
                spritebatch.Begin();
                foreach (Antagonist a in enemies)
                {
                    if(a.Visible)
                        spritebatch.Draw(a.Texture, a.Location, Color.White);
                }
                spritebatch.End();
            }
            base.Draw(gameTime);
        }
    }
}