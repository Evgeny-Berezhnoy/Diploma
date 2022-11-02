using UnityEngine;
using Zenject;
using ExitGames.Client.Photon;

public class NetworkInitializer
{
    #region Injected fields

    [Inject]
    public void Initialize(
        // AbstractionsInjector
        [Inject] Disposer disposer,
        // GameContextInjector
        [Inject] ControllersManager<EGameState> controllersManager,
        [Inject(Id = "GameContext : GameState")] ISubscriptionValue<EGameState> gameState,
        [Inject(Id = "GameContext : onVictory")] ISubscriptionProperty onVictory,
        [Inject(Id = "GameContext : onRetry")] ISubscriptionProperty onRetry,
        // PlayerContextInjector
        [Inject(Id = "PlayerContext : onResurrectionContact")] ISubscriptionProperty<Collider2D> onResurrectionContact,
        // EnemiesInjector
        [Inject(Id = "EnemiesInjector : onAllEnemiesDestroyed")] ISubscriptionProperty onAllEnemiesDestroyed,
        // InputInjector
        [Inject(Id = "InputInjector : onRetry")] ISubscriptionProperty onRetryPress)
    {
        var photonGameStateNotifier = new PhotonGameStateNotifier(onVictory, onRetry, gameState);

        onAllEnemiesDestroyed.Subscribe(photonGameStateNotifier.OnVictory);

        onRetryPress.Subscribe(photonGameStateNotifier.OnRetry);

        controllersManager.AddController(photonGameStateNotifier, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Victory);

        onResurrectionContact.Subscribe(_ =>
        {
            PlayfabCore.Instance.RecordStatisticChange(PlayfabPlayerStatistic.RESURRECTIONS, 1);
        });

        disposer.Subscribe(photonGameStateNotifier.Dispose);

        PhotonPeer.RegisterType(typeof(NetworkMessage), (byte)'M', NetworkMessage.Serialize, NetworkMessage.Deserialize);
    }

    #endregion
}
