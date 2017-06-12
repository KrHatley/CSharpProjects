using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels.GameStates
{
    public interface ITitleState: IGameState { }
   public sealed class TitleScreen:BaseGameState,ITitleState
    {
        private Texture2D texture;
        private SpriteFont font;

        public TitleScreen(Game game,IGameStateManager manager):base(game,manager)
        {
            game.Services.AddService(typeof(ITitleState), this);
        }
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)==true)
            {
                GameManager.PushState(((ScreenStrategyGameStateManager)(this.GameManager)).InstructionsState.Value);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) == true)
            {
                GameManager.PushState(((ScreenStrategyGameStateManager)(this.GameManager)).InstructionsState.Value);
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            this.Game.GraphicsDevice.Clear(Color.Black);
            Vector2 position = new Vector2(100, 100);
            spritebatch.Begin();
            spritebatch.Draw(texture, position, Color.White);
            spritebatch.DrawString(font,"press space to continue", new Vector2(this.Game.GraphicsDevice.Viewport.Height - (this.Game.GraphicsDevice.Viewport.Height / 3), this.GraphicsDevice.Viewport.Width / 2), Color.Red);
            spritebatch.End();
            base.Draw(gameTime);
        }
        protected override void LoadContent()
        {
            texture = this.Game.Content.Load<Texture2D>("Title");
            font = this.Game.Content.Load<SpriteFont>("Xanadu");
            base.LoadContent();
        }
    }
}
