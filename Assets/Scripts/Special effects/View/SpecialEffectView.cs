using UnityEngine;

public class SpecialEffectView : MonoBehaviour
{
    #region Fields

    [SerializeField] private Animator _animator;

    private bool _isPlaying;

    #endregion

    #region Properties

    public bool IsPlaying => _isPlaying;

    #endregion

    #region Methods

    public void Play(AnimationClip animation)
    {
        _animator.enabled = true;

        _animator.Play(animation.name);

        _isPlaying = true;
    }

    public void OnReturnToIdle()
    {
        _isPlaying = false;

        _animator.enabled = false;
    }

    public void PlayAudio(AudioClip audioclip)
    {
        AudioSource.PlayClipAtPoint(audioclip, transform.position);
    }

    #endregion
}
