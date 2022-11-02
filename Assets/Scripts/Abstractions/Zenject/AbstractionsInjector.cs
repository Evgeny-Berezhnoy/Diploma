using Zenject;

public class AbstractionsInjector : MonoInstaller
{
    #region Base methods

    public override void InstallBindings()
    {
        Container
            .Bind<Disposer>()
            .AsSingle();

        Container
            .Bind<DiContainerWrapper>()
            .AsSingle();
    }

    #endregion
}