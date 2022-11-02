using System;
using UnityEngine;

public struct InputKeyInstruction
{
    #region Fields

    private KeyCode _key;
    private Action _instruction;
    private bool _onKeyDown;

    #endregion

    #region Constructors

    public InputKeyInstruction(KeyCode key, Action instruction, bool onKeyDown = false)
    {
        _key            = key;
        _instruction    = instruction;
        _onKeyDown      = onKeyDown;
    }

    #endregion

    #region Methods

    public void CheckInput()
    {
        if(_onKeyDown && Input.GetKeyDown(_key))
        {
            _instruction.Invoke();
        }
        else if (!_onKeyDown && Input.GetKey(_key))
        {
            _instruction.Invoke();
        };
    }

    #endregion
}
