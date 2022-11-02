using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    #region Injected fields

    // AbstractionsInjector
    [Inject] private DiContainerWrapper _diContainer;
    [Inject] private Disposer _disposer;
    // GameContextInjector
    [Inject(Id = "GameContext : GameState")] private ISubscriptionValue<EGameState> _gameState;
    [Inject] private ControllersManager<EGameState> _controllersManager;

    #endregion

    #region Unity events

    private void Start()
    {
        _diContainer.Inject(new GameInitializer());

        PhotonCore.Instance.AddQuitGameplaySceneListener(_disposer.Dispose);

        _gameState.Value = EGameState.Gameplay;
    }

    private void Update()
    {
        _controllersManager.OnUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _controllersManager.OnFixedUpdate(Time.fixedDeltaTime);
    }

    private void OnDestroy()
    {
        _disposer.Dispose();
    }

    #endregion
}