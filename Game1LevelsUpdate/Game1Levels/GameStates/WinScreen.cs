using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels.GameStates
{
    public interface IWinScreen : IGameState { }
    public class WinScreen:BaseGameState,IWinScreen
    {

        private Texture2D wintexture;
        private SpriteFont font;
        internal Hero hero;
        public WinScreen(Game game, IGameStateManager manager,Hero h):base(game,manager)
        {
            this.hero = h;

        }
        protected override void LoadContent()
        {
           this.wintexture = this.Game.Content.Load<Texture2D>("winscreen");
            font = this.Game.Content.Load<SpriteFont>("Xanadu");
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            hero.Visible = true;
            hero.Location = new Vector2((this.Game.GraphicsDevice.Viewport.Width / 2) - hero.SpriteTexture.Width, (this.Game.GraphicsDevice.Viewport.Height / 2) - hero.SpriteTexture.Height);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            spritebatch.Begin();

            spritebatch.Draw(this.wintexture, new Rectangle(0, 0, this.Game.GraphicsDevice.Viewport.Width, this.Game.GraphicsDevice.Viewport.Height), Color.White);
            spritebatch.DrawString(font, "press escape to close the game", new Vector2(hero.Location.X, hero.Location.Y + hero.SpriteTexture.Height),Color.Red);
            spritebatch.DrawString(font, "Congratulations!!! You Won", new Vector2(hero.Location.X, hero.Location.Y - hero.SpriteTexture.Height), Color.Red);
            spritebatch.Draw(this.hero.SpriteTexture, this.hero.Location, Color.White);
            spritebatch.End();

            base.Draw(gameTime);
        }
    }
}
