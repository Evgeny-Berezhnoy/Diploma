using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class AuthentificationWindow : UIWindow<EMainMenuWindow>, IConnectionCallbacks
{
    #region Fields

    [Header("UI")]
    [SerializeField] private InputField _username;
    [SerializeField] private InputField _password;
    [SerializeField] private SoundButton _connectButton;
    [SerializeField] private SoundButton _createAccountButton;
    [SerializeField] private SoundButton _quitButton;

    #endregion

    #region Unity events

    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);

        _connectButton.onClick.AddListener(() => PlayfabConnect());
        _createAccountButton.onClick.AddListener(() => Switch(EMainMenuWindow.AccountCreation));
        _quitButton.onClick.AddListener(() => UnityApplication.Quit());
    }

    protected override void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);

        _connectButton.onClick.RemoveAllListeners();
        _createAccountButton.onClick.RemoveAllListeners();
        _quitButton.onClick.RemoveAllListeners();

        base.OnDestroy();
    }

    #endregion

    #region Base methods

    public override void Open(object parameters = null)
    {
        var windowData = parameters as AuthentificationWindowData;

        if (windowData != null && windowData.Forceful)
        {
            _username.Clear();
            _password.Clear();

            PlayerPrefs.DeleteKey(PlayfabCore.USERNAME_STORAGE_KEY);
            PlayerPrefs.DeleteKey(PlayfabCore.PASSWORD_STORAGE_KEY);

            PlayfabCore.Instance.Disconnect();
            PhotonCore.Instance.Disconnect();

            base.Open(parameters);
        }
        else if (!PlayfabCore.IsConnected)
        {
            if (PlayerPrefs.HasKey(PlayfabCore.USERNAME_STORAGE_KEY))
            {
                var username = PlayerPrefs.GetString(PlayfabCore.USERNAME_STORAGE_KEY);
                var password = PlayerPrefs.GetString(PlayfabCore.PASSWORD_STORAGE_KEY);

                _username.SetTextWithoutNotify(username);
                _password.SetTextWithoutNotify(password);

                PlayfabConnect();

                return;
            };
            
            base.Open(parameters);
        }
        else if (!PhotonNetwork.IsConnected)
        {
            PhotonConnect();
        }
        else
        {
            Switch(EMainMenuWindow.Title);
        };
    }

    #endregion

    #region Interface methods

    public void OnConnected() {}

    public void OnConnectedToMaster()
    {
        Switch(EMainMenuWindow.Title);
    }

    public void OnDisconnected(DisconnectCause cause) {}

    public void OnRegionListReceived(RegionHandler regionHandler) {}

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data) {}

    public void OnCustomAuthenticationFailed(string debugMessage) {}

    #endregion

    #region Methods

    private void PlayfabConnect()
    {
        var error = "";

        if (string.IsNullOrWhiteSpace(_username.text))
        {
            error = string.Concat(error, $"\nUsername can't be empty.");
        };

        if (string.IsNullOrWhiteSpace(_password.text))
        {
            error = string.Concat(error, $"\nPassword can't be empty.");
        };

        if (!string.IsNullOrWhiteSpace(error))
        {
            var errorData =
                new ErrorWindowData(
                    error,
                    WindowType);

            Switch(EMainMenuWindow.Error, errorData);

            return;
        };

        PlayfabCore.Instance.Connect(_username.text, _password.text, OnPlayfabConnect);
    }

    private void OnPlayfabConnect(bool success, object response)
    {
        if (!success)
        {
            #if UNITY_EDITOR

            Debug.LogError(response.ToString());

            #endif

            var data = new AuthentificationWindowData(true);

            var errorData =
                new ErrorWindowData(
                    "Failed to connect your profile. Please, try again.",
                    WindowType,
                    data);

            Switch(EMainMenuWindow.Error, errorData);

            return;
        };

        PlayerPrefs.SetString(PlayfabCore.USERNAME_STORAGE_KEY, _username.text);
        PlayerPrefs.SetString(PlayfabCore.PASSWORD_STORAGE_KEY, _password.text);
        
        Open();
    }

    private void PhotonConnect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonCore.Instance.Connect(PlayfabCore.Instance.Username);
        }
        else
        {
            Switch(EMainMenuWindow.Title);
        };
    }

    #endregion
}
