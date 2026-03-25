using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

public class GeminiManager : MonoBehaviour
{
    [Header("API Configuration")]
    [SerializeField] private string apiKey = "AIzaSyDn37EEG0BbDF2aHgjrW64UMcCh8a-BV8A"; 
    
    // ✅ CHANGED: 1.5-flash is retired. Using 2.5-flash for 2026 stability.
    [SerializeField] private string modelName = "gemini-2.5-flash"; 

    public QuestionManager questionManager;

    public void GenerateFromNotes(string notes)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("GeminiManager: NO API KEY! Paste your new key in the Inspector.");
            return;
        }
        StartCoroutine(RequestGemini(notes));
    }

    private IEnumerator RequestGemini(string notes)
    {
        // ✅ URL: Using v1beta as it supports the newest 2.5 and 3.1 models
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelName}:generateContent?key={apiKey}";

        string promptText = "Create 5 MCQ questions in this format:\n" +
                            "Question: ...\nA: ...\nB: ...\nC: ...\nD: ...\nAnswer: ...\n" +
                            "From: " + notes;

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
                Debug.Log("<color=green>Gemini Success!</color>");
                HandleResponse(request.downloadHandler.text);
            }
            else
            {
                // This will show exactly why it failed (404, 400, etc.)
                Debug.LogError($"API ERROR {request.responseCode}: {request.downloadHandler.text}");
            }
        }
    }

    private void HandleResponse(string json)
    {
        try {
            var response = JsonUtility.FromJson<GeminiResponse>(json);
            if (response?.candidates != null && response.candidates.Length > 0) {
                questionManager.ParseMultiple(response.candidates[0].content.parts[0].text);
            }
        } catch (Exception e) { Debug.LogError("Parse Error: " + e.Message); }
    }

    [Serializable] public class GeminiRequest { public Content[] contents; }
    [Serializable] public class GeminiResponse { public Candidate[] candidates; }
    [Serializable] public class Candidate { public Content content; }
    [Serializable] public class Content { public Part[] parts; }
    [Serializable] public class Part { public string text; }
}