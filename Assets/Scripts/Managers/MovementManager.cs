using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MovementManager : MonoBehaviour
{
    public static MovementManager Instance;

    [SerializeField] List<Enemy> enemies;

    [SerializeField] List<Transform> pathpoints;

    LineRenderer line;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        pathpoints = new List<Transform>();
        enemies = new List<Enemy>();
        line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        RetrievePathpoints();
        Drawpath();
    }

    private void Drawpath()
    {
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

        line.positionCount = pathpoints.Count;

        for (int i = 0; i < pathpoints.Count; i++)
        {
            line.SetPosition(i, pathpoints[i].position);
        }
    }

    private void RetrievePathpoints()
    {
        Transform pathpointsGroup = transform.GetChild(0);
        for (int i = 0; i < pathpointsGroup.childCount; i++)
        {
            pathpoints.Add(pathpointsGroup.GetChild(i));
        }
    }

    public void AddEnemy(Enemy enemy) 
    { 
        enemies.Add(enemy);
        enemy.OnTargetReached += SetNextTarget;

        SetNextTarget(enemy, 0);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        enemy.OnTargetReached -= SetNextTarget;

        Destroy(enemy.gameObject);
    }

    private void SetNextTarget(Enemy enemy, int currentPathpointIndex) 
    {
        if (currentPathpointIndex >= pathpoints.Count - 1)
        { 
            return;
        }

        enemy.SetTarget(pathpoints[currentPathpointIndex + 1]);
    }

    
}
