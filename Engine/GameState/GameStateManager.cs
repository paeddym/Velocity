namespace Engine
{
    public static class GameStateManager
    {
        public enum GameState
        {
            MainMenu,
            Playing,
            Paused,
            Countdown
        }

        private static GameState _currentState = GameState.MainMenu;
        public static GameState CurrentState => _currentState;
        public static void ChangeState(GameState newState)
        {
            _currentState = newState;
        }
        public static bool IsState(GameState state)
        {
            return _currentState == state;
        }
    }
}
