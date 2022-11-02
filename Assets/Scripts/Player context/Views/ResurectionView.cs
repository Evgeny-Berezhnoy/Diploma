public class ResurectionView : NetworkStation
{
    #region Static fields

    public static readonly int RESURRECTION_MESSAGE = 0;

    #endregion

    #region Observers

    private ISubscriptionProperty _onResurection;

    #endregion

    #region Properties

    public ISubscriptionProperty OnResurrection
    {
        set => _onResurection = value;
    }

    #endregion

    #region Base methods

    protected override void InitializeReadInstrctions()
    {
        _readInstructions.Add(RESURRECTION_MESSAGE, ReadResurrectionMessage);
    }

    #endregion

    #region Methods

    public void RecordResurrectionMessage(int addresser)
    {
        RecordMessage(
            RESURRECTION_MESSAGE,
            addresser);
    }

    private void ReadResurrectionMessage(NetworkMessage message)
    {
        _onResurection.Invoke();
    }

    #endregion
}
