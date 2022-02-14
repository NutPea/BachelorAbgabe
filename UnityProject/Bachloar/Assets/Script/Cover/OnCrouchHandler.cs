using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCrouchHandler : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color notCrouchColor;
    public Color crouchColor;
    Animator anim;

    private void Start()
    {
        notCrouchColor = spriteRenderer.color;
        anim = GetComponent<Animator>();
    }

    public void OnCrouch()
    {
        spriteRenderer.color = crouchColor;
        anim.SetBool("IsCrouching", true);
    }

    public void StopCrouch()
    {
        spriteRenderer.color = notCrouchColor;
        anim.SetBool("IsCrouching", false);
    }

}
