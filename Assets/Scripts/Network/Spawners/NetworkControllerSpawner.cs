using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkControllerSpawner<TController, TView, TSentryService>
    where TController : IController
    where TView : MonoBehaviour
    where TSentryService : PhotonSentryService
{
    #region Variables

    protected int _index;
    protected TController _controller;
    protected TView _view;
    protected GameObject _go;

    #endregion

    #region Fields

    protected string _templatePath;
    protected GameObject _template;
    protected int _bufferQuantity;
    protected TSentryService _sentryService;
    protected Queue<TController> _instances;

    #endregion

    #region Properties

    public GameObject Template => _template;
    
    #endregion

    #region Constructors

    public NetworkControllerSpawner(
        string templatePath,
        TSentryService sentryService,
        int bufferQuantity)
    {
        _templatePath   = templatePath;
        _template       = Resources.Load<GameObject>(templatePath);
        _bufferQuantity = bufferQuantity;
        _sentryService  = sentryService;

        _instances  = new Queue<TController>();
    }

    public NetworkControllerSpawner(
        string template,
        TSentryService sentryService,
        int bufferQuantity,
        int heatQuantity)
    :
    this(
        template,
        sentryService,
        bufferQuantity)
    {
        Heat(heatQuantity);
    }

    #endregion

    #region Methods

    public virtual TController Pop()
    {
        while(_instances.Count <= _bufferQuantity)
        {
            Push(Create());
        };

        _controller = _instances.Dequeue();

        _view = GetView(_controller);

        EnableView(_view);

        return _controller;
    }

    public virtual void Push(TController controller)
    {
        _view = GetView(controller);

        DisableView(_view);

        _instances.Enqueue(controller);
    }

    public virtual void Heat(int quantity)
    {
        for (_index = 0; _index < quantity; _index++)
        {
            Push(Create());
        };
    }

    protected virtual TController Create()
    {
        _go = Object.Instantiate(_template);

        NetworkInstantiate();

        _sentryService.AddSentry(_go);

        _controller = CreateController(_go);

        return _controller;
    }

    protected abstract TView GetView(TController controller);
    protected abstract void EnableView(TView view);
    protected abstract void DisableView(TView view);
    protected abstract void NetworkInstantiate();
    protected abstract TController CreateController(GameObject go);
    
    #endregion
}