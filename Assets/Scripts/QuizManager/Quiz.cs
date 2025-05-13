using UnityEngine;

[CreateAssetMenu(fileName = "Quiz", menuName = "Quiz")]
public class Quiz : ScriptableObject
{
    public Question[] questions = new Question[4];
}

[System.Serializable]
public class Question
{
    public string questionText;
    public string[] answers = new string[4];
    public int correctAnswerIndex;
    public StatChange correctAnswerStats;
    public StatChange wrongAnswerStats;
}

[System.Serializable]
public class StatChange
{
    public float foreignAffairsDelta;
    public float euroscepticismDelta;
    public int budgetDelta;
    public bool failQuiz = true;
}

public enum QuizMode
{
    NORMAL,
    MAIN_EVENT
}

