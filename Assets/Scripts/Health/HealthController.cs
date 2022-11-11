using System;
using System.Linq;

public class HealthController : IController, IDisposableAdvanced
{
    #region Events

    private event Action _onDeath;
    private event Action<float> _onHealthChanged;

    #endregion

    #region Fields

    private bool _isDisposed;
    private int _currentHealth;
    private int _maxHealth;
    private float _healthPercentage;

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

        var deathHandlers =
            _onDeath
                ?.GetInvocationList()
                .Cast<Action>()
                .ToList();

        if (deathHandlers != null)
        {
            for (int i = 0; i < deathHandlers.Count; i++)
            {
                _onDeath -= deathHandlers[i];
            };
        };

        var healthChangedHandlers =
            _onHealthChanged
                ?.GetInvocationList()
                .Cast<Action<float>>()
                .ToList();

        if (healthChangedHandlers != null)
        {
            for (int i = 0; i < healthChangedHandlers.Count; i++)
            {
                _onHealthChanged -= healthChangedHandlers[i];
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

    public void AddHealthChangedListener(Action<float> action)
    {
        _onHealthChanged += action;
    }

    public void Hurt(int damage)
    {
        _currentHealth -= damage;

        OnHealthChanged();

        if (_currentHealth <= 0)
        {
            _onDeath?.Invoke();
        };
    }

    public void Recover()
    {
        _currentHealth = _maxHealth;

        OnHealthChanged();
    }

    private void OnHealthChanged()
    {
        _healthPercentage = (float)_currentHealth / _maxHealth;

        _onHealthChanged?.Invoke(_healthPercentage);
    }

    #endregion
}
