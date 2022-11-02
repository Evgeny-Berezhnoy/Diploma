using System;
using System.Linq;

public class HealthController : IController, IDisposableAdvanced
{
    #region Events

    private event Action _onDeath;

    #endregion

    #region Fields

    private bool _isDisposed;
    private int _currentHealth;
    private int _maxHealth;

    #endregion

    #region Interface properties

    public bool IsDisposed => _isDisposed;

    #endregion

    #region Properties

    public bool IsDead => (_currentHealth <= 0);

    #endregion

    #region Constructors

    public HealthController(int maxHealth)
    {
        _currentHealth  = maxHealth;
        _maxHealth      = maxHealth;
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _isDisposed = true;

        var handlers =
            _onDeath
                ?.GetInvocationList()
                .Cast<Action>()
                .ToList();

        if (handlers != null)
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                _onDeath -= handlers[i];
            };
        };

        GC.SuppressFinalize(this);
    }

    #endregion

    #region Methods

    public void AddDeathListener(Action action)
    {
        _onDeath += action;
    }

    public void Hurt(int damage)
    {
        _currentHealth -= damage;
        
        if(_currentHealth <= 0)
        {
            _onDeath?.Invoke();
        };
    }

    public void Recover()
    {
        _currentHealth = _maxHealth;
    }

    #endregion
}
