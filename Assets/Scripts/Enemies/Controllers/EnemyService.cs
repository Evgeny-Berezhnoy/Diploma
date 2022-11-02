using System.Collections.Generic;

public class EnemyService : IUpdate
{
    #region Fields

    private List<EnemyController> _controllers;

    #endregion

    #region Observers

    private ISubscriptionProperty<EnemyController> _onRemoveController;

    #endregion

    #region Constructors

    public EnemyService(
        ISubscriptionProperty<EnemyController> onRemoveController,
        ISubscriptionMessenger<int, HealthController> onRemoteHit)
    {
        _controllers        = new List<EnemyController>();

        _onRemoveController = onRemoveController;

        onRemoteHit.Subscribe(GetHealthController);
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        for(int i = _controllers.Count - 1; i >= 0; i--)
        {
            var controller = _controllers[i];

            CheckDespawn(controller, i);
        };
    }

    #endregion

    #region Methods

    public void OnAddController(EnemyController controller)
    {
        _controllers.Add(controller);
    }

    public void Clear()
    {
        for (int i = _controllers.Count - 1; i >= 0; i--)
        {
            var controller = _controllers[i];

            _controllers.RemoveAt(i);

            _onRemoveController.Value = controller;
        };
    }

    private void CheckDespawn(EnemyController controller, int index)
    {
        if (controller.NeedsToDespawn)
        {
            _controllers.RemoveAt(index);

            _onRemoveController.Value = controller;
        };
    }

    private HealthController GetHealthController(int photonViewID)
    {
        for(int i = 0; i < _controllers.Count; i++)
        {
            var controller = _controllers[i];

            if(controller.View.PhotonView.ViewID == photonViewID)
            {
                return controller.HealthController;
            };
        };

        return null;
    }

    #endregion
}
