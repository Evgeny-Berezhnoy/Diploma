using UnityEngine;
using Zenject;

public class GameContextInjector : MonoInstaller
{
    #region Fields

    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _gameobjectsRoot;
    [SerializeField] private Transform _pool;
    [SerializeField] private Transform[] _spawnPoints;

    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container
            .Bind<Camera>()
            .WithId("GameContext : MainCamera")
            .FromInstance(_camera);

        Container
            .Bind<Transform>()
            .WithId("GameContext : GameobjectsRoot")
            .FromInstance(_gameobjectsRoot);

        Container
            .Bind<Transform>()
            .WithId("GameContext : Pool")
            .FromInstance(_pool);

        Container
            .Bind<Transform[]>()
            .WithId("GameContext : SpawnPoints")
            .FromInstance(_spawnPoints);

        Container.BindSubscriptionValue<EGameState>("GameContext : GameState");

        Container
            .Bind<ControllersManager<EGameState>>()
            .AsCached();

        Container.BindSubscriptionProperty("GameContext : onDeath");
        Container.BindSubscriptionProperty("GameContext : onResurrection");
        Container.BindSubscriptionProperty("GameContext : onDefeat");
        Container.BindSubscriptionProperty("GameContext : onVictory");
        Container.BindSubscriptionProperty("GameContext : onRetry");
        Container.BindSubscriptionProperty("GameContext : onQuitGame");
    }

    #endregion
}