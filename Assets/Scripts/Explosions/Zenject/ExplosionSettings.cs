using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Explosion settings", menuName = "Game/Settings/Explosions")]
public class ExplosionSettings : ScriptableObjectInstaller
{
    #region Fields

    [SerializeField, Range(2, 6)] private int _spawnerBufferQuantity;
    [SerializeField] private ExplosionData _data;

    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container
            .Bind<int>()
            .WithId("ExplosionSettings : BufferQuantity")
            .FromInstance(_spawnerBufferQuantity);

        Container
            .Bind<ExplosionData>()
            .WithId("ExplosionSettings : Data")
            .FromInstance(_data);
    }

    #endregion
}
