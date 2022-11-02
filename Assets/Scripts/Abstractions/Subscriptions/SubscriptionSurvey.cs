using System;
using System.Collections.Generic;
using Zenject;

public class SubscriptionSurvey<TResult> : ISubscriptionSurvey<TResult>
{
    #region Fields

    protected bool _useFirstResult;
    protected List<Func<TResult>> _listeners;
    protected TResult _result;
    protected TResult _listenerResult;
    protected TResult _defaultResult;
    protected ValueComparer<TResult> _valueComparer;

    #endregion

    #region Interfaces properties

    public bool IsDisposed { get; private set; }
    
    #endregion

    #region Constructors

    public SubscriptionSurvey(bool useFirstResult = true, TResult defaultResult = default(TResult))
    {
        _useFirstResult = useFirstResult;
        _listeners      = new List<Func<TResult>>();
        _defaultResult  = defaultResult;
        _valueComparer  = new ValueComparer<TResult>();
    }

    #endregion

    #region Interfaces Methods

    public TResult Get()
    {
        _result = _defaultResult;

        for (int i = 0; i < _listeners.Count; i++)
        {
            _listenerResult = _listeners[i].Invoke();

            if (!_valueComparer.AreEqual(_listenerResult, _defaultResult))
            {
                _result = _listenerResult;

                if (_useFirstResult) break;
            };
        };

        return _result;
    }

    public void Subscribe(Func<TResult> function)
    {
        if (_listeners.Contains(function)) return;

        _listeners.Add(function);
    }

    public void Unsubscribe(Func<TResult> function)
    {
        if (!_listeners.Contains(function)) return;

        _listeners.Remove(function);
    }

    public void UnsubscribeAll()
    {
        _listeners.Clear();
    }

    public void Dispose()
    {
        if (IsDisposed) return;

        IsDisposed = true;

        UnsubscribeAll();

        GC.SuppressFinalize(this);
    }

    #endregion

    #region Injected methods

    [Inject]
    protected void Initialize(
        // AbstractionsInjector
        [Inject] Disposer disposer)
    {
        disposer.Subscribe(Dispose);
    }

    #endregion
}
