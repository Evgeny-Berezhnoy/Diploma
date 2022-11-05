using System;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PhotonCore : MonoBehaviourPunCallbacks
{
    #region Constants
    
    public static readonly TypedLobby TYPED_LOBBY = new TypedLobby("customLobby", LobbyType.Default);

    public static readonly string ROOM_USER_NAME_PROPERTY = "n";
    public static readonly string ROOM_SCENE_LOADING_PROPERTY = "sl";

    public static readonly string PLAYER_PROPERTY_CHARACTER_DATA = "cd";

    #endregion

    #region Events

    public event Action<PhotonView> _onViewInstantiated;
    public event Action _onQuitGameplayScene;

    #endregion

    #region Static fields

    private static int _currentScene;
    private static PhotonCore _instance;

    #endregion

    #region Fields

    private Hashtable _playerCustomProperties;
    private CharacterData _character;
    private TransformProxy _pool;
    private int _spawnPointIndex;

    #endregion

    #region Static properties

    public static PhotonCore Instance
    {
        get
        {
            if (!_instance)
            {
                var go = new GameObject("Photon core");

                _instance       = go.AddComponent<PhotonCore>();
                _instance._pool = new TransformProxy();
            };

            return _instance;
        }
    }

    #endregion

    #region Properties

    public CharacterData Character
    {
        get => _character;
        set
        {
            _character = value;

            _playerCustomProperties[PLAYER_PROPERTY_CHARACTER_DATA] = _character.DataPath;

            PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
        }
    }
    public TransformProxy Pool
    {
        get => _pool;
        set => _pool = value;
    }
    public bool RoomIsOpened => PhotonNetwork.CurrentRoom.IsOpen;
    public string RoomName => PhotonNetwork.CurrentRoom.CustomProperties[ROOM_USER_NAME_PROPERTY].ToString();
    public int SpawnPointIndex => _spawnPointIndex;

    #endregion

    #region Unity events

    private void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);

        _playerCustomProperties = new Hashtable();
        _playerCustomProperties.Add(PLAYER_PROPERTY_CHARACTER_DATA, "");

        _currentScene = Scenes.MAIN_MENU;

        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        
        if (_currentScene == Scenes.GAMEPLAY)
        {
            ClearGameplayScene();
        };

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        };
    }

    #endregion

    #region Base methods

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);

        base.OnConnectedToMaster();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if(_currentScene == Scenes.GAMEPLAY)
        {
            QuitGameplayScene();
        };
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(ROOM_SCENE_LOADING_PROPERTY))
        {
            var sceneIsLoading = (bool)propertiesThatChanged[ROOM_SCENE_LOADING_PROPERTY];

            if (sceneIsLoading)
            {
                LoadGame();
            };
        };
    }

    #endregion

    #region Methods

    public void Connect(string nickName)
    {
        PhotonNetwork.NickName                  = nickName;
        PhotonNetwork.AutomaticallySyncScene    = true;

        PhotonNetwork.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        };
    }

    public void CreateRoom(string roomName, byte maxPlayers)
    {
        var customProperties = new Hashtable();

        customProperties.Add(ROOM_USER_NAME_PROPERTY, roomName);
        customProperties.Add(ROOM_SCENE_LOADING_PROPERTY, false);

        var customPropertiesForLobby = new string[2]
        {
            ROOM_USER_NAME_PROPERTY,
            ROOM_SCENE_LOADING_PROPERTY
        };

        var roomOptions = new RoomOptions();

        roomOptions.MaxPlayers                      = maxPlayers;
        roomOptions.CustomRoomProperties            = customProperties;
        roomOptions.CustomRoomPropertiesForLobby    = customPropertiesForLobby;
        roomOptions.PublishUserId                   = true;

        var roomRealName = Guid.NewGuid().ToString();

        PhotonNetwork.CreateRoom(roomRealName, roomOptions);
    }

    public void OpenRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen    = true;
        PhotonNetwork.CurrentRoom.IsVisible = true;
    }

    public void CloseRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen    = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    public void NotifyToLoadGame()
    {
        var propertiesToSet = new Hashtable();
        propertiesToSet.Add(ROOM_SCENE_LOADING_PROPERTY, true);

        PhotonNetwork.CurrentRoom.SetCustomProperties(propertiesToSet);
    }

    public void LoadGame()
    {
        _currentScene = Scenes.GAMEPLAY;

        SetSpawnPointIndex();

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(Scenes.GAMEPLAY);
        };
    }

    public GameObject InstantiateInstance(string path)
    {
        return InstantiateInstance(path, Vector3.zero, Quaternion.identity);
    }
    
    public GameObject InstantiateInstance(string path, Transform transform)
    {
        return InstantiateInstance(path, transform.position, transform.rotation);
    }

    public GameObject InstantiateInstance(string path, Vector3 position, Quaternion rotation)
    {
        return PhotonNetwork.Instantiate(path, position, rotation);
    }
    
    public void DestroyInstance(GameObject instance)
    {
        PhotonNetwork.Destroy(instance);
    }

    public void AddViewInstantiationListener(Action<PhotonView> action)
    {
        _onViewInstantiated += action;
    }

    public void AddQuitGameplaySceneListener(Action action)
    {
        _onQuitGameplayScene += action;
    }

    public void OnViewInstantiated(PhotonView photonView)
    {
        _onViewInstantiated.Invoke(photonView);
    }

    private void SetSpawnPointIndex()
    {
        _spawnPointIndex = 0;

        var playerList = PhotonNetwork.PlayerList;

        for (int i = 0; i < playerList.Length; i++)
        {
            if(PhotonNetwork.LocalPlayer == playerList[i])
            {
                break;
            };

            _spawnPointIndex++;
        };
    }

    private void QuitGameplayScene()
    {
        _onQuitGameplayScene.Invoke();

        ClearGameplayScene();

        _currentScene = Scenes.MAIN_MENU;

        PhotonNetwork.LoadLevel(Scenes.MAIN_MENU);
    }

    private void ClearGameplayScene()
    {
        ClearViewInstantiationListeners();

        ClearQuitGameplaySceneListeners();
    }

    private void ClearViewInstantiationListeners()
    {
        var handlers =
            _onViewInstantiated
                ?.GetInvocationList()
                .Cast<Action<PhotonView>>()
                .ToArray();

        if (handlers != null)
        {
            for (int i = 0; i < handlers.Length; i++)
            {
                _onViewInstantiated -= handlers[i];
            };
        };
    }

    private void ClearQuitGameplaySceneListeners()
    {
        var handlers =
            _onQuitGameplayScene
                ?.GetInvocationList()
                .Cast<Action>()
                .ToArray();

        if (handlers != null)
        {
            for (int i = 0; i < handlers.Length; i++)
            {
                _onQuitGameplayScene -= handlers[i];
            };
        };
    }

    #endregion
}
