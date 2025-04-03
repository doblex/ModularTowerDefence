using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "WaveManagement/Round", fileName = "Round")]
[Serializable]
public class Round : ScriptableObject
{
    [SerializeField] public List<Wave> Waves;
}
