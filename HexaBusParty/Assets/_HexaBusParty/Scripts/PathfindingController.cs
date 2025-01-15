using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingController : MonoBehaviour
{
    [Header("Elements")] [SerializeField] private AstarPath navmeshGenerator;


    private void OnEnable()
    {
        HexStack.onCheckForFreeSeats += RescastNavMesh;
    }

    private void OnDisable()
    {
        HexStack.onCheckForFreeSeats -= RescastNavMesh;
    }

    public void RescastNavMesh(Transform freseSeat)
    {
        StartCoroutine(RecastCoroutine(freseSeat));
    }

    IEnumerator RecastCoroutine(Transform freeSeatTransform)
    {
        yield return new WaitForSeconds(1.5f);
        navmeshGenerator.Scan();
        yield return new WaitForSeconds(0.3f);
        PassengerManagers.SendPassengerToSit(freeSeatTransform);
    }
}