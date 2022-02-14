using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchCoverHandler : MonoBehaviour
{
    public LayerMask coverLayer;
    public Coverpoint currentCoverPoint;
    public bool didntFoundCover;

    EnemyStats enemyStats;
    MovementTargetHolder movementTargetHolder;
    PlayerHolder playerHolder;


    private void Awake()
    {
        enemyStats = GetComponent<EnemyStatsHolder>().enemyStats;
        movementTargetHolder = GetComponent<MovementTargetHolder>();
        playerHolder = GetComponent<PlayerHolder>();  
    }

    //Finds a usable coverpoint
    public void GetCoverPoint(Vector2 target)
    {
        GameObject coverGameobject = null;
        GetFreeAndClosestCoverInRange(enemyStats.coverSearchDistance, out coverGameobject, out didntFoundCover);
        if (!didntFoundCover)
        {
            currentCoverPoint = coverGameobject.GetComponent<CoverPointManager>().GetFreeCoverPosition(target);
            movementTargetHolder.target = currentCoverPoint.coverPoint.transform;
        }
    }

    //Gets a valueable Cover in Range
    public void GetFreeAndClosestCoverInRange(float coverSearchDistance, out GameObject choosenCoverGameobject, out bool didntFoundCover)
    {
        didntFoundCover = false;
        choosenCoverGameobject = null;

        Collider2D[] allCoverInRange = Physics2D.OverlapCircleAll(transform.position, coverSearchDistance, coverLayer);
        List<CoverPointManager> freeCoverPointManager = new List<CoverPointManager>();
        foreach (Collider2D col in allCoverInRange)
        {
            CoverPointManager inspectedManager = col.GetComponent<CoverPointManager>();
            float distanceToShotTarget= Vector2.Distance(col.gameObject.transform.position, playerHolder.target.transform.position);
            float distanceToObject = Vector2.Distance(col.gameObject.transform.position, transform.position);

            if (distanceToShotTarget < distanceToObject)
            {
                continue;
            }

            if (inspectedManager.HasFreeCoverPoints() && inspectedManager.IsPLayerShotableFromCover() && !inspectedManager.playerIsClose)
            {
                freeCoverPointManager.Add(inspectedManager);
            }

        }

        if (freeCoverPointManager.Count > 0)
        {
            Transform closestCover = freeCoverPointManager[0].transform;
            float distanceToClosestCover = Vector2.Distance(closestCover.position, transform.position);
            for (int i = 1; i < freeCoverPointManager.Count; i++)
            {
                float currentDistanceToClosestCover = Vector2.Distance(freeCoverPointManager[i].transform.position, transform.position);
                if (currentDistanceToClosestCover < distanceToClosestCover)
                {
                    closestCover = freeCoverPointManager[i].transform;
                    distanceToClosestCover = currentDistanceToClosestCover;
                }
            }
            choosenCoverGameobject = closestCover.gameObject;
        }
        else
        {
            didntFoundCover = true;
        }

    }

    public void RemoveUsedCoverPoint()
    {
        if (currentCoverPoint != null)
        {
            currentCoverPoint.isOccupied = false;
            currentCoverPoint = null;
        }
    }

    //Checks if a position is behind Cover
    public bool CheckIfTransformIsBehindCover(Transform target,Transform source,float checkDistance)
    {
        Vector2 targetDir = target.position - source.position;
        targetDir = targetDir.normalized;
        bool targetIsInCover = false;
        GameObject hittendObject = null;
        RaycastHit2D[] hits = Physics2D.RaycastAll(source.position, targetDir,checkDistance, coverLayer);
        foreach (RaycastHit2D hit in hits)
        {
            if(hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Cover"))
                {
                    hittendObject = hit.collider.transform.gameObject;                
                }
            }
        }

        if(hittendObject != null)
        {
            float distanceToCover = Vector2.Distance(source.position, hittendObject.transform.position);
            float distanceToTarget = Vector2.Distance(source.position, target.transform.position);

            if(distanceToCover < distanceToTarget)
            {
                targetIsInCover = true;
            }
        }


        return targetIsInCover;

    }
   
}
