using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverPointManager : MonoBehaviour
{
    public List<Coverpoint> coverpoints;
    Transform playerTransform;
    public LayerMask playerAndEnviromentMask;
    public bool playerIsClose;
    public float maxDistanceToPlayer;




    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(Vector2.Distance(playerTransform.position , transform.position) < maxDistanceToPlayer)
        {
            playerIsClose = true;
        }
        else
        {
            playerIsClose = false;
        }

    }

    public bool HasFreeCoverPoints()
    {
        bool hasFreePoints = false;
        foreach(Coverpoint c in coverpoints)
        {
            if (!c.isOccupied && IsCoverPointInCover(c))
            {
                hasFreePoints = true;
                break;
            }
        }
        return hasFreePoints;
    }

    public Coverpoint GetFreeCoverPosition(Vector2 playerPosition)
    {


        Coverpoint choosenCoverPoint = coverpoints[0];
        float distanceToPlayerPosition = Vector2.Distance(choosenCoverPoint.coverPoint.position, playerPosition);
        int startingIndex = 0;

        for (int i = 0; i < coverpoints.Count; i++)
        {
            if (!coverpoints[i].isOccupied )
            {
                choosenCoverPoint = coverpoints[i];
                distanceToPlayerPosition = Vector2.Distance(choosenCoverPoint.coverPoint.position, playerPosition);
                startingIndex = i;
                break;
            }
        }

        for (int i = startingIndex; i<coverpoints.Count; i++)
        {
            if (!coverpoints[i].isOccupied )
            {
                float currentDistanceToPlayerPosition = Vector2.Distance(coverpoints[i].coverPoint.transform.position, playerPosition);
                if (currentDistanceToPlayerPosition > distanceToPlayerPosition)
                {
                    distanceToPlayerPosition = currentDistanceToPlayerPosition;
                    choosenCoverPoint = coverpoints[i];
                }
            }
        }

        choosenCoverPoint.isOccupied = true;
        return choosenCoverPoint;
    }

    public bool IsCoverPointInCover(Coverpoint coverpoint)
    {
        Vector2 dirToPlayer = playerTransform.transform.position - coverpoint.coverPoint.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(coverpoint.coverPoint.transform.position, dirToPlayer, 2);
        bool inCover = false;
        if (hit.collider != null)
        {
            if (hit.transform.gameObject == transform.gameObject)
            {
                inCover = true;
            }
        }
        return inCover;
    }


    public bool IsPLayerShotableFromCover()
    {
        Vector2 dirToPlayer = playerTransform.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, 100, playerAndEnviromentMask);
        bool isHitable = false;
        if(hit.collider != null)
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                isHitable = true;
            }
        }
        return isHitable;
    }
}
