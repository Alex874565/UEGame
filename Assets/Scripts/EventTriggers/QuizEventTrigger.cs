using UnityEngine;
using UnityEngine.EventSystems;

public class QuizEventTrigger : MonoBehaviour, IPointerClickHandler
{
    QuizzesDatabase.Quiz currentQuizEvent;

    public void Initialize(QuizzesDatabase.Quiz currentQuizEvent)
    {
        this.currentQuizEvent = currentQuizEvent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupManager.Instance.ShowQuizEvent(currentQuizEvent);
        Destroy(gameObject);
    }
}
