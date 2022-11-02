using Photon.Pun;
using Zenject;

public class GameInitializer
{
    #region Injected methods

    [Inject]
    public void Initialize(
        // AbstractionsInjector
        [Inject] DiContainerWrapper diContainer)
    {
        diContainer.Inject(new InputInitializer());
        diContainer.Inject(new GameplayMenuInitializer());
        diContainer.Inject(new GameTransitionInitializer());
        diContainer.Inject(new PlayerContextInitializer());
        diContainer.Inject(new ShootingInitializer());
        diContainer.Inject(new NetworkInitializer());
        diContainer.Inject(new AudioInitializer());

        if (PhotonNetwork.IsMasterClient)
        {
            diContainer.Inject(new EnemyInitializer());
        };
    }

    #endregion
}