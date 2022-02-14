using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CoverShotAtShotingTarget", menuName = "PluggableAI /Actions/Shoting/CoverShotAtShotingTarget")]
public class CoverShotAtShotingTarget_Action : PluggableAction
{
    CrouchHandler crouchHandler;
    ShotingHandler shotingHandler;
    PlayerHolder shotingTargetHolder;
    SearchCoverHandler searchCoverHandler;
    UtilityAIFlag aggresionFlag;
    UtilityAIFlag deffensivFlag;
    Animator anim;
    
    bool inCover;
    public float minShotingTimer;
    public float maxShotingTimer;
    float currentShotingTimer;

    public float outOfCoverTimer;
    float currentOutOfCoverTimer;

    public float inCoverTimer;
    float currentInOfCoverTimer;

    public override void Act(StateController sceneRef)
    {
        if (inCover)
        {
            if(currentInOfCoverTimer < 0)
            {
                currentInOfCoverTimer = GetInCoverTime();
                inCover = false;
                crouchHandler.StopCrouching();
            }
            else
            {
                currentInOfCoverTimer -= Time.deltaTime;
            }
        }
        else
        {
            if(currentOutOfCoverTimer < 0)
            {
                currentOutOfCoverTimer = outOfCoverTimer;
                float isInCoverValue = Random.Range(0.0f, 1.0f);
                if(isInCoverValue > aggresionFlag.utilityScore)
                {
                    inCover = true;
                    crouchHandler.StartCrouching();
                }
            }
            else
            {
                currentOutOfCoverTimer -= Time.deltaTime;
            }

            Vector2 dir = shotingTargetHolder.target.transform.position - sceneRef.transform.position;
            dir = dir.normalized;
            if (shotingHandler.IsPlayerShotable(dir))
            {
                if (currentShotingTimer < 0)
                {
                    shotingHandler.Shot(dir);
                    anim.SetTrigger("Shoting");
                    currentShotingTimer = Random.Range(minShotingTimer,maxShotingTimer);
                }
                else
                {
                    currentShotingTimer -= Time.deltaTime;
                }
            }


        }
    }

    public override void OnActEnd(StateController sceneRef)
    {
        crouchHandler.StopCrouching();
        searchCoverHandler.RemoveUsedCoverPoint();
    }

    public override void OnActInit(StateController sceneRef)
    {
        crouchHandler = sceneRef.GetComponent<CrouchHandler>();
        shotingHandler = sceneRef.GetComponent<ShotingHandler>();
        shotingTargetHolder = sceneRef.GetComponent<PlayerHolder>();
        UtilityManager utilityManager = sceneRef.GetComponent<UtilityManager>();
        aggresionFlag = utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.Aggressiv);
        deffensivFlag = utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.Flankable);
        searchCoverHandler = sceneRef.GetComponent<SearchCoverHandler>();
        anim = sceneRef.GetComponent<AnimatorHolder>().anim;
    }

    public override void OnActStart(StateController sceneRef)
    {
        currentInOfCoverTimer = GetInCoverTime();
        currentOutOfCoverTimer = outOfCoverTimer;
        currentShotingTimer = Random.Range(minShotingTimer, maxShotingTimer);
    }

    public float GetInCoverTime()
    {
        return inCoverTimer - (inCoverTimer - 1) * (1 - deffensivFlag.utilityScore);
    }
}
