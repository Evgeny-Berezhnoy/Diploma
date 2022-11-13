using UnityEngine;
using Zenject;

public class EnemiesInjector : MonoInstaller
{
    #region Fields

    [SerializeField] private Transform _pool;
    [SerializeField] private EnemyMap _movementMap;

    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container
            .Bind<Transform>()
            .WithId("EnemiesInjector : Pool")
            .FromInstance(_pool);

        Container
            .Bind<EnemyMap>()
            .WithId("EnemiesInjector : MovementMap")
            .FromInstance(_movementMap);

        Container.BindSubscriptionProperty("EnemiesInjector : OnEnemySequenceEnd");
        Container.BindSubscriptionProperty("EnemiesInjector : onAllEnemiesDestroyed");
        Container.BindSubscriptionProperty<EnemyController>("EnemiesInjector : OnAddController");
        Container.BindSubscriptionProperty<EnemyController>("EnemiesInjector : OnRemoveController");
    }

    #endregion
}
