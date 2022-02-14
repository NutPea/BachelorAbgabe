using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    //Stays
    public State currentState;
    public List<State> allreadyUsedStates;
    private UtilityManager utilityManager;
    private bool aiActive;

    void Awake()
    {
        aiActive = true;
    }

    private void Start()
    {
        allreadyUsedStates = new List<State>();
        InitState(currentState);
        utilityManager = GetComponent<UtilityManager>();
    }

    void Update()
    {
        if (!aiActive)
            return;
        UpdateActions();
        UpdateDecisions();
    }

    public void UpdateActions()
    {
        for (int i = 0; i < currentState.actions.Count; i++)
        {
            currentState.actions[i].Act(this);
        }
    }

    public void UpdateDecisions()
    {

        if (currentState.decisions.Count == 0)
        {
            return;
        }

        for (int i = 0; i < currentState.decisions.Count; i++)
        {
            bool decisionSucceeded = currentState.decisions[i].Decide(this);
            if (decisionSucceeded)
            {
                if (currentState.decisions[i].utilityTransitions.Count == 0)
                {
                    if (currentState.decisions[i].trueState != null)
                    {
                        TransitionToState(currentState.decisions[i].trueState);
                    }
                }
                else
                {
                    utilityManager.utilityEvent.Invoke();
                    TransitionToState(currentState.decisions[i].GetStateWithMostUtilitie(utilityManager));
                    
                }
            }
        }
    }

    public void InitState(State nextState)
    {
        currentState = currentState.Clone(this);
        allreadyUsedStates.Add(currentState);

        foreach (PluggableAction a in currentState.actions)
        {
            a.OnActStart(this);
        }
        foreach (Decision d in currentState.decisions)
        {
            d.OnDecideStart(this);
        }
    }

    public virtual void TransitionToState(State nextState)
    {
        if (nextState.stateName != currentState.stateName)
        {
            foreach (PluggableAction a in currentState.actions)
            {
                a.OnActEnd(this);
            }
            foreach (Decision d in currentState.decisions)
            {
                d.OnDecideEnd(this);
            }

            bool stateAllreadyUsed = false;
            foreach (State s in allreadyUsedStates)
            {
                if (nextState.name == s.stateName)
                {
                    stateAllreadyUsed = true;
                    currentState = s;
                    break;
                }
            }
            if (!stateAllreadyUsed)
            {
                currentState = nextState.Clone(this);
                allreadyUsedStates.Add(currentState);
            }

            foreach (PluggableAction a in currentState.actions)
            {
                a.OnActStart(this);
            }
            foreach (Decision d in currentState.decisions)
            {
                d.OnDecideStart(this);
            }
        }
    }



    IEnumerator transition(State state, float delay)
    {
        yield return new WaitForSeconds(delay);
        TransitionToState(state);
    }


   



}
