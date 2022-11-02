using UnityEngine;
using Zenject;

public static class DiContainerExtensions
{
    #region Extension methods

    public static void BindSubscriptionProperty(this DiContainer Container, object id)
    {
        var subscriptionProperty = new ActionSubscriptionProperty();

        Container.Inject(subscriptionProperty);

        Container
            .Bind<ISubscriptionProperty>()
            .WithId(id)
            .To<ActionSubscriptionProperty>()
            .FromInstance(subscriptionProperty)
            .AsCached();
    }

    public static void BindSubscriptionProperty<T>(this DiContainer Container)
    {
        var subscriptionProperty = new SubscriptionProperty<T>();

        Container.Inject(subscriptionProperty);

        Container
            .Bind<ISubscriptionProperty<T>>()
            .To<SubscriptionProperty<T>>()
            .FromInstance(subscriptionProperty)
            .AsCached();
    }

    public static void BindSubscriptionProperty<T>(this DiContainer Container, object id)
    {
        var subscriptionProperty = new SubscriptionProperty<T>();

        Container.Inject(subscriptionProperty);

        Container
            .Bind<ISubscriptionProperty<T>>()
            .WithId(id)
            .To<SubscriptionProperty<T>>()
            .FromInstance(subscriptionProperty)
            .AsCached();
    }

    public static void BindSubscriptionValue<T>(this DiContainer Container)
    {
        var subscriptionValue = new SubscriptionValue<T>();

        Container.Inject(subscriptionValue);

        Container
            .Bind<ISubscriptionValue<T>>()
            .To<SubscriptionValue<T>>()
            .FromInstance(subscriptionValue)
            .AsCached();
    }

    public static void BindSubscriptionValue<T>(this DiContainer Container, object id)
    {
        var subscriptionValue = new SubscriptionValue<T>();

        Container.Inject(subscriptionValue);

        Container
            .Bind<ISubscriptionValue<T>>()
            .WithId(id)
            .To<SubscriptionValue<T>>()
            .FromInstance(subscriptionValue)
            .AsCached();
    }

    public static void BindSubscriptionMessenger<TValue, TResult>(this DiContainer Container, bool useFirstResult = true, TResult defaultResult = default(TResult))
    {
        var subscriptionMessenger = new SubscriptionMessenger<TValue, TResult>(useFirstResult, defaultResult);

        Container.Inject(subscriptionMessenger);

        Container
            .Bind<ISubscriptionMessenger<TValue, TResult>>()
            .To<SubscriptionMessenger<TValue, TResult>>()
            .FromInstance(subscriptionMessenger)
            .AsCached();
    }

    public static void BindSubscriptionMessenger<TValue, TResult>(this DiContainer Container, object id, bool useFirstResult = true, TResult defaultResult = default(TResult))
    {
        var subscriptionMessenger = new SubscriptionMessenger<TValue, TResult>(useFirstResult, defaultResult);

        Container.Inject(subscriptionMessenger);

        Container
            .Bind<ISubscriptionMessenger<TValue, TResult>>()
            .WithId(id)
            .To<SubscriptionMessenger<TValue, TResult>>()
            .FromInstance(subscriptionMessenger)
            .AsCached();
    }

    public static void BindSubscriptionSurvey<TResult>(this DiContainer Container, bool useFirstResult = true, TResult defaultResult = default(TResult))
    {
        var subscriptionSurvey = new SubscriptionSurvey<TResult>(useFirstResult, defaultResult);

        Container.Inject(subscriptionSurvey);

        Container
            .Bind<ISubscriptionSurvey<TResult>>()
            .To<SubscriptionSurvey<TResult>>()
            .FromInstance(subscriptionSurvey)
            .AsCached();
    }

    public static void BindSubscriptionSurvey<TResult>(this DiContainer Container, object id, bool useFirstResult = true, TResult defaultResult = default(TResult))
    {
        var subscriptionSurvey = new SubscriptionSurvey<TResult>(useFirstResult, defaultResult);

        Container.Inject(subscriptionSurvey);

        Container
            .Bind<ISubscriptionSurvey<TResult>>()
            .WithId(id)
            .To<SubscriptionSurvey<TResult>>()
            .FromInstance(subscriptionSurvey)
            .AsCached();
    }

    public static void BindComponent<T>(this DiContainer Container, T injectable)
        where T : Component
    {
        Container
            .Bind<T>()
            .FromInstance(injectable);
    }

    public static void BindComponent<T>(this DiContainer Container, T injectable, object id)
        where T : Component
    {
        Container
            .Bind<T>()
            .WithId(id)
            .FromInstance(injectable);
    }
    
    public static void BindMonoBehaviour<T>(this DiContainer Container, T injectable)
        where T : MonoBehaviour
    {
        Container
            .Bind<T>()
            .FromInstance(injectable);
    }

    public static void BindMonoBehaviour<T>(this DiContainer Container, T injectable, object id)
        where T : MonoBehaviour
    {
        Container
            .Bind<T>()
            .WithId(id)
            .FromInstance(injectable);
    }

    #endregion
}