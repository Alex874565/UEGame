using UnityEngine;
using UnityEngine.EventSystems;

public class BudgetEventTrigger : GeneralTrigger, IPointerClickHandler
{
    BudgetDatabase.Budget currentBudgetEvent;

    public new void Start()
    {
        base.Start();
    }

    public void Initialize(BudgetDatabase.Budget currentBudgetEvent)
    {
        this.currentBudgetEvent = currentBudgetEvent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySound();
        PopupManager.Instance.ShowBudgetEvent(currentBudgetEvent);
        Destroy(gameObject);
    }
}
