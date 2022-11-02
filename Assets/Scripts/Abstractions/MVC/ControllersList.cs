using System;
using System.Collections.Generic;

public class ControllersList : IUpdate, IFixedUpdate, IDisposableAdvanced
{
    #region Fields

    private bool _isDisposed;

    private List<IController> _gameControllers;
    private List<IUpdate> _updateControllers;
    private List<IFixedUpdate> _fixedUpdateControllers;

    #endregion

    #region Interface properties

    public bool IsDisposed => _isDisposed;
    
    #endregion

    #region Constructors

    public ControllersList()
    {
        _gameControllers        = new List<IController>();
        _updateControllers      = new List<IUpdate>();
        _fixedUpdateControllers = new List<IFixedUpdate>();
    }

    #endregion

    #region Interfaces Methods

    public void OnUpdate(float deltaTime)
    {
        for(var i = 0; i < _updateControllers.Count; i++)
        {
            if (_isDisposed) break;

            _updateControllers[i].OnUpdate(deltaTime);
        };
    }

    public void OnFixedUpdate(float deltaFixedUpdate)
    {
        for (var i = 0; i < _fixedUpdateControllers.Count; i++)
        {
            if (_isDisposed) break;

            _fixedUpdateControllers[i].OnFixedUpdate(deltaFixedUpdate);
        };
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _isDisposed = true;

        GC.SuppressFinalize(this);
    }

    #endregion

    #region Methods

    public void AddController(IController controller)
    {
        _gameControllers.Add(controller);

        if (controller is IUpdate updateController)
        {
            _updateControllers.Add(updateController);
        };

        if (controller is IFixedUpdate fixedUpdateController)
        {
            _fixedUpdateControllers.Add(fixedUpdateController);
        };
    }

    #endregion
}