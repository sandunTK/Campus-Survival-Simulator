using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class GeminiManager : MonoBehaviour
{
    private string apiKey = "AIzaSyBLKFBBdpQuQUQDRW7vU5KXJfrrJKryojE";

    public QuestionManager questionManager;

    public void GenerateFromNotes(string notes)
    {
        StartCoroutine(RequestGemini(notes));
    }

    IEnumerator RequestGemini(string notes)
    {
        if (string.IsNullOrEmpty(notes))
        {
            Debug.LogError("Notes empty!");
            yield break;
        }

        string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest?key=" + apiKey;

        string safeNotes = notes.Replace("\"", "\\\"");

        string prompt =
            "Create 5 MCQ questions EXACTLY in this format:\n\n" +
            "Question: ...\nA: ...\nB: ...\nC: ...\nD: ...\nAnswer: ...\n\n" +
            "From this:\n" + safeNotes;

        string json =
            "{ \"contents\": [{ \"parts\": [{ \"text\": \"" + prompt + "\" }] }] }";
             // ✅ Declare request early
    UnityWebRequest request = new UnityWebRequest(url, "POST");
    request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");

    // Send the request
    yield return request.SendWebRequest();

    // Check for success
    if (request.result == UnityWebRequest.Result.Success)
    {
        string response = request.downloadHandler.text;
        Debug.Log("Raw response: " + response);

        // Extract text safely
        string extractedText = ExtractText(response);
        Debug.Log("Extracted Text: " + extractedText);

        // Send to your QuestionManager
        questionManager.ParseMultiple(extractedText);
    }
    else
    {
        Debug.LogError($"Request failed: {request.result} - {request.error}\nResponse: {request.downloadHandler.text}");
    }

    // Local function to extract text from Gemini response
    string ExtractText(string json)
    {
        try
        {
            var data = JsonUtility.FromJson<GeminiResponse>(json);

            if (data.candidates == null || data.candidates.Length == 0)
            {
                Debug.LogError("No candidates found in response");
                return "";
            }

            var firstCandidate = data.candidates[0];
            if (firstCandidate.content?.parts == null || firstCandidate.content.parts.Length == 0)
            {
                Debug.LogError("No parts found in candidate content");
                return "";
            }

            return firstCandidate.content.parts[0].text;
        }
        catch
        {
            Debug.LogError("Failed to parse Gemini response: " + json);
            return "";
    }
}
    }

    // ✅ Move these classes OUTSIDE the method but inside the MonoBehaviour
    [System.Serializable]
    public class GeminiResponse
    {
        public Candidate[] candidates;
    }

    [System.Serializable]
    public class Candidate
    {
        public Content content;
    }

    [System.Serializable]
    public class Content
    {
        public Part[] parts;
    }

    [System.Serializable]
    public class Part
    {
        public string text;
    }
}