using UnityEngine;
using TMPro;

public class InputManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public GeminiManager aiManager; // ✅ FIXED

    public void OnClickGenerate()
    {
        string notes = inputField.text;

        if (string.IsNullOrEmpty(notes))
        {
            Debug.LogError("Input is empty!");
            return;
        }

        aiManager.GenerateFromNotes(notes);
    }
}