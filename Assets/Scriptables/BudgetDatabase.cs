using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BudgetDatabase", menuName = "Scriptable Objects/BudgetDatabase")]
public class BudgetDatabase : ScriptableObject
{
    public List<Budget> budgets;

    [System.Serializable]
    public class Budget
    {
        public string countryName;
        public string budgetAllocationDate;
        public long budget;
    }
}
