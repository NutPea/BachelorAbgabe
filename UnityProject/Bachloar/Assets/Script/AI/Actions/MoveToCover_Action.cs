using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveToCover", menuName = "PluggableAI/Actions/MoveToCover")]
public class MoveToCover_Action : PluggableAction
{
    EnemyStats enemyStats;
    SearchCoverHandler searchCoverHandler;
    PlayerHolder shotingTargetHolder;
    MovementTargetHolder movementTargetHolder;
    A_Star_Movement movement;

    public override void Act(StateController sceneRef)
    {
        
    }

    public override void OnActEnd(StateController sceneRef)
    {
        movement.StopMovement();
    }

    public override void OnActInit(StateController sceneRef)
    {
        movementTargetHolder= sceneRef.GetComponent<MovementTargetHolder>();
        shotingTargetHolder = sceneRef.GetComponent<PlayerHolder>();
        searchCoverHandler = sceneRef.GetComponent<SearchCoverHandler>();
        movement = sceneRef.GetComponent<A_Star_Movement>();
        enemyStats = sceneRef.GetComponent<EnemyStatsHolder>().enemyStats;
    }

    public override void OnActStart(StateController sceneRef)
    {
        movement.StartMovement();
        searchCoverHandler.GetCoverPoint(shotingTargetHolder.target.position);
        if(!searchCoverHandler.didntFoundCover)
        {
            movementTargetHolder.target = searchCoverHandler.currentCoverPoint.coverPoint;
            movement.SetTarget(searchCoverHandler.currentCoverPoint.coverPoint.position);
        }

    }
}
