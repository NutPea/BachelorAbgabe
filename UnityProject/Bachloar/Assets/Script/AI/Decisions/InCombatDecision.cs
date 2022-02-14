using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InCombat", menuName = "PluggableAI/Decision/InCombat")]
public class InCombatDecision : Decision
{
    EnemyDamageManager enemyDamageManager;

    public override bool Decide(StateController sceneRef)
    {
        return enemyDamageManager.isInCombat;
    }

    public override void OnDecideEnd(StateController sceneRef)
    {

    }

    public override void OnDecideInit(StateController sceneRef)
    {
        enemyDamageManager = sceneRef.GetComponent<EnemyDamageManager>();
    }

    public override void OnDecideStart(StateController sceneRef)
    {

    }
}
