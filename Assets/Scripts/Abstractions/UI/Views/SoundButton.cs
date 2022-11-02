using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundButton : Button
{
    #region Fields

    [SerializeField] public AudioClip _clip;

    #endregion

    #region Base methods

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (interactable)
        {
            AudioSource.PlayClipAtPoint(_clip, Vector3.zero);
        };

        base.OnPointerClick(eventData);
    }

    #endregion
}
