using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager inst;
    private void Awake()
    {
        inst = this;
    }

    [SerializeField] private Transform m_PlayerTransform;
    public Vector3 PlayerPosition => m_PlayerTransform.position;

    //---Score---
    [SerializeField] private float ScoreEveryXSec = 10;
    private float m_Time;
    public float ScoreRadius = 8;
    public Action ScoreTimeForEnemies;
    public int Score = 0;
    [SerializeField] private int ScoreMultiplier = 10;

    //---Terminate---
    public float TerminationSqrDst = 20 * 20;
    public Action TerminateEnemies;
    [SerializeField] private float RespawnPlayerAtY = -50;
    [SerializeField] private float m_PlayerRespawnHigh = 10;


    private void Start()
    {
        m_Time = Time.time + ScoreEveryXSec;
    }

    private void FixedUpdate()
    {
        if (PlayerPosition.y < RespawnPlayerAtY)
        {
            m_PlayerTransform.position = new Vector3(0, m_PlayerRespawnHigh, 0);
            Debug.Log("Terminate All Enemies");
            ScoreTimeForEnemies = null;
            if (TerminateEnemies != null)
            {
                TerminateEnemies();
                TerminateEnemies = null;
            }
            WaveManager.inst.SpawnNewWave();
        }
    }

    private void Update()
    {
        if (NeedToScore() && CanScore())
        {
            CalculateScore();
        }
    }

    public bool NeedToScore()
    {
        if (m_Time < Time.time)
        {
            m_Time = Time.time + ScoreEveryXSec;
            return true;
        }
        return false;
    }

    public bool CanScore()
    {
        return PlayerPosition.sqrMagnitude < ScoreRadius * ScoreRadius;
    }

    public void CalculateScore()
    {
        Debug.Log("Calculating Score");
        if (ScoreTimeForEnemies != null)
            ScoreTimeForEnemies();
        Debug.Log($"New Score is: {Score}");

        WaveManager.inst.SpawnNewWave();
    }
    public void AddToScore(int value = 1)
    {
        Score += 1 * ScoreMultiplier;
    }
}