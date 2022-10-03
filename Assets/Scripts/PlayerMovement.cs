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

    [SerializeField] private LayerMask m_MouseMask;
    private Vector3 m_CameraPos;

    private void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody>();

        m_MainCamRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

        m_CameraPos = Camera.main.transform.position;
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

            MoveToDir(dir);
            LookToDir(dir);
        }
        else if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            Vector3 dir = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 50, m_MouseMask))
                dir = hit.point.normalized;

            if (Input.GetMouseButton(1))
            {
                MoveToDir(dir);
            }

            LookToDir(dir);
        }
        //else
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x * m_Decelation,
                                               m_Rigidbody.velocity.y,
                                               m_Rigidbody.velocity.z * m_Decelation);
        }
    }

    private void MoveToDir(Vector3 dir)
    {
        if (m_Rigidbody.velocity.sqrMagnitude < m_MaxSpeed)
        {
            m_Rigidbody.AddForce(dir * m_AccelerationMul);
            //m_Rigidbody.velocity = dir * m_MaxSpeed;
        }
    }
    private void LookToDir(Vector3 dir)
    {
        m_Transform.rotation = Quaternion.LookRotation(dir);
    }
}