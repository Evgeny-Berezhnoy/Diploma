using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetService : IUpdate
{
    #region Fields

    private Transform _defaultTarget;
    private List<EnemyTargetController> _controllers;

    #endregion

    #region Observers

    private ISubscriptionSurvey<Transform> _targetSurvey;

    #endregion

    #region Constructors

    public EnemyTargetService(
        Transform defaultTarget,
        ISubscriptionSurvey<Transform> targetSurvey)
    {
        _defaultTarget  = defaultTarget;
        _controllers    = new List<EnemyTargetController>();

        _targetSurvey = targetSurvey;
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        for (int i = 0; i < _controllers.Count; i++)
        {
            var controller = _controllers[i];

            if (!controller.TargetIsActive)
            {
                controller.Target = GetTarget();
            };

            controller.OnUpdate(deltaTime);
        };
    }

    #endregion

    #region Methods

    public void OnAddController(EnemyController enemyController)
    {
        enemyController.TargetController.Target = GetTarget();

        _controllers.Add(enemyController.TargetController);
    }

    public void OnRemoveController(EnemyController enemyController)
    {
        _controllers.Remove(enemyController.TargetController);
    }

    private Transform GetTarget()
    {
        var target = _targetSurvey.Get();

        if(target == default)
        {
            target = _defaultTarget;
        };

        return target;
    }

    #endregion
}
