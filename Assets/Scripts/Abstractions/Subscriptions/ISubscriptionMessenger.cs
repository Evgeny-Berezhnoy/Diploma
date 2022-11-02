using System;

public interface ISubscriptionMessenger<TValue, TResult> : IDisposableAdvanced
{
    #region Properties

    TValue Value { get; set; }
    TResult Result { get; }
    TResult DefaultResult { get; }

    #endregion

    #region Methods

    void Subscribe(Func<TValue, TResult> function);
    void Unsubscribe(Func<TValue, TResult> function);
    void UnsubscribeAll();

    #endregion
}
