using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuizzesDatabase", menuName = "Scriptable Objects/QuizzesDatabase")]
public class QuizzesDatabase : ScriptableObject
{
    [SerializeField] public List<Quiz> quizzes;

    [System.Serializable]
    public class Quiz
    {
        public string quizDate;
        public string quizName;
        public string question;
        [SerializeField] public List<Answer> answers;

        [System.Serializable]
        public class Answer
        {
            public string answerText;
            public bool isCorrect;
        }
    }
}
