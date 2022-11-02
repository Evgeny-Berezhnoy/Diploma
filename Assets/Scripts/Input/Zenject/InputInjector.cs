using UnityEngine;
using Zenject;

public class InputInjector : MonoInstaller
{
    #region Base methods

    public override void InstallBindings()
    {
        Container.BindSubscriptionProperty<Vector2>("InputInjector : onAxisShift");
        Container.BindSubscriptionProperty<EGameState>("InputInjector : onAwakeGameStateChange");
        Container.BindSubscriptionProperty("InputInjector : OnFire");
        Container.BindSubscriptionProperty("InputInjector : onResurrect");
        Container.BindSubscriptionProperty("InputInjector : onEscape");
        Container.BindSubscriptionProperty("InputInjector : onRetry");
    }

    #endregion
}
