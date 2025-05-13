using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BudgetDatabase", menuName = "Scriptable Objects/BudgetDatabase")]
public class BudgetDatabase : ScriptableObject
{
    public List<Budget> budgets;

    public class Budget
    {
        public string budgetAllocationDate;
        public int budget;
    }
}
