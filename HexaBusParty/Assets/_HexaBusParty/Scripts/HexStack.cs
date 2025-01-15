using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HexStack : MonoBehaviour
{
    public List<Hexagon> Hexagons { get; private set; }
    public GridCell CurrentGridCell { get; private set; }
    

    public static Action <Transform> onCheckForFreeSeats; 

    public void Add(Hexagon hexagon)
    {
        if (Hexagons == null)
            Hexagons = new List<Hexagon>();
        Hexagons.Add(hexagon);
        hexagon.Configure(this);
        hexagon.SetParent(transform);
    }

    public void Place()
    {
        foreach (var hexagon in Hexagons)
        {
            // hexagon.DisableCollider();
        }
    }

    public Color GetTopHexagonColor()
    {
        return Hexagons[^1].Color;
    }

    public bool Contains(Hexagon hexagon)
    {
        return Hexagons.Contains(hexagon);
    }

    public void Remove(Hexagon hexagon)
    {
        Hexagons.Remove(hexagon);
        if (Hexagons.Count == 1)
            CheckForFreeSeats();
        if (Hexagons.Count == 0)
            DestroyImmediate(gameObject);
    }


    public void Initialize(GridCell initialGridCell)
    {
        for (int i = 0; i < transform.childCount; i++)
            Add(transform.GetChild(i).GetComponent<Hexagon>());
        CurrentGridCell = initialGridCell;
    }

    public void OccupyCurrentGridCell(GridCell gridCell)
    {
        CurrentGridCell = gridCell;
    }
    public void FreeCurrentGridCell()
    {
        if (CurrentGridCell != null)
            CurrentGridCell.ResetStack();
    }

    public void CheckForFreeSeats()
    {
        if (Hexagons.Count > 1)
            return;
        Hexagon baseHexagon = Hexagons[0];
        if (baseHexagon.Unstackable)
        { 
           onCheckForFreeSeats?.Invoke(baseHexagon.transform);
        }
        
    }
}