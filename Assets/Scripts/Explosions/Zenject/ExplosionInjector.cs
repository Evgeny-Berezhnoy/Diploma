using UnityEngine;
using Zenject;

public class ExplosionInjector : MonoInstaller
{
    #region Fields

    [SerializeField] private Transform _pool;

    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container.BindSubscriptionProperty<Transform>("ExplosionInjector : onExplosion");
        
        Container
            .Bind<Transform>()
            .WithId("ExplosionInjector : Pool")
            .FromInstance(_pool);
    }

    #endregion
}
