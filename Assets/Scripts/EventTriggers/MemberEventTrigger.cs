using UnityEngine;
using UnityEngine.EventSystems;

public class MemberEventTrigger : GeneralTrigger, IPointerClickHandler
{
    MembersDatabase.MemberEvent currentMemberEvent;

    public new void Start()
    {
        base.Start();
    }

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
