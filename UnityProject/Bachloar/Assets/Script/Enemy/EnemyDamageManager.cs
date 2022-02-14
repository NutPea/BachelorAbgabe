using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDamageManager : MonoBehaviour
{
    UtilityManager utilityManager;
    HealthManager healthManager;
    PlayerHolder shotingTargetHolder;
    SquadMemberHandler squadMemberHandler;
    StateController stateController;
    CrouchHandler crouchHandler;
    A_Star_Movement movement;
    Animator anim;
    public float downHealth = 20f;
    bool hasBeenPickedUp;
    public float pickUpHealth = 20f;

    public bool isInCombat;

    public Transform player;
    public UtilityAIFlag flag;
    [HideInInspector] public bool calledForHelp;

    public float lookAddValue;
    public bool isDowned;
    public State isDownedState;

    public GameObject deadBody;

    void Start()
    {
        utilityManager = GetComponent<UtilityManager>();
        squadMemberHandler = GetComponent<SquadMemberHandler>();
        stateController = GetComponent<StateController>();
        crouchHandler = GetComponent<CrouchHandler>();
        movement = GetComponent<A_Star_Movement>();
        anim = GetComponent<AnimatorHolder>().anim;

        healthManager = GetComponent<HealthManager>();
        shotingTargetHolder = GetComponent<PlayerHolder>();
        healthManager.OnCalculateDamage.AddListener(OnCalculateDamage);


    }


    public void OnCalculateDamage(bool isDead,int damage,Transform damageTransform)
    {
        if (!isInCombat)
        {
            isInCombat = true;
        }

        UtilityAIFlag flag = utilityManager.GetFlagByName(UtilityAIFlagName.UtilitieName.NeedFireAssistant);
        if(flag.utilityScore >= 1 && !calledForHelp)
        {
            squadMemberHandler.CallForFireAssistance();
            calledForHelp = true;
        }
        
        squadMemberHandler.squadManager.SquadAttackPlayerDirectly();

        if(healthManager.currentHealth <= downHealth )
        {
            stateController.TransitionToState(isDownedState);
        }

        if (isDead)
        {

            squadMemberHandler.squadManager.RemoveSquadMember(squadMemberHandler);
            movement.RemoveCourotines();
            GameObject dB = Instantiate(deadBody, transform.position, Quaternion.identity);
            dB.transform.up = transform.up;
            Destroy(gameObject);
        }

    }

    
}
