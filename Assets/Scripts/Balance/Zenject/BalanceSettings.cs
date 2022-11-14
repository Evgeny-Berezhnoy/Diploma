using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Balance settings", menuName = "Game/Settings/Balance")]
public class BalanceSettings : ScriptableObjectInstaller
{
    #region Fields

    [SerializeField] private ShipCoefficient[] _coefficients;

    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container
            .Bind<ShipCoefficient[]>()
            .FromInstance(_coefficients);
    }

    #endregion
}
