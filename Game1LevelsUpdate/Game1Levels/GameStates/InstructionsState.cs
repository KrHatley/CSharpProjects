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
    public interface IinstructionState: IGameState { }
   public sealed class InstructionsState:BaseGameState,IinstructionState
    {
        private SpriteFont font;

        public InstructionsState(Game game,IGameStateManager manager): base(game,manager)
        {
            game.Services.AddService(typeof(IinstructionState), this);
        }
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)==true|| Keyboard.GetState().IsKeyDown(Keys.Back) == true)
            {
                GameManager.ChangeState(((ScreenStrategyGameStateManager)(this.GameStateManager)).InstructionsState.Value);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true )
            {
                GameManager.PopState();//pop myself off the stack
                GameManager.ChangeState(((ScreenStrategyGameStateManager)(this.GameStateManager)).PlayingState.Value);
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            this.spritebatch.Begin();
            this.spritebatch.DrawString(font, "Move Hero with the A,S,W,D keys.", new Vector2(100, 100), Color.BlanchedAlmond);
            this.spritebatch.DrawString(font, "Battle the Monsters.", new Vector2(100, 150), Color.BlanchedAlmond);
            this.spritebatch.DrawString(font, "Press ESC to pause", new Vector2(100, 200), Color.BlanchedAlmond);
            this.spritebatch.DrawString(font, "Press I to open Stats", new Vector2(100, 250), Color.BlanchedAlmond);
            this.spritebatch.DrawString(font, "Press enter to start", new Vector2(100, 300), Color.BlanchedAlmond);
            this.spritebatch.End();
            base.Draw(gameTime);
        }
        protected override void StateChanged(object sender,EventArgs e)
        {
            base.StateChanged(sender, e);
            if (GameManager.State != this.Value)
            {
                Visible = true;
            }
        }
        protected override void LoadContent()
        {
            font = this.Game.Content.Load<SpriteFont>("Arial");
            base.LoadContent();
        }
    }
}
