using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    [SerializeField] private EnemyInstruction[] m_InstructionsA;
    [SerializeField] private EnemyInstruction[] m_InstructionsB;
    [SerializeField] private EnemyInstruction[] m_GlobalInstructions;

    private int m_WaveNumber = 1;
    [SerializeField] private int m_UpdateRadiusAtWaveNumber = 4;
    [SerializeField] private int m_MaxWave = 9;


    private int m_WaveCount = 1;
    private int m_WaveCountIncrease = 2;
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

    public void StartGame()
    {
        m_WaveNumber = 0;
        m_WaveCount = 1;
        SpawnNewWave();

        foreach (EnemyMovement enemy in m_Enemies.ToList())
        {
            if (enemy is null)
                continue;

            enemy.gameObject.SetActive(false);
            Destroy(enemy.gameObject);
        }
        m_Enemies.Clear();
    }

    public void HalfWave()
    {
        SetGlobalInstruction(m_GlobalInstructions[Random.Range(0, m_GlobalInstructions.Length)]);
    }

    public void SpawnNewWave()
    {
        m_WaveNumber++;

        if (m_WaveNumber > m_MaxWave)
        {
            GameManager.inst.EndGame(true);
            return;
        }

        CanvasManager.inst.SetWaveCount(m_WaveNumber);

        if (m_WaveNumber >= m_UpdateRadiusAtWaveNumber)
        {
            GameManager.inst.UpdateRadius();
        }

        //SetGlobalInstruction(m_GlobalInstructions[Random.Range(0, m_GlobalInstructions.Length)]);

        //for (int i = 0; i < m_waveCount; i++)
        {
            int index = Random.Range(0, m_SpawnPoints.Length);
            Vector3 spawnPoint = m_SpawnPoints[index];

            EnemyWave.EnPos[] composition;

            if (m_WaveCount >= 8) // Level 2
            {
                int indexWave = Random.Range(0, Level2Waves.Length);
                composition = Level2Waves[indexWave].Composition;

            }
            else if (m_WaveCount >= 5) // Level 1
            {
                int indexWave = Random.Range(0, Level1Waves.Length);
                composition = Level1Waves[indexWave].Composition;
            }
            else // Level 2
            {
                int indexWave = Random.Range(0, Level0Waves.Length);
                composition = Level0Waves[indexWave].Composition;

            }

            int instructionCount = Random.Range(0, 4);
            for (int j = 0; j < composition.Length; j++)
            {
                Vector3 startPos = spawnPoint + Vector3.up * .5f;
                startPos += /*Quaternion.LookRotation(startPos, Vector3.up) **/ composition[j].position;

                EnemyMovement em = Instantiate(composition[j].type, startPos, Quaternion.identity);

                if (instructionCount > 0)
                {
                    //em.AddInstruction(m_InstructionsA[Random.Range(0, m_InstructionsA.Length)], false);

                    Vector3 dir = spawnPoint * -1;
                    var instruction = new EnemyInstruction()
                    {
                        Target = dir,
                        movementType = EnemyInstruction.MovementType.Straight,
                        Relative = true
                    };
                    em.AddInstruction(instruction);
                }
                if (instructionCount > 2)
                    em.AddInstruction(m_InstructionsB[Random.Range(0, m_InstructionsB.Length)], false);
            }
        }
        m_WaveCount += m_WaveCountIncrease;
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