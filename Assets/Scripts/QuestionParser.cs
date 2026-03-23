using UnityEngine;

public class QuestionParser
{
    public static QuestionData Parse(string text)
    {
        QuestionData q = new QuestionData();

        string[] lines = text.Split('\n');

        foreach(string line in lines)
        {
            if(line.StartsWith("Question:"))
                q.question = line.Replace("Question:", "").Trim();

            else if(line.StartsWith("A:"))
                q.A = line.Replace("A:", "").Trim();

            else if(line.StartsWith("B:"))
                q.B = line.Replace("B:", "").Trim();

            else if(line.StartsWith("C:"))
                q.C = line.Replace("C:", "").Trim();

            else if(line.StartsWith("D:"))
                q.D = line.Replace("D:", "").Trim();

            else if(line.StartsWith("Answer:"))
                q.answer = line.Replace("Answer:", "").Trim();
        }

        return q;
    }
}