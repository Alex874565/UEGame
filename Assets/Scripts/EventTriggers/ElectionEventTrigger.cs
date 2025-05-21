using UnityEngine;
using UnityEngine.EventSystems;

public class ElectionEventTrigger : GeneralTrigger, IPointerClickHandler
{
    ElectionsDatabase.Election currentElectionEvent;

    public new void Start()
    {
        base.Start();
    }
    public void Initialize(ElectionsDatabase.Election currentElectionEvent)
    {
        this.currentElectionEvent = currentElectionEvent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySound();
        PopupManager.Instance.ShowElectionEvent(currentElectionEvent);
        Destroy(gameObject);
    }
}
