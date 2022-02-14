using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " DragSquadMemberInCover", menuName = "PluggableAI/Actions/Squad/DragSquadMemberInCover")]
public class DragSquadMemberInCover_Action : PluggableAction
{

    EnemyStats enemyStats;
    SearchCoverHandler searchCoverHandler;
    PlayerHolder shotingTargetHolder;
    MovementTargetHolder movementTargetHolder;
    A_Star_Movement movement;
    SquadMemberHandler helpingSquadMemberHandler;
    Animator anim;
    Animator memberAnim;
    Transform dragPosition;

    public override void Act(StateController sceneRef)
    {
        helpingSquadMemberHandler.transform.position = dragPosition.position;
    }

    public override void OnActEnd(StateController sceneRef)
    {
        movement.StopMovement();
        memberAnim.SetBool("GetDragged", false);
        anim.SetTrigger("StopDragging");
    }

    public override void OnActInit(StateController sceneRef)
    {
        movementTargetHolder = sceneRef.GetComponent<MovementTargetHolder>();
        shotingTargetHolder = sceneRef.GetComponent<PlayerHolder>();
        searchCoverHandler = sceneRef.GetComponent<SearchCoverHandler>();
        movement = sceneRef.GetComponent<A_Star_Movement>();
        enemyStats = sceneRef.GetComponent<EnemyStatsHolder>().enemyStats;
    }

    public override void OnActStart(StateController sceneRef)
    {
        movement.StartMovement();
        helpingSquadMemberHandler = sceneRef.GetComponent<SquadMemberHandler>().downedSquadMember;
        memberAnim = helpingSquadMemberHandler.GetComponent<AnimatorHolder>().anim;
        memberAnim.SetBool("GetDragged", true);

        anim = sceneRef.GetComponent<AnimatorHolder>().anim;
        anim.SetTrigger("StartDragging");

        dragPosition = sceneRef.GetComponent<ShotingHandler>().shotPos;
        searchCoverHandler.GetCoverPoint(shotingTargetHolder.target.position);
        if (!searchCoverHandler.didntFoundCover)
        {
            movementTargetHolder.target = searchCoverHandler.currentCoverPoint.coverPoint;
            movement.SetTarget(searchCoverHandler.currentCoverPoint.coverPoint.position);
        }

    }
}
