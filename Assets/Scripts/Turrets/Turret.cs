using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurretRangeVisualizer))]
[RequireComponent(typeof(SphereCollider))]
public class Turret : Tower
{
    [HideInInspector] public bool isRotationDisabled { get; set; } = false;

    Transform target;
    SphereCollider sphereCollider;
    [SerializeField] Transform shootPosition;

    List<Enemy> enemiesInRange = new List<Enemy>();

    TurretRangeVisualizer rangeVisualizer;

    float modifiedRange;
    float modifiedDamage;
    float modifiedFireRate;
    
    float newRange;
    float newDamage;
    float newFireRate;
    GameObject projectilePrefab;

    float fireRateCooldown = 0;

    private void Awake()
    {
        modifiedRange = 0;
        modifiedDamage = 0;
        modifiedFireRate = 0;
        rangeVisualizer = GetComponent<TurretRangeVisualizer>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        target = enemiesInRange.Count > 0 ? enemiesInRange[0].transform : null;
        if (!isRotationDisabled)
        {
            TurretRotation();
            Shoot();
        }
    }

    private void TurretRotation()
    {
        if (target != null)
        {
            Vector3 vector = target.position - transform.position;
            vector.y = 0;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, vector, 10f * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    private void Shoot()
    {
        if (fireRateCooldown <= 0 && target != null)
        {
            fireRateCooldown = newFireRate > 0 ? newFireRate : 0.1f;

            GameObject proj = Instantiate(projectilePrefab, shootPosition.position, shootPosition.rotation);

            proj.GetComponent<baseProjectile>().target = target;
            proj.GetComponent<baseProjectile>().damage = newDamage > 0 ? newDamage : 1;

        }

        fireRateCooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.OnEnemyDeathEvent += OnEnemyDeath;
            enemiesInRange.Add(enemy);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.OnEnemyDeathEvent -= OnEnemyDeath;
            enemiesInRange.Remove(enemy);
        }
    }

    public override void Initialize(TowerTemplate template)
    {
        base.Initialize(template);

        TurretTemplate turretTemplate = (TurretTemplate)template;

        newDamage = turretTemplate.damage + modifiedDamage;
        newFireRate = turretTemplate.fireRate + modifiedFireRate;
        newRange = turretTemplate.range + modifiedRange;

        projectilePrefab = turretTemplate.projectilePrefab;
        rangeVisualizer.range = newRange > 0 ? newRange : 1;

        sphereCollider.isTrigger = true;
        sphereCollider.radius = newRange > 0 ? newRange : 1;
    }

    public void ResetUpgrade()
    {
        modifiedDamage = 0;
        modifiedFireRate = 0;
        modifiedRange = 0;
        projectilePrefab = ((TurretTemplate)template).projectilePrefab;
    }

    public void Upgrade(PowerUpTemplate powerUpTemplate)
    {
        if (powerUpTemplate.projectilePrefab != null)
        {
            projectilePrefab = powerUpTemplate.projectilePrefab;
        }

        foreach (var powerUp in powerUpTemplate.effects)
        {
            switch (powerUp.powerUpType)
            {
                case PowerUpType.Damage:
                    modifiedDamage += powerUp.value;
                    break;
                case PowerUpType.FireRate:
                    modifiedFireRate += powerUp.value;
                    break;
                case PowerUpType.Range:
                    modifiedRange += powerUp.value;
                    
                    break;
            }
        }

        newDamage = ((TurretTemplate)template).damage + modifiedDamage;
        newFireRate = ((TurretTemplate)template).fireRate + modifiedFireRate;
        newRange = ((TurretTemplate)template).range + modifiedRange;

        rangeVisualizer.range = newRange > 0 ? newRange : 1;
        sphereCollider.radius = newRange > 0 ? newRange : 1;
    }

    public void OnEnemyDeath(Enemy enemy)
    {
        enemiesInRange.Remove(enemy);
    }
}
