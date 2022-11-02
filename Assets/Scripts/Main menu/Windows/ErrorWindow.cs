using UnityEngine;
using UnityEngine.UI;

public class ErrorWindow : UIWindow<EMainMenuWindow>
{
    #region Fields

    [Header("UI")]
    [SerializeField] private SoundButton _backButton;
    [SerializeField] private Text _errorText;

    private EMainMenuWindow _previousWindow;
    private object _previousWindowParameters;

    #endregion

    #region Unity events

    private void Awake()
    {
        _backButton.onClick.AddListener(() => Switch(_previousWindow, _previousWindowParameters));
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
        var errorData = (ErrorWindowData) parameters;

        _errorText.text             = errorData.Message;

        _previousWindow             = errorData.PreviousWindow;
        _previousWindowParameters   = errorData.Parameters;

        base.Open(parameters);
    }

    #endregion
}
