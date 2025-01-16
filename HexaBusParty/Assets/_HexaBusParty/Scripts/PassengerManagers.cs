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
    private List<Passenger> movingPassengers = new List<Passenger>();
    
    
    [Header("Settings")]
    private float distanceBetweenPassengers = 1.8f;


    private void Start()
    {
        RotateFirsPassengerTowardTheBus();
    }

    private void OnEnable()
    {
        HexStack.onCheckForFreeSeats += CheckForFreeSeatsCallback;
    }

    private void OnDisable()
    {
       HexStack.onCheckForFreeSeats -= CheckForFreeSeatsCallback;
    }

    private void Update()
    {
        if(seatsTransform.Count == 0) return;
        for (int i = 0; i < seatsTransform.Count; i++)
        {
            if (movingPassengers.Count == seatsTransform.Count) 
                movingPassengers[i].SetDestination(seatsTransform[i]);
        }
    }

    public static void SendPassengerToSit(Transform seatTransform)
    {
        seatsTransform.Add(seatTransform);
    }

    public void UpgdatePassengersList()
    {
        movingPassengers.Add(passengers[0]);
        passengers.RemoveAt(0);
    }

    public void CheckForFreeSeatsCallback(Transform freeSeat)
    {
        StartCoroutine(PassengerlineUpdateCoroutine());
    }

    IEnumerator PassengerlineUpdateCoroutine()
    {
        yield return new WaitForSeconds(3f);
        UpgdatePassengersList();
        MovePassengersLine();
        RotateFirsPassengerTowardTheBus();
    }

    public void MovePassengersLine()
    {
        float delay = 0;
        foreach (var passenger in passengers)
        {
            LeanTween.moveZ(passenger.gameObject, passenger.transform.position.z + distanceBetweenPassengers,
                0.3f).setDelay(delay);
            delay = .1f;
        }
    }
    public void RotateFirsPassengerTowardTheBus()
    {
        if(passengers.Count == 0) return;
        LeanTween.rotateLocal(passengers[0].gameObject, new Vector3(0, -90, 0), 0.3f);
    }

}
