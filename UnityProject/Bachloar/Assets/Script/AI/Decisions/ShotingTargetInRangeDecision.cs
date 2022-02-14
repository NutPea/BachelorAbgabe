using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShotingTargetInRangeDecision", menuName = "PluggableAI/Decision/ShotingTargetInRangeDecision")]
public class ShotingTargetInRangeDecision : Decision
{
    PlayerHolder targetHolder;
    EnemyStats enemyStats;
    Transform _transform;
    SearchPlayerWatchCone searchPlayerWatchCone;
    public override bool Decide(StateController sceneRef)
    {
        return searchPlayerWatchCone.IsTargetWatchCone();
    }

    public override void OnDecideEnd(StateController sceneRef)
    {
        
    }

    public override void OnDecideInit(StateController sceneRef)
    {
        enemyStats = sceneRef.GetComponent<EnemyStatsHolder>().enemyStats;
        targetHolder = sceneRef.GetComponent<PlayerHolder>();
        _transform = sceneRef.transform;
        searchPlayerWatchCone = sceneRef.GetComponent<SearchPlayerWatchCone>();
        Physics2D.queriesStartInColliders = false;
    }

    public override void OnDecideStart(StateController sceneRef)
    {
        
    }

    
}
