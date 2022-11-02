using System;
using System.Linq;
using UnityEngine;

public abstract class UIWindow<T> : MonoBehaviour
    where T : Enum
{
    #region Events

    public event Action<T, object> SwitchOnWindow;

    #endregion

    #region Fields

    [SerializeField] public T WindowType;

    #endregion

    #region Unity events

    protected virtual void OnDestroy()
    {
        var handlers =
            SwitchOnWindow
                ?.GetInvocationList()
                .Cast<Action<T, object>>()
                .ToList();

        if (handlers != null)
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                SwitchOnWindow -= handlers[i];
            };
        };
    }

    #endregion

    #region Methods

    public virtual void Switch(T windowType, object parameters = null)
    {
        SwitchOnWindow.Invoke(windowType, parameters);
    }

    public virtual void Open(object parameters = null)
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
