using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionUI : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI txtA, txtB, txtC, txtD;
    public TextMeshProUGUI feedbackText;

    public PlayerStats stats;

    private QuestionData currentQuestion;

    public void DisplayQuestion(string aiText)
    {
        currentQuestion = QuestionParser.Parse(aiText);

        questionText.text = currentQuestion.question;
        txtA.text = currentQuestion.A;
        txtB.text = currentQuestion.B;
        txtC.text = currentQuestion.C;
        txtD.text = currentQuestion.D;

        feedbackText.text = "";
    }

    public void SelectAnswer(string option)
    {
        if(option == currentQuestion.answer)
        {
            feedbackText.text = "✅ Correct!";
            stats.ChangeGrade(10);
            stats.ChangeHappiness(5);
        }
        else
        {
            feedbackText.text = "❌ Wrong!";
            stats.ChangeEnergy(-5);
            stats.ChangeGrade(-5);
        }
    }
}