using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public abstract class NetworkStation : MonoBehaviour, IPunObservable
{
    #region Fields

    protected int _me;
    protected Queue<NetworkMessage> _messages;
    protected NetworkMessage _message;
    protected PhotonView _photonView;
    protected Dictionary<int, Action<NetworkMessage>> _readInstructions;

    #endregion

    #region Properties

    public PhotonView PhotonView
    {
        get
        {
            if (!_photonView)
            {
                _photonView = PhotonView.Get(this);
            };

            return _photonView;
        }
    }

    #endregion

    #region Unity events

    public virtual void Awake()
    {
        _me                 = PhotonNetwork.LocalPlayer.ActorNumber;
        _messages           = new Queue<NetworkMessage>();
        _readInstructions   = new Dictionary<int, Action<NetworkMessage>>();

        InitializeReadInstrctions();
    }

    public virtual void Start()
    {
        PhotonCore.Instance.OnViewInstantiated(this);
    }

    #endregion

    #region Interface methods

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PendingShipping(stream))
        {
            SendMessages(stream);
        }
        else if (GotMessages(stream))
        {
            ReadMessages(stream);
        };
    }

    #endregion

    #region Methods

    protected bool PendingShipping(PhotonStream stream)
    {
        return (stream.IsWriting) && (_messages.Count > 0);
    }

    protected virtual void SendMessages(PhotonStream stream)
    {
        while(_messages.Count > 0)
        {
            stream.SendNext(_messages.Dequeue());
        };
    }

    protected bool GotMessages(PhotonStream stream)
    {
        return (stream.IsReading) && (stream.Count > 0);
    }

    protected virtual void ReadMessages(PhotonStream stream)
    {
        while(stream.CurrentItem < stream.Count)
        {
            _message = (NetworkMessage) stream.ReceiveNext();

            if (_message.IsForAddresser(_me))
            {
                ReadMessage(_message);
            };
        };
    }

    protected void ReadMessage(NetworkMessage message)
    {
        if (!_readInstructions.TryGetValue(message.MessageType, out var action)) return;

        action.Invoke(message);
    }

    public virtual void RecordMessage(int messageType, int adresser, params float[] data)
    {
        _message = new NetworkMessage(messageType, adresser, data);

        if (!_message.IsForAddresser(_me))
        {
            _messages.Enqueue(_message);
        }
        else
        {
            ReadMessage(_message);
        };
    }

    public virtual void RecordBroadcastMessage(int messageType, params float[] data)
    {
        RecordMessage(messageType, 0, data);
    }
    
    protected abstract void InitializeReadInstrctions();

    #endregion
}
