using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WaypointReched", menuName = "PluggableAI/Decision/MovementReched")]
public class MovementTargetReched_Decision : Decision
{

    EnemyStats enemyStats;
    MovementTargetHolder movementTargetHolder;

    public override bool Decide(StateController sceneRef)
    {

        return Vector2.Distance(movementTargetHolder.target.position, (Vector2)sceneRef.transform.position) < enemyStats.reachedWaypointDistance;
    }

    public override void OnDecideEnd(StateController sceneRef)
    {
       
    }

    public override void OnDecideInit(StateController sceneRef)
    {
        movementTargetHolder = sceneRef.GetComponent<MovementTargetHolder>();
        enemyStats = sceneRef.GetComponent<EnemyStatsHolder>().enemyStats;
    }

    public override void OnDecideStart(StateController sceneRef)
    {
       
    }

    
}
