using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class ControllersManager<T> : IUpdate, IFixedUpdate, IDisposableAdvanced
    where T : Enum
{
    #region Fields

    private bool _isDisposed;
    private Dictionary<T, ControllersList> _controllersLists;

    #endregion

    #region Observers

    private ISubscriptionValue<T> _gameState;

    #endregion

    #region Interface properties

    public bool IsDisposed => _isDisposed;
    
    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        if (_isDisposed) return;

        _controllersLists[_gameState.Value].OnUpdate(deltaTime);
    }

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        if (_isDisposed) return;

        _controllersLists[_gameState.Value].OnFixedUpdate(fixedDeltaTime);
    }

    #endregion

    #region Injected methods

    [Inject]
    public void Initialize(
        // GameContextInjector
        [Inject(Id = "GameContext : GameState")] ISubscriptionValue<T> gameState)
    {
        _controllersLists = new Dictionary<T, ControllersList>();

        var gameStates =
            Enum.GetValues(typeof(T))
                .Cast<T>()
                .ToList();

        for (int i = 0; i < gameStates.Count; i++)
        {
            _controllersLists.Add(gameStates[i], new ControllersList());
        };

        _gameState = gameState;
    }

    #endregion

    #region Interface methods
    
    public void Dispose()
    {
        if (_isDisposed) return;

        _isDisposed = true;

        foreach(var controllersList in _controllersLists.Values)
        {
            controllersList.Dispose();
        };

        _controllersLists.Clear();

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

    #region Methods

    public void AddController(IController controller, T firstGameState, params T[] otherGameStates)
    {
        AddController(controller, firstGameState);

        for(int i = 0; i < otherGameStates.Length; i++)
        {
            AddController(controller, otherGameStates[i]);
        };
    }

    private void AddController(IController controller, T gameState)
    {
        _controllersLists[gameState].AddController(controller);
    }

    #endregion
}