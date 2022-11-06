using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Special effect settings", menuName = "Game/Settings/Special effect settings")]
public class SpecialEffectSettings : ScriptableObjectInstaller
{
    #region Fields

    [SerializeField, Range(2, 6)] private int _spawnerHeatQuantity;
    [SerializeField] private GameObject _prefab;

    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container
            .Bind<int>()
            .WithId("SpecialEffectSettings : HeatQuantity")
            .FromInstance(_spawnerHeatQuantity);

        Container
            .Bind<GameObject>()
            .WithId("SpecialEffectSettings : Prefab")
            .FromInstance(_prefab);
    }

    #endregion
}
