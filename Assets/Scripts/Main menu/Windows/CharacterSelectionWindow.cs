using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CharacterSelectionWindow : UIWindow<EMainMenuWindow>
{
    #region Fields

    [Header("UI")]
    [SerializeField] private RectTransform _container;
    [SerializeField] private SoundButton _backButton;

    [Header("Prefabs")]
    [SerializeField] private CharacterSlot _prefab;

    private Dictionary<string, CharacterSlot> _cachedCharacters;

    #endregion

    #region Unity events

    private void Awake()
    {
        _backButton.onClick.AddListener(() => Switch(EMainMenuWindow.Title));

        _cachedCharacters = new Dictionary<string, CharacterSlot>();
    }

    protected override void OnDestroy()
    {
        _backButton.onClick.RemoveAllListeners();

        base.OnDestroy();
    }

    #endregion

    #region Base methods

    public override void Open(object parameters = null)
    {
        if(_cachedCharacters.Count == 0)
        {
            PlayfabCore.Instance.GetCharacters(OnGetCharacters);

            return;
        };

        base.Open(parameters);
    }

    #endregion

    #region Methods

    private void OnGetCharacters(bool success, object response)
    {
        var characters = response as List<CharacterData>;

        if (!success || characters == null)
        {
            var errorData =
                new ErrorWindowData(
                    $"Failed to load characters. Please try again later.",
                    EMainMenuWindow.Title);

            Switch(EMainMenuWindow.Error, errorData);

            return;
        }
        else if(characters.Count == 0)
        {
            PlayfabCore.Instance.PurchaseCharacter(OnPurchaseDefaultCharacter);

            return;
        };

        for(int i = 0; i < characters.Count; i++)
        {
            AddCharacter(characters[i]);
        };

        Open();
    }

    private void OnPurchaseDefaultCharacter(bool success, object response)
    {
        if (!success)
        {
            var errorData =
                new ErrorWindowData(
                    $"Failed to load characters. Please try again later.",
                    EMainMenuWindow.Title);

            Switch(EMainMenuWindow.Error, errorData);

            return;
        };

        Open();
    }

    private void AddCharacter(CharacterData data)
    {
        var characterSlot = Instantiate(_prefab, _container);

        characterSlot.OnSelect += OnSelect;
        characterSlot.Data      = data;

        _cachedCharacters.Add(data.Name, characterSlot);
    }

    private void DeleteCharacter(string characterName)
    {
        if (!_cachedCharacters.ContainsKey(characterName)) return;

        _cachedCharacters.TryGetValue(characterName, out var characterSlot);

        if (_cachedCharacters.Remove(characterName))
        {
            Destroy(characterSlot.gameObject);
        };
    }

    private void DeleteAllCharacters()
    {
        var characterNames =
            _cachedCharacters
                .Keys
                .Cast<string>()
                .ToArray();

        for (int i = 0; i < characterNames.Length; i++)
        {
            DeleteCharacter(characterNames[i]);
        };
    }

    private void OnSelect(CharacterData data)
    {
        DeleteAllCharacters();

        PhotonCore.Instance.Character = data;

        Switch(EMainMenuWindow.Lobby);
    }

    #endregion
}
