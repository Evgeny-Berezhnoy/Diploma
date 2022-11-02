public class ValueComparer<T>
{
    #region Fields

    private bool _genericIsClass;

    #endregion

    #region Constructors

    public ValueComparer()
    {
        _genericIsClass = typeof(T).IsClass;
    }

    #endregion

    #region Interface methods

    public bool AreEqual(T first, T second)
    {
        if (_genericIsClass)
        {
            return AreEqualClasses(first, second);
        }
        else
        {
            return AreEqualStructs(first, second);
        };
    }

    private bool AreEqualClasses(object first, object second)
    {
        return first == second;
    }

    private bool AreEqualStructs(T first, T second)
    {
        return first.Equals(second);
    }

    #endregion
}
