using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckForCoverInIntervall", menuName = "PluggableAI/Decision/CheckForCoverInIntervall")]
public class CheckForCoverInIntervallWhenPlayerClose : Decision
{
    SearchCoverHandler searchCoverHandler;
    EnemyStats enemyStats;
    PlayerHolder shotingTargetHolder;

    public float playerDistance = 10;
    public float timeBetweenSearches;
    float currentTimeBetweenSearches;

    public override bool Decide(StateController sceneRef)
    {
        bool hasFoundCover = false;
        if (Vector2.Distance(shotingTargetHolder.target.transform.position, sceneRef.transform.position) < playerDistance)
        {
            if (currentTimeBetweenSearches < 0)
            {
                currentTimeBetweenSearches = timeBetweenSearches;
                GameObject coverManager = null;
                searchCoverHandler.GetFreeAndClosestCoverInRange(enemyStats.coverSearchDistance, out coverManager, out hasFoundCover);
                hasFoundCover = !hasFoundCover;
            }
            else
            {
                currentTimeBetweenSearches -= Time.deltaTime;
            }
        }
        else
        {
            currentTimeBetweenSearches = timeBetweenSearches;
        }

        return hasFoundCover;
    }

    public override void OnDecideEnd(StateController sceneRef)
    {

    }

    public override void OnDecideInit(StateController sceneRef)
    {
        searchCoverHandler = sceneRef.GetComponent<SearchCoverHandler>();
        enemyStats = sceneRef.GetComponent<EnemyStatsHolder>().enemyStats;
        shotingTargetHolder = sceneRef.GetComponent<PlayerHolder>();
    }

    public override void OnDecideStart(StateController sceneRef)
    {
        currentTimeBetweenSearches = timeBetweenSearches;
    }
}
