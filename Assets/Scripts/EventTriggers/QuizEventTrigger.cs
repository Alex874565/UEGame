using UnityEngine;
using UnityEngine.EventSystems;

public class QuizEventTrigger : GeneralTrigger, IPointerClickHandler
{
    QuizzesDatabase.Quiz currentQuizEvent;

    public new void Start()
    {
        base.Start();
    }
    public void Initialize(QuizzesDatabase.Quiz currentQuizEvent)
    {
        this.currentQuizEvent = currentQuizEvent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySound();
        PopupManager.Instance.ShowQuizEvent(currentQuizEvent);
        Destroy(gameObject);
    }
}
