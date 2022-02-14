using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckIfSquadCantSeesPlayer", menuName = "PluggableAI/Decision/CheckIfSquadCantSeesPlayer")]

public class CheckIfSquadCantSeesPlayer : Decision
{
    SquadManager squadManager;

    public override bool Decide(StateController sceneRef)
    {
        return squadManager.squadKnowsPlayerPos;
    }

    public override void OnDecideEnd(StateController sceneRef)
    {

    }

    public override void OnDecideInit(StateController sceneRef)
    {
        squadManager = sceneRef.GetComponent<SquadMemberHandler>().squadManager;
    }

    public override void OnDecideStart(StateController sceneRef)
    {

    }
}
