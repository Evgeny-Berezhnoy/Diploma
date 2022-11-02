#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(SoundButton))]
public class SoundButtonEditor : Editor
{
    #region Base methods

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    #endregion
}

#endif