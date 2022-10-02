using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    [SerializeField] private int m_LifePoint = 3;
    [SerializeField] private GameObject m_DeathParticle;

    public void SetDamage(int damage)
    {
        m_LifePoint -= damage;

        if (m_LifePoint <= 0)
        {
            GetComponent<EnemyExplode>()?.Explode();
            
            Die();
        }
        else
        {
            SoundManager.inst.PlayBotGetDamage();
        }
    }

    public void Die()
    {
        GameObject g = Instantiate(m_DeathParticle, transform.position, Quaternion.identity);
        Destroy(g, .5f);

        SoundManager.inst.PlayBotDie();

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}