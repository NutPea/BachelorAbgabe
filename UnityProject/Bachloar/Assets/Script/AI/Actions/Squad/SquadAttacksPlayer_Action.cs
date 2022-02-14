using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SquadAttacksPlayer", menuName = "PluggableAI /Actions/Squad/SquadAttacksPlayer")]
public class SquadAttacksPlayer_Action : PluggableAction
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
        squadManager.SquadAttackPlayerDirectly();
    }
}
