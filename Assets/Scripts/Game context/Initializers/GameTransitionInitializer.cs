using Zenject;

public class GameTransitionInitializer
{
    #region Injected methods

    [Inject]
    private void InitializeTransitions(
        // GameContextInjector
        [Inject(Id = "GameContext : GameState")] ISubscriptionValue<EGameState> gameState,
        [Inject(Id = "GameContext : onDeath")] ISubscriptionProperty onDeath,
        [Inject(Id = "GameContext : onResurrection")] ISubscriptionProperty onResurrection,
        [Inject(Id = "GameContext : onDefeat")] ISubscriptionProperty onDefeat,
        [Inject(Id = "GameContext : onVictory")] ISubscriptionProperty onVictory,
        [Inject(Id = "GameContext : onRetry")] ISubscriptionProperty onRetry,
        [Inject(Id = "GameContext : onQuitGame")] ISubscriptionProperty onQuitGame,
        // InputInjector
        [Inject(Id = "InputInjector : onAwakeGameStateChange")] ISubscriptionProperty<EGameState> onAwakeGameStateChange)
    {
        onDeath.Subscribe(() => onAwakeGameStateChange.Value = EGameState.Destroyed);
        onDeath.Subscribe(() => gameState.Value = EGameState.Destroyed);

        onResurrection.Subscribe(() => onAwakeGameStateChange.Value = EGameState.Gameplay);
        onResurrection.Subscribe(() => gameState.Value = EGameState.Gameplay);
        
        onDefeat.Subscribe(() => gameState.Value = EGameState.Defeat);

        onVictory.Subscribe(() => gameState.Value = EGameState.Victory);

        onRetry.Subscribe(() => onAwakeGameStateChange.Value = EGameState.Gameplay);
        onRetry.Subscribe(() => gameState.Value = EGameState.Gameplay);

        onQuitGame.Subscribe(() => gameState.Value = EGameState.Quit);
        onQuitGame.Subscribe(PhotonCore.Instance.Disconnect);
    }

    #endregion
}
