using UnityEngine;

public class SpecialEffectViewRegistrator : IController
{
    #region Fields

    private ISpecialEffectSource _specialEffectSource;

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

    private void OnViewInstantiated(MonoBehaviour view)
    {
        _specialEffectSource = view as ISpecialEffectSource;

        if (_specialEffectSource == null) return;

        _specialEffectSource.SpecialEffectSurvey = _specialEffectSurvey;
    }

    #endregion
}
