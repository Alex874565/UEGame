using UnityEngine;
using UnityEngine.EventSystems;

public class MainEventTrigger : GeneralTrigger, IPointerClickHandler
{
    EventsDatabase.Event currentMainEvent;

    public new void Start()
    {
        base.Start();
    }
    public void Initialize(EventsDatabase.Event currentMainEvent)
    {
        this.currentMainEvent = currentMainEvent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupManager.Instance.ShowMainEvent(currentMainEvent);
        Destroy(gameObject);
    }
}
