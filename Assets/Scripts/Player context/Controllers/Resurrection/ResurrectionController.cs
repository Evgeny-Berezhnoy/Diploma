public class ResurrectionController : IController
{
    #region Fields

    private ResurectionView _view;
    
    #endregion

    #region Constructors

    public ResurrectionController(ResurectionView view)
    {
        _view = view;
    }

    #endregion

    #region Methods

    public void Resurrect(int addresser)
    {
        _view.RecordResurrectionMessage(addresser);
    }

    #endregion
}
