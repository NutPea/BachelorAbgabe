using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquadManager : MonoBehaviour
{
    public Transform player;
    public A_Star_Grid A_Star_Grid;
    public List<SquadMemberHandler> squadMembers;

    public float squadSeesPlayerTime = 2f;
    float currentSquadSeesPlayerTime;

    public float squadSplitTimer = 2f;
    public float currentSquadSplitTimer;

    #region SquadMovement
    bool shouldGroup;
    [Space(10)]
    [Header("//-----------------------Squad Movement-------------------------//")]
    public float groupTimer = 2f;
    float currentGroupTimer;

    public LayerMask enviromentMask;
    public float enviromentCheckDistance = 1;
    public float startDistancePercatage = 0.25f;
    public float increacePercentage = 0.05f;
    public float squadMovRot = 30f;
    [Space(10)]
    public State moveSquadMemberState;
    public Transform sqaudFormationLookDirection;
    public Transform[] sqaudMemberPos;
    public int squadMemberWaitingCounter;

    #endregion

    #region PushMovement
    [HideInInspector] public bool squadKnowsPlayerPos = false;
    [Space(10)]
    [Header("//-----------------------Push Movement---------------------------//")]
    public float playerNotSeeTimer = 5f;
    public float pushSearchRadius = 1;
    public float currentPlayerNotSeeTimer = 0f;
    int pushPointHasBeenReachedCounter = 0;
    public State pushPlayerState;
    public LayerMask CoverLayer;
    #endregion

    #region SquadCombatStates
    [Space(10)]
    [Header("//------------------------------SquadCombat------------------------------//")]
    public bool squadIsInCombat;
    public State squadCombatDefaultState;
    #endregion

    #region PickUpOtherMember
    [Space(10)]
    [Header("//--------------------------------PickUpMember------------------------------//")]
    public State pickUpState;
    bool squadMemberIsPickingUp;
    #endregion


    private void Start()
    {
        currentSquadSeesPlayerTime = squadSeesPlayerTime;
        currentPlayerNotSeeTimer = playerNotSeeTimer;
        currentGroupTimer = groupTimer;

        for (int i = 0; i< squadMembers.Count; i++)
        {
            squadMembers[i].currentSquadMemberIndex = i;
            squadMembers[i].squadMemberFireAssistantEvent.AddListener(SquadMemberNeedsFireAssistHelp);
            squadMembers[i].squadMemberPickUpEvent.AddListener(SquadMemberNeedsPickUpHelp);
        }

        MoveSquad();
    }

    private void Update()
    {

        #region Push Player after not seeing him for a couple of Seconds
        SquadSeesPlayerCheck();
        if (squadKnowsPlayerPos)
        {
            if (!SquadMemberSeesPlayerCheck())
            {
                if(currentPlayerNotSeeTimer < 0)
                {
                    currentPlayerNotSeeTimer = playerNotSeeTimer;
                    if (!squadMemberIsPickingUp)
                    {
                        SquadPushPlayer(squadMembers.Count);
                        squadKnowsPlayerPos = false;
                        squadIsInCombat = false;
                    }

                }
                else
                {
                    currentPlayerNotSeeTimer -= Time.deltaTime;
                }
            }
            else
            {
                currentPlayerNotSeeTimer = playerNotSeeTimer;
            }
        }
        #endregion

        if (SquadIsSplit())
        {
            if(currentSquadSplitTimer < 0)
            {
                currentSquadSplitTimer = squadSplitTimer;
                StartGroupTogether();
            }
            else
            {
                currentSquadSplitTimer -= Time.deltaTime;
            }
        }
        else
        {
            currentSquadSplitTimer = squadSplitTimer;
        }


        #region GroupTimer after shouldGroup = true
        if (shouldGroup)
        {
            if(currentGroupTimer < 0)
            {
                MoveSquad();
                StopGrouping();
            }
            else
            {
                currentGroupTimer -= Time.deltaTime;
            }
        }
        #endregion

    }

    public bool SquadIsSplit()
    {
        int halfSquadSize = squadMembers.Count / 2;
        int splittetSquadsMember = 0;

        foreach(SquadMemberHandler squadMember in squadMembers)
        {
            if (!squadMember.SquadMemberAreClose())
            {
                splittetSquadsMember ++;
            }
        }

        if(splittetSquadsMember >= halfSquadSize)
        {
            return true;
        }

        return false;
    }

    #region SquadSeesPlayer
    public void SquadSeesPlayerCheck()
    {
        if (SquadMemberSeesPlayerCheck() && !squadKnowsPlayerPos)
        {
            if (currentSquadSeesPlayerTime < 0)
            {
                currentSquadSeesPlayerTime = squadSeesPlayerTime;
                squadKnowsPlayerPos = true;
                squadIsInCombat = true;
            }
            else
            {
                currentSquadSeesPlayerTime -= Time.deltaTime;
            }
        }
        else
        {
            currentSquadSeesPlayerTime = squadSeesPlayerTime;
        }

    }

    private bool SquadMemberSeesPlayerCheck()
    {
        bool seesPlayer = false;

        foreach (SquadMemberHandler member in squadMembers)
        {
            SearchPlayerWatchCone watchCone = member.GetComponent<SearchPlayerWatchCone>();
            if (watchCone.IsTargetWatchCone())
            {
                seesPlayer = true;
                break;
            }
            else
            {
                EnemyDamageManager enemyDamageManager = member.GetComponent<EnemyDamageManager>();
                if (squadIsInCombat && !member.squadCantChangeState && !enemyDamageManager.isDowned)
                {
                    member.GetComponent<StateController>().TransitionToState(squadCombatDefaultState);
                }
            }
        }




        return seesPlayer;
    }
    #endregion

    #region PushPlayer
    public void SquadPushPlayer(int squadMemberCount)
    {
        Vector2 firstPushPos = GetPushPosition(true, Vector2.zero, pushSearchRadius);
        Vector2 toPlayerDir = (Vector2)player.position - firstPushPos;
        Vector2 opposidePushPos = GetPushPosition(false, (Vector2)player.position + toPlayerDir, pushSearchRadius);

        foreach(SquadMemberHandler member in squadMembers)
        {
            member.GetComponent<UtilityManager>().utilityEvent.Invoke();
        }



        RemoveOldPushPositions();
        GameObject firstPushTransform = new GameObject();
        firstPushTransform.name = "firstPushPoint";
        firstPushTransform.transform.parent = transform.GetChild(0);
        firstPushTransform.transform.position = firstPushPos;

        if (squadMembers.Count > 1)
        {

            GameObject secondPushTransform = new GameObject();
            secondPushTransform.name = "secondPushPoint";
            secondPushTransform.transform.parent = transform.GetChild(0);
            secondPushTransform.transform.position = opposidePushPos;

            Dictionary<int, float> dict = new Dictionary<int, float>();

            for (int i = 0; i < squadMembers.Count; i++)
            {
                dict.Add(i, squadMembers[i].GetComponent<UtilityManager>().GetFlagByName(UtilityAIFlagName.UtilitieName.Flankable).utilityScore);

            }

            var myList = dict.ToList();
            myList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            dict = myList.ToDictionary(x => x.Key, x => x.Value);

            int bestIndex = dict.ElementAt(dict.Count - 1).Key;
            int secondBestIndex = dict.ElementAt(dict.Count - 2).Key;


            squadMembers[bestIndex].GetComponent<MovementTargetHolder>().target = firstPushTransform.transform;
            squadMembers[secondBestIndex].GetComponent<MovementTargetHolder>().target = secondPushTransform.transform;

            squadMembers[bestIndex].GetComponent<StateController>().TransitionToState(pushPlayerState);
            squadMembers[secondBestIndex].GetComponent<StateController>().TransitionToState(pushPlayerState);
        }
        else
        {
            squadMembers[0].GetComponent<MovementTargetHolder>().target = firstPushTransform.transform;
            squadMembers[0].GetComponent<StateController>().TransitionToState(pushPlayerState);
        }

    }
    public Transform GetFirstSearchTransform()
    {
        GameObject lastPlayerTransform = new GameObject();
        lastPlayerTransform.gameObject.name = "Search Transform";
        lastPlayerTransform.transform.parent = transform;
        lastPlayerTransform.transform.position = GetSearchPosition(true, Vector2.zero,pushSearchRadius);
        return lastPlayerTransform.transform;
    }

    public Vector2 GetPushPosition(bool firstSearch, Vector2 beforeSearchPos, float searchRadius)
    {
        int trys = 10;
        for(int i = 0; i< trys; i++)
        {
            Vector2 trySearchPositon = GetSearchPosition(firstSearch, beforeSearchPos, searchRadius);
            Vector2 dirToPlayer = (Vector2)player.transform.position - trySearchPositon;
            RaycastHit2D hit = Physics2D.Raycast(trySearchPositon, dirToPlayer);
            if(hit.collider != null)
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    return trySearchPositon;
                }
            }
        }

        return GetSearchPosition(firstSearch, beforeSearchPos, searchRadius);
    }

    public void PushPointHasBeenReached()
    {
        int squadSize = squadMembers.Count;
        pushPointHasBeenReachedCounter++;

        if(squadMembers.Count > 1)
        {
            if (pushPointHasBeenReachedCounter == 2)
            {
                pushPointHasBeenReachedCounter = 0;
                StartGroupTogether();
            }
        }
        else
        {
            pushPointHasBeenReachedCounter = 0;
            StartGroupTogether();
        }
    }

    public void RemoveOldPushPositions()
    {
        Transform pushPositions = transform.GetChild(0);
        foreach(Transform child in pushPositions)
        {
            Destroy(child.gameObject);
        }
    }

    public Vector2 GetSearchPosition(bool firstSearch,Vector2 beforeSearchPos,float searchRadius)
    {
        bool foundSearchPoint = false;
        Vector2 searchPosition = Vector2.zero;
        int trys = 10;
        for(int i = 0; i<trys; i++)
        {
            Vector2 randomeDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            randomeDir = randomeDir.normalized;
            Vector2 possibleSearchPosition = Vector2.zero;

            if (firstSearch)
            {
                possibleSearchPosition = (Vector2)player.transform.position + randomeDir * searchRadius;
            }
            else
            {
                possibleSearchPosition = beforeSearchPos + randomeDir * searchRadius;
            }

            A_Star_Node possibleNode = A_Star_Grid.WordToNode(possibleSearchPosition);
            if (!possibleNode.walkable)
            {
                continue;
            }
            foundSearchPoint = true;
            searchPosition = possibleNode.worldPos;
        }

        if (!foundSearchPoint)
        {
            searchPosition = player.position;
        }

        return searchPosition;
    }

    #endregion

    #region SquadMemberNeedHelp
    public void SquadMemberNeedsPickUpHelp(int index)
    {
        bool firstViableIndex = false;
        int bestUtilityIndex = 0;
        float bestUtilityValue = 0;
        for (int i = 0; i < squadMembers.Count; i++)
        {
            if (i != index)
            {
                squadMembers[i].GetComponent<UtilitySetter>().CanGivePickUpAssistance(squadMembers[index]);
                if (!firstViableIndex)
                {
                    bestUtilityIndex = i;
                    bestUtilityValue = squadMembers[i].GetComponent<UtilityManager>().GetFlagByName(UtilityAIFlagName.UtilitieName.CanGiveMedicalAssistent).utilityScore;
                    firstViableIndex = true;
                }
            }
        }

        for (int i = bestUtilityIndex; i < squadMembers.Count; i++)
        {
            if (i != index)
            {
                UtilityAIFlag canHelpFlag = squadMembers[i].GetComponent<UtilityManager>().GetFlagByName(UtilityAIFlagName.UtilitieName.CanGiveMedicalAssistent);
                if (canHelpFlag.utilityScore > bestUtilityValue)
                {
                    bestUtilityIndex = i;
                    bestUtilityValue = canHelpFlag.utilityScore;
                }
            }
        }

        if(bestUtilityValue > 0f)
        {
            squadMembers[bestUtilityIndex].GetComponent<MovementTargetHolder>().target = squadMembers[index].transform;
            squadMembers[bestUtilityIndex].GetComponent<StateController>().TransitionToState(pickUpState);
            squadMembers[bestUtilityIndex].squadCantChangeState = true;

            squadMembers[bestUtilityIndex].downedSquadMember = squadMembers[index];

            SquadMemberNeedsFireAssistHelp(bestUtilityIndex);
            squadMemberIsPickingUp = true;
        }
    }
    public void FinishPickingUpSquadMember(SquadMemberHandler pickingUpMember,SquadMemberHandler downedMember)
    {
        pickingUpMember.GetComponent<StateController>().TransitionToState(squadCombatDefaultState);
        downedMember.GetComponent<StateController>().TransitionToState(squadCombatDefaultState);

        squadMemberIsPickingUp = false;
    }
    public void SquadMemberNeedsFireAssistHelp(int index)
    {
        bool firstViableIndex = false;
        int bestUtilityIndex = 0;
        float bestUtilityValue = 0;
        for(int i = 0; i< squadMembers.Count; i++)
        {
            if(i != index)
            {
                squadMembers[i].GetComponent<UtilitySetter>().CanGiveFireAssistant(squadMembers[index]);
                if (!firstViableIndex)
                {
                    bestUtilityIndex = i;
                    bestUtilityValue = squadMembers[i].GetComponent<UtilityManager>().GetFlagByName(UtilityAIFlagName.UtilitieName.CanGiveFireAssistant).utilityScore;
                    firstViableIndex = true;
                }
            }
        }

        for(int i = bestUtilityIndex; i< squadMembers.Count; i++)
        {
            if(i != index)
            {
                UtilityAIFlag canHelpFlag = squadMembers[i].GetComponent<UtilityManager>().GetFlagByName(UtilityAIFlagName.UtilitieName.CanGiveFireAssistant);
                if(canHelpFlag.utilityScore > bestUtilityValue)
                {
                    bestUtilityIndex = i;
                }
            }
        }

        squadMembers[bestUtilityIndex].GetComponent<UtilityManager>().GetFlagByName(UtilityAIFlagName.UtilitieName.Aggressiv).SetUtilityScore(1);

    }

    #endregion

    #region Move Squad
    public void MoveSquad()
    {
        for (int i = 0; i < squadMembers.Count; i++)
        {
            squadMembers[i].GetComponent<MovementTargetHolder>().target = sqaudMemberPos[i];
            squadMembers[i].GetComponent<StateController>().TransitionToState(moveSquadMemberState);
        }
    }
    public void SquadMemberIsWaiting()
    {
        squadMemberWaitingCounter++;
        if(squadMemberWaitingCounter == squadMembers.Count)
        {
            squadMemberWaitingCounter = 0;
            GenerateNewSquadPos();
            MoveSquad();
        }
    }
    public void GenerateNewSquadPos()
    {
        Vector2 dirToPlayer = player.transform.position - sqaudFormationLookDirection.position;
        float distanceToPlayer = Vector2.Distance(player.transform.position, sqaudFormationLookDirection.position);
        dirToPlayer = dirToPlayer.normalized;

        int trys = 20;
        int rotTrys = 5;
        float distancePercantage = startDistancePercatage;
        Vector2 lastPos = sqaudFormationLookDirection.position;

        for (int i = 0; i< trys; i++)
        {
            sqaudFormationLookDirection.position = lastPos + dirToPlayer * (distanceToPlayer * distancePercantage);

            float randomeRot = Random.Range(-squadMovRot, squadMovRot);
            Quaternion q = Quaternion.AngleAxis(randomeRot, Vector3.forward);
            Vector2 randomeDir = q * dirToPlayer;
            sqaudFormationLookDirection.transform.up = randomeDir;
            if (CheckIfSquadPositionIsFine(sqaudMemberPos))
            {
                break;
            }
            else
            {
                for(int y = 0; y < rotTrys; y++)
                {
                    randomeRot = Random.Range(-squadMovRot, squadMovRot);
                    q = Quaternion.AngleAxis(randomeRot, Vector3.forward);
                    randomeDir = q * dirToPlayer;
                    sqaudFormationLookDirection.transform.up = randomeDir;
                    if (CheckIfSquadPositionIsFine(sqaudMemberPos))
                    {
                        break;
                    }

                }
                distancePercantage += increacePercentage;
            }

            if(i == trys-1)
            {
                //Move to player pos
                foreach (SquadMemberHandler sm in squadMembers)
                {
                    sm.GetComponent<StateController>().TransitionToState(squadCombatDefaultState);
                }
                squadKnowsPlayerPos = true;
                squadIsInCombat = true;
            }
        }




    }
    public void StartGroupTogether()
    {
        shouldGroup = true;
    }
    public void StopGrouping()
    {
        shouldGroup = false;
        currentGroupTimer = groupTimer;
    }
    public bool CheckIfSquadPositionIsFine(Transform[] squadPos)
    {
        bool squadPosIsFine = true;
        for(int i = 0; i< squadPos.Length; i++)
        {
            Collider2D[] enviroment = Physics2D.OverlapCircleAll(squadPos[i].position, enviromentCheckDistance, enviromentMask);
            if(enviroment.Length != 0)
            {
                squadPosIsFine = false;
            }
        }
        return squadPosIsFine;
    }

    #endregion


    public void SquadAttackPlayerDirectly()
    {
        if (!squadIsInCombat)
        {
            foreach(SquadMemberHandler sm in squadMembers)
            {
                EnemyDamageManager enemyDamageManager = sm.GetComponent<EnemyDamageManager>();
                if (!enemyDamageManager.isDowned)
                {
                    sm.GetComponent<StateController>().TransitionToState(squadCombatDefaultState);
                }
            }
            squadKnowsPlayerPos = true;
            squadIsInCombat = true;
        }
    }
    public void RemoveSquadMember(SquadMemberHandler squadMember)
    {

        squadMembers.Remove(squadMember);

        for (int i = 0; i < squadMembers.Count; i++)
        {
            if(squadMembers[i].downedSquadMember == squadMember)
            {
                squadMembers[i].GetComponent<StateController>().TransitionToState(squadCombatDefaultState);
            }
            squadMembers[i].currentSquadMemberIndex = i;
        }
    }




}
