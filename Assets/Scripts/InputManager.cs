using UnityEngine;
using TMPro;

public class InputManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public OpenAIManager aiManager;

    public void SendNotes()
    {
        string notes = inputField.text;
        aiManager.GenerateFromNotes(notes); // ✅ FIXED
    }
}