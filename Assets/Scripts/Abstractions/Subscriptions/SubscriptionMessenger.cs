using System;
using System.Collections.Generic;
using Zenject;

public class SubscriptionMessenger<TValue, TResult> : ISubscriptionMessenger<TValue, TResult>
{
    #region Fields

    protected bool _useFirstResult;
    protected List<Func<TValue, TResult>> _listeners;
    protected TValue _value;
    protected TResult _result;
    protected TResult _listenerResult;
    protected TResult _defaultResult;
    protected ValueComparer<TResult> _valueComparer;

    #endregion

    #region Interfaces properties

    public bool IsDisposed { get; private set; }
    public TValue Value
    {
        get => _value;
        set => OnValueChanged(value);
    }
    public TResult Result
    {
        get => _result;
    }
    public TResult DefaultResult
    {
        get => _defaultResult;
    }
    
    #endregion

    #region Constructors

    public SubscriptionMessenger(bool useFirstResult = true, TResult defaultResult = default(TResult))
    {
        _useFirstResult = useFirstResult;
        _listeners      = new List<Func<TValue, TResult>>();
        _defaultResult  = defaultResult;
        _valueComparer  = new ValueComparer<TResult>();
    }

    #endregion

    #region Interfaces Methods

    public void Dispose()
    {
        if (IsDisposed) return;

        IsDisposed = true;

        UnsubscribeAll();

        GC.SuppressFinalize(this);
    }

    public void Subscribe(Func<TValue, TResult> function)
    {
        if (_listeners.Contains(function)) return;

        _listeners.Add(function);
    }

    public void Unsubscribe(Func<TValue, TResult> function)
    {
        if (!_listeners.Contains(function)) return;

        _listeners.Remove(function);
    }

    public void UnsubscribeAll()
    {
        _listeners.Clear();
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

    #region Methods

    private void OnValueChanged(TValue value)
    {
        _value  = value;
        _result = _defaultResult;

        for(int i = 0; i < _listeners.Count; i++)
        {
            _listenerResult = _listeners[i].Invoke(value);
            
            if (!_valueComparer.AreEqual(_listenerResult, _defaultResult))
            {
                _result = _listenerResult;

                if (_useFirstResult) break;
            };
        };
    }

    #endregion
}
