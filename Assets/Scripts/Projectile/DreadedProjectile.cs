using UnityEngine;

public class DreadedProjectile : baseProjectile
{
    [Header("Dreaded Projectile Settings")]
    [SerializeField] float slowAmount;
    [SerializeField] float slowDuration;
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage((int)damage);
            enemy.StartCoroutine(enemy.Slow(slowAmount, slowDuration));
            Destroy(gameObject);
        }
    }
}
