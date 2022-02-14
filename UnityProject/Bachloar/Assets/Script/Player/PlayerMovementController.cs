using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Camera mainCam;
    CrouchHandler coverHandler;
    Vector2 mousePos;
    PlayerInput inputActions;

    public Transform aimPivot;
    public float movementSpeed;
    public float crouchMovementSpeed;
    float currentMovementSpeed;
    public float watchDistance;
     Animator anim;

    public void Awake()
    {
        inputActions = new PlayerInput();
        inputActions.Keyboard.Crouch.performed += ctx => OnStartCrouch();
        inputActions.Keyboard.Crouch.canceled += ctx => OnStopCrouch();

    }

    public void OnEnable()
    {
        inputActions.Enable();
    }

    public void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        coverHandler = GetComponent<CrouchHandler>();
        anim = GetComponentInChildren<Animator>();
        currentMovementSpeed = movementSpeed;
    }

    #region Crouching
    public void OnStartCrouch()
    {
        coverHandler.StartCrouching();
        currentMovementSpeed = crouchMovementSpeed;
    }

    public void OnStopCrouch()
    {
        coverHandler.StopCrouching();
        currentMovementSpeed = movementSpeed;
    }
    #endregion

    void FixedUpdate()
    {
        #region PlayerMovement
        Vector2 movementInput = new Vector2(inputActions.Keyboard.Horizontal.ReadValue<float>(), inputActions.Keyboard.Vertical.ReadValue<float>());
        movementInput = movementInput.normalized;
        if (movementInput == Vector2.zero)
        {
            anim.SetBool("IsWalking", false);
        }
        else
        {
            anim.SetBool("IsWalking", true);
        }

        Vector2 horizMoveVec = movementInput.x * currentMovementSpeed * Vector2.right * Time.fixedDeltaTime;
        Vector2 vertMoveVec = movementInput.y * currentMovementSpeed * Vector2.up * Time.fixedDeltaTime;
        rb2d.MovePosition((Vector2)transform.position + horizMoveVec + vertMoveVec);
        #endregion

        #region WatchDirOfPlayer
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 watchDir = mousePos - (Vector2)transform.position;
        watchDir = watchDir.normalized;

        aimPivot.transform.position = (Vector2)transform.position + watchDir * watchDistance;

        transform.up = watchDir;
        #endregion
    }
}
