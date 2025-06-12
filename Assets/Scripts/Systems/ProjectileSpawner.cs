using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float interval = 3f;

    private System.Action<GameObject> onProjectileSpawned;

    public void SetPrefabCallback(GameObject prefab, System.Action<GameObject> callback)
    {
        if (prefab == projectilePrefab)
        {
            onProjectileSpawned = callback;
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnProjectile), Random.Range(0f, 3f), interval);
    }

    private void SpawnProjectile()
    {
        var instance = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        onProjectileSpawned?.Invoke(instance);
    }
}
