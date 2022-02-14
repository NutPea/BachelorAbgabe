using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPlayerWatchCone : MonoBehaviour
{
    Transform playerTransform;
    EnemyStats enemyStats;
    ShotingHandler shotingHandler;

    private void Start()
    {
        playerTransform = GetComponent<PlayerHolder>().target;
        enemyStats = GetComponent<EnemyStatsHolder>().enemyStats;
        shotingHandler = GetComponent<ShotingHandler>();
    }

    public bool IsTargetWatchCone()
    {
        Vector2 directionToTarget = playerTransform.position - transform.position;
        if(directionToTarget.magnitude <= enemyStats.aggroRadius)
        {
            return true;
        }

        if (directionToTarget.magnitude > enemyStats.aggroRange) return false;
        directionToTarget = directionToTarget.normalized;
        float angle = Vector2.Angle(transform.up, directionToTarget);
        if (angle > enemyStats.coneAngle/2) return false;
        if (shotingHandler.IsPlayerShotable(directionToTarget))
        {
            return true;
        }
        return false;
    }

}
