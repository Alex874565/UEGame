using UnityEngine;

public class GeneralTrigger : MonoBehaviour
{
    [SerializeField] private GameObject triggerSFX;
    public void Start()
    {
        Instantiate(triggerSFX, transform);
    }
}
