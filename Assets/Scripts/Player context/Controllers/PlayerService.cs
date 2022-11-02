using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerService : IUpdate, IDisposableAdvanced, IInRoomCallbacks
{
    #region Fields

    private bool _isDisposed;
    private List<PlayerView> _views;
    private ResurrectionService _resurrectionService;

    #endregion

    #region Events

    private ISubscriptionProperty _onDefeat;
    
    #endregion

    #region Interface properties

    public bool IsDisposed => _isDisposed;
    
    #endregion

    #region Constructors

    public PlayerService(
        ResurrectionService resurrectionService,
        ISubscriptionProperty onDefeat,
        ISubscriptionProperty onRetry)
    {
        _views                  = new List<PlayerView>();
        _resurrectionService    = resurrectionService;

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

        for(int i = 0; i < _views.Count; i++)
        {
            if (!_views[i].IsDead) return;
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
        var view =
            _views
                .Where(x => x.PhotonView.Owner == otherPlayer)
                .First();

        _views.Remove(view);
    }

    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {}

    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {}

    public void OnMasterClientSwitched(Player newMasterClient) {}

    #endregion

    #region Methods

    public Transform GetEnemyTarget()
    {
        var player =
            _views
                .Where(x => x.EnemyTarget.gameObject.activeSelf)
                .Random();

        if (player == default)
        {
            return null;
        };

        return player.EnemyTarget;
    }

    public void Resurrect(Collider2D collider)
    {
        var view = _views.First(x => x.ResurrectionTargetCollider == collider);

        _resurrectionService.Resurrect(view.PhotonView.OwnerActorNr);
    }

    public void ResurrectAll()
    {
        for(int i = 0; i < _views.Count; i++)
        {
            _resurrectionService.Resurrect(_views[i].PhotonView.OwnerActorNr);
        };
    }

    private void OnViewInstantiated(PhotonView photonView)
    {
        var view =
            photonView
                .gameObject
                .GetComponent<PlayerView>();

        if (!view) return;

        _views.Add(view);
    }

    #endregion
}
