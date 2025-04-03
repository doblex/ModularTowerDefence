using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public static MovementManager Instance;

    List<Enemy> enemies;

    List<Transform> pathpoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        pathpoints = new List<Transform>();
        enemies = new List<Enemy>();
    }

    private void Start()
    {
        RetrievePathpoints();
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
    }

    private void SetNextTarget(Enemy enemy, int currentPathpointIndex) 
    {
        if (currentPathpointIndex >= pathpoints.Count - 1)
        { 
            enemy.Arrived = true;
            return;
        }
        enemy.SetTarget(pathpoints[currentPathpointIndex + 1]);
    }


    private void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        { 
            Enemy enemy = enemies[i];

            if (enemy.Arrived)
            {   
                enemies.Remove(enemy);
                i--;
                continue;
            }
        }
    }

}
