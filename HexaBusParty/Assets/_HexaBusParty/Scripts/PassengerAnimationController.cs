using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerAnimationController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Animator animator;
    
    
    public void SetAnimation(string animationName) => animator.SetTrigger(animationName);

    public void PlayIdle()
    {
        SetAnimation("Idle");
    }

    public void PlayWalk()
    {
        SetAnimation("Walk");
    }

    public void PlayDance()
    {
        SetAnimation("Dance");
    }
}
