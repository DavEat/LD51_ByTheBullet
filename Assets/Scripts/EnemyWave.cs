using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "ScriptableObjects/EnemyWave", order = 1)]
public class EnemyWave : ScriptableObject
{
    public EnPos[] Composition;

    [System.Serializable]
    public class EnPos
    {
        public EnemyMovement type;
        public Vector3 position;
    }
}