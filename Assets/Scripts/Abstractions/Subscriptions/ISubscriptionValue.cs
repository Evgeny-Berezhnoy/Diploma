public interface ISubscriptionValue<T> : ISubscriptionProperty<T>
{
    #region Properties

    new T Value { get; set; }

    #endregion

    #region Methods

    void SetValue(T value);

    #endregion
}