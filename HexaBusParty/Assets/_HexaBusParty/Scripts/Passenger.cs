using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Pathfinding;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private AIPath aiPath;
    private Transform targetDestination;

    public AIPath AIPath
    {
        get => aiPath;
        set => aiPath = value;
    }

    public void SetDestination(Transform destination)
    {
        if (destination == null) return;
        aiPath.destination = destination.position;
    }


}
