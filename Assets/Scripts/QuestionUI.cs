using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionUI : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI txtA, txtB, txtC, txtD;
    public TextMeshProUGUI feedbackText;

    public Button btnA, btnB, btnC, btnD;

    public PlayerStats stats;

    private QuestionData currentQuestion;
    private bool answered = false;

    // ================= SHOW QUESTION =================
    public void DisplayQuestion(string aiText)
    {
        currentQuestion = QuestionParser.Parse(aiText);

        questionText.text = currentQuestion.question;
        txtA.text = currentQuestion.A;
        txtB.text = currentQuestion.B;
        txtC.text = currentQuestion.C;
        txtD.text = currentQuestion.D;

        feedbackText.text = "";
        answered = false;

        EnableButtons(true);
    }

    // ================= SELECT ANSWER =================
    public void SelectAnswer(string option)
    {
        if (answered) return; // ❗ prevent double click

        answered = true;

        if(option == currentQuestion.answer)
        {
            feedbackText.text = "✅ Correct!";
            feedbackText.color = Color.green;

            stats.ChangeGrade(+10);
            stats.ChangeHappiness(+5);
            stats.ChangeEnergy(-3);
        }
        else
        {
            feedbackText.text = "❌ Wrong!";
            feedbackText.color = Color.red;

            stats.ChangeEnergy(-8);
            stats.ChangeGrade(-5);
            stats.ChangeHappiness(-3);
        }

        EnableButtons(false);

        // 👉 auto next question (optional)
        Invoke(nameof(ClearFeedback), 2f);
    }

    // ================= CLEAR TEXT =================
    void ClearFeedback()
    {
        feedbackText.text = "";
    }

    // ================= BUTTON CONTROL =================
    void EnableButtons(bool state)
    {
        btnA.interactable = state;
        btnB.interactable = state;
        btnC.interactable = state;
        btnD.interactable = state;
    }
}