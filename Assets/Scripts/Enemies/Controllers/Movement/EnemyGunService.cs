using System.Collections.Generic;

public class EnemyGunService : IUpdate
{
    #region Fields

    private List<EnemyGunController> _controllers;

    #endregion

    #region Constructors

    public EnemyGunService()
    {
        _controllers = new List<EnemyGunController>();
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        for (int i = _controllers.Count - 1; i >= 0; i--)
        {
            _controllers[i].OnUpdate(deltaTime);
        };
    }

    #endregion

    #region Methods

    public void OnAddController(EnemyController controller)
    {
        _controllers.Add(controller.GunController);
    }

    public void OnRemoveController(EnemyController enemyController)
    {
        _controllers.Remove(enemyController.GunController);
    }
    
    #endregion
}
