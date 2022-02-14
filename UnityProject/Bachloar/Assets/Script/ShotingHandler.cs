using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotingHandler : MonoBehaviour
{
    public TeamFlag teamFlag;
    public int damage;
    public float shotDistance;
    public Transform shotPos;
    public GameObject wallHitParticle;
    public GameObject characterHitParticle;
    public GameObject lineTracer;
    bool hasHitCharacter;
    public LayerMask shotingLayer;


    public void Shot(Vector2 shotDir)
    {
        Vector2 hitPos = Vector2.zero;
        bool beforeWasCover = false;
        shotDir = shotDir.normalized;
        bool hasHit = false;
        hasHitCharacter = false;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, shotDir, shotDistance, shotingLayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                GameObject hittendObject = hit.collider.transform.gameObject;
                if (hittendObject.CompareTag("Cover"))
                {
                    beforeWasCover = true;
                }
                else if (hittendObject.CompareTag("Enemy"))
                {
                    bool isCrouching = hittendObject.GetComponent<CrouchHandler>().isCrouching;
                    if (isCrouching)
                    {
                        if (!beforeWasCover)
                        {
                            hasHit = true;
                            hasHitCharacter = true;
                        }
                    }
                    else
                    {
                        hasHit = true;
                        hasHitCharacter = true;
                    }
                }
                else if (hittendObject.CompareTag("Environment"))
                {
                    hasHit = true;
                }
                else if (hittendObject.CompareTag("Player"))
                {
                    bool isCrouching = hittendObject.GetComponent<CrouchHandler>().isCrouching;
                    if (isCrouching)
                    {
                        if (!beforeWasCover)
                        {
                            hasHit = true;
                            hasHitCharacter = true;
                        }
                    }
                    else
                    {
                        hasHit = true;
                        hasHitCharacter = true;
                    }
                }


                if (hasHit)
                {
                    HealthManager characterHealthManager = hittendObject.GetComponent<HealthManager>();
                    if (characterHealthManager != null)
                    {
                        characterHealthManager.CalculateDamage(damage, teamFlag, transform);
                    }

                    hitPos = hit.point;
                    GameObject partikle;
                    if (hasHitCharacter)
                    {
                        partikle = Instantiate(characterHitParticle, hit.point, Quaternion.identity);
                    }
                    else
                    {
                        partikle = Instantiate(wallHitParticle, hit.point, Quaternion.identity);
                    }
                    partikle.transform.up = hit.normal;
                    Destroy(partikle, 1f);

                    GameObject tracer = Instantiate(lineTracer, transform.position, Quaternion.identity);
                    LineRenderer r = tracer.GetComponent<LineRenderer>();
                    r.SetPosition(0, shotPos.transform.position);
                    r.SetPosition(1, hitPos);
                    Destroy(tracer, 0.05f);

                    break;
                }
            }

        }

        if (!hasHit)
        {
            hitPos = shotDir * shotDistance;

            GameObject tracer = Instantiate(lineTracer, transform.position, Quaternion.identity);
            LineRenderer r = tracer.GetComponent<LineRenderer>();
            r.SetPosition(0, shotPos.transform.position);
            r.SetPosition(1, (Vector2)transform.position + hitPos);
            Destroy(tracer, 0.05f);

        }

    }
    public bool  IsPlayerShotable(Vector2 shotDir)
    {
        bool beforeWasCover = false;
        shotDir = shotDir.normalized;
        bool hasHit = false;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, shotDir, shotDistance, shotingLayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                GameObject hittendObject = hit.collider.transform.gameObject;
                if (hittendObject.CompareTag("Cover"))
                {
                    beforeWasCover = true;
                }
                else if (hittendObject.CompareTag("Player"))
                {
                    bool isCrouching = hittendObject.GetComponent<CrouchHandler>().isCrouching;
                    if (isCrouching)
                    {
                        if (!beforeWasCover)
                        {
                            hasHit = true;
                        }
                    }
                    else
                    {
                        hasHit = true;
                    }
                }
                else if (hittendObject.CompareTag("Environment"))
                {
                    hasHit = false;
                    break;
                }


                if (hasHit)
                {
                    break;
                }
            }

        }

        return hasHit;
    }
}
