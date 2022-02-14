using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : ScriptableObject
{
    [Header("TransitionStates")]
    public State trueState;

    [Header("Utilitie")]
    public List<UtilityTransition> utilityTransitions;

    public abstract void OnDecideInit(StateController sceneRef);
    public abstract void OnDecideStart(StateController sceneRef);
    public abstract bool Decide(StateController sceneRef);
    public abstract void OnDecideEnd(StateController sceneRef);

    public Decision Clone()
    {
        return Instantiate(this);
    }

    public State GetStateWithMostUtilitie(UtilityManager utilityManager)
    {
        List<UtilityAIFlagName.UtilitieName> allUsedUtilities = new List<UtilityAIFlagName.UtilitieName>();
        foreach(UtilityTransition uT in utilityTransitions)
        {
            allUsedUtilities.Add(uT.utilitieName);
        }
        if(allUsedUtilities.Count <= 0)
        {
            Debug.Log("NoUtilitiesInList");
            return null;
        }

        UtilityAIFlag bestFlag = utilityManager.GetFlagWithMostUtilitie(allUsedUtilities);
        foreach (UtilityTransition uT in utilityTransitions)
        {
            if(bestFlag.utilityName == uT.utilitieName)
            {
                return uT.utilitieTransitionState;
            }
        }

        Debug.Log("Didnt found Best Flag!!!");
        return null;
    }
}