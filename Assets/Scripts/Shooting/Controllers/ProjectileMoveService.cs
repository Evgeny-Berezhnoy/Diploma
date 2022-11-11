using System.Collections.Generic;

public class ProjectileMoveService : IUpdate
{
    #region Fields

    private List<ProjectileMoveController> _controllers;
    private ProjectileMoveController _controller;

    #endregion

    #region Constructors

    public ProjectileMoveService()
    {
        _controllers = new List<ProjectileMoveController>();
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        for (int i = _controllers.Count - 1; i >= 0; i--)
        {
            _controller = _controllers[i];

            _controller.OnUpdate(deltaTime);
        };
    }

    #endregion

    #region Methods

    public void OnAddController(ProjectileController projectileController)
    {
        _controllers.Add(projectileController.MoveController);
    }

    public void OnRemoveController(ProjectileController projectileController)
    {
        _controllers.Remove(projectileController.MoveController);
    }

    #endregion
}
