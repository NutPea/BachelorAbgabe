using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGizmoDrawer : MonoBehaviour
{
    EnemyStats enemyStats;
    public bool showStats;
    public bool showWatchCone;
    bool gameStarts;
    public bool showCoverDistanceSearch;
    UtilitySetter utilitySetter;
    void Start()
    {

        enemyStats = GetComponent<EnemyStatsHolder>().enemyStats;
        utilitySetter = GetComponent<UtilitySetter>();
        gameStarts = true;

    }

    private void OnDrawGizmos()
    {
        if (gameStarts)
        {
            if (showStats)
            {
                Gizmos.DrawWireSphere(transform.position, enemyStats.reachedWaypointDistance);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, enemyStats.coverSearchDistance);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, enemyStats.aggroRange);

            }
            if (showCoverDistanceSearch)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position,utilitySetter.minCoverDistance);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, utilitySetter.maxCoverDistance);

            }
            if (showWatchCone)
            {
                Gizmos.color = Color.red;
                Quaternion coneRotationPositiv = Quaternion.AngleAxis(enemyStats.coneAngle / 2, -Vector3.forward);
                Quaternion coneRotationNegativ = Quaternion.AngleAxis(enemyStats.coneAngle / 2, Vector3.forward);
                Gizmos.DrawLine(transform.position, transform.position + transform.up * enemyStats.aggroRange);
                Gizmos.DrawLine(transform.position, transform.position + coneRotationPositiv * transform.up * enemyStats.aggroRange);
                Gizmos.DrawLine(transform.position, transform.position + coneRotationNegativ * transform.up * enemyStats.aggroRange);
                Gizmos.DrawLine(transform.position + coneRotationPositiv * transform.up * enemyStats.aggroRange, transform.position + transform.up * enemyStats.aggroRange);
                Gizmos.DrawLine(transform.position + transform.up * enemyStats.aggroRange, transform.position + coneRotationNegativ * transform.up * enemyStats.aggroRange);

                Gizmos.DrawWireSphere(transform.position, enemyStats.aggroRadius);
            }
        }
    }
}
