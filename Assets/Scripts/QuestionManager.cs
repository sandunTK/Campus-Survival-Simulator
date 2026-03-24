using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI txtA, txtB, txtC, txtD;
    public TextMeshProUGUI feedbackText;

    public PlayerStats stats;

    private List<QuestionData> questions = new List<QuestionData>();
    private int currentIndex = 0;

    private QuestionData currentQuestion;

    // 🔥 Parse MULTIPLE questions
    public void ParseMultiple(string aiText)
    {
        questions.Clear();

        string[] blocks = aiText.Split("Question:");

        foreach (string block in blocks)
        {
            if (block.Trim() == "") continue;

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

        currentIndex = 0;
        ShowQuestion();
    }
    public void AnswerA()
{
    Answer("A");
}

public void AnswerB()
{
    Answer("B");
}

public void AnswerC()
{
    Answer("C");
}

public void AnswerD()
{
    Answer("D");
}

    // 🔥 Show current question
    public void ShowQuestion()
    {
        if (currentIndex >= questions.Count)
        {
            questionText.text = "🎉 Quiz Finished!";
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

    // 🔥 Answer check
    public void Answer(string option)
    {
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

    // 🔥 Next Question
    public void NextQuestion()
    {
        currentIndex++;
        ShowQuestion();
    }



}