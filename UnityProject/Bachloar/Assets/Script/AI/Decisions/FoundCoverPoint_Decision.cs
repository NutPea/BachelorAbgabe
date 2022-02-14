using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " FoundCoverPoint", menuName = "PluggableAI/Decision/FoundCoverPoint")]
public class FoundCoverPoint_Decision : Decision
{
    SearchCoverHandler searchCoverHandler;
    public override bool Decide(StateController sceneRef)
    {
        return searchCoverHandler.didntFoundCover;
    }

    public override void OnDecideEnd(StateController sceneRef)
    {
        
    }

    public override void OnDecideInit(StateController sceneRef)
    {
        searchCoverHandler = sceneRef.GetComponent<SearchCoverHandler>();
    }

    public override void OnDecideStart(StateController sceneRef)
    {
        
    }
}
