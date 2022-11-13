using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Enemy settings", menuName = "Game/Settings/Enemies")]
public class EnemySettings : ScriptableObjectInstaller
{
    #region Fields

    [SerializeField, Range(0, 20)] private int _spawnerHeatQuantity;
    [SerializeField, Range(0, 6)] private int _spawnerBufferQuantity;
    [SerializeField, Range(3, 10)] private float _spawnInterval;
    [SerializeField] private EnemySequenceData _sequence;
    [SerializeField] private string _sentryService;

    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container
            .Bind<int>()
            .WithId("EnemySettings : SpawnerHeatQuantity")
            .FromInstance(_spawnerHeatQuantity);

        Container
            .Bind<int>()
            .WithId("EnemySettings : SpawnerBufferQuantity")
            .FromInstance(_spawnerBufferQuantity);

        Container
            .Bind<float>()
            .WithId("EnemySettings : SpawnInterval")
            .FromInstance(_spawnInterval);

        Container
            .Bind<EnemySequenceData>()
            .FromInstance(_sequence);

        Container
            .Bind<string>()
            .WithId("EnemySettings : SentryService")
            .FromInstance(_sentryService);
    }

    #endregion
}
