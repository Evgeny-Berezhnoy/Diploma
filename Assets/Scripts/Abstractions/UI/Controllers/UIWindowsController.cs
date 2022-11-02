using System;
using System.Linq;
using UnityEngine;

public class UIWindowsController<T> : MonoBehaviour
    where T : Enum
{
    #region Fields

    [SerializeField] protected UIWindow<T>[] _windows;
    [SerializeField] protected T _defaultActiveWindow;

    protected UIWindow<T> _activeWindow;

    #endregion

    #region Properties

    public UIWindow<T>[] Windows => _windows;

    #endregion

    #region Methods

    public virtual void Initialize()
    {
        for(int i = 0; i < _windows.Length; i++)
        {
            var window = _windows[i];

            window.SwitchOnWindow += SwitchOnWindow;
            window.Close();
        };

        _activeWindow = _windows.FirstOrDefault(x => x.WindowType.Equals(_defaultActiveWindow));

        if(_activeWindow == null)
        {
            throw new Exception($"There is no window of type {_defaultActiveWindow}");
        };

        _activeWindow.Open();
    }
    
    protected virtual void SwitchOnWindow(T windowType, object parameters)
    {
        var window = _windows.FirstOrDefault(x => x.WindowType.Equals(windowType));

        if(window == default(UIWindow<T>))
        {
            throw new Exception($"Unable to find a window of type {windowType}");
        };

        _activeWindow.Close();

        _activeWindow = window;

        _activeWindow.Open(parameters);
    }

    #endregion
}
