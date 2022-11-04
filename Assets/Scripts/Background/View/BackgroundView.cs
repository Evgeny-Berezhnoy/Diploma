using UnityEngine;

public class BackgroundView : MonoBehaviour
{
    #region Fields

    [SerializeField] private SpriteRenderer _spriteRenderer;

    #endregion

    #region Properties

    public Sprite Sprite => _spriteRenderer.sprite; 
    public Material RendererMaterial => _spriteRenderer.material;

    #endregion
}
