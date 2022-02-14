using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Star_Movement : MonoBehaviour
{
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;

    public Vector2 target;
    public float movementSpeed = 5;
    public float currentMovementSpeed;
    public Vector2[] path;
    public int targetIndex;
    Rigidbody2D rb2d;
    public Animator anim;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(UpdatePath());
        StartMovement();
    }

    public void StopMovement()
    {
        currentMovementSpeed = 0;
        anim.SetBool("IsWalking", false);
    }

    public void StartMovement()
    {
        currentMovementSpeed = movementSpeed;
        anim.SetBool("IsWalking", true);
    }

    public void RemoveCourotines()
    {
        StopCoroutine("FollowPath");
        StopCoroutine("UpdatePath");
    }

    IEnumerator UpdatePath()
    {
        if(Time.timeSinceLevelLoad < .3f)
        {
            PathRequestManager.RequestPath(transform.position, target, OnPathFound);
        }

        float sqrtMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector2 oldTargetPos = target;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            PathRequestManager.RequestPath(transform.position, target, OnPathFound);
            oldTargetPos = target;
        }
    }

    private void OnPathFound(Vector2[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            targetIndex = 0;
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector2 currentWaypoint = path[0];

        while (true)
        {
            if (Vector2.Distance((Vector2)transform.position, currentWaypoint) < 0.1f)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            Vector2 movementDir = currentWaypoint - (Vector2)transform.position;
            movementDir = movementDir.normalized;
            rb2d.MovePosition((Vector2)transform.position + movementDir * currentMovementSpeed * Time.fixedDeltaTime);

            yield return null;
        }
    }
    public void SetTarget(Vector2 position)
    {
        target = position;
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = targetIndex;i < path.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[i], new Vector2(0.5f,0.5f));

                if(i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
