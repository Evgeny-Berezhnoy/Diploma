using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RoomWindow : UIWindow<EMainMenuWindow>, IMatchmakingCallbacks, IInRoomCallbacks
{
    #region Fields

    [Header("UI")]
    [SerializeField] private RectTransform _container;
    [SerializeField] private Text _roomName;
    [SerializeField] private Text _closeOpenRoomText;
    [SerializeField] private SoundButton _leaveRoomButton;
    [SerializeField] private SoundButton _closeOpenRoomButton;
    [SerializeField] private SoundButton _startGameButton;

    [Header("Prefabs")]
    [SerializeField] private PlayerSlot _prefab;

    private Dictionary<string, PlayerSlot> _cachedPlayers;

    #endregion

    #region Unity events

    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);

        _cachedPlayers = new Dictionary<string, PlayerSlot>();

        _leaveRoomButton.onClick.AddListener(() => LeaveRoom());
        _startGameButton.onClick.AddListener(() => StartGame());
    }

    protected override void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);

        _leaveRoomButton.onClick.RemoveAllListeners();
        _closeOpenRoomButton.onClick.RemoveAllListeners();
        _startGameButton.onClick.RemoveAllListeners();

        base.OnDestroy();
    }

    #endregion

    #region Base methods

    public override void Open(object parameters = null)
    {
        if (!PhotonNetwork.InRoom)
        {
            var roomName = (string) parameters;

            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            _roomName.text = PhotonCore.Instance.RoomName;

            _closeOpenRoomButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
            _startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);

            if (PhotonNetwork.CurrentRoom.IsOpen)
            {
                OpenRoom();
            }
            else
            {
                CloseRoom();
            };

            DeleteAllPlayerSlots();

            var playerList = PhotonNetwork.PlayerList;

            for(int i = 0; i < playerList.Length; i++)
            {
                AddPlayerSlot(playerList[i]);
            };

            base.Open(parameters);
        };
    }

    #endregion

    #region Interface methods

    public void OnCreatedRoom() {}

    public void OnCreateRoomFailed(short returnCode, string message) {}

    public void OnFriendListUpdate(List<FriendInfo> friendList) {}

    public void OnJoinedRoom()
    {
        Open();
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        #if UNITY_EDITOR

        Debug.LogError(message);

        #endif

        var errorData =
            new ErrorWindowData(
                "Failed to join room. Please, try again.",
                EMainMenuWindow.Lobby);

        Switch(EMainMenuWindow.Error, errorData);
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        var errorData =
            new ErrorWindowData(
                "Failed to join room. Please, try again later.",
                EMainMenuWindow.Lobby);

        Switch(EMainMenuWindow.Error, errorData);
    }

    public void OnLeftRoom()
    {
        DeleteAllPlayerSlots();
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerSlot(newPlayer);
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        DeletePlayerSlot(otherPlayer.UserId);
    }

    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (PhotonCore.Instance.RoomIsOpened)
        {
            OpenRoom();
        }
        else
        {
            CloseRoom();
        };

        if (propertiesThatChanged.ContainsKey(PhotonCore.ROOM_SCENE_LOADING_PROPERTY))
        {
            var sceneIsLoading = (bool) propertiesThatChanged[PhotonCore.ROOM_SCENE_LOADING_PROPERTY];

            if (sceneIsLoading)
            {
                Switch(EMainMenuWindow.Loading);
            };
        };
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {}

    public void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonNetwork.LeaveRoom();

        var errorData =
                new ErrorWindowData(
                    "Host has left the room.",
                    EMainMenuWindow.Lobby);

        Switch(EMainMenuWindow.Error, errorData);
    }

    #endregion

    #region Methods
    
    private void AddPlayerSlot(Player newPlayer)
    {
        if (_cachedPlayers.ContainsKey(newPlayer.UserId)) return;

        var characterSlot = Instantiate(_prefab, _container);

        characterSlot.Data          = PlayfabCore.Instance.GetCharacterData((string) newPlayer.CustomProperties[PhotonCore.PLAYER_PROPERTY_CHARACTER_DATA]);
        characterSlot.PlayerName    = newPlayer.NickName;
        characterSlot.UserID        = newPlayer.UserId;

        _cachedPlayers.Add(newPlayer.UserId, characterSlot);
    }

    private void DeletePlayerSlot(string userID)
    {
        if (!_cachedPlayers.ContainsKey(userID)) return;

        _cachedPlayers.TryGetValue(userID, out var playerSlot);

        if (_cachedPlayers.Remove(userID))
        {
            Destroy(playerSlot.gameObject);
        };
    }

    private void DeleteAllPlayerSlots()
    {
        var playerUserIDs =
            _cachedPlayers
                .Keys
                .Cast<string>()
                .ToArray();

        for (int i = 0; i < playerUserIDs.Length; i++)
        {
            DeletePlayerSlot(playerUserIDs[i]);
        };
    }
    
    private void LeaveRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CloseRoom();
        };

        PhotonNetwork.LeaveRoom();

        Switch(EMainMenuWindow.Lobby);
    }

    private void CloseRoom()
    {
        if (PhotonNetwork.IsMasterClient && PhotonCore.Instance.RoomIsOpened)
        {
            PhotonCore.Instance.CloseRoom();
        };

        _closeOpenRoomButton.onClick.RemoveAllListeners();
        _closeOpenRoomButton.onClick.AddListener(() => OpenRoom());

        _closeOpenRoomText.text = "Open";

        _startGameButton.interactable = true;
    }

    private void OpenRoom()
    {
        if (PhotonNetwork.IsMasterClient && !PhotonCore.Instance.RoomIsOpened)
        {
            PhotonCore.Instance.OpenRoom();
        };

        _closeOpenRoomButton.onClick.RemoveAllListeners();
        _closeOpenRoomButton.onClick.AddListener(() => CloseRoom());

        _closeOpenRoomText.text = "Close";

        _startGameButton.interactable = false;
    }

    private void StartGame()
    {
        PhotonCore.Instance.NotifyToLoadGame();
    }

    #endregion
}
