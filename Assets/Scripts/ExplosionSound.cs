using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSound : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;

    [SerializeField] private bool m_Small, m_Medium, m_Big;

    void Start()
    {
        if (m_Small)
            SoundManager.inst.PlayExplosionSmall(m_AudioSource);
        if (m_Medium)
            SoundManager.inst.PlayExplosionMedium(m_AudioSource);
        if (m_Big)
            SoundManager.inst.PlayExplosionBig(m_AudioSource);
    }
}