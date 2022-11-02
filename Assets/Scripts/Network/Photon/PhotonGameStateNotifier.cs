using System;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PhotonGameStateNotifier : IController, IOnEventCallback, IDisposableAdvanced
{
    #region Fields

    private bool _isDisposed;

    #endregion

    #region Observers

    private ISubscriptionProperty _onVictory;
    private ISubscriptionProperty _onRetry;
    private ISubscriptionValue<EGameState> _gameState;

    #endregion

    #region Interface properties

    public bool IsDisposed => _isDisposed;

    #endregion

    #region Constructors

    public PhotonGameStateNotifier(
        ISubscriptionProperty onVictory,
        ISubscriptionProperty onRetry,
        ISubscriptionValue<EGameState> gameState)
    {
        _onVictory  = onVictory;
        _onRetry    = onRetry;
        _gameState  = gameState;

        PhotonNetwork.AddCallbackTarget(this);
    }

    #endregion

    #region Interface methods

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == PhotonEvents.SET_GAME_STATE_VICTORY)
        {
            _onVictory.Invoke();
        }
        else if (photonEvent.Code == PhotonEvents.RETRY_LEVEL)
        {
            _onRetry.Invoke();
        };
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _isDisposed = true;

        PhotonNetwork.RemoveCallbackTarget(this);

        GC.SuppressFinalize(this);
    }

    #endregion

    #region Methods

    public void OnVictory()
    {
        if (_gameState.Value == EGameState.Defeat) return;

        CallEvent(PhotonEvents.SET_GAME_STATE_VICTORY);
    }

    public void OnRetry()
    {
        CallEvent(PhotonEvents.RETRY_LEVEL);
    }

    private void CallEvent(byte eventCode)
    {
        var raiseEventOptions = new RaiseEventOptions();
        
        raiseEventOptions.Receivers = ReceiverGroup.All;

        PhotonNetwork.RaiseEvent(eventCode, null, raiseEventOptions, SendOptions.SendReliable);
    }

    #endregion
}
