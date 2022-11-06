using UnityEngine;
using Zenject;

public class SpecialEffectInjector : MonoInstaller
{
    #region Fields

    [SerializeField] private Transform _pool;

    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container.BindSubscriptionSurvey<SpecialEffectController>("SpecialEffectInjector : Survey");
        
        Container
            .Bind<Transform>()
            .WithId("SpecialEffectInjector : Pool")
            .FromInstance(_pool);
    }

    #endregion
}
