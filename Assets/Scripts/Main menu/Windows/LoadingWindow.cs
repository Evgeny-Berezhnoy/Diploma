using UnityEngine;

public class LoadingWindow : UIWindow<EMainMenuWindow>
{
    #region Fields

    [SerializeField, Range(0.03f, 0.2f)] private float _updateInterval;
    [SerializeField, Range(1f, 5f)] private float _frequancy;
    [SerializeField] private GameObject _loadImage;

    private float _angleSpeed;
    private float _localRotationZ;
    private float _timePassed;

    #endregion

    #region Unity events

    private void Awake()
    {
        _angleSpeed = 360f * _frequancy * _updateInterval;
    }

    private void Update()
    {
        _timePassed += Time.deltaTime;

        if(_timePassed >= _updateInterval)
        {
            _localRotationZ -= _angleSpeed;

            _loadImage.transform.rotation = Quaternion.Euler(0, 0, _localRotationZ);

            _timePassed = 0;
        };
    }

    #endregion

    #region Base methods

    public override void Open(object parameters = null)
    {
        _loadImage.transform.localRotation = Quaternion.identity;

        base.Open(parameters);
    }

    public override void Close()
    {
        _localRotationZ = 0;
        _timePassed     = 0;

        _loadImage.transform.localRotation = Quaternion.identity;

        base.Close();
    }

    #endregion
}
