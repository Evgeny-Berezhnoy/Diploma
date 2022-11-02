using System.Linq;

public class GameplayMenuController : UIWindowsController<EGameplayMenuWindow>, IFixedUpdate
{
    #region Methods

    public void Initialize(
        ISubscriptionValue<EGameState> gameState,
        ISubscriptionProperty<bool> onCheckResurrectNecessity,
        ISubscriptionProperty onEscape,
        ISubscriptionProperty onRetry,
        ISubscriptionProperty onGameQuit)
    {
        Initialize();

        gameState.Subscribe(OnGameStateChanged);

        _windows
            .Where(x => x.WindowType.Equals(EGameplayMenuWindow.Gameplay))
            .Cast<GameplayWindow>()
            .First()
            .Initialize(onCheckResurrectNecessity);

        _windows
            .Where(x => x.WindowType.Equals(EGameplayMenuWindow.Pause))
            .Cast<PauseWindow>()
            .First()
            .Initialize(onEscape, onGameQuit);

        _windows
            .Where(x => x.WindowType.Equals(EGameplayMenuWindow.FinishGame))
            .Cast<FinishGameWindow>()
            .First()
            .Initialize(onRetry, onGameQuit);
    }

    #endregion

    #region Interface Methods

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        if (_activeWindow is IFixedUpdate fixedUpdateWindow)
        {
            fixedUpdateWindow.OnFixedUpdate(fixedDeltaTime);
        };
    }

    #endregion

    #region Methods

    private void OnGameStateChanged(EGameState gameState)
    {
        if(gameState == EGameState.Pause)
        {
            SwitchOnWindow(EGameplayMenuWindow.Pause, null);
        }
        else if(gameState == EGameState.Victory)
        {
            SwitchOnWindow(EGameplayMenuWindow.FinishGame, new FinishGameWindowData(true));
        }
        else if(gameState == EGameState.Defeat)
        {
            SwitchOnWindow(EGameplayMenuWindow.FinishGame, new FinishGameWindowData(false));
        }
        else
        {
            SwitchOnWindow(EGameplayMenuWindow.Gameplay, null);
        };
    }

    #endregion
}
