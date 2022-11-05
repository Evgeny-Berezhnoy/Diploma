using System.Collections.Generic;
using UnityEngine;

public class ExplosionView : MonoBehaviour
{
    #region Static fields

    public const short NO_SPRITE_INDEX = -1;

    #endregion

    #region Fields

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private AudioClip _clip;

    private Dictionary<short, Sprite> _spriteNumbers = new Dictionary<short, Sprite>();

    private short _spriteNumber;
    
    #endregion

    #region Properties

    public short SpriteNumber
    {
        get => _spriteNumber;
        set
        {
            _spriteNumber = value;

            if (_spriteNumbers.ContainsKey(_spriteNumber))
            {
                _spriteRenderer.sprite = _spriteNumbers[_spriteNumber];
            };
        }
    }

    public int SpritesMaxIndex => _sprites.Length - 1;
    
    #endregion

    #region Unity events

    private void Start()
    {
        _spriteNumbers.Add(-1, null);

        for(short i = 0; i < _sprites.Length; i++)
        {
            _spriteNumbers.Add(i, _sprites[i]);
        };

        SpriteNumber = NO_SPRITE_INDEX;
    }

    #endregion

    #region Methods

    public void Spawn()
    {
        SpriteNumber = NO_SPRITE_INDEX;

        AudioSource.PlayClipAtPoint(_clip, transform.position);
    }

    public void Despawn()
    {
        SpriteNumber = NO_SPRITE_INDEX;
    }

    #endregion
}
