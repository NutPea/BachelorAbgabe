using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats" ,menuName = "EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public float aggroRadius = 2.5f;
    public float aggroRange = 10f;
    public float coneAngle = 22.5f;
    public float reachedWaypointDistance = 0.5f;
    public float coverSearchDistance = 10;

    public float playerInCoverCheckDistance = 20;
}
