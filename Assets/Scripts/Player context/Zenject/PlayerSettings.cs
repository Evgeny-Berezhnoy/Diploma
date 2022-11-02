using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Player settings", menuName = "Player/Player settings")]
public class PlayerSettings : ScriptableObjectInstaller
{
    #region Fields

    [SerializeField, Range(1, 4)] private int _resurrectionContactsAmount;
    [SerializeField] private string _resurrectionViewPrefab;

    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container
            .Bind<int>()
            .WithId("PlayerSettings : ResurrectionContactsAmount")
            .FromInstance(_resurrectionContactsAmount);

        Container
            .Bind<string>()
            .WithId("PlayerSettings : ResurrectionViewPrefab")
            .FromInstance(_resurrectionViewPrefab);
    }

    #endregion
}
