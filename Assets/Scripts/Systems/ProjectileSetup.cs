using UnityEngine;

public class ProjectileSetup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created    
    [SerializeField] private GameObject projectilePrefab;
    private ProjectileSpawner spawner;
    [SerializeField] private float damage = 1;

    private void Start()
    {
        spawner = GetComponent<ProjectileSpawner>();
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
                player.TakeDamage(damage); // Cambia el valor según el daño deseado
            }
        };
    }
}
