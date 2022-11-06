using Photon.Pun;

public class SpecialEffectViewRegistrator : IController
{
    #region Fields

    private ISpecialEffectSource _view;

    #endregion

    #region Observers

    private ISubscriptionSurvey<SpecialEffectController> _specialEffectSurvey;

    #endregion

    #region Constructors

    public SpecialEffectViewRegistrator(ISubscriptionSurvey<SpecialEffectController> specialEffectSurvey)
    {
        _specialEffectSurvey = specialEffectSurvey;

        PhotonCore.Instance.AddViewInstantiationListener(OnViewInstantiated);
    }

    #endregion

    #region Methods

    private void OnViewInstantiated(PhotonView photonView)
    {
        _view =
            photonView
                .gameObject
                .GetComponent<ISpecialEffectSource>();

        if (_view == null) return;

        _view.SpecialEffectSurvey = _specialEffectSurvey;
    }

    #endregion
}
