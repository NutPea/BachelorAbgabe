using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UtilityAIFlag {

    public UtilityAIFlagName.UtilitieName utilityName;
    public float utilityScore = 0.0f;
    public bool isLocked;

    public void SetUtilityScoreLocked(float value,bool lockedValue)
    {
        isLocked = lockedValue;
        SetUtilityScore(value);
    }
    public void SetUtilityScore(float value)
    {
        if (!isLocked)
        {
            if (value > 1)
            {
                utilityScore = 1;
            }
            else if (value < 0)
            {
                utilityScore = 0;
            }
            else
            {
                utilityScore = value;
            }
        }
    }

    public void AddUtilityScore(float value)
    {
        if (!isLocked)
        {
            float newValue = utilityScore + value;
            if (newValue > 1)
            {
                utilityScore = 1;
            }
            else
            {
                utilityScore = newValue;
            }
        }
    }

    public void AddUtilityScoreWithCap(float value, float capValue)
    {
        if (!isLocked)
        {
            float newValue = this.utilityScore + value;
            if (newValue < capValue)
            {
                utilityScore += value;
            }
        }
    }

    public void SubFromUtilityScore(float value)
    {
        if (!isLocked)
        {
            if (utilityScore - value < 0)
            {
                utilityScore = 0;
            }
            else
            {
                utilityScore -= value;
            }
        }
    }


    public UtilityAIFlag(UtilityAIFlagName.UtilitieName name)
    {
        utilityName = name;
    }

}
