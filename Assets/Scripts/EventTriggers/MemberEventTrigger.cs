using UnityEngine;
using UnityEngine.EventSystems;

public class MemberEventTrigger : MonoBehaviour, IPointerClickHandler
{
    MembersDatabase.MemberEvent currentMemberEvent;

    public void Initialize(MembersDatabase.MemberEvent currentMemberEvent)
    {
        this.currentMemberEvent = currentMemberEvent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupManager.Instance.ShowMemberEvent(currentMemberEvent);
        Destroy(gameObject);
    }
}
