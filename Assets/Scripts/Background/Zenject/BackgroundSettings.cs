using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Background settings", menuName = "Game/Settings/Background")]
public class BackgroundSettings : ScriptableObjectInstaller
{
    #region Fields

    [SerializeField] private GameObject _prefab;
    [SerializeField, Range(0.05f, 1f)] private float _speed;

    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container
            .Bind<BackgroundView>()
            .WithId("BackgroundSettings : Prefab")
            .FromComponentOn(_prefab)
            .AsCached();

        Container
            .Bind<float>()
            .WithId("BackgroundSettings : Speed")
            .FromInstance(_speed);
    }

    #endregion
}
