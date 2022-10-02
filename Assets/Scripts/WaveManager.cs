using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager inst;
    private void Awake()
    {
        inst = this;
    }

    [SerializeField] private EnemyWave[] Level0Waves;
    [SerializeField] private EnemyWave[] Level1Waves;
    [SerializeField] private EnemyWave[] Level2Waves;

    [SerializeField] private EnemyInstruction[] m_Instructions;

    private int m_waveCount = 1;
    private int m_waveCountIncrease = 2;
    [SerializeField] private List<EnemyMovement> m_Enemies = new List<EnemyMovement>();

    private Vector3[] m_SpawnPoints = new Vector3[]
    {
        new Vector3(+20, 0, +00),
        new Vector3(-20, 0, -00),
        new Vector3(+00, 0, +20),
        new Vector3(-00, 0, -20),
        new Vector3(+15, 0, +15),
        new Vector3(+15, 0, -15),
        new Vector3(-15, 0, +15),
        new Vector3(-15, 0, -15),
    };

    public void SpawnNewWave()
    {
        SetGlobalInstruction(m_Instructions[Random.Range(0, m_Instructions.Length)]);

        //for (int i = 0; i < m_waveCount; i++)
        {
            int index = Random.Range(0, m_SpawnPoints.Length);
            Vector3 spawnPoint = m_SpawnPoints[index];

            EnemyWave.EnPos[] composition;

            if (m_waveCount >= 8) // Level 2
            {
                int indexWave = Random.Range(0, Level2Waves.Length);
                composition = Level2Waves[indexWave].Composition;

            }
            else if (m_waveCount >= 5) // Level 1
            {
                int indexWave = Random.Range(0, Level1Waves.Length);
                composition = Level1Waves[indexWave].Composition;
            }
            else // Level 2
            {
                int indexWave = Random.Range(0, Level0Waves.Length);
                composition = Level0Waves[indexWave].Composition;

            }

            for (int j = 0; j < composition.Length; j++)
            {
                Vector3 startPos = spawnPoint + Vector3.up * .5f;
                startPos += /*Quaternion.LookRotation(startPos, Vector3.up) **/ composition[j].position;

                EnemyMovement em = Instantiate(composition[j].type, startPos, Quaternion.identity);
                //em.AddInstruction();
            }
        }
        m_waveCount += m_waveCountIncrease;
    }

    public void SetGlobalInstruction(EnemyInstruction instruction)
    {
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            m_Enemies[i].AddInstruction(instruction);
        }
    }

    public void Register(EnemyMovement enemy)
    {
        m_Enemies.Add(enemy);
    }
    public void Unregister(EnemyMovement enemy)
    {
        m_Enemies.Remove(enemy);
    }
}