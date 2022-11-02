using System.Collections.Generic;

public class EnemyMoveService : IUpdate
{
    #region Fields

    private EnemyMapController _mapController;
    private List<EnemyMoveController> _controllers;

    #endregion

    #region Constructors

    public EnemyMoveService(
        EnemyMapController mapController)
    {
        _mapController  = mapController;
        _controllers    = new List<EnemyMoveController>();
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        for(int i = 0; i < _controllers.Count; i++)
        {
            var controller = _controllers[i];

            controller.OnUpdate(deltaTime);

            if(controller.CurrentPhase == EEnemyMovementPhase.AwaitingDestination)
            {
                if (_mapController.TryOccupyNewPoint(controller.Transform, out var newDestination, out var endofItinarary))
                {
                    controller.Target = newDestination;
                }
                else if(endofItinarary)
                {
                    controller.CurrentPhase = EEnemyMovementPhase.Disabled;
                };
            };
        };
    }

    #endregion

    #region Methods

    public void OnAddController(EnemyController enemyController)
    {
        _controllers.Add(enemyController.MoveController);

        _mapController.TryOccupyNewPoint(enemyController.MoveController.Transform, out var newDestination, out var _);

        enemyController.MoveController.Target = newDestination;
    }

    public void OnRemoveController(EnemyController enemyController)
    {
        _controllers.Remove(enemyController.MoveController);

        _mapController.DeoccupyPoint(enemyController.MoveController.Transform);
    }

    #endregion
}
