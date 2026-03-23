using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class OpenAIManager : MonoBehaviour
{
    private string apiKey = "Ysk-proj-yYq2rrcht7KWx4JgMmpl1NdKZIGDn8tfuA0dFqBTkOlvo2K76et8fmbAu0PYKS7QaGbViLmVGpT3BlbkFJFuO-JnZDicqeFgzk3uNh-GcDMgthMezl72OLhtv4sH-of02Kaw-SMj98oNzAjuuxxNBxsA94gA";

    public QuestionManager questionManager;

    public void GenerateFromNotes(string notes)
    {
        StartCoroutine(RequestAI(notes));
    }

    IEnumerator RequestAI(string notes)
    {
        string url = "https://api.openai.com/v1/chat/completions";

        string prompt =
        "Create 1 MCQ question in this format:\n" +
        "Question: ...\nA: ...\nB: ...\nC: ...\nD: ...\nAnswer: ...\n\n" +
        "From this:\n" + notes;

        string json =
        "{ \"model\": \"gpt-4.1-mini\", " +
        "\"messages\": [{\"role\":\"user\",\"content\":\"" + prompt + "\"}] }";

        UnityWebRequest req = new UnityWebRequest(url, "POST");

        byte[] body = Encoding.UTF8.GetBytes(json);
        req.uploadHandler = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();

        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string response = req.downloadHandler.text;
            questionManager.ParseAndDisplay(response);
        }
        else
        {
            Debug.LogError(req.error);
        }
    }
}