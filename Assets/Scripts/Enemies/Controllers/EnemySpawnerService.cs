using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerService : IUpdate
{
    #region Fields

    private float _spawnTime;
    private float _currentSpawnTime;
    
    private EnemyMapController _mapController;
    private Dictionary<string, EnemySpawner> _spawners;

    private LinkedList<List<string>> _enemyPacks;
    private LinkedListNode<List<string>> _currentPackNode;
    private Queue<string> _currentEnemyPack;
    
    #endregion

    #region Observers

    private ISubscriptionProperty<EnemyController> _onAddController;
    private ISubscriptionProperty _onEnemySequenceEnd;

    #endregion

    #region Constructors

    public EnemySpawnerService(
        float spawnTime,
        EnemyMapController mapController,
        Dictionary<string, EnemySpawner> spawners,
        LinkedList<List<string>> enemyPacks,
        ISubscriptionProperty<EnemyController> onAddController,
        ISubscriptionProperty onEnemySequenceEnd)
    {
        _spawnTime          = spawnTime;
        
        _mapController      = mapController;
        _spawners           = spawners;

        _enemyPacks         = enemyPacks;
        
        _onAddController    = onAddController;
        _onEnemySequenceEnd = onEnemySequenceEnd;

        RestartEnemyPackSequence();
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        _currentSpawnTime += deltaTime;
        _currentSpawnTime = Mathf.Clamp(_currentSpawnTime, 0, _spawnTime);

        if (_currentSpawnTime == _spawnTime)
        {
            _currentSpawnTime = 0;

            Spawn();
        };
    }

    #endregion

    #region Methods

    private void Spawn()
    {
        if(_currentEnemyPack.Count == 0)
        {
            if (_currentPackNode.Next == null)
            {
                _onEnemySequenceEnd.Invoke();
            }
            else
            {
                _currentPackNode = _currentPackNode.Next;

                for(int i = 0; i < _currentPackNode.Value.Count; i++)
                {
                    _currentEnemyPack.Enqueue(_currentPackNode.Value[i]);
                };
            };
        };

        while(_mapController.HasFreeSpawnPoints && _currentEnemyPack.Count > 0)
        {
            var template    = _currentEnemyPack.Dequeue();

            var controller  = _spawners[template].Pop();

            _onAddController.Value = controller;
        };
    }

    public void OnRemoveController(EnemyController controller)
    {
        _spawners[controller.Template].Push(controller);
    }

    public void RestartEnemyPackSequence()
    {
        _currentSpawnTime   = 0;
        _currentPackNode    = _enemyPacks.First;
        _currentEnemyPack   = new Queue<string>(_currentPackNode.Value);
    }

    #endregion
}
