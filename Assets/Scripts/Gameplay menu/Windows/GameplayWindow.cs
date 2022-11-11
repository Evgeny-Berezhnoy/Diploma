using UnityEngine;
using UnityEngine.UI;

public class GameplayWindow : UIWindow<EGameplayMenuWindow>, IFixedUpdate
{
    #region Fields

    [Header("Components")]
    [SerializeField] private Text _resurrectionText;
    [SerializeField] private Transform _playerPicture;
    [SerializeField] private Image _health;
    
    [Header("Settings")]
    [SerializeField, Range(0.16f, 0.5f)] private float _flickerTime;
    [SerializeField, Range(0.01f, 0.3f)] private float _healthChangeSpeed;

    private bool _resurrectionIsNeeded;
    private float _flickerTimeCurrent;
    private float _healthChangeSpeedCurrent;
    private float _healthRatio;
    private float _healthRatioCurrent;
    private float _healthRatioDifference;

    #endregion

    #region Interface methods

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        if (_resurrectionIsNeeded)
        {
            _flickerTimeCurrent += fixedDeltaTime;

            if (_flickerTimeCurrent >= _flickerTime)
            {
                _flickerTimeCurrent = 0;

                _resurrectionText.gameObject.SetActive(!_resurrectionText.gameObject.activeSelf);
            };
        };
        
        _healthChangeSpeedCurrent = _healthChangeSpeed * fixedDeltaTime;
        
        if(_healthRatio != _healthRatioCurrent)
        {
            _healthRatioDifference = _healthRatio - _healthRatioCurrent;

            if (Mathf.Abs(_healthRatioDifference) <= _healthChangeSpeedCurrent)
            {
                _healthRatioCurrent = _healthRatio;
            }
            else
            {
                _healthRatioCurrent += (_healthChangeSpeedCurrent * Mathf.Sign(_healthRatioDifference));                
            };

            _health.fillAmount = _healthRatioCurrent;
        };

        if (_healthRatio > 0)
        {
            _playerPicture.Rotate(Vector3.forward, -360 * _healthChangeSpeedCurrent * _healthRatioCurrent);
        };
    }

    #endregion

    #region Methods

    public void Initialize(
        ISubscriptionProperty<bool> onCheckResurrectNecessity,
        ISubscriptionProperty<float> onPlayerHealthChanged)
    {
        _healthRatio        = 1;
        _healthRatioCurrent = 1;

        onCheckResurrectNecessity.Subscribe(SetResurrectNecessity);
        onPlayerHealthChanged.Subscribe(OnHealthChanged);
    }

    private void SetResurrectNecessity(bool resurrectionIsNeeded)
    {
        _resurrectionIsNeeded = resurrectionIsNeeded;

        if (!_resurrectionIsNeeded && _resurrectionText.gameObject.activeSelf)
        {
            _flickerTimeCurrent = 0;

            _resurrectionText.gameObject.SetActive(false);
        };
    }

    private void OnHealthChanged(float percentage)
    {
        _healthRatio = percentage;
    }

    #endregion
}
