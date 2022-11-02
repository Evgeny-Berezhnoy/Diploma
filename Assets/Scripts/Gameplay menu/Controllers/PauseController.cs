public class PauseController : IController
{
    #region Fields

    private EGameState _awakeGameState;

    #endregion

    #region Observers

    private ISubscriptionValue<EGameState> _gameState;

    #endregion

    #region Constructors

    public PauseController(
        ISubscriptionProperty onEscape,
        EGameState awakeGameState,
        ISubscriptionValue<EGameState> gameState,
        ISubscriptionProperty<EGameState> onAwakeGameStateChange)
    {
        onEscape.Subscribe(OnEscape);

        _awakeGameState = awakeGameState;

        _gameState = gameState;

        onAwakeGameStateChange.Subscribe(OnAwakeGameStateChange);
    }

    #endregion

    #region Methods

    private void OnEscape()
    {
        if(_gameState.Value == _awakeGameState)
        {
            _gameState.Value = EGameState.Pause;
        }
        else
        {
            _gameState.Value = _awakeGameState;
        };
    }

    private void OnAwakeGameStateChange(EGameState target)
    {
        _awakeGameState = target;
    }

    #endregion
}
