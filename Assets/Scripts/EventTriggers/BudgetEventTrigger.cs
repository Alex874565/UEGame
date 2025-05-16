using UnityEngine;
using UnityEngine.EventSystems;

public class BudgetEventTrigger : MonoBehaviour, IPointerClickHandler
{
    BudgetDatabase.Budget currentBudgetEvent;

    public void Initialize(BudgetDatabase.Budget currentBudgetEvent)
    {
        this.currentBudgetEvent = currentBudgetEvent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupManager.Instance.ShowBudgetEvent(currentBudgetEvent);
        Destroy(gameObject);
    }
}
