using UnityEngine;
using Zenject;

public class PlayerContextInjector : MonoInstaller
{
    #region Base methods

    public override void InstallBindings()
    {
        Container.BindSubscriptionProperty<Collider2D>("PlayerContext : onResurrectionContact");
        Container.BindSubscriptionProperty<bool>("PlayerContext : onCheckResurrectNecessity");
    }

    #endregion
}
