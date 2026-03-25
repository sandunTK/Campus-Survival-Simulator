using UnityEngine;
using TMPro;

public class AnswerSystem : MonoBehaviour
{
    public TextMeshProUGUI feedbackText;
    public PlayerStats stats;

    private string correctAnswer;

    public void SetCorrectAnswer(string ans)
    {
        correctAnswer = ans;
        feedbackText.text = "";
    }

    public void SelectAnswer(string option)
    {
        string selected = option.Trim().ToUpper();
        string correct = correctAnswer.Trim().ToUpper();

        if (selected == correct)
        {
            feedbackText.text = "✅ Correct!";
            feedbackText.color = Color.green;

            stats.ChangeGrade(+10);
            stats.ChangeHappiness(+5);
        }
        else
        {
            feedbackText.text = "❌ Wrong!";
            feedbackText.color = Color.red;

            stats.ChangeEnergy(-5);
            stats.ChangeGrade(-5);
        }
        Debug.Log("feedbackText = " + feedbackText);
Debug.Log("stats = " + stats);
Debug.Log("correctAnswer = " + correctAnswer);
    }
    

    public void ClearResult()
    {
        feedbackText.text = "";
    }
}