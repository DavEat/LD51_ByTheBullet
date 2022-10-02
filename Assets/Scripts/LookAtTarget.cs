using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    private Transform m_Transform;
    [SerializeField] private Transform m_Target;

    private void Start()
    {
        m_Transform = GetComponent<Transform>();
    }
    private void FixedUpdate()
    {
        m_Transform.LookAt(m_Target);
    }
}