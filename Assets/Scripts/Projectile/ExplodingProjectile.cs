using UnityEngine;

public class ExplodingProjectile : baseProjectile
{
    [Header("Exploding Projectile Settings")]
    [SerializeField] float explosionRadius;
    [SerializeField] GameObject explosionEffect;

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Enemy targetEnemy = collider.GetComponent<Enemy>();
                    targetEnemy.TakeDamage((int)damage);
                }
            }
            if (explosionEffect)
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
