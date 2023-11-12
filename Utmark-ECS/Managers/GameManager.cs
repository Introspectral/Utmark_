using Utmark_ECS.Enums;

namespace Utmark_ECS.Managers
{
    public class GameManager
    {
        public GameState CurrentState { get; private set; }

        public GameManager()
        {
            CurrentState = GameState.GamePlay;
        }

        public void ChangeState(GameState newState)
        {
            CurrentState = newState;

        }
    }
}
