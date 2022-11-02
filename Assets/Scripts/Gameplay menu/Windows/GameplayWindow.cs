using UnityEngine;
using UnityEngine.UI;

public class GameplayWindow : UIWindow<EGameplayMenuWindow>, IFixedUpdate
{
    #region Fields

    [SerializeField] private Text _resurrectionText;
    [SerializeField, Range(0.16f, 0.5f)] private float _flickerTime;

    private float _currentFlickerTime;
    private bool _resurrectionIsNeeded;

    #endregion

    #region Interface methods

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        if (_resurrectionIsNeeded)
        {
            _currentFlickerTime += fixedDeltaTime;

            if (_currentFlickerTime >= _flickerTime)
            {
                _currentFlickerTime = 0;

                _resurrectionText.enabled = !_resurrectionText.enabled;
            };
        };
    }

    #endregion

    #region Methods

    public void Initialize(ISubscriptionProperty<bool> onCheckResurrectNecessity)
    {
        onCheckResurrectNecessity.Subscribe(SetResurrectNecessity);
    }

    private void SetResurrectNecessity(bool resurrectionIsNeeded)
    {
        _resurrectionIsNeeded = resurrectionIsNeeded;

        if (!_resurrectionIsNeeded && _resurrectionText.enabled)
        {
            _currentFlickerTime = 0;

            _resurrectionText.enabled = false;
        };
    }

    #endregion
}
