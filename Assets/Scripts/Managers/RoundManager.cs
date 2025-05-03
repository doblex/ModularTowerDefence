using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    public delegate void OnWaveChanged(float wave, float round);

    public OnWaveChanged onWaveChanged;



    [SerializeField] List<Round> rounds;

    int currentRoundIndex = 0;
    int currentWaveIndex = 0;

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
    }

    public Wave GetNextWave(ref List<WaveDescriptor> waveDescriptors)
    {
        waveDescriptors.Clear();

        Wave returnWave = null;

        if (currentRoundIndex >= rounds.Count)
        {
            GameManager.Instance.AllRoundFinished();
            waveDescriptors.Clear();
        }
        else
        { 
            returnWave = rounds[currentRoundIndex].Waves[currentWaveIndex];

            rounds[currentRoundIndex].Waves[currentWaveIndex].Init();

            List<EnemyWave> enemyWaves = rounds[currentRoundIndex].Waves[currentWaveIndex].EnemyWaves;

            foreach (var enemyWave in enemyWaves)
            {
                waveDescriptors.Add(enemyWave.GetWaveDescriptor());
            }

            if (currentWaveIndex >= rounds[currentRoundIndex].Waves.Count - 1)
            {
                onWaveChanged?.Invoke(currentWaveIndex, currentRoundIndex);
                currentRoundIndex++;
                Debug.Log("Next Round");
            }
            else
            {
                onWaveChanged?.Invoke(currentWaveIndex, currentRoundIndex);
                currentWaveIndex++;
                Debug.Log("Next Wave");
            }
        }

        

        return returnWave;
    }
}
