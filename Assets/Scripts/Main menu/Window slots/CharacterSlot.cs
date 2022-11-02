using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    #region Events

    public event Action<CharacterData> OnSelect;

    #endregion

    #region Fields

    [SerializeField] private Image _image;
    [SerializeField] private Text _name;
    [SerializeField] private SoundButton _selectButton;

    private CharacterData _data;

    #endregion

    #region Properties

    public CharacterData Data
    {
        get => _data;
        set
        {
            _data = value;

            _image.sprite   = _data.Sprite;
            _name.text      = _data.Name;
        }
    }

    #endregion

    #region Unity events

    private void Start()
    {
        _selectButton.onClick.AddListener(() => Select());
    }

    private void OnDestroy()
    {
        _selectButton.onClick.RemoveAllListeners();

        var handlers =
            OnSelect
                ?.GetInvocationList()
                .Cast<Action<CharacterData>>()
                .ToList();

        if (handlers != null)
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                OnSelect -= handlers[i];
            };
        };
    }

    #endregion

    #region Methods

    private void Select()
    {
        OnSelect.Invoke(_data);
    }

    #endregion
}
