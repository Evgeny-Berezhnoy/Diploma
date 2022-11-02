public sealed class ErrorWindowData
{
    #region Fields

    public readonly string Message;
    public readonly EMainMenuWindow PreviousWindow;
    public readonly object Parameters;

    #endregion

    #region Constructors

    public ErrorWindowData(string message, EMainMenuWindow previousWindow, object parameters = null)
    {
        Message         = message;
        PreviousWindow  = previousWindow;
        Parameters      = parameters;
    }

    #endregion
}
