using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels
{
    public class Camp : DrawableGameComponent, IObserver
    {
        Texture2D texture;
        public Rectangle location;
        int uses = 4;
        SpriteBatch sb;

        public Camp(Game game, Hero h) : base(game)
        {
            h.observers.Add(this);
            h._Camps.Add(this);
        }
        public override void Initialize()
        {
            texture = Game.Content.Load<Texture2D>("Camp");
            location = new Rectangle((this.Game.GraphicsDevice.Viewport.Width / 2) - 20, (this.Game.GraphicsDevice.Viewport.Height / 2) - 50, 50, 50);
            sb = new SpriteBatch(this.Game.GraphicsDevice);

            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            if (uses <= 0)
            {
                texture = Game.Content.Load<Texture2D>("Burntout");
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.Draw(texture, location, Color.White);
            sb.End();
            base.Draw(gameTime);
        }

        public void ObserverUpdate(object sender, object message)
        {
            if (sender is Hero && message is string)
            {
                if ((string)message == "Heal")
                {
                    --uses;
                }
                if ((string)message == "Touched Top")
                {
                    this.location.Y -= this.GraphicsDevice.Viewport.Height;



                }
                if ((string)message == "Touched Bottom")
                {

                    this.location.Y += this.GraphicsDevice.Viewport.Height;




                }
                if ((string)message == "Touched Right")
                {

                    this.location.X += this.GraphicsDevice.Viewport.Width;



                }
                if ((string)message == "Touched Left")
                {


                    this.location.X -= this.GraphicsDevice.Viewport.Width;




                }

            }
        }
    }
}
