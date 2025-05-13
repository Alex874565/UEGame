using UnityEngine;
using UnityEngine.EventSystems;

public class MainEventTrigger : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        QuizManager.Instance.StartMainEvent();
    }
}
