using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] private float lifePeriod = 3f;
    [SerializeField] private GameObject vfxPrefab;

    void Start()
    {
        Destroy(gameObject, lifePeriod);
    }

    private void OnDestroy()
    {
        // Spawn VFX at this object's position
        if (vfxPrefab != null)
        {
            GameObject go = Instantiate(vfxPrefab, transform.position, Quaternion.identity);
            go.transform.localScale = Vector3.one;

            var ps = go.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                AudioManager.Instance.PlaySoundPoof();
                Destroy(go, ps.main.duration + ps.main.startLifetime.constantMax); // Auto-destroy VFX
            }
        }
    }
}
