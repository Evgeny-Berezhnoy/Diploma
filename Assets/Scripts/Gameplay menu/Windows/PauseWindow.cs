using UnityEngine;

public class PauseWindow : UIWindow<EGameplayMenuWindow>
{
    #region Fields

    [SerializeField] private SoundButton _backButton;
    [SerializeField] private SoundButton _leaveMatchButton;

    #endregion

    #region Observers

    private ISubscriptionProperty _onEscape;
    private ISubscriptionProperty _onQuitGame;

    #endregion

    #region Unity events

    private void Start()
    {
        _backButton.onClick.AddListener(() => _onEscape.Invoke());
        _leaveMatchButton.onClick.AddListener(() => _onQuitGame.Invoke());
    }

    protected override void OnDestroy()
    {
        _backButton.onClick.RemoveAllListeners();
        _leaveMatchButton.onClick.RemoveAllListeners();

        base.OnDestroy();
    }

    #endregion

    #region Injected methods

    public void Initialize(
        ISubscriptionProperty onEscape,
        ISubscriptionProperty onQuitGame)
    {
        _onEscape   = onEscape;
        _onQuitGame = onQuitGame;
    }

    #endregion
}
