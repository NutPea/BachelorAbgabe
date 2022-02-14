using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UtilityManager : MonoBehaviour
{
    public List<UtilityAIFlag> utilityAIFlags = new List<UtilityAIFlag>{ new UtilityAIFlag(UtilityAIFlagName.UtilitieName.Aggressiv), new UtilityAIFlag(UtilityAIFlagName.UtilitieName.Flankable), new UtilityAIFlag(UtilityAIFlagName.UtilitieName.RunTowardsCoverAttack),
        new UtilityAIFlag(UtilityAIFlagName.UtilitieName.RunTowardsCoverSprint) , new UtilityAIFlag(UtilityAIFlagName.UtilitieName.NeedFireAssistant),new UtilityAIFlag(UtilityAIFlagName.UtilitieName.CanGiveFireAssistant)
        ,new UtilityAIFlag(UtilityAIFlagName.UtilitieName.CanGiveMedicalAssistent),new UtilityAIFlag(UtilityAIFlagName.UtilitieName.CanPickUpImmediately),new UtilityAIFlag(UtilityAIFlagName.UtilitieName.NeedToTakeCover)};

    public UnityEvent utilityEvent;


    #region Utility
    public UtilityAIFlag GetFlagWithMostUtilitie(List<UtilityAIFlagName.UtilitieName> compareFlagNames)
    {
        //Vergleicht die angegebene Liste compare Flags mit den utilityAIFlags und gibt die passenden
        //utilityAIFlags in Form einer Liste zurück
        List<UtilityAIFlag> choosenUtilityFlags = GetComparedFlags(compareFlagNames);
        UtilityAIFlag bestFlag = choosenUtilityFlags[0];
        float bestValue = choosenUtilityFlags[0].utilityScore;
        if(choosenUtilityFlags.Count > 1)
        {
            for(int i = 1; i< choosenUtilityFlags.Count; i++)
            {
                if (choosenUtilityFlags[i].utilityScore > bestValue)
                {
                    bestValue = choosenUtilityFlags[i].utilityScore;
                    bestFlag = choosenUtilityFlags[i];
                }
            }
        }
        return bestFlag;
    }


    public List<UtilityAIFlag> GetComparedFlags(List<UtilityAIFlagName.UtilitieName> compareFlagNames)
    {
        List<UtilityAIFlag> resultFlags = new List<UtilityAIFlag>();
        foreach (UtilityAIFlag flag in utilityAIFlags)
        {
            for (int i = 0; i < compareFlagNames.Count; i++)
            {
                if (flag.utilityName == compareFlagNames[i])
                {
                    resultFlags.Add(flag);
                    continue;
                }
            }
        }
        return resultFlags;

    }

    public UtilityAIFlag GetFlagByName(UtilityAIFlagName.UtilitieName flagName)
    {
        for (int i = 0; i < utilityAIFlags.Count; i++)
        {
            if (utilityAIFlags[i].utilityName == flagName)
            {
                return utilityAIFlags[i];
            }
        }
        Debug.Log("Did not found Flag!");
        return null;
    }


    #endregion

}
