using UnityEngine;
using UnityEngine.UI;

public class AccountCreationWindow : UIWindow<EMainMenuWindow>
{
    #region Fields

    [Header("UI")]
    [SerializeField] private InputField _username;
    [SerializeField] private InputField _mail;
    [SerializeField] private InputField _password;
    [SerializeField] private InputField _confirmPassword;
    [SerializeField] private SoundButton _backButton;
    [SerializeField] private SoundButton _submitButton;

    #endregion

    #region Unity events

    private void Awake()
    {
        _backButton.onClick.AddListener(() => Switch(EMainMenuWindow.Authentification));
        _submitButton.onClick.AddListener(() => Submit());
    }

    protected override void OnDestroy()
    {
        _backButton.onClick.RemoveAllListeners();
        _submitButton.onClick.RemoveAllListeners();

        base.OnDestroy();
    }

    #endregion

    #region Base methods

    public override void Open(object parameters = null)
    {
        _username.Clear();
        _mail.Clear();
        _password.Clear();
        _confirmPassword.Clear();

        base.Open(parameters);
    }

    #endregion

    #region Methods

    private void Submit()
    {
        var error = "";

        if (string.IsNullOrWhiteSpace(_username.text))
        {
            error = string.Concat(error, $"\nUsername can't be empty.");
        };

        if (string.IsNullOrWhiteSpace(_mail.text))
        {
            error = string.Concat(error, $"\nMail can't be empty.");
        };
        
        if (string.IsNullOrWhiteSpace(_password.text))
        {
            error = string.Concat(error, $"\nPassword can't be empty.");
        };

        if (!_password.text.Equals(_confirmPassword.text))
        {
            error = string.Concat(error, $"\nConfirm password field must be equal to password.");
        };
        
        if(_password.text.Length < 6
            || _password.text.Length > 100)
        {
            error = string.Concat(error, $"\nPassword should contain string with a length between 6 and 100.");
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

        PlayfabCore
            .Instance
            .CreateAccount(
                _username.text,
                _mail.text,
                _password.text,
                OnSubmit);
    }

    private void OnSubmit(bool success, object response)
    {
        if (!success)
        {
            var errorData =
                new ErrorWindowData(
                    "Account creation failed. Please, try again.",
                    WindowType);

            Switch(EMainMenuWindow.Error, errorData);

            return;
        };

        PlayerPrefs.SetString(PlayfabCore.USERNAME_STORAGE_KEY, _username.text);
        PlayerPrefs.SetString(PlayfabCore.PASSWORD_STORAGE_KEY, _password.text);

        Switch(EMainMenuWindow.Authentification);
    }

    #endregion
}
