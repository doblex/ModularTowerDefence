using UnityEngine;

public class HighCaliberProjectile : baseProjectile 
{
    [Header("High Caliber Projectile Settings")]
    [SerializeField] int targetPenetartion;
    int penetrationCount = 0;

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (enemy.TakeDamage((int)damage))
            {
                if (penetrationCount < targetPenetartion)
                {
                    penetrationCount++;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
