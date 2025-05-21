using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainEventTrigger : GeneralTrigger, IPointerClickHandler
{
    EventsDatabase.Event currentMainEvent;

    public new void Start()
    {
        base.Start();
    }
    public void Initialize(EventsDatabase.Event currentMainEvent)
    {
        this.GetComponentInParent<Image>().sprite = currentMainEvent.eventIcon;
        this.currentMainEvent = currentMainEvent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySound();
        PopupManager.Instance.ShowMainEvent(currentMainEvent);
        Destroy(gameObject);
    }
}
