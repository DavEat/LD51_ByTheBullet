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
    public float ScoreRadiusSqr = 8 * 8;
    public Action ScoreTimeForEnemies;
    public int Score = 0;
    [SerializeField] private int ScoreMultiplier = 10;

    //---Terminate---
    public float TerminationSqrDst = 20 * 20;
    public Action TerminateEnemies;
    [SerializeField] private float RespawnPlayerAtY = -50;
    [SerializeField] private float m_PlayerRespawnHigh = 10;

    //---Context---
    private bool m_MainMenu = true;
    private bool m_Paused;
    private bool m_GameOver;
    [SerializeField] private float m_TimeBeforeRestart = .8f;

    private void Start()
    {
        SetPause(true);
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

                SoundManager.inst.PlayNoScore();
            }
            ResetTimer();
            UpdateUITimer();
            WaveManager.inst.SpawnNewWave();
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && !m_Paused)
        {
            SetPause(true);
        }
        else if (Input.GetButtonDown("MainMenu") && !m_MainMenu)
        {
            SetPause(false);
            EndGame(false);
            StartGame();
            m_MainMenu = true;
            CanvasManager.inst.ShowMainMenu(m_MainMenu);
            SetPause(true);
        }
        else if (Input.anyKeyDown)
        {
            if (m_MainMenu)
            {
                m_MainMenu = false;
                CanvasManager.inst.ShowMainMenu(m_MainMenu);
                StartGame();
            }
            else if (m_GameOver && m_Time < Time.unscaledTime)
            {
                EndGame(false);
                SoundManager.inst.RestartMusic();
                StartGame();
            }
            else if (m_Paused)
            {
                SetPause(false);
            }
        }

        if (!m_Paused)
        {
            if (NeedToScore())
            {
                if (CanScore())
                {
                    CalculateScore();
                }
                else
                    SoundManager.inst.PlayNoScore();
                WaveManager.inst.SpawnNewWave();
            }

            UpdateUITimer();
        }
    }

    public void UpdateUITimer()
    {
        CanvasManager.inst.TimerUpdate(1 - (m_Time - Time.time) / ScoreEveryXSec);
    }

    public bool NeedToScore()
    {
        if (m_Time < Time.time)
        {
            ResetTimer();
            return true;
        }
        return false;
    }
    public void ResetTimer()
    {
        m_Time = Time.time + ScoreEveryXSec;
    }

    public bool CanScore()
    {
        return PlayerPosition.sqrMagnitude < ScoreRadiusSqr;
    }

    public void CalculateScore()
    {
        Debug.Log("Calculating Score");
        int currentScore = Score;

        if (ScoreTimeForEnemies != null)
            ScoreTimeForEnemies();

        if (currentScore < Score)
        {
            SoundManager.inst.PlayScore();
        }
        else SoundManager.inst.PlayNoScore();

        Debug.Log($"New Score is: {Score}");
        CanvasManager.inst.SetScore(Score);
    }
    public void AddToScore(int value = 1)
    {
        Score += value * ScoreMultiplier;
    }

    public void UpdateRadius(int index = -1)
    {
        ScoreRadiusSqr = CanvasManager.inst.NewWaveRadius(index);
    }

    public void StartGame()
    {
        SetPause(false);

        m_Time = Time.time + ScoreEveryXSec;
        UpdateRadius(0);

        Score = 0;

        CanvasManager.inst.SetScore(Score);
        WaveManager.inst.StartGame();
    }

    public void SetPause(bool value)
    {
        m_Paused = value;
        Time.timeScale = m_Paused ? 0 : 1.0f;
        
        CanvasManager.inst.ShowPauseMenu(m_Paused && !m_MainMenu && !m_GameOver);
    }

    public void EndGame(bool value)
    {
        m_GameOver = value;
        SetPause(value);

        m_Time = Time.unscaledTime + m_TimeBeforeRestart;

        CanvasManager.inst.ShowGameOverMenu(value, Score);
    }
}