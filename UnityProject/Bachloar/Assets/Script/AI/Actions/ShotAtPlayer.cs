using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShotAtPlayer", menuName = "PluggableAI/Actions/Shoting/ShotAtPlayer")]

public class ShotAtPlayer : PluggableAction
{
    public float minTimeBetweenShots;
    public float maxTimeBetweenShots;
    float currentTimeBetweenShots;

    Transform sceneRefTransform;
    ShotingHandler shotingHandler;
    PlayerHolder shotingTarget;
    Animator anim;

    public override void Act(StateController sceneRef)
    {
        Vector2 shotDir = shotingTarget.target.transform.position - sceneRef.transform.position;
        shotDir = shotDir.normalized;
        if (shotingHandler.IsPlayerShotable(shotDir))
        {
            if (currentTimeBetweenShots < 0)
            {
                currentTimeBetweenShots = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
                shotingHandler.Shot(shotDir);
                anim.SetTrigger("Shoting");
            }
            else
            {
                currentTimeBetweenShots -= Time.deltaTime;
            }
        }
    }

    public override void OnActEnd(StateController sceneRef)
    {
       
    }

    public override void OnActInit(StateController sceneRef)
    {
        shotingHandler = sceneRef.GetComponent<ShotingHandler>();
        shotingTarget = sceneRef.GetComponent<PlayerHolder>();
        anim = sceneRef.GetComponent<AnimatorHolder>().anim;
    }

    public override void OnActStart(StateController sceneRef)
    {
        currentTimeBetweenShots = Random.Range(minTimeBetweenShots,maxTimeBetweenShots);
    }
}
