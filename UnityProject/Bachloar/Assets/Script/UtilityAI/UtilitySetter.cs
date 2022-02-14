using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilitySetter : MonoBehaviour
{
    private PlayerHolder playerHolder;
    private UtilityManager utilityManager;
    private SearchCoverHandler searchCoverHandler;
    private HealthManager healthManager;
    private EnemyDamageManager enemyDamageManager;
    private ShotingHandler shotingHandler;

    private EnemyStats enemyStats;

    [Header("//------------------------AggressiveUtility---------------------//")]
    public AnimationCurve aggressiveCurve;
    [Header("//------------------------FlankUtility---------------------//")]
    public AnimationCurve flankCurve;
    public float maxFlankDistanceToPlayer = 5;
    public float minFlankDistanceToPlayer = 2;
    [Header("//------------------------SetCoverUtility---------------------//")]
    public AnimationCurve sprintTowardsCoverCurve;
    public AnimationCurve shotAtPlayerTowardsCoverCurve;
    public float minCoverDistance = 3;
    public float maxCoverDistance = 5;
    public float towardsCoverWeight = 0.5f;
    public float isPlayerBehindCoverWeight = 0.25f;
    public float playerIsFarAwayCoverWeight = 0.25f;
    [Header("//------------------------SetUtilityNeedHelp---------------------//")]
    public float onShotValue = 0.1f;
    public float onShotAgreesionSub = 0.1f;
    public float timeBetweenLookAtChecks;
    public float currentTimeBetweenLookAtChecks;
    public float minAngle = 5f;
    public float lookAddValue;
    [Header("//------------------------SetCanGiveFireAssistant---------------------//")]
    public AnimationCurve fireAssistantCurve;
    public float distanceUtilityWeight = 0.25f;
    public float playerShotableUtilitieWeight = 0.5f;
    public float healthUtilityWeight = 0.25f;


    public float minDistanceToSquadMember = 2f;
    public float maxDistanceToSquadMember = 5f;
    [Header("//------------------------CanGivePickUpAssistance---------------------//")]
    public AnimationCurve pickUpAssistantCurve;
    public float pickUpDistanceUtilityWeight = 0.5f;
    public float pickUpHealthUtilityWeight = 0.5f;
    public float minHelpDistanceToSquadMember = 3f;
    public float maxHelpDistanceToSquadMember = 6f;


    // Start is called before the first frame update
    void Start()
    {
        playerHolder = GetComponent<PlayerHolder>();
        utilityManager = GetComponent<UtilityManager>();
        enemyStats = GetComponent<EnemyStatsHolder>().enemyStats;
        searchCoverHandler = GetComponent<SearchCoverHandler>();
        healthManager = GetComponent<HealthManager>();
        enemyDamageManager = GetComponent<EnemyDamageManager>();
        shotingHandler = GetComponent<ShotingHandler>();


        utilityManager.utilityEvent.AddListener(SetFlankUtilitie);
        utilityManager.utilityEvent.AddListener(SetAggressivUtilitie);

        utilityManager.utilityEvent.AddListener(SetCoverUtilitie);
        utilityManager.utilityEvent.AddListener(SetCanHelpImmediatly);
        healthManager.OnCalculateDamage.AddListener(SetUtilityOnReceivingDamage);
        currentTimeBetweenLookAtChecks = timeBetweenLookAtChecks;
    }

    public void SetAggressivUtilitie()
    {
        // When the player gets damage enemys are more agressive

        HealthManager shotingTargetHealthManager = playerHolder.target.GetComponent<HealthManager>();
        float utilityScore = 1.0f - (float)shotingTargetHealthManager.currentHealth/100.0f;
        utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.NeedFireAssistant).SetUtilityScore(aggressiveCurve.Evaluate(utilityScore));
    }

    public void SetFlankUtilitie()
    {
        //Depending on the distanceToThePlayer the utility gets set. the more the enemy is away from the player the more like he flanks him
        float utilityScore = 0;
        float distanceToPlayer = Vector2.Distance(transform.position, playerHolder.target.transform.position);
        if (distanceToPlayer >= maxFlankDistanceToPlayer)
        {
            utilityScore = 1;
        }
        else if (distanceToPlayer <= minFlankDistanceToPlayer)
        {
            utilityScore = 0;
        }
        else
        {
            float range = maxFlankDistanceToPlayer - minFlankDistanceToPlayer;
            float rangeValue = maxFlankDistanceToPlayer - distanceToPlayer;
            utilityScore = rangeValue / range;
            utilityScore = -1 * (utilityScore - 1);
        }

        utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.Flankable).SetUtilityScore(flankCurve.Evaluate(utilityScore));
    }

    public void SetCoverUtilitie()
    {
        //Depending on the Distance to the next Cover the utilityscore gets set
        //Depending if the player is behind Cover or not
        // Depending if the player is far away

        float closeCoverUtilityScore = 0;
        GameObject coverGameobject = null;
        bool didntFoundCover;
        searchCoverHandler.GetFreeAndClosestCoverInRange(enemyStats.coverSearchDistance, out coverGameobject, out didntFoundCover);
        if (coverGameobject == null)
        {
            return;
        }
        float distanceToCover = Vector2.Distance(transform.position, coverGameobject.transform.position);
        if (distanceToCover <= minCoverDistance)
        {
            closeCoverUtilityScore = 1;
        }
        else if (distanceToCover <= maxCoverDistance)
        {
            float range = maxCoverDistance - minCoverDistance;
            float rangeValue = maxCoverDistance - distanceToCover;
            closeCoverUtilityScore = rangeValue / range;
        }

        bool isPlayerBehindCover = searchCoverHandler.CheckIfTransformIsBehindCover(playerHolder.target, transform, enemyStats.playerInCoverCheckDistance);

        float toShotTargetDistance = Vector2.Distance(playerHolder.target.transform.position, transform.position);

        float runTowardsCoverUtilityScore = closeCoverUtilityScore * towardsCoverWeight;
        float shotAtPlayerCoverUtilityScore = closeCoverUtilityScore * towardsCoverWeight;
        if (isPlayerBehindCover)
        {
            runTowardsCoverUtilityScore += isPlayerBehindCoverWeight;
        }
        else
        {
            shotAtPlayerCoverUtilityScore += isPlayerBehindCoverWeight;
        }

        bool shotTargetIsFarAway = toShotTargetDistance < enemyStats.playerInCoverCheckDistance;
        if (shotTargetIsFarAway)
        {
            runTowardsCoverUtilityScore += playerIsFarAwayCoverWeight;
        }
        else
        {
            shotAtPlayerCoverUtilityScore += isPlayerBehindCoverWeight;
        }


        utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.RunTowardsCoverSprint).AddUtilityScore(sprintTowardsCoverCurve.Evaluate(runTowardsCoverUtilityScore));
        utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.RunTowardsCoverAttack).AddUtilityScore(shotAtPlayerTowardsCoverCurve.Evaluate(shotAtPlayerCoverUtilityScore));
    }

    public void SetCanHelpImmediatly()
    {
        if (searchCoverHandler.CheckIfTransformIsBehindCover(transform, playerHolder.target, 30f))
        {
            utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.CanPickUpImmediately).AddUtilityScore(1);
            utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.NeedToTakeCover).AddUtilityScore(0);
        }
        else
        {
            utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.CanPickUpImmediately).AddUtilityScore(0);
            utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.NeedToTakeCover).AddUtilityScore(1);
        }

    }

    public void SetUtilityOnReceivingDamage(bool arg0, int arg1, Transform arg2)
    {
        utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.NeedFireAssistant).AddUtilityScore(onShotValue);
        currentTimeBetweenLookAtChecks = timeBetweenLookAtChecks;
    }

    private void Update()
    {
        Vector2 playerDir = transform.position - playerHolder.target.position;
        float playerToEnemyAngle = Vector2.Angle(playerDir, playerHolder.target.up);
        if (currentTimeBetweenLookAtChecks < 0)
        {
            currentTimeBetweenLookAtChecks = timeBetweenLookAtChecks;
            UtilityAIFlag flag = utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.NeedFireAssistant);
            if (playerToEnemyAngle < minAngle)
            {
                if (!enemyDamageManager.calledForHelp)
                {
                    flag.AddUtilityScoreWithCap(lookAddValue, 0.5f);
                }
            }
            else
            {
                flag.SubFromUtilityScore(lookAddValue);
                if (flag.utilityScore < 0.2)
                {
                    enemyDamageManager.calledForHelp = false;
                }

            }
        }
        else
        {
            currentTimeBetweenLookAtChecks -= Time.deltaTime;
        }

    }

    public void CanGiveFireAssistant(SquadMemberHandler targetSquadMember)
    {
        float utilityScore = 0;
        float distanceToSquadMember = Vector2.Distance(targetSquadMember.transform.position, transform.position);

        if (distanceToSquadMember <= minDistanceToSquadMember)
        {
            utilityScore = distanceUtilityWeight;
        }
        else if (distanceToSquadMember <= maxDistanceToSquadMember)
        {
            float range = maxDistanceToSquadMember - minDistanceToSquadMember;
            float rangeValue = maxDistanceToSquadMember - distanceToSquadMember;
            utilityScore = distanceUtilityWeight * (rangeValue / range);
        }

        Vector2 toPlayerDir = playerHolder.target.position - transform.position;
        if (shotingHandler.IsPlayerShotable(toPlayerDir))
        {
            utilityScore += playerShotableUtilitieWeight;
        }

        utilityScore += healthUtilityWeight * (float)healthManager.currentHealth / 100.0f;

        utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.CanGiveFireAssistant).SetUtilityScore(fireAssistantCurve.Evaluate(utilityScore));
    }

    public void CanGivePickUpAssistance(SquadMemberHandler targetSquadMember)
    {
        float utilityScore = 0;
        float distanceToSquadMember = Vector2.Distance(targetSquadMember.transform.position, transform.position);
        if (distanceToSquadMember <= minHelpDistanceToSquadMember)
        {
            utilityScore = pickUpDistanceUtilityWeight;
        }
        else if (distanceToSquadMember <= maxHelpDistanceToSquadMember)
        {
            float range = maxDistanceToSquadMember - minDistanceToSquadMember;
            float rangeValue = maxDistanceToSquadMember - distanceToSquadMember;
            utilityScore = pickUpDistanceUtilityWeight * (rangeValue / range);
        }

        utilityScore += pickUpHealthUtilityWeight * (float)healthManager.currentHealth / 100.0f;
        utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.CanGiveMedicalAssistent).SetUtilityScore(pickUpAssistantCurve.Evaluate(utilityScore));
    }
}
