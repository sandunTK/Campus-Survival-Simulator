using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI txtA, txtB, txtC, txtD;
    public TextMeshProUGUI feedbackText;

    [Header("Player Stats")]
    public PlayerStats stats;

    // List of parsed questions
    private List<QuestionData> questions = new List<QuestionData>();
    private int currentIndex = 0;
    private QuestionData currentQuestion;

    // 🔥 Parse MULTIPLE questions from AI/Gemini
    public void ParseMultiple(string aiText)
    {
        questions.Clear();
        currentIndex = 0;

        // Split AI text into blocks based on "Question:"
        string[] blocks = aiText.Split("Question:");

        foreach (string block in blocks)
        {
            if (string.IsNullOrWhiteSpace(block)) continue;

            QuestionData q = new QuestionData();
            string full = "Question:" + block;
            string[] lines = full.Split('\n');

            foreach (string line in lines)
            {
                if (line.StartsWith("Question:"))
                    q.question = line.Replace("Question:", "").Trim();
                else if (line.StartsWith("A:"))
                    q.A = line.Replace("A:", "").Trim();
                else if (line.StartsWith("B:"))
                    q.B = line.Replace("B:", "").Trim();
                else if (line.StartsWith("C:"))
                    q.C = line.Replace("C:", "").Trim();
                else if (line.StartsWith("D:"))
                    q.D = line.Replace("D:", "").Trim();
                else if (line.StartsWith("Answer:"))
                    q.answer = line.Replace("Answer:", "").Trim();
            }

            questions.Add(q);
        }

        // Show first question after parsing
        if (questions.Count > 0)
            ShowQuestion();
        else
            questionText.text = "⚠️ No questions generated!";
    }

    // 🔥 Show current question on UI
    public void ShowQuestion()
    {
        if (currentIndex >= questions.Count)
        {
            questionText.text = "🎉 Quiz Finished!";
            txtA.text = txtB.text = txtC.text = txtD.text = "";
            feedbackText.text = "";
            return;
        }

        currentQuestion = questions[currentIndex];

        questionText.text = currentQuestion.question;
        txtA.text = currentQuestion.A;
        txtB.text = currentQuestion.B;
        txtC.text = currentQuestion.C;
        txtD.text = currentQuestion.D;
        feedbackText.text = "";
    }

    // 🔥 Answer checks
    public void AnswerA() => Answer("A");
    public void AnswerB() => Answer("B");
    public void AnswerC() => Answer("C");
    public void AnswerD() => Answer("D");

    private void Answer(string option)
    {
        if (currentQuestion == null) return;

        if (option == currentQuestion.answer)
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

    // 🔥 Next question button
    public void NextQuestion()
    {
        currentIndex++;
        ShowQuestion();
    }

    // Optional: Reset quiz manually if needed
    public void ResetQuiz()
    {
        currentIndex = 0;
        questions.Clear();
        questionText.text = "Click Study to start!";
        txtA.text = txtB.text = txtC.text = txtD.text = "";
        feedbackText.text = "";
    }
}


