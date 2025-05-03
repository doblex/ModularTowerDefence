using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Template/TurretTemplate", fileName = "TurretTemplate")]
public class TurretTemplate : TowerTemplate
{
    [Header("Turret Descriptors")]
    public float damage;
    public float fireRate = 1f;
    public float range = 10f;
    public GameObject projectilePrefab;
}
