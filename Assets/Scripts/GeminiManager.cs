using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

public class GeminiManager : MonoBehaviour
{
    [Header("API Settings")]
    [SerializeField] private string apiKey = "AIzaSyBp4OD5Ia1NBsDH345OXq6w10kqGfKQPLw"; // ⚠️ Replace with a new key!
    
    // ✅ TRY THESE STRINGS IF ONE 404s: 
    // 1. "gemini-1.5-flash-latest"
    // 2. "gemini-2.0-flash-exp" (Latest 2.0 version)
    // 3. "gemini-1.5-flash-002" (Specific versioned name)
    [SerializeField] private string modelName = "gemini-1.5-flash-latest"; 

    public QuestionManager questionManager;

    public void GenerateFromNotes(string notes)
    {
        StartCoroutine(RequestGemini(notes));
    }

    IEnumerator RequestGemini(string notes)
    {
        if (string.IsNullOrEmpty(notes)) yield break;

        // ✅ Using v1beta with the "models/" prefix explicitly
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelName}:generateContent?key={apiKey}";

        string promptText = "Create 5 MCQ questions in this format:\n\n" +
                            "Question: ...\nA: ...\nB: ...\nC: ...\nD: ...\nAnswer: ...\n\n" +
                            "From this text: " + notes;

        GeminiRequest requestData = new GeminiRequest {
            contents = new Content[] { new Content { parts = new Part[] { new Part { text = promptText } } } }
        };

        string jsonPayload = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Gemini Success!");
                HandleResponse(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"<color=red><b>GEMINI ERROR {request.responseCode}</b></color>");
                Debug.LogError("Server Response: " + request.downloadHandler.text);
                
                // 💡 AUTO-HINT for 404
                if (request.responseCode == 404) {
                    Debug.LogWarning("404 FIX: Try changing modelName to 'gemini-1.5-flash-latest' or 'gemini-2.0-flash' in the Inspector.");
                }
            }
        }
    }

    private void HandleResponse(string json)
    {
        try {
            var response = JsonUtility.FromJson<GeminiResponse>(json);
            if (response?.candidates != null && response.candidates.Length > 0) {
                string text = response.candidates[0].content.parts[0].text;
                questionManager.ParseMultiple(text);
            }
        } catch (Exception e) {
            Debug.LogError("Parse Error: " + e.Message);
        }
    }

    [Serializable] public class GeminiRequest { public Content[] contents; }
    [Serializable] public class GeminiResponse { public Candidate[] candidates; }
    [Serializable] public class Candidate { public Content content; }
    [Serializable] public class Content { public Part[] parts; }
    [Serializable] public class Part { public string text; }
}