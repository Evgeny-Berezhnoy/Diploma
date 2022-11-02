using System.Collections.Generic;

public abstract class Spawner<TTemplate, TSpawn>
{
    #region Fields

    protected TTemplate _template;
    protected Stack<TSpawn> _instances;

    #endregion

    #region Properties

    public TTemplate Template => _template;
    public bool IsEmpty => (_instances.Count == 0);

    #endregion

    #region Constructors

    public Spawner(TTemplate prefab)
    {
        _template   = prefab;

        _instances  = new Stack<TSpawn>();
    }

    #endregion

    #region Methods

    public virtual void Push(TSpawn instance)
    {
        _instances.Push(instance);
    }

    public virtual TSpawn Pop()
    {
        if(_instances.Count > 0)
        {
            return _instances.Pop();
        };

        return Create();
    }

    protected abstract TSpawn Create();

    public virtual void Heat(int quantity)
    {
        for(int i = 0; i < quantity; i++)
        {
            Push(Create());
        };
    }

    public virtual List<TSpawn> PopAll()
    {
        var instances = new List<TSpawn>();

        while (!IsEmpty) instances.Add(Pop());

        return instances;
    }

    #endregion
}