using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager inst;
    private void Awake()
    {
        inst = this;
    }

    [SerializeField] private TextMeshProUGUI m_WaveCount;
    [SerializeField] private TextMeshProUGUI m_ScoreText;

    [SerializeField] private Image[] m_Timers;
    [SerializeField] private Transform[] RadiusTimers;
    private int m_TimerIndex = 0;

    [SerializeField] private GameObject m_MainMenu;
    [SerializeField] private GameObject m_PauseMenu;
    [SerializeField] private GameObject m_GameOverMenu;
    [SerializeField] private TextMeshProUGUI m_Gameover_ScoreText;


    public void SetWaveCount(int value)
    {
        m_WaveCount.text = value.ToString();
    }
    public void SetScore(int value)
    {
        m_ScoreText.text = value > 0 ? value.ToString() : "";
    }

    public void TimerUpdate(float percentComplited)
    {
        m_Timers[m_TimerIndex].fillAmount = percentComplited;
    }
    public float NewWaveRadius(int index = -1)
    {
        m_Timers[m_TimerIndex].transform.parent.gameObject.SetActive(false);
        
        if (index >= 0)
        {
            m_TimerIndex = index;
        }
        else
        {
            m_TimerIndex++;
        }
        if (m_TimerIndex >= m_Timers.Length)
            m_TimerIndex = m_Timers.Length - 1;

        m_Timers[m_TimerIndex].transform.parent.gameObject.SetActive(true);

        return RadiusTimers[m_TimerIndex].position.sqrMagnitude;
    }
    public void ShowMainMenu(bool value)
    {
        m_MainMenu.SetActive(value);
    }
    public void ShowPauseMenu(bool value)
    {
        m_PauseMenu.SetActive(value);
    }
    public void ShowGameOverMenu(bool value, int score)
    {
        m_GameOverMenu.SetActive(value);
        m_Gameover_ScoreText.text = $"Your final score is: {score}";
    }
}