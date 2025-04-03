using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WaveManagement/Wave", fileName = "Wave")]
[Serializable]
public class Wave : ScriptableObject
{
    [SerializeField] public List<EnemyWave> EnemyWaves;

    [SerializeField] float timeforWave;

    public float TimeForWave() =>  timeforWave;
    

}
