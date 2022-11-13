using UnityEngine;

public class ResurrectionViewRegistrator : IController
{
    #region Fields

    private ResurectionView _resurrector;

    #endregion

    #region Observers

    private ISubscriptionProperty _onResurrection;

    #endregion

    #region Constructors

    public ResurrectionViewRegistrator(ISubscriptionProperty onResurrection)
    {
        _onResurrection = onResurrection;

        PhotonCore.Instance.AddViewInstantiationListener(OnViewInstantiated);
    }

    #endregion

    #region Methods

    private void OnViewInstantiated(MonoBehaviour view)
    {
        _resurrector = view as ResurectionView;

        if (!_resurrector) return;

        _resurrector.OnResurrection = _onResurrection;
    }

    #endregion
}
