using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CalculateDamageEvent : UnityEvent<bool,int,Transform>
{

}

public class OnDamageEvent : UnityEvent<int,TeamFlag,Transform>
{

}

public class HealthManager : MonoBehaviour
{

    public HealthData healthData;
    public int currentHealth;

    public OnDamageEvent OnDamageEvent;
    public CalculateDamageEvent OnCalculateDamage;
    public UnityEvent OnDeath;


    private void Awake()
    {
        OnDamageEvent = new OnDamageEvent();
        OnCalculateDamage = new CalculateDamageEvent();
    }
    private void Start()
    {
        currentHealth = healthData.health;
        OnDamageEvent.AddListener(CalculateDamage);
    }

    private void OnDisable()
    {
        OnDamageEvent.RemoveAllListeners();
        OnCalculateDamage.RemoveAllListeners();
    }



    public void CalculateDamage(int damage,TeamFlag team,Transform hitpos)
    {

        if(team != healthData.team)
        {
            if (currentHealth <= 0) return;
            currentHealth -= damage;
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                OnDeath.Invoke();
                OnCalculateDamage.Invoke(true, damage, hitpos);
            }
            else
            {
                OnCalculateDamage.Invoke(false, damage, hitpos);
            }
        }
    }
    

}
