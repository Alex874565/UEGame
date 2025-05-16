using UnityEngine;
using UnityEngine.EventSystems;

public class ElectionEventTrigger : MonoBehaviour, IPointerClickHandler
{
    ElectionsDatabase.Election currentElectionEvent;

    public void Initialize(ElectionsDatabase.Election currentElectionEvent)
    {
        this.currentElectionEvent = currentElectionEvent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupManager.Instance.ShowElectionEvent(currentElectionEvent);
        Destroy(gameObject);
    }
}
