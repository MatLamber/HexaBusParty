using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Pathfinding;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private AIPath aiPath;
    [SerializeField] private PassengerAnimationController passengerAnimationController;
    private Transform targetDestination;
    private bool isIdle = true;

    public AIPath AIPath
    {
        get => aiPath;
        set => aiPath = value;
    }

    public void SetDestination(Transform destination)
    {
        if (destination == null) return;

        if (aiPath.reachedDestination)
        {

            if (!isIdle)
            {
                PlayIdle();
                FaceCamera();
            }
            isIdle = true;
        }   
        else
        {
            if (isIdle)
            {
                isIdle = false;
                PlayWalk(); 
            }

        }
        aiPath.destination = destination.position;


    }
    
    public void PlayIdle() => passengerAnimationController.PlayIdle();
    public void PlayWalk() => passengerAnimationController.PlayWalk();
    public void PlayDance() => passengerAnimationController.PlayDance();

    public void FaceCamera() => LeanTween.rotateLocal(gameObject, new Vector3(0,180,0), 0.3f).setOnComplete(PlayDance);


}
