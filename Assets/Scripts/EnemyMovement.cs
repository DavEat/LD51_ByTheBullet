using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;
    public Vector3 Position => m_Transform.position;

    [SerializeField] private float m_AccelerationMul = 150;
    [SerializeField] private float m_Decelation = .8f;

    [SerializeField] private bool m_Terminating;
    [SerializeField] private float m_TerminatingSpeedMul = 3;

    [SerializeField] private EnemyInstruction m_CurrentInstruction;
    [SerializeField] private Vector3 m_InstructionTargetPos;
    [SerializeField] private List<EnemyInstruction> m_Instructions = new List<EnemyInstruction>();
    private const float MAX_TIME_FOR_AN_INSTRUCTION = 10;
    private float m_InstructionTime;

    private void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody>();

        GameManager.inst.TerminateEnemies += Terminate;
        WaveManager.inst.Register(this);
    }

    private void OnDisable()
    {
        GameManager.inst.TerminateEnemies -= Terminate;
        WaveManager.inst.Unregister(this);
    }

    private void FixedUpdate()
    {
        if (m_Terminating || m_Instructions.Count <= 0)
        {
            NoInstrcution();
            return;
        }

        if (m_CurrentInstruction is null && m_Instructions.Count > 0)
        {
            m_CurrentInstruction = m_Instructions[0];
            m_InstructionTargetPos = m_CurrentInstruction.Init(Position);
            m_InstructionTime = Time.time + MAX_TIME_FOR_AN_INSTRUCTION;
        }
        else if (m_CurrentInstruction.IsCompleted(Position, m_InstructionTargetPos) || m_InstructionTime < Time.time)
        {
            m_Instructions.RemoveAt(0);
            m_CurrentInstruction = null;
        }
        else
        {
            Vector3 dir = m_CurrentInstruction.GetDirection(Position, m_InstructionTargetPos);

            //Debug.DrawRay(Position, dir, Color.red, 30);
            //Debug.DrawLine(Position, m_InstructionTargetPos, Color.green);

            //if (dir.sqrMagnitude < .1f)
            {
                m_Rigidbody.AddForce(dir * m_AccelerationMul);
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

    public void AddInstruction(EnemyInstruction instruction, bool onlyIfNoInstruction = true)
    {
        if (!onlyIfNoInstruction || m_Instructions.Count <= 0)
        {
            m_Instructions.Add(instruction);
        }
    }

    private void NoInstrcution()
    {
        Vector3 playerEn = (GameManager.inst.PlayerPosition - m_Transform.position).normalized;
        float dotPlayer = Vector3.Dot(playerEn, GameManager.inst.PlayerPosition.normalized);

        Vector3 target;

        if (dotPlayer < GameManager.inst.m_DotPlayerLimit)
        {
            float crossDot = Vector3.Dot(playerEn, Vector3.Cross(GameManager.inst.PlayerPosition, Vector3.up).normalized);
            target = crossDot > 0 ? GameManager.inst.PlayerPositionL2 : GameManager.inst.PlayerPositionL1;
        }
        else
        {
            target = GameManager.inst.PlayerPosition;
        }

        Vector3 dir = target - m_Transform.position;
        dir.y = 0;

        float acceleration = m_AccelerationMul;
        if (m_Terminating)
        {
            if (dir.sqrMagnitude > GameManager.inst.TerminationSqrDst)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }

            dir *= -1;
            acceleration *= m_TerminatingSpeedMul;
        }

        //if (dir.sqrMagnitude < .1f)
        {
            m_Rigidbody.AddForce(dir.normalized * acceleration);
            m_Transform.rotation = Quaternion.LookRotation(dir);
        }
        //else
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x * m_Decelation,
                                   m_Rigidbody.velocity.y,
                                   m_Rigidbody.velocity.z * m_Decelation);
        }
    }

    public void Terminate()
    {
        m_Terminating = true;
    }
}