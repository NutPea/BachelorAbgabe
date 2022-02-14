using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/State")]
public class State : ScriptableObject
{

    public List<PluggableAction> actions = new List<PluggableAction>();
    public List<Decision> decisions = new List<Decision>();
    [HideInInspector] public string stateName;



    public State Clone(StateController sceneRef)
    {
        State cloneState = Instantiate(this);
        cloneState.actions.Clear();
        cloneState.decisions.Clear();
        stateName = name;
        foreach (PluggableAction a in actions)
        {
            if(a == null)
            {
                Debug.Log(stateName + "Action could not get found");
            }
            PluggableAction clonedAction = a.Clone();
            clonedAction.OnActInit(sceneRef);
            cloneState.actions.Add(clonedAction);
        }
        foreach (Decision d in decisions)
        {
            if (d == null)
            {
                Debug.Log(stateName + "Decisioun could not get found");
            }
            Decision clonedDecision = d.Clone();
            clonedDecision.OnDecideInit(sceneRef);
            cloneState.decisions.Add(clonedDecision);
        }

        return cloneState;
    }

}