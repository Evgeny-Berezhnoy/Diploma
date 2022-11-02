using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class FinishGameWindow : UIWindow<EGameplayMenuWindow>
{
    #region Constants

    private const string VICTORY_TITLE  = "VICTORY";
    private const string DEFEAT_TITLE   = "DEFEAT";

    #endregion

    #region Fields

    [SerializeField] private Text _title;
    [SerializeField] private SoundButton _retryButton;
    [SerializeField] private SoundButton _leaveMatchButton;

    #endregion

    #region Observers

    private ISubscriptionProperty _onRetry;
    private ISubscriptionProperty _onQuitGame;
    
    #endregion

    #region Unity events

    private void Start()
    {
        _retryButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        _retryButton.onClick.AddListener(_onRetry.Invoke);

        _leaveMatchButton.onClick.AddListener(_onQuitGame.Invoke);
    }

    protected override void OnDestroy()
    {
        _retryButton.onClick.RemoveAllListeners();
        _leaveMatchButton.onClick.RemoveAllListeners();

        base.OnDestroy();
    }

    #endregion

    #region Base methods

    public override void Open(object parameters = null)
    {
        var windowData = (FinishGameWindowData) parameters;

        if (windowData.Victory)
        {
            _title.text = VICTORY_TITLE;
        }
        else
        {
            _title.text = DEFEAT_TITLE;
        };

        _retryButton.gameObject.SetActive(false);
        _leaveMatchButton.gameObject.SetActive(false);

        PlayfabCore.Instance.UpdatePlayerStatistics(OnPlayfabOperationsCompleted);
        
        base.Open(parameters);
    }

    #endregion

    #region Methods

    public void Initialize(
        ISubscriptionProperty onRetry,
        ISubscriptionProperty onQuitGame)
    {
        _onRetry    = onRetry;
        _onQuitGame = onQuitGame;
    }
    
    private void OnPlayfabOperationsCompleted(bool success, object data)
    {
        _retryButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        _leaveMatchButton.gameObject.SetActive(true);
    }

    #endregion
}
