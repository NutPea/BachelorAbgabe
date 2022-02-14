using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CanSeePlayer", menuName = "PluggableAI/Decision/CanSeePlayer")]

public class CanSeePlayer_Decision : Decision
{
    PlayerHolder targetHolder;
    ShotingHandler shotingHandler;
    public float doNotSeeTimer = 3f;
    float currentDoNotSeeTimer;

    public override bool Decide(StateController sceneRef)
    {
        Vector2 dir = targetHolder.target.position - sceneRef.transform.position;
        dir = dir.normalized;
        bool doNotSeePlayer = false;
        if (!shotingHandler.IsPlayerShotable(dir))
        {
            if(currentDoNotSeeTimer < 0)
            {
                doNotSeePlayer = true;
            }
            else
            {
                currentDoNotSeeTimer -= Time.deltaTime;
            }
        }
        else
        {
            currentDoNotSeeTimer = doNotSeeTimer;
        }

        return doNotSeePlayer;  
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
        currentDoNotSeeTimer = doNotSeeTimer;
    }
}
