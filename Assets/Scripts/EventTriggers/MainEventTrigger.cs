using UnityEngine;
using UnityEngine.EventSystems;

public class MainEventTrigger : MonoBehaviour, IPointerClickHandler
{
    EventsDatabase.Event currentMainEvent;

    public void Initialize(EventsDatabase.Event currentMainEvent)
    {
        this.currentMainEvent = currentMainEvent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupManager.Instance.ShowEvent(currentMainEvent);
        Destroy(gameObject);
    }
}
