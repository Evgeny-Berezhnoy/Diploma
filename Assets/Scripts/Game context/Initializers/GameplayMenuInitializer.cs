﻿using Zenject;

public class GameplayMenuInitializer
{
    #region Injected methods

    [Inject]
    private void Initialize(
        // GameContextInjector
        [Inject(Id = "GameContext : GameState")] ISubscriptionValue<EGameState> gameState,
        [Inject] ControllersManager<EGameState> controllersManager,
        [Inject(Id = "GameContext : onQuitGame")] ISubscriptionProperty onQuitGame,
        // PlayerContextInjector
        [Inject(Id = "PlayerContext : onCheckResurrectNecessity")] ISubscriptionProperty<bool> onCheckResurrectNecessity,
        [Inject(Id = "PlayerContext : onPlayerHealthChanged")] ISubscriptionProperty<float> onPlayerHealthChanged,
        // InputInjector
        [Inject(Id = "InputInjector : onEscape")] ISubscriptionProperty onEscape,
        [Inject(Id = "InputInjector : onRetry")] ISubscriptionProperty onRetryPress,
        // GameplayMenuInjector
        [Inject] GameplayMenuController controller)
    {
        controller
            .Initialize(
                gameState,
                onCheckResurrectNecessity,
                onPlayerHealthChanged,
                onEscape,
                onRetryPress,
                onQuitGame);

        controllersManager.AddController(controller, EGameState.Gameplay);
    }

    #endregion
}
