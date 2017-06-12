using Microsoft.Xna.Framework;
using MonoGameLibrary.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1Levels.GameStates
{
    class ScreenStrategyGameStateManager:GameStateManager
    {
        public ITitleState TitleScreen;//first State
        public IinstructionState InstructionsState;
        public IPlayingState PlayingState;
        public IinventoryState inventoryState;
       
        public ScreenStrategyGameStateManager(Game game):base(game)
        {
            TitleScreen = new TitleScreen(game, this);
            InstructionsState = new InstructionsState(game, this);
            inventoryState = new InventoryState(game, this);
            
            PlayingState = new PlayingState(game, this, inventoryState);
           
            this.ChangeState(TitleScreen.Value);
        }
    }
}
