using UnityEngine;

public class TitleWindow : UIWindow<EMainMenuWindow>
{
    #region Fields

    [SerializeField] private SoundButton _selectCharacterButton;
    [SerializeField] private SoundButton _logoutButton;
    [SerializeField] private SoundButton _quitButton;

    #endregion

    #region Unity events

    private void Awake()
    {
        _selectCharacterButton.onClick.AddListener(() => Switch(EMainMenuWindow.CharacterSelection, null));
        _logoutButton.onClick.AddListener(() => Logout());
        _quitButton.onClick.AddListener(() => UnityApplication.Quit());
    }

    protected override void OnDestroy()
    {
        _selectCharacterButton.onClick.RemoveAllListeners();
        _logoutButton.onClick.RemoveAllListeners();
        _quitButton.onClick.RemoveAllListeners();

        base.OnDestroy();
    }

    #endregion

    #region Methods

    private void Logout()
    {
        var windowData = new AuthentificationWindowData(true);

        Switch(EMainMenuWindow.Authentification, windowData);
    }

    #endregion
}
