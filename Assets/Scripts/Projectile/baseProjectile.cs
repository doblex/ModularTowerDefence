using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class baseProjectile : MonoBehaviour
{
    public Transform target;
    private Enemy targetEnemy;

    [Header("Projectile Settings")]
    [SerializeField] float speed;
    [HideInInspector] public float damage;
    [SerializeField] float duration;


    private void Start()
    {
        Destroy(gameObject, duration);
    }

    void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        if (targetEnemy == null && target != null)
        {
            targetEnemy = target.GetComponent<Enemy>();
        }

        if (target != null)
        {
            Vector3 targetPosition = target.position;

            if (targetEnemy != null)
            {
                
                Vector3 toTarget = targetPosition - transform.position;
                float distance = toTarget.magnitude;
                float timeToTarget = distance / speed;

                Vector3 futurePosition = targetPosition + targetEnemy.estimatedTargetVelocity * timeToTarget;

                Vector3 direction = (futurePosition - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;

                // Optional: Align projectile forward
                transform.forward = direction;
            }
            else
            {
                // Fallback to direct movement if no Rigidbody
                Vector3 direction = (targetPosition - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
        else
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            OnCollisionEnemy(enemy);
        }
    }

    public virtual void OnCollisionEnemy(Enemy enemy)
    {
        enemy.TakeDamage((int)damage);
        Destroy(gameObject);
    }
}
