using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabCore : MonoBehaviour
{
    #region Static fields

    public static readonly string USERNAME_STORAGE_KEY = "Playfab_username";
    public static readonly string PASSWORD_STORAGE_KEY = "Playfab_password";

    private static readonly string CATALOG_PATH             = "Playfab/Catalog";
    private static readonly string PLAYER_STATISTICS_PATH   = "Playfab/Player statistics";

    private static PlayfabCore _instance;

    #endregion

    #region Fields

    private PlayfabCatalog _catalog;
    private List<CharacterData> _availableCharacters;
    private PlayfabPlayerStatistics _playerStatistics;
    private string _username;
    private string _id;

    #endregion

    #region Static properties

    public static bool IsConnected => PlayFabClientAPI.IsClientLoggedIn();

    public static PlayfabCore Instance
    {
        get
        {
            if (!_instance)
            {
                var go = new GameObject("Playfab core");

                _instance = go.AddComponent<PlayfabCore>();
            };

            return _instance;
        }
    }

    #endregion

    #region Properties

    public PlayfabCatalog Catalog => _catalog;
    public string Username => _username;
    public string ID => _id;

    #endregion

    #region Unity events

    private void Start()
    {
        var exceptionStringBuilder = new StringBuilder();

        _catalog = Resources.Load<PlayfabCatalog>(CATALOG_PATH);

        if(_catalog == null)
        {
            exceptionStringBuilder.AppendLine($"Playfab catalog at directory Resources/{CATALOG_PATH} has not been found.");
        };

        _playerStatistics = Resources.Load<PlayfabPlayerStatistics>(PLAYER_STATISTICS_PATH);

        if (_playerStatistics == null)
        {
            exceptionStringBuilder.AppendLine($"Player statistics at directory Resources/{PLAYER_STATISTICS_PATH} has not been found.");
        };

        if (!string.IsNullOrWhiteSpace(exceptionStringBuilder.ToString()))
        {
            throw new Exception(exceptionStringBuilder.ToString());
        };

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Methods

    public void CreateAccount(string username, string email, string password, Action<bool, object> connectionCallback)
    {
        var request = new RegisterPlayFabUserRequest()
        {
            Username    = username,
            Email       = email,
            Password    = password
        };

        PlayFabClientAPI.
            RegisterPlayFabUser(
                request,
                OnCreateAccountSuccess,
                OnFailure,
                connectionCallback);
    }
    
    private void OnCreateAccountSuccess(RegisterPlayFabUserResult result)
    {
        var connectionCallback = (Action<bool, object>) result.CustomData;

        connectionCallback.Invoke(true, null);

        UpdatePlayerStatistics(PlayfabPlayerStatistics.Empty(), ConnectionCallbackEmpty);
    }

    public void Connect(string username, string password, Action<bool, object> connectionCallback)
    {
        var infoRequestParameters = new GetPlayerCombinedInfoRequestParams();
        infoRequestParameters.GetUserAccountInfo = true;

        var request = new LoginWithPlayFabRequest()
        {
            Username = username,
            Password = password,
            InfoRequestParameters = infoRequestParameters
        };

        PlayFabClientAPI
            .LoginWithPlayFab(
                request,
                OnConnectSuccess,
                OnFailure,
                connectionCallback);
    }

    private void OnConnectSuccess(LoginResult result)
    {
        _username   = result.InfoResultPayload.AccountInfo.Username;
        _id         = result.PlayFabId;
        
        var connectionCallback = (Action<bool, object>) result.CustomData;

        connectionCallback.Invoke(true, _id);

        DownloadPlayerStatistics(ConnectionCallbackEmpty);
    }

    public void Disconnect()
    {
        PlayFabClientAPI.ForgetAllCredentials();

        _username   = "";
        _id         = "";
    }

    private void DownloadPlayerStatistics(Action<bool, object> connectionCallback)
    {
        var request = new GetPlayerStatisticsRequest();

        var playerStatistics = PlayfabPlayerStatistics.List();

        request.StatisticNames = new List<string>();

        for(int i = 0; i < playerStatistics.Count; i++)
        {
            request.StatisticNames.Add(playerStatistics[i].ToString("g"));
        };

        PlayFabClientAPI
            .GetPlayerStatistics(
                request,
                OnDownloadPlayerStatistics,
                OnFailure,
                connectionCallback);
    }

    private void OnDownloadPlayerStatistics(GetPlayerStatisticsResult result)
    {
        var statistics = new PlayfabPlayerStatistic[result.Statistics.Count];

        var statisticType = typeof(EPlayfabPlayerStatisticName);

        for (int i = 0; i < result.Statistics.Count; i++)
        {
            var playfabStatistic    = result.Statistics[i];

            var statistic = (EPlayfabPlayerStatisticName) Enum.Parse(statisticType, playfabStatistic.StatisticName);

            statistics[i] =
                new PlayfabPlayerStatistic(
                    statistic,
                    playfabStatistic.Value);
        };

        _playerStatistics.Statistics = statistics;

        var connectionCallback = (Action<bool, object>) result.CustomData;

        connectionCallback.Invoke(true, null);
    }

    public void RecordStatisticChange(EPlayfabPlayerStatisticName statisticName, int value)
    {
        _playerStatistics
            .Statistics
            .First(x => x.Name == statisticName)
            .Value += value;
    }

    public void UpdatePlayerStatistics(Action<bool, object> connectionCallback)
    {
        UpdatePlayerStatistics(
            _playerStatistics.Statistics,
            (_, __) => CheckCharactersAcquisition(connectionCallback));
    }

    private void UpdatePlayerStatistics(PlayfabPlayerStatistic[] statistics, Action<bool, object> connectionCallback)
    {
        var playfabStatistics = new List<StatisticUpdate>();

        for(int i = 0; i < statistics.Length; i++)
        {
            var statistic = statistics[i];

            var playfabStatistic            = new StatisticUpdate();
            playfabStatistic.StatisticName  = statistic.Name.ToString("g");
            playfabStatistic.Value          = statistic.Value;

            playfabStatistics.Add(playfabStatistic);
        };

        var request = new UpdatePlayerStatisticsRequest();
        
        request.Statistics = playfabStatistics;

        PlayFabClientAPI
            .UpdatePlayerStatistics(
                request,
                OnUpdatePlayerStatistics,
                OnFailure,
                connectionCallback);
    }

    private void OnUpdatePlayerStatistics(UpdatePlayerStatisticsResult result)
    {
        var connectionCallback = (Action<bool, object>) result.CustomData;

        connectionCallback.Invoke(true, null);
    }

    public void PurchaseCharacter(Action<bool, object> connectionCallback, PlayfabCharacterToken character = null)
    {
        if(character == null)
        {
            character = _catalog.DefaultCharacterToken;
        };

        var request = new PurchaseItemRequest();

        request.CatalogVersion  = _catalog.Version;
        request.VirtualCurrency = _catalog.Currency;
        request.ItemId          = character.ItemID;
        request.Price           = character.Price;

        PlayFabClientAPI
            .PurchaseItem(
                request,
                OnPurchaseCharacter,
                OnFailure,
                connectionCallback);
    }

    private void OnPurchaseCharacter(PurchaseItemResult result)
    {
        var item                = result.Items[0];
        var connectionCallback  = (Action<bool, object>) result.CustomData;

        GrantCharacter(item, connectionCallback);
    }

    private void GrantCharacter(ItemInstance item, Action<bool, object> connectionCallback)
    {
        var request = new GrantCharacterToUserRequest();

        request.CatalogVersion  = _catalog.Version;
        request.ItemId          = item.ItemId;
        
        request.CharacterName =
            _catalog
                .Characters
                .First(x => x.ItemID == item.ItemId)
                .Character
                .Name;

        PlayFabClientAPI
            .GrantCharacterToUser(
                request,
                OnGrantCharacter,
                OnFailure,
                connectionCallback);
    }

    private void OnGrantCharacter(GrantCharacterToUserResult result)
    {
        var connectionCallback = (Action<bool, object>)result.CustomData;

        connectionCallback.Invoke(true, null);
    }

    private void CheckCharactersAcquisition(Action<bool, object> connectionCallback)
    {
        var closedCharacters = new Queue<PlayfabCharacterToken>();

        for(int i = 0; i < _catalog.Characters.Length; i++)
        {
            var characterToken = _catalog.Characters[i];

            if (_availableCharacters.Contains(characterToken.Character)) continue;

            var acquisitionCondition = characterToken.AcquisitionCondition;

            var playerStatistic =
                _playerStatistics
                    .Statistics
                    .First(x => x.Name == acquisitionCondition.Statistic);

            if(playerStatistic.Value >= acquisitionCondition.Value)
            {
                closedCharacters.Enqueue(characterToken);
            };
        };

        PurchaseCharactersLoop(closedCharacters, connectionCallback);
    }

    private void PurchaseCharactersLoop(Queue<PlayfabCharacterToken> closedCharacters, Action<bool, object> connectionCallback)
    {
        if(closedCharacters.Count > 0)
        {
            var characterToken = closedCharacters.Dequeue();

            _availableCharacters.Add(characterToken.Character);

            PurchaseCharacter(
                (_, __) => PurchaseCharactersLoop(closedCharacters, connectionCallback),
                characterToken);
        }
        else
        {
            connectionCallback.Invoke(true, null);
        };
    }

    public void GetCharacters(Action<bool, object> connectionCallback)
    {
        var request = new ListUsersCharactersRequest();

        PlayFabClientAPI
            .GetAllUsersCharacters(
                request,
                OnGetCharacters,
                OnFailure,
                connectionCallback);
    }

    private void OnGetCharacters(ListUsersCharactersResult result)
    {
        _availableCharacters = new List<CharacterData>();

        for(int i = 0; i < result.Characters.Count; i++)
        {
            var playfabCharacter = result.Characters[i];

            var characterData =
                _catalog
                    .Characters
                    .First(x => x.Character.Name == playfabCharacter.CharacterName)
                    .Character;

            _availableCharacters.Add(characterData);
        };

        var connectionCallback = (Action<bool, object>) result.CustomData;

        connectionCallback.Invoke(true, _availableCharacters);
    }

    public CharacterData GetCharacterData(string dataPath)
    {
        return
            _catalog
                .Characters
                .First(x => x.Character.DataPath == dataPath)
                .Character;
    }

    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();

        #if UNITY_EDITOR

        Debug.LogError($"Operation has been failed: {errorMessage}");

        #endif

        var connectionCallback = (Action<bool, object>) error.CustomData;

        connectionCallback.Invoke(false, errorMessage);
    }

    private void ConnectionCallbackEmpty(bool success, object response)
    {
    }

    #endregion
}
