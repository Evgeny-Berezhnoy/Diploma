using UnityEngine;
using Zenject;

public class InputInitializer
{
    #region Injected methods

    [Inject]
    public void Initialize(
        // GameContextInjector
        [Inject(Id = "GameContext : GameState")] ISubscriptionValue<EGameState> gameState,
        [Inject] ControllersManager<EGameState> controllersManager,
        // InputInjector
        [Inject(Id = "InputInjector : onAxisShift")] ISubscriptionProperty<Vector2> onAxisShift,
        [Inject(Id = "InputInjector : onAwakeGameStateChange")] ISubscriptionProperty<EGameState> onAwakeGameStateChange,
        [Inject(Id = "InputInjector : OnFire")] ISubscriptionProperty onFire,
        [Inject(Id = "InputInjector : onResurrect")] ISubscriptionProperty onResurrect,
        [Inject(Id = "InputInjector : onEscape")] ISubscriptionProperty onEscape)
    {
        var pauseController =
            new PauseController(
                onEscape,
                EGameState.Gameplay,
                gameState,
                onAwakeGameStateChange);

        var inputAxisController = new InputAxisController(onAxisShift);

        var gameplayInputKeysController =
            new InputKeysController(
                new InputKeyInstruction(InputKeys.ESCAPE    , () => onEscape.Invoke(), true),
                new InputKeyInstruction(InputKeys.FIRE      , () => onFire.Invoke()),
                new InputKeyInstruction(InputKeys.RESURRECT , () => onResurrect.Invoke(), true));
        
        var destroyedInputKeysController =
            new InputKeysController(
                new InputKeyInstruction(InputKeys.ESCAPE    , () => onEscape.Invoke(), true));

        var pauseInputKeysController =
            new InputKeysController(
                new InputKeyInstruction(InputKeys.ESCAPE    , () => onEscape.Invoke(), true));

        controllersManager.AddController(inputAxisController, EGameState.Gameplay);
        controllersManager.AddController(gameplayInputKeysController, EGameState.Gameplay);
        controllersManager.AddController(destroyedInputKeysController, EGameState.Destroyed);
        controllersManager.AddController(pauseInputKeysController, EGameState.Pause);
    }

    #endregion
}
