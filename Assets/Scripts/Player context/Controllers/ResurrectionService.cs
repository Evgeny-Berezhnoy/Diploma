using Photon.Pun;

public class ResurrectionService : IController
{
    #region Fields

    private ResurectionView _view;
    
    #endregion

    #region Observers

    private ISubscriptionProperty _onResurrection;

    #endregion

    #region Constructors

    public ResurrectionService(ISubscriptionProperty onResurrection)
    {
        _onResurrection = onResurrection;

        PhotonCore.Instance.AddViewInstantiationListener(OnViewInstantiated);
    }

    #endregion

    #region Methods

    public void Resurrect(int addresser)
    {
        _view.RecordResurrectionMessage(addresser);
    }

    private void OnViewInstantiated(PhotonView photonView)
    {
        var view =
            photonView
                .gameObject
                .GetComponent<ResurectionView>();

        if (!view) return;

        view.OnResurrection = _onResurrection;

        if (photonView.IsMine)
        {
            _view = view;
        };
    }

    #endregion
}
