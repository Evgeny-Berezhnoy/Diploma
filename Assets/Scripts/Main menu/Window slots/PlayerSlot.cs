using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{
    #region Fields

    [Header("UI")]
    [SerializeField] private Image _image;
    [SerializeField] private Text _playerName;
    [SerializeField] private Text _characterName;

    private CharacterData _data;
    private string _userID;

    #endregion

    #region Properties

    public CharacterData Data
    {
        get => _data;
        set
        {
            _data = value;

            _image.sprite       = _data.Sprite;
            _characterName.text = _data.Name;
        }
    }

    public string PlayerName
    {
        get => _playerName.text;
        set => _playerName.text = value;
    }

    public string UserID
    {
        get => _userID;
        set => _userID = value;
    }

    #endregion
}
