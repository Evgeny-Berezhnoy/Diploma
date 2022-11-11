using System.Collections.Generic;

public class ProjectilePhysicsService : IFixedUpdate
{
    #region Fields

    private List<ProjectilePhysicsController> _controllers;
    private ProjectilePhysicsController _controller;

    #endregion

    #region Constructors

    public ProjectilePhysicsService()
    {
        _controllers = new List<ProjectilePhysicsController>();
    }

    #endregion

    #region Interface methods

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        for (int i = _controllers.Count - 1; i >= 0; i--)
        {
            _controller = _controllers[i];

            _controller.OnFixedUpdate(fixedDeltaTime);
        };
    }

    #endregion

    #region Methods

    public void OnAddController(ProjectileController projectileController)
    {
        _controllers.Add(projectileController.PhysicsController);
    }

    public void OnRemoveController(ProjectileController projectileController)
    {
        _controllers.Remove(projectileController.PhysicsController);
    }

    #endregion
}
