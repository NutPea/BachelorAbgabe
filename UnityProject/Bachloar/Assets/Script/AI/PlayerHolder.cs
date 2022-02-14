using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolder : MonoBehaviour
{
    public Transform target;
    bool targetIsPlayer;

    private void Start()
    {
        if (target == null)
        {
            if (targetIsPlayer)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    }
}
