using UnityEngine;

public class EnemyExplode : MonoBehaviour
{
    private Transform m_Transform;

    [SerializeField] private float m_ExplodeAtFromPlayer = 1;
    [SerializeField] private float m_ExplotionForce = 50;
    [SerializeField] private float m_ExplotionRadius = 1.5f;
    [SerializeField] private int m_ExplotionDamage = 2;

    [SerializeField] private LayerMask m_LayerMask;

    private bool m_Exploded = false;

    private void Start()
    {
        m_Transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if ((m_Transform.position - GameManager.inst.PlayerPosition).sqrMagnitude < m_ExplodeAtFromPlayer)
        {
            Explode();
            GetComponent<EnemyLife>().Die();
        }
    }

    public void Explode()
    {
        if (m_Exploded)
            return;

        m_Exploded = true;

        Collider[] touched = Physics.OverlapSphere(m_Transform.position, m_ExplotionRadius, m_LayerMask);

        for (int i = 0; i < touched.Length; i++)
        {
            touched[i].GetComponent<Rigidbody>()?.AddExplosionForce(m_ExplotionForce, m_Transform.position, m_ExplotionRadius);
            touched[i].GetComponent<EnemyLife>()?.SetDamage(m_ExplotionDamage);
        }
    }
}