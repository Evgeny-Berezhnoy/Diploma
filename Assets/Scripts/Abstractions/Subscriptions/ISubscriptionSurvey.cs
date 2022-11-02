using System;

public interface ISubscriptionSurvey<TResult> : IDisposableAdvanced
{
    #region Methods

    TResult Get();
    void Subscribe(Func<TResult> function);
    void Unsubscribe(Func<TResult> function);
    void UnsubscribeAll();

    #endregion
}
