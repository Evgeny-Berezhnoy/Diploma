using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyWindow : UIWindow<EMainMenuWindow>, ILobbyCallbacks
{
    #region Fields
    
    [Header("UI")]
    [SerializeField] private RectTransform _container;
    [SerializeField] private SoundButton _backButton;
    [SerializeField] private SoundButton _createRoomButton;

    [Header("Prefabs")]
    [SerializeField] private RoomSlot _prefab;

    private Dictionary<string, RoomSlot> _cachedRooms;

    #endregion

    #region Unity events

    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);

        _cachedRooms = new Dictionary<string, RoomSlot>();

        _backButton.onClick.AddListener(() => PhotonNetwork.LeaveLobby());
        _createRoomButton.onClick.AddListener(() => Switch(EMainMenuWindow.RoomCreation));
    }

    protected override void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);

        _backButton.onClick.RemoveAllListeners();
        _createRoomButton.onClick.RemoveAllListeners();

        base.OnDestroy();
    }

    #endregion

    #region Base methods

    public override void Open(object parameters = null)
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby(PhotonCore.TYPED_LOBBY);
        }
        else
        {
            base.Open(parameters);
        };
    }

    #endregion

    #region Interface methods
    
    public void OnJoinedLobby()
    {
        Open();
    }

    public void OnLeftLobby()
    {
        DeleteAllRooms();

        Switch(EMainMenuWindow.Title);
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            var roomInfo = roomList[i];

            if (roomInfo.RemovedFromList
                || !roomInfo.IsOpen
                || !roomInfo.IsVisible)
            {
                DeleteRoom(roomInfo);
            }
            else
            {
                UpdateRoom(roomInfo);
            };
        };
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics){}

    #endregion

    #region Methods

    private void AddRoom(RoomInfo roomInfo)
    {
        var roomView = Instantiate(_prefab, _container);

        roomView.OnButtonClicked += SelectRoom;

        _cachedRooms.Add(roomInfo.Name, roomView);
    }

    private void UpdateRoom(RoomInfo roomInfo)
    {
        if (!_cachedRooms.ContainsKey(roomInfo.Name))
        {
            AddRoom(roomInfo);
        };

        _cachedRooms[roomInfo.Name].RoomInformation = roomInfo;
    }

    private void DeleteRoom(RoomInfo roomInfo)
    {
        if (!_cachedRooms.ContainsKey(roomInfo.Name)) return;

        _cachedRooms.TryGetValue(roomInfo.Name, out var room);

        if (_cachedRooms.Remove(roomInfo.Name))
        {
            Destroy(room.gameObject);
        };
    }

    private void DeleteAllRooms()
    {
        var roomLinkViews =
            _cachedRooms
                .Values
                .Cast<RoomSlot>()
                .ToArray();

        for(int i = 0; i < roomLinkViews.Length; i++)
        {
            DeleteRoom(roomLinkViews[i].RoomInformation);
        };
    }

    private void SelectRoom(RoomInfo roomInformation)
    {
        Switch(EMainMenuWindow.Room, roomInformation.Name);
    }

    #endregion
}
