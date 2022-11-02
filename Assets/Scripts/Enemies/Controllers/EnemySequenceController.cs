using System.Collections.Generic;

public class EnemySequenceController : IController
{
    #region Constructors

    private bool _enemySequenceEnd;
    private List<EnemyController> _controllersOnBattlefield;

    #endregion

    #region Observers

    private ISubscriptionProperty _onAllEnemiesDestroyed;

    #endregion

    #region Constructors

    public EnemySequenceController(ISubscriptionProperty onAllEnemiesDestroyed)
    {
        _onAllEnemiesDestroyed = onAllEnemiesDestroyed;

        _controllersOnBattlefield = new List<EnemyController>();
    }

    #endregion

    #region Methods

    public void OnRestart()
    {
        _enemySequenceEnd = false;

        _controllersOnBattlefield.Clear();
    }

    public void OnEnemySequenceEnd()
    {
        _enemySequenceEnd = true;

        CheckSequenceConclusion();
    }

    public void OnAddController(EnemyController enemyController)
    {
        _controllersOnBattlefield.Add(enemyController);
    }

    public void OnRemoveController(EnemyController enemyController)
    {
        _controllersOnBattlefield.Remove(enemyController);

        CheckSequenceConclusion();
    }
    
    private void CheckSequenceConclusion()
    {
        if(_enemySequenceEnd && _controllersOnBattlefield.Count == 0)
        {
            _onAllEnemiesDestroyed.Invoke();
        };
    }

    #endregion
}
