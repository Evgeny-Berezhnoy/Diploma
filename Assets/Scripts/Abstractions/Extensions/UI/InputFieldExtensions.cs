using UnityEngine.UI;

public static class InputFieldExtensions
{
    #region Static methods

    public static void Clear(this InputField inputField)
    {
        inputField.SetTextWithoutNotify("");
    }

    #endregion
}
