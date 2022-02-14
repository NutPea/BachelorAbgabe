using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IsDowned", menuName = "PluggableAI/Actions/IsDowned")]
public class IsDowned_Action : PluggableAction
{


    A_Star_Movement movement;
    CrouchHandler crouchHandler;
    SquadMemberHandler squadMemberHandler;
    EnemyDamageManager enemyDamageManager;

    public float timeBetweenHelpCalls = 5f;
    float currentTimeBetweenHelpCalls;

    Animator anim;

    public override void Act(StateController sceneRef)
    {
        if(currentTimeBetweenHelpCalls < 0)
        {
            currentTimeBetweenHelpCalls = timeBetweenHelpCalls;
            squadMemberHandler.CallForPickUpAssistance();
        }
        else
        {
            currentTimeBetweenHelpCalls -= Time.deltaTime;
        }
    }

    public override void OnActEnd(StateController sceneRef)
    {
        crouchHandler.isCrouching = false;
        anim.SetTrigger("GetPickedUp");
        enemyDamageManager.isDowned = false;
    }

    public override void OnActInit(StateController sceneRef)
    {
        anim = sceneRef.GetComponent<AnimatorHolder>().anim;
        enemyDamageManager = sceneRef.GetComponent<EnemyDamageManager>();
        squadMemberHandler = sceneRef.GetComponent<SquadMemberHandler>();
        movement = sceneRef.GetComponent<A_Star_Movement>();
        crouchHandler = sceneRef.GetComponent<CrouchHandler>();
    }

    public override void OnActStart(StateController sceneRef)
    {
        movement.StopMovement();
        crouchHandler.isCrouching = true;
        anim.SetTrigger("GetDowned");
        squadMemberHandler.CallForPickUpAssistance();
        enemyDamageManager.isDowned = true;
        currentTimeBetweenHelpCalls = timeBetweenHelpCalls;
    }
}
