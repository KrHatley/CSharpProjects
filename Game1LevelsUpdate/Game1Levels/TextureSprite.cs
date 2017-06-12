using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels
{
 public  class TextureSprite: DrawableGameComponent //MonoGameLibrary.Sprite.DrawableAnimatableSprite
    {
        public Texture2D Texture;
       public Vector2 Location;
        public Rectangle DestRec;
        protected Vector2 Direction;
        protected float Speed;
        public SpriteBatch spriteBatch;
        public Vector2 origin;

        public TextureSprite(Game game) : base(game)
        {

        }
        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
           // this.Texture = this.Game.Content.Load<Texture2D>("one");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            DrawTexture();
            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected virtual void DrawTexture()
        {
            spriteBatch.Draw(this.Texture, this.Location, Color.White);
        }
    }
}
