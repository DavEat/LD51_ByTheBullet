using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private Rigidbody m_Rigidbody;

    [SerializeField] private Bullet m_BulletPrefab;
    [SerializeField] private Transform m_Canon;

    [SerializeField] private float m_CoolDownBtwBullet = .1f;
    [SerializeField] private float m_CoolDownAfterFiring = 10f;
    private float m_Time;
    [SerializeField] private bool m_Firing = false;

    [SerializeField] private float m_BulletSpeed = 10;
    [SerializeField] private float m_BulletSpeedToPlayerMul = .5f;
    [SerializeField] private float m_SpreadingDeg = 10;

    [SerializeField] private AudioSource m_GunSourceA;
    [SerializeField] private AudioSource m_GunSourceB;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckInput();

        if (m_Firing)
        {
            Fire();
        }
    }

    private void CheckInput()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            if (m_Firing)
            {
                m_Firing = false;
                m_Time = Time.time + m_CoolDownAfterFiring;
            }
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            if (!m_Firing && m_Time < Time.time)
            {
                m_Firing = true;
            }
        }
    }

    private void Fire()
    {
        if (m_Time > Time.time)
            return;

        m_Time = Time.time + m_CoolDownBtwBullet;

        Bullet b = Instantiate(m_BulletPrefab, m_Canon.position, m_Canon.rotation, null);
        Rigidbody rb = b.GetComponent<Rigidbody>();

        float randAngle = Random.Range(-m_SpreadingDeg, m_SpreadingDeg);
        Vector3 vel = Quaternion.Euler(0, randAngle, 0) * rb.transform.forward * m_BulletSpeed;

        rb.velocity = vel;

        m_Rigidbody.AddForce(-vel * m_BulletSpeedToPlayerMul, ForceMode.Impulse);

        SoundManager.inst.PlayFireGun(m_GunSourceA, m_GunSourceB);
    }
}