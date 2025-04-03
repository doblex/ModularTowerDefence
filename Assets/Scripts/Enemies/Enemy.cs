using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void TargetReached(Enemy enemy, int currentPathpointIndex);

    public TargetReached OnTargetReached;

    bool arrived = false;
    

    int health = 10;
    float speed = 5f;
    int damage = 1;

    int currentPathpointIndex = 0;

    Transform target;

    public bool Arrived { get => arrived; set => arrived = value; }

    public void TakeDamage(int damage) 
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
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
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            OnTargetReached?.Invoke(this, currentPathpointIndex);
        }
    }
}
