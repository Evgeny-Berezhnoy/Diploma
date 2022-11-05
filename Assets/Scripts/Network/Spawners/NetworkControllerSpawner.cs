using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class NetworkControllerSpawner<TController, TView>
    where TController : IController
    where TView : MonoBehaviour
{
    #region Fields

    protected string _template;
    protected int _bufferQuantity;
    protected PhotonView _root;
    protected Queue<TController> _instances;

    protected TController _controller;
    protected TView _view;
    
    #endregion

    #region Properties

    public string Template => _template;
    public PhotonView Root => _root;
    
    #endregion

    #region Constructors

    public NetworkControllerSpawner(string template, int bufferQuantity, PhotonView root)
    {
        _template       = template;
        _bufferQuantity = bufferQuantity;
        _root           = root;

        _instances  = new Queue<TController>();
    }

    public NetworkControllerSpawner(string template, int bufferQuantity, PhotonView root, int heatQuantity) : this(template, bufferQuantity, root)
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
        for (int i = 0; i < quantity; i++)
        {
            Push(Create());
        };
    }

    protected virtual TController Create()
    {
        var go = PhotonCore.Instance.InstantiateInstance(_template, _root.transform.position, _root.transform.rotation);

        return CreateController(go);
    }

    protected abstract TView GetView(TController controller);
    protected abstract void EnableView(TView view);
    protected abstract void DisableView(TView view);
    protected abstract TController CreateController(GameObject go);
    
    #endregion
}