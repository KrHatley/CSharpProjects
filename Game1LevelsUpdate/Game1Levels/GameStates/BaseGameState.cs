using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Util;
using MonoGameLibrary.State;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1Levels.GameStates
{
    public partial class BaseGameState :GameState
    {
        public SpriteBatch spritebatch;
        public IGameStateManager GameStateManager;
        public BaseGameState(Game game, IGameStateManager manager):base(game)
        {
            this.GameStateManager = manager;
        }
        protected override void LoadContent()
        {
            this.spritebatch = new SpriteBatch(this.Game.GraphicsDevice);
            base.LoadContent();
        }
    }
}
