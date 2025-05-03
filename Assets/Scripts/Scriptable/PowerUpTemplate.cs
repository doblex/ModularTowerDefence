using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Template/PowerUpTemplate", fileName = "PowerUpTemplate")]
public class PowerUpTemplate : TowerTemplate
{
    [Serializable]
    public class PowerUp
    {
        public PowerUpType powerUpType;
        public float value;
    }

    [Header("PowerUp Descriptors")]
    public GameObject projectilePrefab;
    public string projectileName;
    public PowerUp[] effects;
}