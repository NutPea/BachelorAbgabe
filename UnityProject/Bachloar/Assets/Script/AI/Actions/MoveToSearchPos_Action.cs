using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveToSearchPos", menuName = "PluggableAI/Actions/Movement/MovementMoveToSearchPos")]
public class MoveToSearchPos_Action : PluggableAction
{
    EnemyStats enemyStats;
    SquadManager squadManager;
    MovementTargetHolder movementTargetHolder;
    A_Star_Movement movement;
    EnemyDamageManager enemyDamageManager;

    public override void Act(StateController sceneRef)
    {
        movement.SetTarget(movementTargetHolder.target.position);
    }

    public override void OnActEnd(StateController sceneRef)
    {
        movement.StopMovement();
    }

    public override void OnActInit(StateController sceneRef)
    {
        movement = sceneRef.GetComponent<A_Star_Movement>();
        movementTargetHolder = sceneRef.GetComponent<MovementTargetHolder>();
        squadManager = sceneRef.GetComponent<SquadMemberHandler>().squadManager;
        enemyStats = sceneRef.GetComponent<EnemyStatsHolder>().enemyStats;
        enemyDamageManager = sceneRef.GetComponent<EnemyDamageManager>();
    }

    public override void OnActStart(StateController sceneRef)
    {
        movement.StartMovement();
        movementTargetHolder.target = squadManager.GetFirstSearchTransform();
        enemyDamageManager.isInCombat = false;
    }

}
