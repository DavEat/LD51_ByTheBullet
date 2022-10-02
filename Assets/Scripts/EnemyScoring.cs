using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScoring : MonoBehaviour
{
    [SerializeField] private int m_ScorePoints = 1;

    void Start()
    {
        GameManager.inst.ScoreTimeForEnemies += CheckScoring;
    }

    private void OnDisable()
    {
        GameManager.inst.ScoreTimeForEnemies -= CheckScoring;
    }

    private void CheckScoring()
    {
        if (gameObject.activeSelf && transform.position.sqrMagnitude < GameManager.inst.ScoreRadius * GameManager.inst.ScoreRadius)
        {
            GameManager.inst.AddToScore(m_ScorePoints);
        }
    }
}