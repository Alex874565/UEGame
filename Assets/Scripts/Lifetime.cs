using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] private float lifePeriod = 3f;
    
    void Start()
    {
        Destroy(gameObject, lifePeriod);
    }
}
