using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotingHandler : MonoBehaviour
{

    ShotingHandler shotingHandler;
    Camera mainCam;
    Animator anim;
    CrouchHandler crouchHandler;
    PlayerInput inputActions;
    public float timeBetweenShots;
    float currentTimeBetweenShots;

    bool isShooting;
    private void Awake()
    {
        inputActions = new PlayerInput();

        inputActions.Keyboard.LeftMouseIsDown.performed += ctx => OnShot();
        inputActions.Keyboard.LeftMouseIsDown.canceled += ctx => OnStopShoting();
    }

    public void OnShot()
    {
        isShooting = true;
    }

    public void OnStopShoting()
    {
        isShooting = false;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        shotingHandler = GetComponent<ShotingHandler>();
        currentTimeBetweenShots = timeBetweenShots;
        mainCam = Camera.main;
        anim = GetComponentInChildren<Animator>();
        crouchHandler = GetComponent<CrouchHandler>();
    }




    // Update is called once per frame
    void Update()
    {
        if (!isShooting || crouchHandler.isCrouching) return;

        if (currentTimeBetweenShots < 0)
        {
            Vector2 shotDir = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            shotDir = shotDir.normalized;
            shotingHandler.Shot(shotDir);
            currentTimeBetweenShots = timeBetweenShots;
            anim.SetTrigger("Shoting");
        }
        else
        {
            currentTimeBetweenShots -= Time.deltaTime;
        }
    }
}
