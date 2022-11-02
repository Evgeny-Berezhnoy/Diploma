using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomCreationWindow : UIWindow<EMainMenuWindow>, IMatchmakingCallbacks
{
    #region Fields

    [Header("Settings")]
    [SerializeField, Range(4, 8)] private int _maximumPlayersNumber;
    
    [Header("UI")]
    [SerializeField] private InputField _roomName;
    [SerializeField] private Text _playersNumberSign;
    [SerializeField] private Slider _playersNumberSlider;
    [SerializeField] private SoundButton _createRoomButton;
    [SerializeField] private SoundButton _backButton;
    
    #endregion

    #region Unity events

    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);

        _playersNumberSlider.onValueChanged.AddListener(value => _playersNumberSign.text = value.ToString());

        _playersNumberSlider.minValue   = 1;
        _playersNumberSlider.maxValue   = _maximumPlayersNumber;
        _playersNumberSlider.value      = 1;
        
        _createRoomButton.onClick.AddListener(() => CreateRoom());
        _backButton.onClick.AddListener(() => Switch(EMainMenuWindow.Lobby));
    }

    protected override void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);

        _playersNumberSlider.onValueChanged.RemoveAllListeners();

        _createRoomButton.onClick.RemoveAllListeners();
        _backButton.onClick.RemoveAllListeners();

        base.OnDestroy();
    }

    #endregion

    #region Interface methods

    public void OnCreatedRoom()
    {
        Switch(EMainMenuWindow.Room);
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        #if UNITY_EDITOR
        
        Debug.LogError($"Failed to create room. An error has arised:{message}.");

        #endif

        var errorData =
            new ErrorWindowData(
                "Failed to create room. Please, try again later.",
                WindowType);

        Switch(EMainMenuWindow.Error, errorData);
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList) {}

    public void OnJoinedRoom() {}

    public void OnJoinRandomFailed(short returnCode, string message) {}

    public void OnJoinRoomFailed(short returnCode, string message) {}

    public void OnLeftRoom() {}

    #endregion

    #region Methods

    private void CreateRoom()
    {
        if (string.IsNullOrWhiteSpace(_roomName.text))
        {
            var errorData =
                new ErrorWindowData(
                    "Room name can't be empty.",
                    WindowType);

            Switch(EMainMenuWindow.Error, errorData);

            return;
        };

        PhotonCore.Instance.CreateRoom(_roomName.text, (byte)_playersNumberSlider.value);
    }

    #endregion
}
