using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamageHandler : MonoBehaviour
{
    private HealthManager healthManager;
    Animator anim;

    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        healthManager.OnCalculateDamage.AddListener(OnCalculateDamage);

        anim = GetComponentInChildren<Animator>();
    }

    public void OnCalculateDamage(bool isDead, int damage, Transform damageTransform)
    {
        if (isDead)
        {
            anim.SetTrigger("GetDowned");
            GetComponent<PlayerMovementController>().enabled = false;
            GetComponent<PlayerShotingHandler>().enabled = false;
            GetComponent<Rigidbody2D>().freezeRotation = true;
            StartCoroutine(RestartLevel(2));
        }

    }

    IEnumerator RestartLevel(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
