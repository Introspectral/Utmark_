using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utmark_ECS.Enums;

namespace Utmark_ECS.Managers
{
    public class GameManager
    {
        public GameState CurrentState { get; private set; }

        public GameManager()
        {
            CurrentState = GameState.GamePlay; // Initial state
        }

        public void ChangeState(GameState newState)
        {
            CurrentState = newState;

            // Handle the new state transition if necessary
            // For example, setting up the UI, pausing the game, etc.
        }
    }
}
