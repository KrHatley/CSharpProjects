using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.State;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels.GameStates
{
    public interface IinventoryState: IGameState { }
    class InventoryState:BaseGameState,IinventoryState
    {
        private Texture2D inventorytexture;
        private SpriteFont font;
        internal Hero dummy;
       
        InputHandler input;

        public InventoryState(Game game, IGameStateManager manager) : base(game, manager)
        {
            game.Services.AddService(typeof(IinventoryState), this);
            
           
            
            input = (InputHandler)this.Game.Services.GetService<IInputHandler>();
            if (input == null)
            {
                input = new InputHandler(this.Game);
                this.Game.Components.Add(input);
            }
        }
        protected override void LoadContent()
        {
            this.inventorytexture = this.Game.Content.Load<Texture2D>("inventory");
            this.font = this.Game.Content.Load<SpriteFont>("Xanadu");
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            if (input.KeyboardState.HasReleasedKey(Keys.I)==true)
            {
                
                GameManager.PopState();
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            Rectangle fullscreen = new Rectangle(0, 0, this.Game.GraphicsDevice.Viewport.Width, this.Game.GraphicsDevice.Viewport.Height);
            spritebatch.Begin();
            this.spritebatch.Draw(inventorytexture, fullscreen, Color.White);
            this.spritebatch.DrawString(font, "Stats: press 'i' to resume", new Vector2(this.Game.GraphicsDevice.Viewport.Width/4,
                this.Game.GraphicsDevice.Viewport.Height-(this.Game.GraphicsDevice.Viewport.Height/16)), Color.White);
            this.spritebatch.DrawString(font,  dummy.HP.ToString()+"/"+dummy.Maxhitpoints.ToString(), new Vector2((this.Game.GraphicsDevice.Viewport.Width/2), 
                (this.Game.GraphicsDevice.Viewport.Height -(this.Game.GraphicsDevice.Viewport.Height/6))), Color.White);
            this.spritebatch.DrawString(font, dummy.ATK.ToString(), new Vector2((this.Game.GraphicsDevice.Viewport.Width / 2),
                (this.Game.GraphicsDevice.Viewport.Height - 4*(this.Game.GraphicsDevice.Viewport.Height / 6))), Color.White);
            this.spritebatch.DrawString(font,  dummy.DEF.ToString(), new Vector2((this.Game.GraphicsDevice.Viewport.Width / 2),
                (this.Game.GraphicsDevice.Viewport.Height - 3*(this.Game.GraphicsDevice.Viewport.Height / 6))), Color.White);
            this.spritebatch.DrawString(font, dummy.SPL.ToString(), new Vector2((this.Game.GraphicsDevice.Viewport.Width / 2),
                (this.Game.GraphicsDevice.Viewport.Height - 2*(this.Game.GraphicsDevice.Viewport.Height / 6))), Color.White);
            this.spritebatch.DrawString(font,  dummy.lvl.ToString(), new Vector2((this.Game.GraphicsDevice.Viewport.Width / 2),
                (this.Game.GraphicsDevice.Viewport.Height - 5 * (this.Game.GraphicsDevice.Viewport.Height / 6))), Color.White);
            spritebatch.End();


            base.Draw(gameTime);


        }
    }
}
