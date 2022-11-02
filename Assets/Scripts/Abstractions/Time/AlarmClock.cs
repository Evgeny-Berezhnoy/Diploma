using System;
using System.Linq;
using UnityEngine;

public class AlarmClock : IUpdate, IDisposableAdvanced
{
    #region Events

    public event Action OnAlarm;

    #endregion

    #region Fields

    private bool _isDisposed;
    private bool _isRunning;
    private float _limit;
    private float _currentTime;

    #endregion

    #region Interface properties

    public bool IsDisposed => _isDisposed;

    #endregion

    #region Properties

    public bool IsRunning => _isRunning;

    #endregion

    #region Constructors

    public AlarmClock(float limit)
    {
        _limit = limit;
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        if (!_isRunning) return;

        _currentTime += deltaTime;
        _currentTime = Mathf.Clamp(_currentTime, 0, _limit);

        if(_currentTime == _limit)
        {
            _isRunning = false;

            _currentTime = 0;

            OnAlarm?.Invoke();
        };
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _isDisposed = true;

        var handlers =
            OnAlarm
                ?.GetInvocationList()
                .Cast<Action>()
                .ToList();

        if(handlers != null)
        {
            for(int i = 0; i < handlers.Count; i++)
            {
                OnAlarm -= handlers[i];
            };
        };

        GC.SuppressFinalize(this);
    }

    #endregion

    #region Methods

    public void Start()
    {
        _isRunning = true;
    }

    public void Stop()
    {
        _isRunning = false;
    }

    public void Restart()
    {
        _isRunning      = true;
        _currentTime    = 0;
    }

    #endregion
}
