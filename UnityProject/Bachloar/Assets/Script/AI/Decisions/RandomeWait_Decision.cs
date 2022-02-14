using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomeWait", menuName = "PluggableAI/Decision/RandomeWait")]

public class RandomeWait_Decision : Decision
{
    public float minTimer;
    public float maxTimer;
    float currentTimer;
    public override bool Decide(StateController sceneRef)
    {
        currentTimer -= Time.deltaTime;
        return currentTimer < 0;
    }

    public override void OnDecideEnd(StateController sceneRef)
    {
        
    }

    public override void OnDecideInit(StateController sceneRef)
    {
        
    }

    public override void OnDecideStart(StateController sceneRef)
    {
        currentTimer = Random.Range(minTimer, maxTimer);
    }
}
