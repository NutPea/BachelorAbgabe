using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WaitOnSquadPos", menuName = "PluggableAI /Actions/Squad/WaitOnSquadPos")]
public class WaitOnSquadPos_Action : PluggableAction
{
    SquadManager squadManager;

    public override void Act(StateController sceneRef)
    {

    }

    public override void OnActEnd(StateController sceneRef)
    {

    }

    public override void OnActInit(StateController sceneRef)
    {
        squadManager = sceneRef.GetComponent<SquadMemberHandler>().squadManager;
    }

    public override void OnActStart(StateController sceneRef)
    {
        squadManager.SquadMemberIsWaiting();
    }
}
