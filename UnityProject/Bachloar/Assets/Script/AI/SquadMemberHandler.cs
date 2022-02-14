using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SquadMemberHandler : MonoBehaviour
{
    public SquadManager squadManager;
    ShotingHandler shotingHandler;
    HealthManager healthManager;
    Transform playerTransform;
    UtilityManager utilityManager;
    public SquadMemberHandler downedSquadMember;
    public bool squadCantChangeState;
    public int currentSquadMemberIndex;
    public SquadMemberAssistantEvent squadMemberFireAssistantEvent = new SquadMemberAssistantEvent();
    public SquadMemberAssistantEvent squadMemberPickUpEvent = new SquadMemberAssistantEvent();


    public float distanceToOtherSquadMember = 3f;


    private void Start()
    {
        shotingHandler = GetComponent<ShotingHandler>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        healthManager = GetComponent<HealthManager>();
        utilityManager = GetComponent<UtilityManager>();
    }

    public class SquadMemberAssistantEvent : UnityEvent<int> { 
    }

    public void CallForFireAssistance()
    {
        squadMemberFireAssistantEvent.Invoke(currentSquadMemberIndex);
    }

    public void CallForPickUpAssistance()
    {
        squadMemberPickUpEvent.Invoke(currentSquadMemberIndex);
    }

 

    public bool SquadMemberAreClose()
    {
        foreach(SquadMemberHandler squadMember in squadManager.squadMembers)
        {
            if(squadMember == this)
            {
                continue;
            }

            float distance = Vector2.Distance(transform.position, squadMember.transform.position);
            if(distance < distanceToOtherSquadMember)
            {
                return true;
            }
            
        }
        return false;
    }

}

