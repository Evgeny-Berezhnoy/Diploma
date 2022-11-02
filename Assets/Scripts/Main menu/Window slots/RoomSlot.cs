using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomSlot : MonoBehaviour
{
    #region Events

    public event Action<RoomInfo> OnButtonClicked;

    #endregion

    #region Fields

    [SerializeField] private SoundButton _joinRoomButton;

    [SerializeField] private Text _roomName;
    [SerializeField] private Text _occupied;
    [SerializeField] private Text _capacity;

    private RoomInfo _roomInformation;

    #endregion

    #region Properties

    public RoomInfo RoomInformation
    {
        get => _roomInformation;
        set
        {
            _roomInformation = value;

            _roomName.text = (string)_roomInformation.CustomProperties[PhotonCore.ROOM_USER_NAME_PROPERTY];
            _occupied.text = _roomInformation.PlayerCount.ToString();
            _capacity.text = _roomInformation.MaxPlayers.ToString();

            _joinRoomButton.interactable =
                !_roomInformation.RemovedFromList
                && (_roomInformation.PlayerCount < _roomInformation.MaxPlayers);
        }
    }

    #endregion

    #region Unity events

    private void Start()
    {
        _joinRoomButton.onClick.AddListener(() => JoinRoom());
    }

    private void OnDestroy()
    {
        _joinRoomButton.onClick.RemoveAllListeners();

        var handlers =
            OnButtonClicked
                ?.GetInvocationList()
                .Cast<Action<RoomInfo>>()
                .ToList();

        if(handlers != null)
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                OnButtonClicked -= handlers[i];
            };
        };
    }

    #endregion

    #region Methods

    public void JoinRoom()
    {
        OnButtonClicked?.Invoke(_roomInformation);
    }

    #endregion
}