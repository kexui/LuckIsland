using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_JUMP = "IsJump";

    private Animator anim;
    private PlayerController playerMovement;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerController>();
    }
    private void Update()
    {
        //anim.SetBool(IS_WALKING, playerMovement.isWalking);
        
    }
    public void Jump()
    {
        anim.SetTrigger(IS_JUMP);
    }
}
