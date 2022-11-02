#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public static class UnityApplication
{
    #region Static methods

    public static void Quit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    #endregion
}
