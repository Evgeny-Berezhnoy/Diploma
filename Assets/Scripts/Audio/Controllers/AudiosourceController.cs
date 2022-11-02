using UnityEngine;

public class AudiosourceController : IController
{
    #region Fields

    private bool _isPaused;
    private AudiosourceView _view;

    #endregion

    #region Properties

    public AudiosourceView View => _view;
    public AudioClip Clip => _view.Audiosource.clip;
    public bool IsPlaying => _view.Audiosource.isPlaying;

    #endregion

    #region Constructors

    public AudiosourceController(AudiosourceView view, bool loop)
    {
        _view = view;
        _view.Audiosource.loop = loop;
    }

    public AudiosourceController(AudiosourceView view, bool loop, AudioClip clip, bool playOnAwake) : this(view, loop)
    {
        _view.Audiosource.clip = clip;

        if (playOnAwake)
        {
            Play();
        };
    }
    
    #endregion

    #region Methods

    public void Play()
    {
        if (_isPaused)
        {
            Unpause();
        }
        else
        {
            _view.Audiosource.Play();
        };

        _isPaused = false;
    }

    public void Play(AudioClip clip)
    {
        if (_view.Audiosource.clip == clip && _view.Audiosource.isPlaying)
        {
            return;
        }
        else if (_view.Audiosource.clip == clip && !_view.Audiosource.isPlaying)
        {
            Play();
        }
        else if (_view.Audiosource.isPlaying)
        {
            Stop();

            _view.Audiosource.clip = clip;

            Play();
        }
        else
        {
            _view.Audiosource.clip = clip;

            Play();
        };
    }

    public void Pause()
    {
        if (_isPaused) return;

        _isPaused = true;

        _view.Audiosource.Pause();
    }

    public void Unpause()
    {
        if (!_isPaused) return;

        _isPaused = false;

        _view.Audiosource.UnPause();
    }

    public void Stop()
    {
        _isPaused = false;

        _view.Audiosource.Stop();
    }

    #endregion
}