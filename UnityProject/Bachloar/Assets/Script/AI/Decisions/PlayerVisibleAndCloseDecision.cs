using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerVisibleAndCloseDecision", menuName = "PluggableAI/Decision/PlayerVisibleAndCloseDecision")]
public class PlayerVisibleAndCloseDecision : Decision
{
    PlayerHolder targetHolder;
    ShotingHandler shotingHandler;

    public float distance;
    public override bool Decide(StateController sceneRef)
    {
        Vector2 dir = targetHolder.target.position - sceneRef.transform.position;
        dir = dir.normalized;

        return shotingHandler.IsPlayerShotable(dir) && Vector2.Distance(targetHolder.target.transform.position, sceneRef.transform.position)< distance;
    }

    public override void OnDecideEnd(StateController sceneRef)
    {

    }

    public override void OnDecideInit(StateController sceneRef)
    {
        targetHolder = sceneRef.GetComponent<PlayerHolder>();
        shotingHandler = sceneRef.GetComponent<ShotingHandler>();
        Physics2D.queriesStartInColliders = false;
    }

    public override void OnDecideStart(StateController sceneRef)
    {
    }
}
