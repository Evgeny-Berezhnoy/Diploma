using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Shooting settings", menuName = "Game/Settings/Shooting")]
public class ShootingSettings : ScriptableObjectInstaller
{
    #region Fields

    [SerializeField, Range(1, 8)] private int _hitContactsAmount;
    [SerializeField, Range(0, 20)] private int _spawnerHeatQuantity;
    [SerializeField, Range(0, 6)] private int _spawnerBufferQuantity;
    [SerializeField] private string _sentryService;
    
    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container
            .Bind<int>()
            .WithId("ShootingSettings : hitContactsAmount")
            .FromInstance(_hitContactsAmount);

        Container
            .Bind<int>()
            .WithId("ShootingSettings : SpawnerHeatQuantity")
            .FromInstance(_spawnerHeatQuantity);

        Container
            .Bind<int>()
            .WithId("ShootingSettings : SpawnerBufferQuantity")
            .FromInstance(_spawnerBufferQuantity);

        Container
            .Bind<string>()
            .WithId("ShootingSettings : SentryService")
            .FromInstance(_sentryService);
    }

    #endregion
}
