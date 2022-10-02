using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;
    public Vector3 Position => m_Transform.position;

    private float m_AxeX, m_AxeY;

    private const float JOYSTICK_TOLERANCE = .3f;

    [SerializeField] private float m_AccelerationMul = 10;
    [SerializeField] private float m_MaxSpeed = 30;
    [SerializeField] private float m_Decelation = .9f;

    private Quaternion m_MainCamRotation;

    private void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody>();

        m_MainCamRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
    }


    private void Update()
    {
        m_AxeX = Input.GetAxis("Horizontal");
        m_AxeY = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(m_AxeX) > JOYSTICK_TOLERANCE || Mathf.Abs(m_AxeY) > JOYSTICK_TOLERANCE)
        {
            Vector3 dir = m_MainCamRotation * new Vector3(m_AxeX, 0, m_AxeY).normalized;
            
            if (m_Rigidbody.velocity.sqrMagnitude < m_MaxSpeed)
            {
                m_Rigidbody.AddForce(dir * m_AccelerationMul);
                //m_Rigidbody.velocity = dir * m_MaxSpeed;
            }

            m_Transform.rotation = Quaternion.LookRotation(dir);
        }
        //else
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x * m_Decelation,
                                               m_Rigidbody.velocity.y,
                                               m_Rigidbody.velocity.z * m_Decelation);
        }
    }
}