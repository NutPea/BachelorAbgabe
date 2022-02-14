using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PluggableAction : ScriptableObject
{
    public abstract void OnActInit(StateController sceneRef);
    public abstract void OnActStart(StateController sceneRef);
    public abstract void Act(StateController sceneRef);
    public abstract void OnActEnd(StateController sceneRef);
    public PluggableAction Clone()
    {
        return Instantiate(this);
    }

}