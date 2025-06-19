using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_JUMP = "IsJump";
    private const string WALK = "Walk";

    private Animator anim;
    private PlayerController playerMovement;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        //playerMovement = GetComponent<PlayerController>();
    }
    private void Update()
    {
        //anim.SetBool(IS_WALKING, playerMovement.isWalking);
        
    }
    public void Jump()
    {
        anim.SetTrigger(IS_JUMP);
    }
    public void Walk()
    {
        if (anim.GetBool(WALK))
        {
            anim.SetBool(WALK, false);
        }
        else
        {
            anim.SetBool(WALK, true);
        }
    }
    public void SetWalk(bool isWalk)
    {
        anim.SetBool(WALK, isWalk);
    }
}
