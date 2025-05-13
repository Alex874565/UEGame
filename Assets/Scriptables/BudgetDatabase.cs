using UnityEngine;

[CreateAssetMenu(fileName = "BudgetDatabase", menuName = "Scriptable Objects/BudgetDatabase")]
public class BudgetDatabase : ScriptableObject
{
    public string budgetAllocationDate;
    public int budget;
}
