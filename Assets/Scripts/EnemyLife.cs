using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    [SerializeField] private int m_LifePoint = 3;

    public void SetDamage(int damage)
    {
        m_LifePoint -= damage;

        if (m_LifePoint <= 0)
        {
            GetComponent<EnemyExplode>()?.Explode();
            
            Die();
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}