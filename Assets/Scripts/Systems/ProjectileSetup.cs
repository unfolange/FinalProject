using UnityEngine;

public class ProjectileSetup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private ProjectileSpawner spawner;
    [SerializeField] private GameObject projectilePrefab;

    private void Start()
    {
        spawner.SetPrefabCallback(projectilePrefab, OnProjectileSpawned);
    }

    private void OnProjectileSpawned(GameObject projectileGO)
    {
        var projectile = projectileGO.GetComponent<Projectile>();
        projectile.OnHitPlayer += () =>
        {
            var player = PlayerController.Instance;
            if (player != null)
            {
                player.TakeDamage(0.5f); // Cambia el valor según el daño deseado
            }
        };
    }
}
