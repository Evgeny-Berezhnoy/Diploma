using UnityEngine;
using Zenject;

public class BackgroundInitializer
{
    #region Injected methods

    [Inject]
    private void Initialize(
        // GameContextInjector
        [Inject] ControllersManager<EGameState> controllersManager,
        [Inject(Id = "GameContext : MainCamera")] Camera camera,
        [Inject(Id = "GameContext : GameobjectsRoot")] Transform gameobjectsRoot,
        // BackgroundSettings
        [Inject(Id = "BackgroundSettings : Prefab")] BackgroundView prefab,
        [Inject(Id = "BackgroundSettings : Speed")] float speed)
    {
        var view = Object.Instantiate(prefab, gameobjectsRoot);

        var backgroundController =
            new BackgroundController(
                view,
                camera,
                speed);

        controllersManager.AddController(backgroundController, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Victory);
    }

    #endregion
}
