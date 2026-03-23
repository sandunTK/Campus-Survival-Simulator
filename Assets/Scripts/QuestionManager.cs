using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button btnA, btnB, btnC, btnD;
    public TextMeshProUGUI feedbackText;

    public PlayerStats stats;

    private string correctAnswer;

    public void ParseAndDisplay(string aiText)
    {
        // SIMPLE PARSER (basic)
        string content = aiText;

        questionText.text = content;

        // TEMP: set correct manually later improve parsing
        correctAnswer = "A";
    }

    public void Answer(string option)
    {
        if(option == correctAnswer)
        {
            feedbackText.text = "Correct!";
            stats.ChangeGrade(10);
            stats.ChangeHappiness(5);
        }
        else
        {
            feedbackText.text = "Wrong!";
            stats.ChangeEnergy(-5);
            stats.ChangeGrade(-5);
        }
    }
}