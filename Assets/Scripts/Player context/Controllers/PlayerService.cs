using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerService : IUpdate, IDisposableAdvanced, IInRoomCallbacks
{
    #region Variables

    private int _index;
    private PlayerView _player;

    #endregion

    #region Fields

    private bool _isDisposed;
    private List<PlayerView> _views;
    private ResurrectionController _resurrectionController;

    #endregion

    #region Events

    private ISubscriptionProperty _onDefeat;
    
    #endregion

    #region Interface properties

    public bool IsDisposed => _isDisposed;
    
    #endregion

    #region Constructors

    public PlayerService(
        ResurrectionController resurrectionController,
        ISubscriptionProperty onDefeat,
        ISubscriptionProperty onRetry)
    {
        _views                  = new List<PlayerView>();
        _resurrectionController = resurrectionController;

        _onDefeat = onDefeat;

        onRetry.Subscribe(ResurrectAll);

        PhotonNetwork.AddCallbackTarget(this);

        PhotonCore.Instance.AddViewInstantiationListener(OnViewInstantiated);
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        if (_views.Count == 0) return;

        for(_index = 0; _index < _views.Count; _index++)
        {
            if (_views[_index].Sentry.IsObserving) return;
        };

        _onDefeat.Invoke();
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _isDisposed = true;

        PhotonNetwork.RemoveCallbackTarget(this);

        GC.SuppressFinalize(this);
    }

    public void OnPlayerEnteredRoom(Player newPlayer) {}

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        _player =
            _views
                .Where(x => x.Sentry.PhotonView.Owner == otherPlayer)
                .First();

        _views.Remove(_player);
    }

    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {}

    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {}

    public void OnMasterClientSwitched(Player newMasterClient) {}

    #endregion

    #region Methods

    public Transform GetEnemyTarget()
    {
        _player =
            _views
                .Where(x => x.EnemyTarget.gameObject.activeSelf)
                .Random();

        if (_player == default)
        {
            return null;
        };

        return _player.EnemyTarget;
    }

    public void Resurrect(Collider2D collider)
    {
        _player = _views.First(x => x.ResurrectionTargetCollider == collider);

        _resurrectionController.Resurrect(_player.Sentry.PhotonView.OwnerActorNr);
    }

    public void ResurrectAll()
    {
        for(_index = 0; _index < _views.Count; _index++)
        {
            _resurrectionController.Resurrect(_views[_index].Sentry.PhotonView.OwnerActorNr);
        };
    }

    private void OnViewInstantiated(MonoBehaviour view)
    {
        _player = view as PlayerView;

        if (!_player) return;

        _views.Add(_player);
    }

    #endregion
}
