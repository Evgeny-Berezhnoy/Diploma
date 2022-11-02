using UnityEngine;
using Zenject;

public class GameplayMenuInjector : MonoInstaller
{
    #region Fields

    [SerializeField] private GameplayMenuController _gameplayMenuController;

    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container
            .Bind<GameplayMenuController>()
            .FromInstance(_gameplayMenuController);
    }

    #endregion
}
