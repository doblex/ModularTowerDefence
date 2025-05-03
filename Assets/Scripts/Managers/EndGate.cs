using UnityEngine;

public class EndGate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                MovementManager.Instance.RemoveEnemy(enemy);
                GameManager.Instance.DoDamage(enemy.damage);
            }
        }
    }
}
