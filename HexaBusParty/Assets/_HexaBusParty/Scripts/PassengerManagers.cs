using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerManagers : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private List<Passenger> passengers;
    [SerializeField] private Transform grid;
    private List<GridCell> gridCells;
    private static List<Transform> seatsTransform = new List<Transform>();


    private void OnEnable()
    {
        //HexStack.onCheckForFreeSeats += SendPassengerToSit;
    }

    private void OnDisable()
    {
      //  HexStack.onCheckForFreeSeats -= SendPassengerToSit;
    }

    private void Update()
    {
        if(seatsTransform.Count == 0) return;
        for (int i = 0; i < seatsTransform.Count; i++)
        {
            passengers[i].SetDestination(seatsTransform[i]);
        }
    }

    public static void SendPassengerToSit(Transform seatTransform)
    {
        seatsTransform.Add(seatTransform);
    }

}
