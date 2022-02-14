using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickUpSquadMember", menuName = "PluggableAI /Actions/Squad/PickUpSquadMember")]
public class PickUpSquadMember_Action : PluggableAction
{
    public float timeToPickUp;
    float currentTimeToPickUp;
    SquadMemberHandler squadMemberHandler;
    CrouchHandler crouchHandler;
    Animator anim;

    public override void Act(StateController sceneRef)
    {
       if(currentTimeToPickUp < 0)
       {
            squadMemberHandler.squadManager.FinishPickingUpSquadMember(squadMemberHandler,squadMemberHandler.downedSquadMember);
       }
       else
       {
            currentTimeToPickUp -= Time.deltaTime;
       }
    }

    public override void OnActEnd(StateController sceneRef)
    {
        anim.SetBool("IsPickingUp", false);
        crouchHandler.isCrouching = false;
    }

    public override void OnActInit(StateController sceneRef)
    {
        anim = sceneRef.GetComponent<AnimatorHolder>().anim;
        crouchHandler = sceneRef.GetComponent<CrouchHandler>();
    }

    public override void OnActStart(StateController sceneRef)
    {
        squadMemberHandler = sceneRef.GetComponent<SquadMemberHandler>();
        currentTimeToPickUp = timeToPickUp;
        crouchHandler.isCrouching = true;
        anim.SetBool("IsPickingUp", true);
    }
}
