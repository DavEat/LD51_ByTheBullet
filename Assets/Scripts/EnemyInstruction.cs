using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInstrcution", menuName = "ScriptableObjects/EnemyInstruction", order = 1)]
public class EnemyInstruction : ScriptableObject
{
    public Vector3 Target;
    public float RotationAngleDeg = 0;

    public float SpeedMul = 1;

    public enum MovementType { Straight, Circular, ZigZag }
    public MovementType movementType = MovementType.Straight;

    public bool Relative = true;

    const float TARGET_MEET_AT = .1f;

    public Vector3 Init(Vector3 position)
    {
        Vector3 target = Relative ? Target + position : Target;
        if (Relative && movementType == MovementType.Circular)
        {
            target = Quaternion.Euler(0, RotationAngleDeg, 0) * position;
        }

        return target;
    }
    public bool IsCompleted(Vector3 position, Vector3 target)
    {
        if (!Relative)
            target = Target;

        return (target - position).sqrMagnitude < TARGET_MEET_AT;
    }
    public Vector3 GetDirection(Vector3 position, Vector3 target)
    {
        if (!Relative)
            target = Target;

        Vector3 dir;

        if (movementType == MovementType.Circular)
        {
            // old does not work -> dir = Vector3.Cross(position * target.magnitude, Vector3.up);

            dir = target - position;
            if (dir.sqrMagnitude > 3)
            {
                float sign = -Mathf.Sign(RotationAngleDeg);                
                
                dir = Vector3.Cross(position * target.magnitude, Vector3.up) * sign;
            }
        }
        else if (movementType == MovementType.ZigZag)
        {
            Vector3 direction = target - position;
            dir = (direction) + Vector3.Cross(direction, Vector3.up);
        }
        else
        {
            dir = target - position;
        }
        return dir.normalized * SpeedMul;
    }
}