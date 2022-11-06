using UnityEngine;
using Zenject;

public class SpecialEffectInitializer
{
    #region Injected methods

    [Inject]
    private void Initialize(
        // GameContextInjector
        [Inject] ControllersManager<EGameState> controllersManager,
        // SpecialEffectSettings
        [Inject(Id = "SpecialEffectSettings : HeatQuantity")] int heatQuantity,
        [Inject(Id = "SpecialEffectSettings : Prefab")] GameObject prefab,
        // ExplosionInjector
        [Inject(Id = "SpecialEffectInjector : Survey")] ISubscriptionSurvey<SpecialEffectController> specialEffectSurvey,
        [Inject(Id = "SpecialEffectInjector : Pool")] Transform pool)
    {
        var registrator = new SpecialEffectViewRegistrator(specialEffectSurvey);

        var spawner = new SpecialEffectSpawner(prefab, pool, heatQuantity);

        var service = new SpecialEffectService(spawner, specialEffectSurvey);

        controllersManager.AddController(registrator, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Victory);
        controllersManager.AddController(service, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Victory);
    }

    #endregion
}
