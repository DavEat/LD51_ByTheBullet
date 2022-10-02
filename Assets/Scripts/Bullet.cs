using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;

    [SerializeField] private LayerMask m_EnemyLayer;

    [SerializeField] float m_LifeTime = 3;
    private float m_DeathTime;

    private const string ENEMY_TAG = "Enemy";

    void Start()
    {
        m_DeathTime = Time.time + m_LifeTime;
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (m_DeathTime < Time.time)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }

        if (Physics.Raycast(m_Transform.position, m_Transform.forward, out RaycastHit hit, 1, m_EnemyLayer))
        {
            if (hit.collider.CompareTag(ENEMY_TAG))
            {
                hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(m_Rigidbody.velocity, m_Transform.position, ForceMode.Impulse);
                hit.collider.GetComponent<EnemyLife>().SetDamage(1);

                gameObject.SetActive(false);
                Destroy(gameObject);

                //hit.collider.gameObject.SetActive(false);
            }
        }
    }
}
