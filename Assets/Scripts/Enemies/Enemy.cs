using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void TargetReached(Enemy enemy, int currentPathpointIndex);
    public delegate void OnEnemyDeath(Enemy enemy);

    public TargetReached OnTargetReached;
    public OnEnemyDeath OnEnemyDeathEvent;

    [HideInInspector] public Vector3 estimatedTargetVelocity;

    Vector3 lastTargetPosition;
   

    bool arrived = false;

    [SerializeField] int health = 10;
    [SerializeField] float speed = 5f;
    [SerializeField] public int damage { get; set; } = 1;

    float modifiedSpeed;
    public bool isSlowed = false;
    int currentPathpointIndex = 0;

    Transform target;

    Wave wave;

    public bool Arrived { get => arrived; set => arrived = value; }
    public Wave Wave {set => wave = value; }

    public bool TakeDamage(int damage) 
    {
        health -= damage;
        if (health <= 0)
        {
            wave.EnemyDeath();
            OnEnemyDeathEvent?.Invoke(this);
            Destroy(gameObject);
            return true;
        }

        return false;
    }

    public void SetTarget(Transform nextTarget)
    { 
        target = nextTarget;
        currentPathpointIndex++;
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }
        Vector3 direction = target.position - transform.position;

        estimatedTargetVelocity = (target.position - lastTargetPosition) / Time.deltaTime;
        lastTargetPosition = target.position;

        float targetSpeed = speed + modifiedSpeed < 0 ? 0.5f : speed + modifiedSpeed;

        transform.Translate(targetSpeed * Time.deltaTime * direction.normalized, Space.World);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            OnTargetReached?.Invoke(this, currentPathpointIndex);
        }
    }

    public IEnumerator Slow(float slowAmount, float slowDuration)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            modifiedSpeed = -slowAmount;
            yield return new WaitForSeconds(slowDuration);
            modifiedSpeed = 0;
            isSlowed = false;
        }
    }
}
